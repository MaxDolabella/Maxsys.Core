namespace Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Provides extension methods for <see cref="PropertyBuilder"/>.
/// </summary>
public static class PropertyBuilderExtensions
{
    /// <summary>
    /// Shortcut for <c>builder.IsRequired(false);</c><para/>
    /// Configures this property as nullable. A property can only be configured as non-required if it is based on a CLR type that can be assigned null.
    /// </summary>
    /// <typeparam name="TProperty"></typeparam>
    /// <param name="builder"></param>
    /// <returns>The same builder instance so that multiple configuration calls can be chained.</returns>
    public static PropertyBuilder<TProperty> IsOptional<TProperty>(this PropertyBuilder<TProperty> builder) => builder.IsRequired(false);

    /// <summary>
    /// Shortcut for <c>builder.IsRequired(false);</c><para/>
    /// Configures this property as nullable. A property can only be configured as non-required if it is based on a CLR type that can be assigned null.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TRelatedEntity"></typeparam>
    /// <param name="builder"></param>
    /// <returns>The same builder instance so that multiple configuration calls can be chained.</returns>
    public static ReferenceReferenceBuilder<TEntity, TRelatedEntity> IsOptional<TEntity, TRelatedEntity>(this ReferenceReferenceBuilder<TEntity, TRelatedEntity> builder)
        where TEntity : class
        where TRelatedEntity : class
        => builder.IsRequired(false);
}