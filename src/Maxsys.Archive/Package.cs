using System.Text.Json;

namespace Maxsys.Archive;

public class Package
{
    public string Version { get; set; } = PackageConsts.CURRENT_VERSION;
    public object Meta { get; set; } = new { };
    public object Contents { get; set; } = new { };
    public List<PackageFile> Files { get; } = [];

    public T GetContents<T>() => JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(Contents))!;

    public T GetMeta<T>() => JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(Meta))!;

    public PackageFile? GetFile(string name) => Files.FirstOrDefault(f => f.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

    public void AttachFile(PackageFile file) => Files.Add(file);
    public void AttachFile(string name, string filePath) => AttachFile(new PackageFile(name, File.ReadAllBytes(filePath)));
    public void AttachFile(string name, byte[] data) => AttachFile(new PackageFile(name, data));

}
