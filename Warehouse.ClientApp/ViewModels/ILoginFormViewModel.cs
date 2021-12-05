using Microsoft.Toolkit.Mvvm.Input;
using System.Security;

namespace Warehouse.ClientApp.ViewModels
{
    public interface ILoginFormViewModel
    {
        AsyncRelayCommand LoginCommand { get; }
        string Name { get; set; }
        SecureString Password { get; set; }
        bool IsProcessing { get; set; }
        bool IsError { get; set; }
        string ErrorMessage { get; set; }
    }
}
