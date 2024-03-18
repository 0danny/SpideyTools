using System.Configuration;
using System.Data;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SpideyTools.Core;
using SpideyTools.Core.ViewModels;

namespace SpideyTools
{
    public partial class App : Application
    {
        private static readonly IHost _host = Host
           .CreateDefaultBuilder()
           .ConfigureAppConfiguration(c => { c.SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)); })
           .ConfigureServices((context, services) =>
           {

               services.AddScoped<WindowVM>();
               services.AddSingleton<MainWindow>();
               services.AddSingleton<Memory>();
           }).Build();

        private async void OnStartup(object sender, StartupEventArgs e)
        {
            // Make sure to start the host
            await _host.StartAsync();

            // Retrieve the MainWindow from the service provider
            var mainWindow = _host.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private async void OnExit(object sender, ExitEventArgs e)
        {
            using (_host)
            {
                await _host.StopAsync();
            }
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // Handle exceptions here
        }
    }

}
