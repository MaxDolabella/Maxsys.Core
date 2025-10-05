namespace Maxsys.Archive;

public class PackageFile
{
    public string Name { get; }
    public byte[] Data { get; }

    public PackageFile(string name, byte[] data)
    {
        Name = name;
        Data = data;
    }
}
