using System.Diagnostics;
using System.Reflection;

namespace Maxsys.Core.Helpers;

/// <summary>
/// Contém métodos de extensão que se utilizam de reflection.
/// </summary>
public static class ReflectionHelper
{
    /// <summary>
    /// Obtém um dicionário com as interfaces públicas que herdam da interface <typeparamref name="TInterface"/> como Key,
    /// sua implementação como Value, a partir dos <paramref name="assemblies"/> referenciados
    /// e, caso <paramref name="suffix"/> diferente de nulo, que terminam com o sufixo passado como parâmetro (interface e implementação).<br/>
    /// ATENÇÃO: Interfaces sem implementações serão ignoradas.
    /// </summary>
    /// <typeparam name="TInterface"></typeparam>
    /// <param name="assemblies"></param>
    /// <param name="suffix"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static IReadOnlyDictionary<Type, Type> GetImplementationDictionary<TInterface>(Assembly[] assemblies, string? suffix = null, Func<Type, bool>? predicate = null) where TInterface : class
    {
        return GetImplementationDictionary<TInterface>(assemblies, assemblies, suffix, predicate);
    }

    /// <summary>
    /// Obtém um dicionário com as interfaces públicas que herdam da interface <typeparamref name="TInterface"/> como Key, suas implementações como Value.<br/>
    /// As interfaces herdadas de <typeparamref name="TInterface"/> são buscados nos assemblies <paramref name="interfaceAssemblies"/>.<br/>
    /// As implementações dessas interfaces são buscadas nos assemblies <paramref name="implementationAssemblies"/>.<br/>
    /// Caso <paramref name="suffix"/> diferente de nulo, somente interfaces e implementações terminadas com esse sufixo serão buscadas.
    /// </summary>
    /// <remarks>ATENÇÃO: Interfaces sem implementações serão ignoradas.</remarks>
    /// <typeparam name="TInterface">O tipo do serviço a ser registrado.</typeparam>
    /// <param name="interfaceAssemblies">os assemblies de onde serão obtidas as interfaces.</param>
    /// <param name="implementationAssemblies">os assemblies de onde serão obtidas as implementações.</param>
    /// <param name="suffix">O sufixo das interfaces e implementações a serem registrados.</param>
    /// <param name="predicate"></param>
    /// <returns>um dicionário de interface e implementação</returns>
    public static IReadOnlyDictionary<Type, Type> GetImplementationDictionary<TInterface>(Assembly[] interfaceAssemblies, Assembly[] implementationAssemblies, string? suffix = null, Func<Type, bool>? predicate = null) where TInterface : class
    {
        var interfaces = GetInterfaces<TInterface>(interfaceAssemblies, suffix, predicate);
        var implementations = GetImplementation<TInterface>(implementationAssemblies, suffix, predicate);
        var interfacesWithoutImplementations = new List<Type>();
        var dictionary = new Dictionary<Type, Type>();
        foreach (var @interface in interfaces)
        {
            var @class = implementations.FirstOrDefault(x => x.IsAssignableTo(@interface));
            if (@class is null)
                interfacesWithoutImplementations.Add(@interface);
            else
                dictionary.Add(@interface, @class);
        }

        foreach (var item in interfacesWithoutImplementations)
            Debug.WriteLine($"Not found implementation for {item.Name}");

        return dictionary;
    }

    /// <summary>
    /// Obtém as interfaces públicas não obsoletas que herdam da interface <typeparamref name="TInterface"/>
    /// a partir dos <paramref name="assemblies"/> referenciados
    /// e, caso <paramref name="suffix"/> diferente de nulo, que terminam com o sufixo passado como parâmetro.
    /// </summary>
    /// <typeparam name="TInterface"></typeparam>
    /// <param name="assemblies"></param>
    /// <param name="suffix"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static IReadOnlyList<Type> GetInterfaces<TInterface>(Assembly[] assemblies, string? suffix = null, Func<Type, bool>? predicate = null) where TInterface : class
    {
        return assemblies.SelectMany((a) => a.ExportedTypes)
            .Where(t => !t.GetCustomAttributes<DependencyInjectionIgnoreAttribute>().Any())
            .Where(t => t.IsInterface)
            .Where(t => t.IsAssignableTo(typeof(TInterface)))
            .Where(t => suffix is null || t.Name.EndsWith(suffix))
            .Where(t => predicate is null || predicate(t))
            .ToList();
    }

    /// <summary>
    /// Obtém as classes públicas não obsoletas e não abtratas que implementam a interface <typeparamref name="TInterface"/>
    /// a partir dos <paramref name="assemblies"/> referenciados
    /// e, caso <paramref name="suffix"/> diferente de nulo, que terminam com o sufixo passado como parâmetro.
    /// </summary>
    /// <typeparam name="TInterface"></typeparam>
    /// <param name="assemblies"></param>
    /// <param name="suffix"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static IReadOnlyList<Type> GetImplementation<TInterface>(Assembly[] assemblies, string? suffix = null, Func<Type, bool>? predicate = null) where TInterface : class
    {
        return assemblies.SelectMany((a) => a.ExportedTypes)
            .Where(t => !t.GetCustomAttributes<DependencyInjectionIgnoreAttribute>().Any())
            .Where(t => t.IsClass && !t.IsAbstract)
            .Where(t => t.IsAssignableTo(typeof(TInterface)))
            .Where(t => suffix is null || t.Name.EndsWith(suffix))
            .Where(t => predicate is null || predicate(t))
            .ToList();
    }
}