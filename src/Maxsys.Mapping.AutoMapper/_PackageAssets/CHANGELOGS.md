# Maxsys.Mapping.AutoMapper

:mortar_board: Cada lançamento é um novo aprendizado!!

## 17.0.0
* :tada: Versão inicial do pacote;
* :sparkles: `AutoMapperAdapter` implementando `IObjectMapper` (Map de instâncias) e `IQueryProjector` (`ProjectTo` de queryables) — abstrações definidas em `Maxsys.Core.Interfaces.Mapping`;
* :sparkles: Registro via `AddMaxsysAutoMapper<TEntry>()` / `AddMaxsysAutoMapper(assemblies)` / `AddMaxsysAutoMapper(configure, assemblies)` com scan automático de `Profile`s;
* :package: Único pacote do ecossistema Maxsys que referencia `AutoMapper` (fixado em 14.0.0, última versão gratuita). Até a v16, `Maxsys.Core` dependia de AutoMapper diretamente.
