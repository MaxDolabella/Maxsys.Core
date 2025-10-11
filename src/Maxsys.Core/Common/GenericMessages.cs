using System.ComponentModel;

namespace Maxsys.Core;

/// <summary>
/// Contém mensagens mais comuns utilizadas na aplicação.
/// </summary>
public static class GenericMessages
{
    ///<summary>Item não encontrado.</summary>
    [Description("Item não encontrado.")]
    public const string ITEM_NOT_FOUND = nameof(ITEM_NOT_FOUND);

    ///<summary>Ocorreu um erro ao adicionar item.</summary>
    [Description("Ocorreu um erro ao adicionar item.")]
    public const string ERROR_ADDING_ITEM = nameof(ERROR_ADDING_ITEM);

    ///<summary>Ocorreu um erro ao atualizar item.</summary>
    [Description("Ocorreu um erro ao atualizar item.")]
    public const string ERROR_UPDATING_ITEM = nameof(ERROR_UPDATING_ITEM);

    ///<summary>Ocorreu um erro ao deletar item.</summary>
    [Description("Ocorreu um erro ao deletar item.")]
    public const string ERROR_DELETING_ITEM = nameof(ERROR_DELETING_ITEM);

    ///<summary>Ocorreu um erro ao salvar.</summary>
    [Description("Ocorreu um erro ao salvar.")]
    public const string ERROR_SAVE = nameof(ERROR_SAVE);

    ///<summary>Campo obrigatório.</summary>
    [Description("Campo obrigatório.")]
    public const string WARNING_FIELD_REQUIRED = nameof(WARNING_FIELD_REQUIRED);

    ///<summary>Campo inválido.</summary>
    [Description("Campo inválido.")]
    public const string WARNING_FIELD_INVALID = nameof(WARNING_FIELD_INVALID);

    ///<summary>Campo deve ser único.</summary>
    [Description("Campo deve ser único.")]
    public const string WARNING_FIELD_UNIQUE = nameof(WARNING_FIELD_UNIQUE);

    ///<summary>Tamanho do campo é inválido.</summary>
    [Description("Tamanho do campo é inválido.")]
    public const string WARNING_FIELD_LENGTH = nameof(WARNING_FIELD_LENGTH);

    ///<summary>Não autorizado</summary>
    [Description("Não autorizado.")]
    public const string UNAUTHORIZED = nameof(UNAUTHORIZED);

    ///<summary>Erro</summary>
    [Description("Erro.")]
    public const string ERROR = nameof(ERROR);

    ///<summary>Aviso</summary>
    [Description("Aviso.")]
    public const string WARNING = nameof(WARNING);

    ///<summary>Informação</summary>
    [Description("Informação.")]
    public const string INFORMATION = nameof(INFORMATION);

    ///<summary>Operação Inválida</summary>
    [Description("Operação Inválida.")]
    public const string INVALID_OPERATION = nameof(INVALID_OPERATION);

    ///<summary>XML inválido</summary>
    [Description("XML inválido.")]
    public const string INVALID_XML = nameof(INVALID_XML);

    ///<summary>Schema inválido</summary>
    [Description("Schema inválido.")]
    public const string INVALID_SCHEMA = nameof(INVALID_SCHEMA);

    ///<summary>Erro em leitura de Schema</summary>
    [Description("Erro em leitura de Schema.")]
    public const string SCHEMA_READING_ERROR = nameof(SCHEMA_READING_ERROR);
}