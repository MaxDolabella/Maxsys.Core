<img src="src\Maxsys.Core\maxsys-core.png" alt="drawing" width="256" />

# Maxsys Core 

**Maxsys.Core** é uma biblioteca desenvolvida em C# contendo itens básicos para criação de aplicações Maxsys.
O framework de destino utilizado é o `.NET 6.0`.

Esse pacote contém interfaces e classes bases como `IRepositoryBase`, `EntityBase`, `ViewModelBase`, além de classes *helpers* e *extensions* como `GuidGen` para gerar Guids sequenciais e `ValidationResultExtensions` que possui métodos de extensão para a classe `FluentValidation.ValidationResult`.

Essa biblioteca auxilia no desenvolvimento de minhas aplicações `Windows Forms`, `WPF` e `ASP.NET`.

## ⛓ Dependências

- [FluentValidation](https://www.nuget.org/packages/FluentValidation/)
- [System.Drawing.Common](https://www.nuget.org/packages/System.Drawing.Common/)

## 🛠 Release notes

##### [6.1.0](https://www.nuget.org/packages/Maxsys.Core/6.1.0)
- `IServiceBase` e `ServiceBase` removidos!!
- Classe `IReadonlyRepositoryBase` removida.
- `IRepositoryBase` remodelado tendo métodos synchronous em excluídos.
- Correções menores de sintaxe.
- EnumExtensions adicionada.
- Ajustes em GuidGen.

##### [6.0.2](https://www.nuget.org/packages/Maxsys.Core/6.0.2)
- Correções menores.

##### [6.0.1](https://www.nuget.org/packages/Maxsys.Core/6.0.1)
- `ViewModelBase` agora reescrita para implementar `MVVMObject` que por sua vez, implementa `NotifiableObject`. `NotifiableObject` é a implementação de `INotifyPropertyChanged`.
- TargetFramework alterado para `.net6.0`.
- Versionamento alterado para coincidir com o a versão do .net para o TargetFramework.
- Métodos obsoletos removidos.

##### [1.2.0](https://www.nuget.org/packages/Maxsys.Core/1.2.0)
- Interface `IDialogService` totalmente reescrita.
- Refatoração da interface `IServiceBase` e da classe `ServiceBase`:
  - Os métodos `Add()`/`AddAsync()` e `Update()`/`UpdateAsync()` agora permitem a passagem de um `IValidator` como parâmetro. Nos métodos sem `IValidator`, nenhuma validação será realizada.

##### [1.1.4](https://www.nuget.org/packages/Maxsys.Core/1.1.4)
- Adicionada referência para `System.Threading.Tasks.Extensions`.
- Alterações em `IOHelper`:
  - Documentação xml adicionada.
  - Adicionados métodos assíncronos para operações com arquivos (`MoveFileAsync()`, `MoveOrOverwriteFileAsync()`, `CopyFileAsync()` e `DeleteFileAsync()`).
- Alterações em `IEnumerableExtensions`:
  - Adicionados métodos de extensão `ToObservableCollection()` e `ToReadOnlyObservableCollection()`.
- Adicionado método `DateTimeToUID_64Bits()` em `GuidGen`.

##### [1.1.3](https://www.nuget.org/packages/Maxsys.Core/1.1.3)
- Alterações em `ValidationResultExtensions`:
  - `ErrorsToString()` foi descontinuado.
  - Adicionado método `ErrorMessagesAsEnumerable()` para substituir `ErrorsAsEnumerable()` que foi descontinuado.
  - Adicionado sobrecarga em método `ValidationResult.AddFailure()` que agora aceita `Exeception` como parâmetro.
- Alterações em `ImageHelper`:
  - Documentação xml adicionada ao código.
  - `SavePicture()` foi descontinuado.
  - Corrigido bug em `ImageFromBytes()`.
  - Adicionados métodos `SaveByteArrayImageIntoJpgFile()`, `SaveImageIntoJpgFile()`, bem como suas versões `async` para `.net5.0` ou maior.

##### [1.1.2](https://www.nuget.org/packages/Maxsys.Core/1.1.2)
- Ajuste em `IEnumerableExtensions` para compatibilidade com `.net5.0`.

##### [1.1.1](https://www.nuget.org/packages/Maxsys.Core/1.1.1)
- `IReadonlyRepositoryBase` adicionado ao projeto.
- Ajustes em `.csproj`:
     - Mudança de TargetFramework para `.net5.0`.
     - Inserção de tags xml.
- `ViewModelBase` alterada para permitir comparação de valores nulos.

##### [1.0.0](https://www.nuget.org/packages/Maxsys.Core/1.0.0)
- Primeiro lançamento.

## ✒️ Autores

* [@MaxDolabella](https://www.github.com/MaxDolabella)

Aqui uma menção à [Jeremy H. Todd](https://github.com/jhtodd), autor de uma das features usadas nesse projeto (geração de guid sequencial).

## 🧐 Aprendizagem

Através desse projeto, tenho a oportunidade de por em prática parte do conhecimento adquirido. Obviamente, ainda é limitado, mas a intenção é sempre buscar a melhora.

## 🗝 Licença

[![License](https://img.shields.io/apm/l/atomic-design-ui.svg?)](LICENSE)
  
## 📧 Feedback

Quaisquer sugestões ou outro contato, escreva-me em maxsystech@outlook.com.

  