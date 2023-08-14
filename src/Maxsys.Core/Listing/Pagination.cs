namespace Maxsys.Core;

public sealed class Pagination
{
    #region CTOR

    public Pagination()
    { }

    public Pagination(int index, int size)
    {
        if (index < 0)
            throw new ArgumentOutOfRangeException(nameof(index),
                $"Para usar esse construtor, '{nameof(index)}' deve possuir valor maior ou igual a 0.");

        if (size <= 0)
            throw new ArgumentOutOfRangeException(nameof(size),
                $"Para usar esse construtor, '{nameof(size)}' deve possuir valor maior que 0.");

        Index = index;
        Size = size;
    }

    #endregion CTOR

    public int Index { get; set; } = 0;
    public int Size { get; set; } = 0;

    public bool IsNotEmpty() => Index >= 0 && Size > 0;
}