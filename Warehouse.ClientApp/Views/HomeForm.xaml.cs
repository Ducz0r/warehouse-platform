using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;
using Warehouse.ClientApp.ViewModels;

namespace Warehouse.ClientApp.Views
{
    /// <summary>
    /// Interaction logic for HomeForm.xaml
    /// </summary>
    public partial class HomeForm : UserControl, IHomeForm
    {
        public HomeForm(IHomeFormViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
