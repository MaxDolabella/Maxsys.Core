namespace Maxsys.Messaging.Extensions;

/// <summary>
/// Configurações para o registro do pipeline de messaging.
/// </summary>
public sealed class MessagingOptions
{
    internal List<Type> BehaviorTypes { get; } = [];

    /// <summary>
    /// Adiciona um behavior open generic ao pipeline. Ex: typeof(LoggingBehavior&lt;,&gt;)
    /// </summary>
    public MessagingOptions AddOpenBehavior(Type behaviorType)
    {
        BehaviorTypes.Add(behaviorType);
        return this;
    }
}