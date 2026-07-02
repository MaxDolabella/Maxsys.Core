namespace Maxsys.Messaging.Abstractions.Commands;

public abstract class CommandBase : ICommand;

public abstract class CommandBase<TResponse> : ICommand<TResponse>;