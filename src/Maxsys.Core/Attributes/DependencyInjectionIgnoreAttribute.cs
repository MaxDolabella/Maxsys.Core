using Maxsys.Core.Extensions;

namespace Maxsys.Core;

/// <summary>
/// Indica que o objeto não será registrado no ServiceProvider pelos métodos de extensão em <see cref="IServiceCollectionExtensions"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class DependencyInjectionIgnoreAttribute : Attribute
{ }