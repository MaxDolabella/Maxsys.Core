using System;

namespace Maxsys.ModelCore.Interfaces.Services;

/// <summary>
/// The type of Message to show.
/// </summary>
public enum MessageType
{
    /// <summary>
    /// Status message.
    /// </summary>
    Status,

    /// <summary>
    /// Information message.
    /// </summary>
    Information,

    /// <summary>
    /// Warning message.
    /// </summary>
    Warning,

    /// <summary>
    /// Error message.
    /// </summary>
    Error
};

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
    /// Displays a message according to MessageType.
    /// </summary>
    /// <param name="messageType">The type of Message to show.</param>
    /// <param name="message">The text to display in the message box.</param>
    /// <param name="title">The text to display in the title bar of the message box.</param>
    void ShowMessage(MessageType messageType, string message, string title = null);
}