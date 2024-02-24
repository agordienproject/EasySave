using EasySave.HostBuilders;
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
            Mutex runOnce = null;

            if (EasySave.Properties.Settings.Default.IsRestarting)
            {
                EasySave.Properties.Settings.Default.IsRestarting = false;
                EasySave.Properties.Settings.Default.Save();
                Thread.Sleep(3000);
                
            }

            try
            {
                runOnce = new Mutex(true, "SOME_MUTEX_NAME");

                if (runOnce.WaitOne(TimeSpan.Zero))
                {
                    _host.Start();

                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(EasySave.Properties.Settings.Default.CurrentCulture);

                    Window window = _host.Services.GetRequiredService<MainWindow>();
                    window.Show();

                    base.OnStartup(e);
                }
            }
            finally
            {
                if (null != runOnce)
                    runOnce.Close();
            }

        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await _host.StopAsync();
            _host.Dispose();

            base.OnExit(e);
        }
    }

}
