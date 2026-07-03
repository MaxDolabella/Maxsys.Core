using Microsoft.Extensions.DependencyInjection;

namespace Maxsys.Bootstrap;

/// <summary>
/// Extensões de registro do Maxsys.Bootstrap no MVC.
/// </summary>
public static class IoCExtensions
{
    /// <summary>
    /// Registra o assembly do Maxsys.Bootstrap como <i>Application Part</i> do MVC,
    /// habilitando a descoberta dos <b>ViewComponents</b> (<c>&lt;vc:bs-*&gt;</c>).
    /// </summary>
    /// <remarks>
    /// Os <b>TagHelpers</b> (<c>&lt;bs-*&gt;</c>) não dependem deste registro — basta adicionar
    /// <c>@addTagHelper *, Maxsys.Bootstrap</c> no <c>_ViewImports.cshtml</c>.
    /// <code>
    /// // Program.cs
    /// builder.Services.AddControllersWithViews().AddMaxsysBootstrap();
    /// </code>
    /// </remarks>
    public static IMvcBuilder AddMaxsysBootstrap(this IMvcBuilder builder)
    {
        return builder.AddApplicationPart(typeof(IoCExtensions).Assembly);
    }
}
