using System.ComponentModel.DataAnnotations;

namespace Maxsys.Core;

/// <summary>
/// Specifies that a field data cannot be the <see langword="default"/> value of it's type.
/// </summary>

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public class NotDefaultAttribute : ValidationAttribute
{
    public const string DefaultErrorMessage = "The {0} field must not have the default value";

    public NotDefaultAttribute() : base(DefaultErrorMessage)
    { }

    public override bool IsValid(object? value)
    {
        //NotDefault doesn't necessarily mean required
        if (value is null)
        {
            return true;
        }

        var type = value.GetType();
        if (type.IsValueType)
        {
            var defaultValue = Activator.CreateInstance(type);
            return !value.Equals(defaultValue);
        }

        // non-null ref type
        return true;
    }
}