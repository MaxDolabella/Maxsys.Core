using FluentValidation.Results;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DataAnnotations = System.ComponentModel.DataAnnotations;

namespace Maxsys.AppCore
{
    /// <summary>
    /// Base class for ViewModel classes.
    /// Contains attribute annotations validation
    /// <para/>
    /// Implements <see cref="MVVMObject"/>
    /// </summary>
    /// <inheritdoc/>
    public abstract class ViewModelBase : MVVMObject
    {
        #region Validation

        [Browsable(false)]
        public ValidationResult ValidationResult { get; protected set; } = null;

        [Browsable(false)]
        public bool IsValid
        {
            get
            {
                Validate();

                return ValidationResult.IsValid;
            }
        }

        private void Validate()
        {
            var validationResults = new List<DataAnnotations.ValidationResult>();
            DataAnnotations.Validator.TryValidateObject(this, new DataAnnotations.ValidationContext(this), validationResults, true);

            var validationFailures = validationResults.Select(x => new ValidationFailure(EMPTY_STRING, x.ErrorMessage));
            ValidationResult = new ValidationResult(validationFailures);
        }

        #endregion Validation
    }
}