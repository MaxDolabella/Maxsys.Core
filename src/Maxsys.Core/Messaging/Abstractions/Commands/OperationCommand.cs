namespace Maxsys.Core.Messaging.Abstractions.Commands;

/// <summary>
/// Implements <see cref="ICommand{TResponse}"/> where TResponse is <see cref="OperationResult"/>
/// </summary>
public abstract class OperationCommand : CommandBase<OperationResult>
{
    public DateTime Timestamp { get; private set; }

    public OperationResult OperationResult { get; set; }

    protected OperationCommand()
    {
        Timestamp = DateTime.Now;
        OperationResult = new OperationResult();
    }
}

/// <summary>
/// Implements <see cref="ICommand{X}"/> where TResponse is <see cref="OperationResult{T}"/>
/// </summary>
public abstract class OperationCommand<T> : CommandBase<OperationResult<T>>
{
    public DateTime Timestamp { get; private set; }

    public OperationResult<T> OperationResult { get; set; }

    protected OperationCommand()
    {
        Timestamp = DateTime.Now;
        OperationResult = new OperationResult<T>();
    }
}