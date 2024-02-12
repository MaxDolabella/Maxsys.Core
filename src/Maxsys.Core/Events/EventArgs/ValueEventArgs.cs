namespace Maxsys.Core;

[Serializable]
public class ValueEventArgs : EventArgs
{
    public object? Value { get; set; } = default;

    public ValueEventArgs(object? e)
    {
        Value = e;
    }

    public T? GetValueAs<T>() where T : class
    {
        return Value as T;
    }

    public bool IsValue<T>() where T : class
    {
        return Value is T;
    }
}