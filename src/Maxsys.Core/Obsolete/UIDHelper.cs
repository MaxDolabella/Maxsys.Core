namespace Maxsys.Core.Helpers;

// remover em v18
[Obsolete("Utilizar UIDGen", true)]
public static class UIDHelper
{
    public static string GenerateUID(UIDBits bits, UIDGenerationOptions options = UIDGenerationOptions.None) => throw new NotImplementedException();

    public static string GenerateUID(int bytes, UIDGenerationOptions options = UIDGenerationOptions.None) => throw new NotImplementedException();
}