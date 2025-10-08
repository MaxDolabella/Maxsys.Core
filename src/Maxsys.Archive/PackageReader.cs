using System.IO.Compression;
using System.Text;
using System.Text.Json;

namespace Maxsys.Archive;

public static class PackageReader
{
    public static Package Read(string filePath)
    {
        using var archive = GetArchive(filePath);

        return ReadToPackage(archive);
    }

    public static Package Read(Stream stream)
    {
        using var archive = GetArchive(stream);

        return ReadToPackage(archive);
    }

    private static ZipArchive GetArchive(string filePath) => ZipFile.OpenRead(filePath);

    private static ZipArchive GetArchive(Stream stream) => new(stream);

    private static Package ReadToPackage(ZipArchive archive)
    {
        var pkg = new Package();

        // VERSION
        var versionEntry = archive.GetEntry(PackageConsts.ENTRY_VERSION);
        if (versionEntry != null)
        {
            using var reader = new StreamReader(versionEntry.Open(), Encoding.UTF8);
            pkg.Version = reader.ReadToEnd();
        }

        // META
        var metaEntry = archive.GetEntry(PackageConsts.ENTRY_META);
        if (metaEntry != null)
        {
            using var reader = new StreamReader(metaEntry.Open(), Encoding.UTF8);
            pkg.Meta = JsonSerializer.Deserialize<object>(reader.ReadToEnd())!;
        }

        // CONTENTS
        var contentsEntry = archive.GetEntry(PackageConsts.ENTRY_CONTENTS);
        if (contentsEntry != null)
        {
            using var reader = new StreamReader(contentsEntry.Open(), Encoding.UTF8);
            pkg.Contents = JsonSerializer.Deserialize<object>(reader.ReadToEnd())!;
        }

        // FILES/*
        foreach (var entry in archive.Entries)
        {
            if (entry.FullName.StartsWith(PackageConsts.FILES_FOLDER))
            {
                using var ms = new MemoryStream();
                using var es = entry.Open();
                es.CopyTo(ms);
                pkg.Files.Add(new PackageFile(entry.FullName[PackageConsts.FILES_FOLDER.Length..], ms.ToArray()));
            }
        }

        return pkg;
    }
}