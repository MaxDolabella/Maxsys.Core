namespace Maxsys.AppCore.MVP.Interfaces
{
    public interface IPresenter
    {
        IView View { get; }
    }

    public interface IPresenter<TView> : IPresenter
        where TView : IView
    {
        new TView View { get; }
    }
}