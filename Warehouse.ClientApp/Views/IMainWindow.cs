using System;

namespace Warehouse.ClientApp.Views
{
    public interface IMainWindow
    {
        void Show();
        void SetContent(Type type);
    }
}
