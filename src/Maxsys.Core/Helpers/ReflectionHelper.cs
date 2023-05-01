using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Maxsys.Core.Helpers;

/// <summary>
/// Contém métodos de extensão que se utilizam de reflection.
/// </summary>
public static class ReflectionHelper
{
    /// <summary>
    /// Obtém um dicionário com as interfaces públicas que herdam da interface <typeparamref name="TInterface"/> como Key,
    /// sua implementação como Value. As interfaces e implementações serão obtidas a partir dos assemblies <paramref name="assemblies"/>.
    /// Caso <paramref name="suffix"/> diferente de nulo, somente serão obtidas interfaces e implementações com o sufixo passado como parâmetro.<br/>
    /// ATENÇÃO: Interfaces sem implementações serão ignoradas.
    /// </summary>
    /// <typeparam name="TInterface"></typeparam>
    /// <param name="assemblies">assemblies de onde serão obtidas as intefaces e implementações.</param>
    /// <param name="suffix"></param>
    /// <returns></returns>
    public static IReadOnlyDictionary<Type, Type> GetImplementationDictionary<TInterface>(Assembly[] assemblies, string? suffix = null) where TInterface : class
    {
        return GetImplementationDictionary<TInterface>(assemblies, assemblies, suffix);
    }

    /// <summary>
    /// Obtém um dicionário com as interfaces públicas que herdam da interface <typeparamref name="TInterface"/> como Key,
    /// sua implementação como Value. As interfaces serão obtidas a partir dos assemblies <paramref name="interfaceAssemblies"/> e as implementações são
    /// dos assemblies <paramref name="implementationAssemblies"/>.
    /// Caso <paramref name="suffix"/> diferente de nulo, somente serão obtidas interfaces e implementações com o sufixo passado como parâmetro.<br/>
    /// ATENÇÃO: Interfaces sem implementações serão ignoradas.
    /// </summary>
    /// <typeparam name="TInterface"></typeparam>
    /// <param name="interfaceAssemblies">assemblies de onde serão obtidas as intefaces.</param>
    /// <param name="implementationAssemblies">assemblies de onde serão obtidas as implementações.</param>
    /// <param name="suffix"></param>
    /// <returns></returns>
    public static IReadOnlyDictionary<Type, Type> GetImplementationDictionary<TInterface>(Assembly[] interfaceAssemblies, Assembly[] implementationAssemblies, string? suffix = null) where TInterface : class
    {
        var interfaces = GetInterfaces<TInterface>(interfaceAssemblies, suffix);
        var implementations = GetImplementation<TInterface>(implementationAssemblies, suffix);
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
    /// Obtém as interfaces públicas que herdam da interface <typeparamref name="TInterface"/>
    /// a partir dos <paramref name="assemblies"/> referenciados
    /// e, caso <paramref name="suffix"/> diferente de nulo, que terminam com o sufixo passado como parâmetro.
    /// </summary>
    /// <typeparam name="TInterface"></typeparam>
    /// <param name="assemblies"></param>
    /// <param name="suffix"></param>
    /// <returns></returns>
    public static IReadOnlyList<Type> GetInterfaces<TInterface>(Assembly[] assemblies, string? suffix = null) where TInterface : class
    {
        return assemblies.SelectMany((a) => a.ExportedTypes)
            .Where(t => t.IsInterface)
            .Where(t => t.IsAssignableTo(typeof(TInterface)))
            .Where(t => suffix is null || t.Name.EndsWith(suffix))
            .ToList();
    }

    /// <summary>
    /// Obtém as classes públicas não abtratas que implementam a interface <typeparamref name="TInterface"/>
    /// a partir dos <paramref name="assemblies"/> referenciados
    /// e, caso <paramref name="suffix"/> diferente de nulo, que terminam com o sufixo passado como parâmetro.
    /// </summary>
    /// <typeparam name="TInterface"></typeparam>
    /// <param name="assemblies"></param>
    /// <param name="suffix"></param>
    /// <returns></returns>
    public static IReadOnlyList<Type> GetImplementation<TInterface>(Assembly[] assemblies, string? suffix = null) where TInterface : class
    {
        return assemblies.SelectMany((a) => a.ExportedTypes)
            .Where(t => t.IsClass && !t.IsAbstract)
            .Where(t => t.IsAssignableTo(typeof(TInterface)))
            .Where(t => suffix is null || t.Name.EndsWith(suffix))
            .ToList();
    }
}