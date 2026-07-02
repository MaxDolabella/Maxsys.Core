# Maxsys.Drawing

:mortar_board: Cada lançamento é um novo aprendizado!!

## 17.0.0
* :tada: Versão inicial do pacote — `ImageHelper` extraído do `Maxsys.Core` (versões ≤ 16) para um pacote dedicado;
* :warning: **Windows-only**: depende de `System.Drawing.Common`, suportado apenas em Windows no .NET 10;
* :sparkles: Conversões `ImageToBytes` (extensão de `Image`) e `ImageFromBytes`;
* :sparkles: Gravação de imagem em arquivo JPG (`SaveImageIntoJpgFile`, `SaveByteArrayImageIntoJpgFile`) com variantes assíncronas (`*Async`) — criam o diretório de destino e sobrescrevem arquivo existente;
* :sparkles: Métodos de gravação retornam `ValidationResult` (FluentValidation) em vez de lançar exceção — falhas de IO viram `ValidationFailure`;
