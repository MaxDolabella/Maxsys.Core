using System;

namespace Maxsys.AppCore.MVP.Interfaces
{
    public interface IView : IDisposable
    {
        event Action ViewLoadedAction;

        //event Action ViewLoading;
        //event Action ViewClosing;
        //event Action ViewClosed;

        /*
        IDialogService DialogService { get; set; }

        void ShowInformation(string message);
        void ShowError(string message);
        void ShowWarning(string message);
        */
    }
}