using System.ComponentModel;

namespace Maxsys.Core;

/// <summary>
/// Contém mensagens mais comuns utilizadas na aplicação.
/// </summary>
public static partial class GenericMessages
{
    // ── Severidades base ───────────────────────────────────────────────────

    public const string SUCCESS = "warnings.common.success";
    public const string ERROR = "warnings.common.error";
    public const string WARNING = "warnings.common.warning";
    public const string INFORMATION = "warnings.common.information";

    // ── Acesso / segurança ─────────────────────────────────────────────────

    public const string UNAUTHORIZED = "warnings.common.unauthorized";

    // ── CRUD / persistência ────────────────────────────────────────────────

    public const string ITEM_NOT_FOUND = "warnings.common.item_not_found";
    public const string ERROR_ADDING = "warnings.common.error_adding";
    public const string ERROR_UPDATING = "warnings.common.error_updating";
    public const string ERROR_DELETING = "warnings.common.error_deleting";
    public const string ERROR_SAVE = "warnings.common.error_save";

    // ── Operações ──────────────────────────────────────────────────────────

    public const string INVALID_OPERATION = "warnings.common.invalid_operation";
    public const string INVALID_OBJECT = "warnings.common.invalid_object";
    public const string INVALID_XML = "warnings.common.invalid_xml";
    public const string INVALID_SCHEMA = "warnings.common.invalid_schema";
    public const string SCHEMA_READING_ERROR = "warnings.common.schema_reading_error";

    // ── Validação de campo ─────────────────────────────────────────────────

    public const string FIELD_REQUIRED = "warnings.common.field_required";
    public const string FIELD_INVALID = "warnings.common.field_invalid";
    public const string FIELD_UNIQUE = "warnings.common.field_unique";
    public const string FIELD_LENGTH = "warnings.common.field_length";
    public const string FIELD_FORMAT = "warnings.common.field_format";
    public const string FIELD_RANGE = "warnings.common.field_range";
    public const string FIELDS_CONFLICT = "warnings.common.fields_conflict";

    // ── Validação de item/objeto ───────────────────────────────────────────

    public const string ITEM_REQUIRED = "warnings.common.item_required";
    public const string ITEM_DUPLICATE = "warnings.common.item_duplicate";
}