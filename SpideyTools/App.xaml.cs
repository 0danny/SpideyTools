using System;
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
        private static readonly IHost _host;

        static App()
        {
            var builder = Host.CreateDefaultBuilder();

            builder.ConfigureAppConfiguration((context, config) =>
            {
                var assemblyLocation = System.AppContext.BaseDirectory;
                if (string.IsNullOrEmpty(assemblyLocation))
                {
                    throw new InvalidOperationException("Could not determine the location of the entry assembly.");
                }

                var basePath = Path.GetDirectoryName(assemblyLocation);
                if (string.IsNullOrEmpty(basePath))
                {
                    throw new InvalidOperationException("Could not determine the directory of the entry assembly.");
                }

                config.SetBasePath(basePath);
            });

            builder.ConfigureServices((context, services) =>
            {
                services.AddScoped<WindowVM>();
                services.AddSingleton<MainWindow>();
                services.AddSingleton<Memory>();
            });

            _host = builder.Build();
        }

        private async void OnStartup(object sender, StartupEventArgs e)
        {
            await _host.StartAsync();

            var mainWindow = _host.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private async void OnExit(object sender, ExitEventArgs e)
        {
            // Stop the host and dispose of it.
            await _host.StopAsync();
            _host.Dispose();
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // Prevent default unhandled exception processing.
            e.Handled = true;
        }
    }
}
