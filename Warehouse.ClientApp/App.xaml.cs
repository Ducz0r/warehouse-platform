using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Windows;
using Warehouse.ClientApp.Common;
using Warehouse.ClientApp.Handlers.Web.Utils;
using Warehouse.ClientApp.ViewModels;
using Warehouse.ClientApp.Views;

namespace Warehouse.ClientApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider serviceProvider;

        public App()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            serviceProvider = services.BuildServiceProvider();
        }
        private static void ConfigureServices(ServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddSingleton<ICurrentCredentials, CurrentCredentials>();
            services.AddSingleton<ICustomHttpClient, CustomHttpClient>();

            services.AddSingleton<ILoginFormViewModel, LoginFormViewModel>();
            services.AddSingleton<IHomeFormViewModel, HomeFormViewModel>();

            services.AddSingleton<IMainWindow, MainWindow>();
            services.AddTransient<ILoginForm, LoginForm>();
            services.AddTransient<IHomeForm, HomeForm>();
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = serviceProvider.GetService<IMainWindow>();
            mainWindow.SetContent(typeof(ILoginForm));
            mainWindow.Show();
        }
    }
}
