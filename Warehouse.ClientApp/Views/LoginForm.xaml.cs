using System.Windows;
using System.Windows.Controls;
using Warehouse.ClientApp.ViewModels;

namespace Warehouse.ClientApp.Views
{
    /// <summary>
    /// Interaction logic for LoginForm.xaml
    /// </summary>
    public partial class LoginForm : UserControl, ILoginForm
    {
        public LoginForm(ILoginFormViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
            {
                ((dynamic)DataContext).Password = ((PasswordBox)sender).SecurePassword;
            }
        }
    }
}
