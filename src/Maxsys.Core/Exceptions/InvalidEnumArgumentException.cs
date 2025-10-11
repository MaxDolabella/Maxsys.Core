using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Maxsys.Core.Exceptions;

public class InvalidEnumArgumentException<TEnum> : InvalidEnumArgumentException where TEnum : struct, Enum
{
    public InvalidEnumArgumentException(TEnum value, [CallerArgumentExpression(nameof(value))] string argumentName = "")
        : base(argumentName, Convert.ToInt32(value), typeof(TEnum))
    { }
}