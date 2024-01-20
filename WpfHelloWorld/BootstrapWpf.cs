using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WpfHelloWorld.ViewModels;
using WpfHelloWorld.Views;

namespace WpfHelloWorld
{
    internal static class BootstrapWpf
    {
        public static IHost? Setup()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, builder) =>
                {
                })
                .ConfigureServices((context, services) =>
                {
                    ConfigureServices(context.Configuration, services);
                })
                .ConfigureLogging(logging =>
                {
                    //logging.AddDebug();   // Microsoft.Extensions.Logging.Debug, if you wish
                })
                .Build();
        }

        private static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
        {
            services.AddSingleton<MainWindow>();
            services.AddSingleton<AboutAssemblyDataProvider>();

            services.AddTransient<AboutDialog>();
            services.AddTransient<MainWindowViewModel>();
        }
    }
}
