using System.Collections.Generic;

namespace Maxsys.Core;

/// <summary>
///
/// </summary>
/// <typeparam name="T"></typeparam>
public class ListDTO<T>
{
    public int Count { get; set; }
    public IReadOnlyList<T> List { get; set; }
}