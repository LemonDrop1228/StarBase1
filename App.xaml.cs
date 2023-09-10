using System.Windows;
using Blazorise;
using Blazorise.Bulma;
using Blazorise.Icons.FontAwesome;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StarBase1.HttpMessageHandlers;
using StarBase1.Options;
using StarBase1.Services;

namespace StarBase1;

public partial class App : Application
{
    private readonly IHost host;
    private const string configPath = "appsettings.json";

    public App()
    {
        host = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration((context, builder) =>
            {
                builder.SetBasePath(context.HostingEnvironment.ContentRootPath)
                    .AddJsonFile(configPath, optional: false, reloadOnChange: true);
            })
            .ConfigureServices(WireupServices)
            .Build();
    }

    private static void WireupServices(HostBuilderContext context, IServiceCollection services)
    {
        services.Configure<AdblockOptions>(context.Configuration.GetSection("AdblockConfig"));
        services.AddSingleton<AdblockService>();
        services.AddSingleton<IFileService, FileService>();
        services.AddTransient<AdBlockHttpMessageHandler>();
        services.AddSingleton<MainWindow>();
        services.AddSingleton<IHostWindowService, HostWindowService>();
        services.AddHttpClient("AdBlockHttpClient")
            .AddHttpMessageHandler<AdBlockHttpMessageHandler>();

        services.AddWpfBlazorWebView();

        services.AddBlazorise(options => { options.Immediate = true; })
            .AddBulmaProviders()
            .AddFontAwesomeIcons();

#if DEBUG
        services.AddBlazorWebViewDeveloperTools();
#endif
    }

    private void Application_Startup(object sender, StartupEventArgs e)
    {
        var mainWindow = host.Services.GetService<MainWindow>();
        mainWindow!.Resources.Add("services", host.Services);
        mainWindow!.Closed += (_, _) => {
            ShutItDown();
        };

        mainWindow.Show();
    }

    private void ShutItDown()
    {
        using (host)
        {
            host.StopAsync();
        }

        Current.Shutdown();
    }
}