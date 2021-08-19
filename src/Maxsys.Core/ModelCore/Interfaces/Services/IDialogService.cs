using System;

namespace Maxsys.ModelCore.Interfaces.Services
{
    /// <summary>
    /// Provides an interface to show common modal dialogs.
    /// Implements <see cref="IDisposable"/>
    /// </summary>
    public interface IDialogService : IDisposable
    {
        /// <summary>
        /// An implementation of a view that will own the modal dialog box.
        /// </summary>
        object Owner { get; set; }

        /// <summary>
        /// Displays an information message box in front of the specified object (<see cref="Owner">Owner</see>)
        /// and with the specified message and title.
        /// </summary>
        /// <param name="message">The text to display in the message box.</param>
        /// <param name="title">The text to display in the title bar of the message box.</param>
        void ShowInfoMessage(string message, string title = null);

        /// <summary>
        /// Displays an error message box in front of the specified object (<see cref="Owner">Owner</see>)
        /// and with the specified message and title.
        /// </summary>
        /// <param name="message">The text to display in the message box.</param>
        /// <param name="title">The text to display in the title bar of the message box.</param>
        void ShowErrorMessage(string message, string title = null);

        /// <summary>
        /// Displays a warning message box in front of the specified object (<see cref="Owner">Owner</see>)
        /// and with the specified message and title.
        /// </summary>
        /// <param name="message">The text to display in the message box.</param>
        /// <param name="title">The text to display in the title bar of the message box.</param>
        void ShowWarningMessage(string message, string title = null);
    }
}