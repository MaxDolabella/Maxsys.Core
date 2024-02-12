using Maxsys.Core.Extensions;

namespace Microsoft.AspNetCore.Mvc.ModelBinding;

public class JsonModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
            throw new ArgumentNullException(nameof(bindingContext));

        var value = bindingContext.ValueProvider.GetValue(bindingContext.FieldName);
        var convertedObject = value.FirstValue.FromJson(bindingContext.ModelType);

        bindingContext.Result = ModelBindingResult.Success(convertedObject);

        return Task.CompletedTask;
    }
}