# Maxsys.Core.Data

### 13.0.0
* :warning: Atualização de framework (`.NET 9`);
* :warning: Atualização de dependências;

---
### 12.0.0
* :warning: Atualização de dependências;
* `RepositoryBase`: Métodos com `object id` em parametros removidos. Somente `object[] keys` mantidos.

---
### 11.0.0
* :warning: Atualização de dependências;
* :warning: Refatoração de implementações de `IRepository` incluindo adição de métodos que aceitam `Expression` para *projection* e remoção de *constraints*;
* :warning: Removido `BusExtensions`. Usar *Interceptors*;

---
### 10.1.0
* :warning: Projeto totalmente refatorado;

---
### [7.1.0](https://www.nuget.org/packages/Maxsys.Core.Data/7.1.0)
* Atualização de pacotes NUGET (`Maxsys.Core` e `Microsoft.EntityFrameworkCore`).
* Alteração de namespace em `IoCExtensions`.
* `RepositoryBase` refatorado (adequação às alterações em `Maxsys.Core`).
* Adicionado log em `UnitOfWorkBase`.

---
### [7.0.0](https://www.nuget.org/packages/Maxsys.Core.Data/7.0.0)
* Commit Inicial com implementações de `IUnitOfWork` e `IRepository`