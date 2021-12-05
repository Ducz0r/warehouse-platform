using Microsoft.Toolkit.Mvvm.Input;

namespace Warehouse.ClientApp.ViewModels
{
    public interface IHomeFormViewModel
    {
        AsyncRelayCommand UpdateCommand { get; }
        string Name { get; }
        int CurrentQuantity { get; set; }
        string CurrentQuantityText { get; }
        int QuantityIncrease { get; set; }
        bool IsProcessing { get; set; }
        bool IsError { get; set; }
        string Message { get; set; }
    }
}
