using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using DataAnnotations = System.ComponentModel.DataAnnotations;

namespace Maxsys.AppCore
{
    /// <summary>
    /// Base class for ViewModel classes.
    /// Contains attribute annotations validation
    /// <para/>
    /// Implements <see cref="INotifyPropertyChanged"/>
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Sets the local value of a property
        /// </summary>
        /// <param name="current">Reference to the current local value.</param>
        /// <param name="value">The new local value.</param>
        /// <param name="canChange">Represents a condition to change occurs.</param>
        /// <param name="actionIfNotifyIsCalled">Action executed after (and if) the change occurs.</param>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected void SetProperty<T>(ref T current, T value, bool canChange, Action actionIfNotifyIsCalled, [CallerMemberName] string propertyName = "")
        {
            if (!current.Equals(value) && canChange)
            {
                current = value;

                NotifyPropertyChanged(propertyName);

                actionIfNotifyIsCalled?.Invoke();
            }
        }

        /// <summary>
        /// Sets the local value of a property
        /// </summary>
        /// <param name="current">Reference to the current local value.</param>
        /// <param name="value">The new local value.</param>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected void SetProperty<T>(ref T current, T value, [CallerMemberName] string propertyName = "")
        {
            SetProperty(ref current, value, true, null, propertyName);
        }

        /// <summary>
        /// Sets the local value of a property
        /// </summary>
        /// <param name="current">Reference to the current local value.</param>
        /// <param name="value">The new local value.</param>
        /// <param name="actionIfNotifyIsCalled">Action executed after (and if) the change occurs.</param>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected void SetProperty<T>(
            ref T current,
            T value,
            Action actionIfNotifyIsCalled,
            [CallerMemberName] string propertyName = "")
        {
            SetProperty(ref current, value, true, actionIfNotifyIsCalled, propertyName);
        }

        /// <summary>
        /// Sets the local value of a property
        /// </summary>
        /// <param name="current">Reference to the current local value.</param>
        /// <param name="value">The new local value.</param>
        /// <param name="canChange">Represents a condition to change occurs.</param>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected void SetProperty<T>(ref T current, T value, bool canChange, [CallerMemberName] string propertyName = "")
        {
            SetProperty(ref current, value, canChange, null, propertyName);
        }

        /// <summary>
        /// This method is called by the Set accessor of each property.
        /// The CallerMemberName attribute that is applied to the optional propertyName
        /// parameter causes the property name of the caller to be substituted as an argument.
        /// </summary>
        /// <param name="propertyName"></param>
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion INotifyPropertyChanged

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

            var validationFailures = validationResults.Select(x => new ValidationFailure("ViewModel", x.ErrorMessage));
            ValidationResult = new ValidationResult(validationFailures);
        }

        #endregion Validation
    }
}