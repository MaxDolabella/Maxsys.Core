namespace Maxsys.Messaging.Abstractions;

/// <summary>Marker para queries com retorno tipado. Leitura pura — sem side effects.</summary>
public interface IQuery<out TResponse>;