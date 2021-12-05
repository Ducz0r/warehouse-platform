using MediatR;
using Microsoft.Toolkit.Mvvm.Input;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using Warehouse.ClientApp.Handlers;
using Warehouse.ClientApp.Handlers.Web.Utils;
using Warehouse.ClientApp.Views;

namespace Warehouse.ClientApp.ViewModels
{
    public class LoginFormViewModel : ViewModelBase, ILoginFormViewModel
    {
        private readonly IMediator _mediator;
        private readonly IMainWindow _mainWindow;

        private string _name;
        private SecureString _password;
        private bool _isProcessing;
        private bool _isError;
        private string _errorMessage;

        public LoginFormViewModel(IMediator mediator, IMainWindow mainWindow)
        {
            _mediator = mediator;
            _mainWindow = mainWindow;
        }

        public AsyncRelayCommand LoginCommand => new(DoLogin);

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                InvokePropertyChanged("Name");
            }
        }

        public SecureString Password
        {
            get => _password;
            set
            {
                _password = value;
                InvokePropertyChanged("Password");
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

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                InvokePropertyChanged("ErrorMessage");
            }
        }

        private async Task DoLogin()
        {
            IsError = false;
            IsProcessing = true;

            if (Name == "" || Password == null)
            {
                IsError = true;
                ErrorMessage = "Name and password are required.";
            }
            else
            {
                // Call login handler
                var result = await _mediator.Send(new Login.Request(Name, Password), CancellationToken.None);

                switch (result.Status)
                {
                    case WebRequestResultStatus.Success:
                        // Navigate to home page
                        _mainWindow.SetContent(typeof(IHomeForm));
                        break;
                    case WebRequestResultStatus.Failure:
                        IsError = true;
                        ErrorMessage = "Invalid credentials!";
                        break;
                    case WebRequestResultStatus.ServerUnavailable:
                    default:
                        IsError = true;
                        ErrorMessage = "Unknown error! Possibly, connection to the server cannot be established.";
                        break;
                }
            }

            IsProcessing = false;
        }
    }
}
