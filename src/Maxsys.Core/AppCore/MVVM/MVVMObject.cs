using Maxsys.Core.AppCore;
using System;
using System.Runtime.CompilerServices;

namespace Maxsys.AppCore
{
    /// <summary>
    /// Base class for notifiable objects.
    /// <para/>
    /// Implements <see cref="NotifiableObject"/>
    /// </summary>
    /// <inheritdoc/>
    public abstract class MVVMObject : NotifiableObject
    {
        private static bool AreDifferentValues<T>(ref T currentValue, T newValue)
        {
            return (currentValue == null && newValue != null)
                || (currentValue != null && newValue == null)
                || !currentValue.Equals(newValue);
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
        protected bool SetProperty<T>(
            ref T currentValue,
            T newValue,
            Func<bool> canChange,
            [CallerMemberName] string propertyName = EMPTY_STRING)
        {
            if (canChange?.Invoke() == false)
                return false;

            if (AreDifferentValues(ref currentValue, newValue) == false)
                return false;

            currentValue = newValue;

            OnPropertyChanged(propertyName);

            return true;
        }

        /// <summary>
        /// Sets the local value.
        /// </summary>
        /// <param name="currentValue">Reference to the current local value.</param>
        /// <param name="newValue">The new local value.</param>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected bool SetProperty<T>(
            ref T currentValue,
            T newValue,
            [CallerMemberName] string propertyName = EMPTY_STRING)
        {
            return SetProperty(ref currentValue, newValue, canChange: null, propertyName);
        }
    }
}