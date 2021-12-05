using MediatR;
using Microsoft.Toolkit.Mvvm.Input;
using System.Threading;
using System.Threading.Tasks;
using Warehouse.ClientApp.Common;
using Warehouse.ClientApp.Handlers;
using Warehouse.ClientApp.Handlers.Web.Utils;

namespace Warehouse.ClientApp.ViewModels
{
    public class HomeFormViewModel : ViewModelBase, IHomeFormViewModel
    {
        private readonly IMediator _mediator;
        private readonly ICurrentCredentials _currentCredentials;

        private int _currentQuantity;
        private string _currentQuantityText;
        private int _quantityIncrease;
        private bool _isProcessing;
        private bool _isError;
        private string _message;

        public HomeFormViewModel(IMediator mediator, ICurrentCredentials currentCredentials)
        {
            _mediator = mediator;
            _currentCredentials = currentCredentials;

            _currentQuantityText = "n/a";
        }

        public AsyncRelayCommand UpdateCommand => new(DoUpdate);

        public string Name { get => _currentCredentials.Name; }

        public int CurrentQuantity
        {
            get => _currentQuantity;
            set
            {
                _currentQuantity = value;
                _currentQuantityText = value.ToString();
                InvokePropertyChanged("CurrentQuantity");
                InvokePropertyChanged("CurrentQuantityText");
            }
        }

        public string CurrentQuantityText
        {
            get => _currentQuantityText;
        }

        public int QuantityIncrease
        {
            get => _quantityIncrease;
            set
            {
                _quantityIncrease = value;
                InvokePropertyChanged("QuantityIncrease");
            }
        }

        public bool IsProcessing
        {
            get => _isProcessing;
            set
            {
                _isProcessing = value;
                InvokePropertyChanged("IsProcessing");
            }
        }

        public bool IsError
        {
            get => _isError;
            set
            {
                _isError = value;
                InvokePropertyChanged("IsError");
            }
        }

        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                InvokePropertyChanged("Message");
            }
        }

        private async Task DoUpdate()
        {
            IsError = false;
            IsProcessing = true;

            if (QuantityIncrease <= 0)
            {
                IsError = true;
                Message = "Quantity increase must be > 0.";
            } else
            {
                // Call the handler
                var result = await _mediator.Send(new IncreaseQuantity.Request(QuantityIncrease), CancellationToken.None);

                switch (result.Status)
                {
                    case WebRequestResultStatus.Success:
                        // Update the quantity with new quantity
                        CurrentQuantity = (int)result.Content;
                        Message = $"Quantity increased successfully to {CurrentQuantity}.";
                        break;
                    case WebRequestResultStatus.Failure:
                    case WebRequestResultStatus.ServerUnavailable:
                    default:
                        IsError = true;
                        Message = "Unknown error! Quantity not updated.";
                        break;
                }
            }

            IsProcessing = false;
        }
    }
}
