namespace Maxsys.Core;

/// <summary>
/// Provides a DTO typification for an object.
/// </summary>
public interface IDTO
{ }

/// <summary>
/// Provides a DTO typification for an object with an Id property.
/// </summary>
public interface IDTO<TKey> : IKey<TKey> where TKey : notnull
{ }