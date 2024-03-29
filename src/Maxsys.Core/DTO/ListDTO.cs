namespace Maxsys.Core;

/// <summary>
/// Usado para retornar uma lista paginada contendo a quantidade total de registros
/// na fonte de dados (<see cref="Count"/>) e a uma lista da página (<see cref="Items"/>).
/// </summary>
/// <typeparam name="T"></typeparam>
public class ListDTO<T>
{
    public ListDTO()
    {
        Items = [];
        Count = 0;
    }

    public ListDTO(List<T> list, int count)
    {
        Count = count;
        Items = list;
    }

    public ListDTO(List<T> list)
        : this(list, list.Count)
    { }

    public int Count { get; set; }
    public List<T> Items { get; set; }
}