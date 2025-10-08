using System.Reflection;
using System.Text.Json;

namespace Maxsys.Archive;

public class Package
{
    public string Version { get; internal set; } = default!;
    public object Meta { get; set; } = new { };
    public object Contents { get; set; } = new { };
    public List<PackageFile> Files { get; } = [];

    public T GetContents<T>() => JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(Contents))!;

    public T GetMeta<T>() => JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(Meta))!;

    public PackageFile? GetFile(string name) => Files.FirstOrDefault(f => f.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

    public void AttachFile(PackageFile file) => Files.Add(file);

    public void AttachFile(string name, string filePath) => AttachFile(new PackageFile(name, File.ReadAllBytes(filePath)));

    public void AttachFile(string name, byte[] data) => AttachFile(new PackageFile(name, data));

    internal void UpdateVersion()
    {
        var fullVersion = Assembly
            .GetExecutingAssembly()
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
            .InformationalVersion ?? PackageConsts.NO_VERSION;

        // Remove build metadata
        var version = fullVersion.Split('+')[0];

        Version = version;
    }
}