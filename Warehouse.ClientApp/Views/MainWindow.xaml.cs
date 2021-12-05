using System;
using System.Windows;

namespace Warehouse.ClientApp.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMainWindow
    {
        private readonly IServiceProvider _serviceProvider;

        public MainWindow(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            InitializeComponent();
        }

        public void SetContent(Type type)
        {
            Dispatcher.Invoke(() =>
            {
                MainView.Content = _serviceProvider.GetService(type);
            });
        }
    }
}
