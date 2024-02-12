namespace Maxsys.Core.Events;

/// <summary>
/// Represents the method that will handle an event that has a value.
/// </summary>
/// <param name="sender">The source of the event.</param>
/// <param name="e">An object that contains the event value.</param>
public delegate void ValueEventHandler(object? sender, ValueEventArgs e);


/// <summary>
/// 
/// </summary>
/// <typeparam name="TEventArgs"></typeparam>
/// <param name="sender"></param>
/// <param name="e"></param>
/// <param name="cancellationToken"></param>
/// <returns></returns>
/// <remarks><see href="https://www.youtube.com/watch?v=c1NtUub2jbo">Brian Lagunas: Custom Async Events (youtube)</see></remarks>
public delegate ValueTask AsyncEventHandler<TEventArgs>(object? sender, TEventArgs e, CancellationToken cancellationToken);


public delegate ValueTask<OperationResult> OperationResultAsyncEventHandler<TEventArgs>(object? sender, TEventArgs e, CancellationToken cancellationToken);
public delegate OperationResult OperationResultEventHandler<TEventArgs>(object? sender, TEventArgs e);