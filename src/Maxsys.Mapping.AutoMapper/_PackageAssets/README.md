<div align="center">
<img src="logo.png" alt="drawing" width="128" />
<h1>Maxsys Mapping — AutoMapper</h1>
</div>

[![License](https://img.shields.io/github/license/maxdolabella/maxsys.core)](LICENSE)

**Maxsys.Mapping.AutoMapper** é o adaptador **AutoMapper** para as abstrações de mapeamento do `Maxsys.Core` (`IObjectMapper` e `IQueryProjector`).

A partir da v17, `Maxsys.Core` e `Maxsys.Data` **não dependem de AutoMapper**: serviços (`ModelServiceBase`) usam `IObjectMapper` e repositórios (`RepositoryBase`/`JoinRepositoryBase`) usam `IQueryProjector`. Este pacote fornece a implementação dessas interfaces via AutoMapper (`Map` / `ProjectTo`), com **auto-registro** de `Profile`s por scan de assembly.

## :gear: Uso

```csharp
// Registra AutoMapper (scan de Profiles no assembly de IApplicationEntry)
// + IObjectMapper/IQueryProjector:
services.AddMaxsysAutoMapper<IApplicationEntry>();

// Overloads: múltiplos assemblies e configuração adicional
services.AddMaxsysAutoMapper(typeof(IApplicationEntry).Assembly, typeof(IOtherEntry).Assembly);
services.AddMaxsysAutoMapper(cfg => cfg.AllowNullCollections = true, typeof(IApplicationEntry).Assembly);
```

Seus `Profile`s continuam AutoMapper puro — nada muda na forma de mapear.

> :bulb: Não usa AutoMapper? Implemente `IObjectMapper`/`IQueryProjector` você mesmo (ex.: Mapster, projeções manuais) e registre no DI — `Maxsys.Core`/`Maxsys.Data` não sabem a diferença.

## :dart: Target
`.NET 10`

## :link: Dependências

- `Maxsys.Core`
- `AutoMapper` **14.0.0** (última versão gratuita — versões posteriores exigem licença paga)

## :black_nib: Autores
[@MaxDolabella](https://www.github.com/MaxDolabella)

## :old_key: Licença
Este código possui licença MIT e está liberado para uso da maneira que se desejar.
