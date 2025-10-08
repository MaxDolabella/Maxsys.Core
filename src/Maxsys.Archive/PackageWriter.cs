using System.IO.Compression;
using System.Text;
using System.Text.Json;

namespace Maxsys.Archive;

public static class PackageWriter
{
    public static void Write(string filePath, Package package)
    {
        if (File.Exists(filePath))
            File.Delete(filePath);

        using var fs = new FileStream(filePath, FileMode.CreateNew);
        using var archive = new ZipArchive(fs, ZipArchiveMode.Create, leaveOpen: false);

        // VERSION
        package.UpdateVersion();
        var versionEntry = archive.CreateEntry(PackageConsts.ENTRY_VERSION);
        using (var writer = new StreamWriter(versionEntry.Open(), Encoding.UTF8))
            writer.Write(package.Version);

        // META
        var metaEntry = archive.CreateEntry(PackageConsts.ENTRY_META);
        using (var writer = new StreamWriter(metaEntry.Open(), Encoding.UTF8))
            writer.Write(JsonSerializer.Serialize(package.Meta));

        // CONTENTS
        var contentsEntry = archive.CreateEntry(PackageConsts.ENTRY_CONTENTS);
        using (var writer = new StreamWriter(contentsEntry.Open(), Encoding.UTF8))
            writer.Write(JsonSerializer.Serialize(package.Contents));

        // FILES
        foreach (var file in package.Files)
        {
            var entry = archive.CreateEntry($"{PackageConsts.FILES_FOLDER}{file.Name}", CompressionLevel.SmallestSize);
            using var entryStream = entry.Open();
            entryStream.Write(file.Data, 0, file.Data.Length);
        }
    }
}