namespace Maxsys.Core;

/// <summary>
/// Usado para retornar uma lista paginada contendo a quantidade total de registros
/// na fonte de dados (<see cref="Count"/>) e a uma lista da página (<see cref="List"/>).
/// </summary>
/// <typeparam name="T"></typeparam>
public class ListDTO<T>
{
    public ListDTO()
    {
        List = new List<T>();
        Count = 0;
    }

    public ListDTO(IReadOnlyList<T> list)
        : this(list, list.Count)
    { }

    public ListDTO(IReadOnlyList<T> list, int count)
    {
        Count = count;
        List = list;
    }

    public ListDTO(List<T> list, int count)
    {
        Count = count;
        List = list;
    }

    public ListDTO(List<T> list)
        : this(list, list.Count)
    { }

    public int Count { get; set; }
    public IReadOnlyList<T> List { get; set; }
}