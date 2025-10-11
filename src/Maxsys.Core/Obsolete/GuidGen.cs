namespace System;

// remover em v18
[Obsolete("Utilizar UIDGen", true)]
public static class GuidGen
{
    public static Guid NewSequentialGuid(SequentialGuidType guidType) => throw new NotImplementedException();

    public static string NewUID_32Bits() => throw new NotImplementedException();

    public static string DateTimeToUID_64Bits(DateTime dateTime = default) => throw new NotImplementedException();
}

public enum SequentialGuidType
{ }