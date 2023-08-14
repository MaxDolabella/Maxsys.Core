using System.ComponentModel;

namespace Maxsys.Core;

/// <summary>
/// Contém mensagens utilizadas na aplicação.
/// </summary>
public static class CommonMessages
{
    #region DATABASE

    ///<summary>Item não encontrado.</summary>
    [Description("Item não encontrado.")]
    public static string ITEM_NOT_FOUND => nameof(ITEM_NOT_FOUND);

    ///<summary>Ocorreu um erro ao adicionar item.</summary>
    [Description("Ocorreu um erro ao adicionar item.")]
    public static string ERROR_ADDING_ITEM => nameof(ERROR_ADDING_ITEM);

    ///<summary>Ocorreu um erro ao atualizar item.</summary>
    [Description("Ocorreu um erro ao atualizar item.")]
    public static string ERROR_UPDATING_ITEM => nameof(ERROR_UPDATING_ITEM);

    ///<summary>Ocorreu um erro ao deletar item.</summary>
    [Description("Ocorreu um erro ao deletar item.")]
    public static string ERROR_DELETING_ITEM => nameof(ERROR_DELETING_ITEM);

    ///<summary>Ocorreu um erro ao salvar.</summary>
    [Description("Ocorreu um erro ao salvar.")]
    public static string ERROR_SAVE => nameof(ERROR_SAVE);

    #endregion DATABASE

    #region REQUIRED

    ///<summary>Campo 'Id' é obrigatório.</summary>
    [Description("Campo 'Id' é obrigatório.")]
    public static string WARNING_FIELD_REQUIRED_ID => nameof(WARNING_FIELD_REQUIRED_ID);

    ///<summary>Campo 'Nome' é obrigatório.</summary>
    [Description("Campo 'Nome' é obrigatório.")]
    public static string WARNING_FIELD_REQUIRED_NAME => nameof(WARNING_FIELD_REQUIRED_NAME);

    ///<summary>Campo 'Descrição' é obrigatório.</summary>
    [Description("Campo 'Descrição' é obrigatório.")]
    public static string WARNING_FIELD_REQUIRED_DESCRIPTION => nameof(WARNING_FIELD_REQUIRED_DESCRIPTION);

    ///<summary>Campo 'Código' é obrigatório.</summary>
    [Description("Campo 'Código' é obrigatório.")]
    public static string WARNING_FIELD_REQUIRED_CODE => nameof(WARNING_FIELD_REQUIRED_CODE);

    ///<summary>Campo 'E-Mail' é obrigatório.</summary>
    [Description("Campo 'E-Mail' é obrigatório.")]
    public static string WARNING_FIELD_REQUIRED_EMAIL => nameof(WARNING_FIELD_REQUIRED_EMAIL);

    ///<summary>Campo 'Número' é obrigatório.</summary>
    [Description("Campo 'Número' é obrigatório.")]
    public static string WARNING_FIELD_REQUIRED_NUMBER => nameof(WARNING_FIELD_REQUIRED_NUMBER);

    ///<summary>Campo 'Data' é obrigatório.</summary>
    [Description("Campo 'Data' é obrigatório.")]
    public static string WARNING_FIELD_REQUIRED_DATE => nameof(WARNING_FIELD_REQUIRED_DATE);

    ///<summary>Campo Status é obrigatório.</summary>
    [Description("Campo 'Status' é obrigatório.")]
    public static string WARNING_FIELD_REQUIRED_STATUS => nameof(WARNING_FIELD_REQUIRED_STATUS);

    ///<summary>Campo User é obrigatório.</summary>
    [Description("Campo 'User' é obrigatório.")]
    public static string WARNING_FIELD_REQUIRED_USER => nameof(WARNING_FIELD_REQUIRED_USER);

    ///<summary>Campo Type é obrigatório.</summary>
    [Description("Campo 'Type' é obrigatório.")]
    public static string WARNING_FIELD_REQUIRED_TYPE => nameof(WARNING_FIELD_REQUIRED_TYPE);

    ///<summary>Campo 'Value' é obrigatório.</summary>
    [Description("Campo 'Value' é obrigatório.")]
    public static string WARNING_FIELD_REQUIRED_VALUE => nameof(WARNING_FIELD_REQUIRED_VALUE);

    #endregion REQUIRED

    #region INVALID

    ///<summary>Campo 'Id' é inválido.</summary>
    [Description("Campo 'Id' é inválido.")]
    public static string WARNING_FIELD_INVALID_ID => nameof(WARNING_FIELD_INVALID_ID);

    ///<summary>Campo 'Nome' é inválido.</summary>
    [Description("Campo 'Nome' é inválido.")]
    public static string WARNING_FIELD_INVALID_NAME => nameof(WARNING_FIELD_INVALID_NAME);

    ///<summary>Campo 'Descrição' é inválido.</summary>
    [Description("Campo 'Descrição' é inválido.")]
    public static string WARNING_FIELD_INVALID_DESCRIPTION => nameof(WARNING_FIELD_INVALID_DESCRIPTION);

    ///<summary>Campo 'Código' é inválido.</summary>
    [Description("Campo 'Código' é inválido.")]
    public static string WARNING_FIELD_INVALID_CODE => nameof(WARNING_FIELD_INVALID_CODE);

    ///<summary>Campo 'E-Mail' é inválido.</summary>
    [Description("Campo 'E-Mail' é inválido.")]
    public static string WARNING_FIELD_INVALID_EMAIL => nameof(WARNING_FIELD_INVALID_EMAIL);

    ///<summary>Campo 'Número' é inválido.</summary>
    [Description("Campo 'Número' é inválido.")]
    public static string WARNING_FIELD_INVALID_NUMBER => nameof(WARNING_FIELD_INVALID_NUMBER);

    ///<summary>Campo 'Data' é inválido.</summary>
    [Description("Campo 'Data' é inválido.")]
    public static string WARNING_FIELD_INVALID_DATE => nameof(WARNING_FIELD_INVALID_DATE);

    ///<summary>Campo Status é inválido.</summary>
    [Description("Campo 'Status' é inválido.")]
    public static string WARNING_FIELD_INVALID_STATUS => nameof(WARNING_FIELD_INVALID_STATUS);

    ///<summary>Campo User é inválido.</summary>
    [Description("Campo 'User' é inválido.")]
    public static string WARNING_FIELD_INVALID_USER => nameof(WARNING_FIELD_INVALID_USER);

    ///<summary>Campo Type é inválido.</summary>
    [Description("Campo 'Type' é inválido.")]
    public static string WARNING_FIELD_INVALID_TYPE => nameof(WARNING_FIELD_INVALID_TYPE);

    ///<summary>Campo 'Value' é inválido.</summary>
    [Description("Campo 'Value' é inválido.")]
    public static string WARNING_FIELD_INVALID_VALUE => nameof(WARNING_FIELD_INVALID_VALUE);

    #endregion INVALID
}