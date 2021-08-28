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
        /// Sets the local value of a property depending on a condition (<paramref name="canChange"/>)<br/>
        /// and executes an <see cref="Action"/> after <see cref= "NotifyPropertyChanged" /> is called.
        /// </summary>
        /// <param name="currentValue">Reference to the current local value.</param>
        /// <param name="newValue">The new local value.</param>
        /// <param name="canChange">Represents a condition to occurs the change.<br/>
        /// If result of <paramref name="canChange"/> is <see langword="true"/>, the property can be changed.
        /// Otherwise, the property will not be changed.</param>
        /// <param name="actionAfterNotification">Action that will be executed IF the change occurs
        /// and AFTER <see cref="NotifyPropertyChanged"/> be called.</param>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected void SetProperty<T>(
            ref T currentValue,
            T newValue,
            Func<bool> canChange,
            Action actionAfterNotification,
            [CallerMemberName] string propertyName = "")
        {
            if (canChange?.Invoke() == false) return;

            if ((currentValue == null && newValue != null)
                || (currentValue != null && newValue == null)
                || !currentValue.Equals(newValue))
            {
                currentValue = newValue;

                NotifyPropertyChanged(propertyName);

                actionAfterNotification?.Invoke();
            }
        }

        /// <summary>
        /// Sets the local value of a property.
        /// </summary>
        /// <param name="currentValue">Reference to the current local value.</param>
        /// <param name="newValue">The new local value.</param>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected void SetProperty<T>(ref T currentValue, T newValue, [CallerMemberName] string propertyName = "")
        {
            SetProperty(ref currentValue, newValue, null, null, propertyName);
        }

        /// <summary>
        /// Sets the local value of a property
        /// and executes an <see cref="Action"/> after <see cref="NotifyPropertyChanged"/> is called.
        /// </summary>
        /// <param name="currentValue">Reference to the current local value.</param>
        /// <param name="newValue">The new local value.</param>
        /// <param name="actionAfterNotification">Action that will be executed IF the change occurs
        /// and AFTER <see cref="NotifyPropertyChanged"/> is called.</param>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected void SetProperty<T>(
            ref T currentValue,
            T newValue,
            Action actionAfterNotification,
            [CallerMemberName] string propertyName = "")
        {
            SetProperty(ref currentValue, newValue, null, actionAfterNotification, propertyName);
        }

        /// <summary>
        /// Sets the local value of a property depending on a condition (<paramref name="canChange"/>).
        /// </summary>
        /// <param name="currentValue">Reference to the current local value.</param>
        /// <param name="newValue">The new local value.</param>
        /// <param name="canChange">Represents a condition to occurs the change.<br/>
        /// If result of <paramref name="canChange"/> is <see langword="true"/>, the property can be changed.
        /// Otherwise, the property will not be changed.</param>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected void SetProperty<T>(
            ref T currentValue,
            T newValue,
            Func<bool> canChange,
            [CallerMemberName] string propertyName = "")
        {
            SetProperty(ref currentValue, newValue, canChange, null, propertyName);
        }

        /// <summary>
        /// This method is called by the Set accessor of each property.
        /// The CallerMemberName attribute that is applied to the optional propertyName
        /// parameter causes the property name of the caller to be substituted as an argument.
        /// </summary>
        /// <param name="propertyName">Is the name of the property that changed.</param>
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