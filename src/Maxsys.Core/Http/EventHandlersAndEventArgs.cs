namespace Maxsys.Core.Http;

public delegate Task SendingEventHandler(object? sender, SendingEventArgs e, CancellationToken cancellationToken);

public delegate Task SentEventHandler(object? sender, SentEventArgs e, CancellationToken cancellationToken);

public delegate Task MaxsysApiResponseInvalidEventHandler(object? sender, MaxsysApiResponseInvalidEventArgs e, CancellationToken cancellationToken);

public delegate Task MaxsysApiResponseValidEventHandler(object? sender, MaxsysApiResponseValidEventArgs e, CancellationToken cancellationToken);

public record SendingEventArgs(HttpRequestMessage HttpRequestMessage);
public record SentEventArgs(HttpResponseMessage HttpResponseMessage);
public record MaxsysApiResponseInvalidEventArgs(MaxsysApiValidationResult ValidationResult);
public record MaxsysApiResponseValidEventArgs(HttpResponseMessage HttpResponseMessage, string ResponseContent);