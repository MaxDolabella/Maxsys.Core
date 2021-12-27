using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Maxsys.Core.AppCore
{
    /// <summary>
    /// Base class for notifiable objects.
    /// <para/>
    /// Implements <see cref="INotifyPropertyChanged"/>
    /// </summary>
    /// <inheritdoc/>
    public abstract class NotifiableObject : INotifyPropertyChanged
    {
        /// <summary>
        /// Constant that represents an empty string.
        /// </summary>
        protected const string EMPTY_STRING = "";

        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Invokes <see cref="INotifyPropertyChanged.PropertyChanged"/> <see langword="event"/>.
        /// </summary>
        /// <param name="name"></param>
        protected void OnPropertyChanged([CallerMemberName] string name = EMPTY_STRING)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}