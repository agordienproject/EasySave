using EasySave.HostBuilders;
using EasySave.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Windows;

namespace EasySave
{
    public partial class App : Application
    {
        private readonly IHost _host;

        private Mutex _instanceMutex = null;

        public App()
        {
            _host = CreateHostBuilder().Build();
        }

        public static IHostBuilder CreateHostBuilder(string[] args = null)
        {
            return Host.CreateDefaultBuilder(args)
                .AddConfiguration()
                .AddServices()
                .AddViewModels()
                .AddViews();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            bool createdNew;
            _instanceMutex = new Mutex(true, @"Global\ControlPanel", out createdNew);
            if (!createdNew)
            {
                _instanceMutex = null;
                Application.Current.Shutdown();
                return;
            }

            _host.Start();

            TCPServerManager.StartServer(8888);

            Thread.CurrentThread.CurrentUICulture = new CultureInfo(EasySave.Properties.Settings.Default.CurrentCulture);

            Window window = _host.Services.GetRequiredService<MainWindow>();
            window.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await _host.StopAsync();
            _host.Dispose();

            if (_instanceMutex != null)
                _instanceMutex.ReleaseMutex();

            Application.Current.Shutdown();
            
            base.OnExit(e);
        }
    }

}
