using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Microsoft.AspNetCore.Mvc;

/// <summary>
/// Desserializa uma string nula ou em formato json para um objeto.<para/>
/// Caso o tipo do objeto de destino não possua um construtor um vazio, uma exceção será lançada ao tentar converter o json.
/// </summary>
public class FromJsonAttribute : ModelBinderAttribute
{
    public FromJsonAttribute() : base(typeof(JsonModelBinder))
    { }
}