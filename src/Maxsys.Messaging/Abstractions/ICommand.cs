namespace Maxsys.Messaging.Abstractions;

/// <summary>Marker para commands sem retorno. Alteram estado — têm side effects.</summary>
public interface ICommand;

/// <summary>Marker para commands com retorno tipado.</summary>
public interface ICommand<out TResponse>;