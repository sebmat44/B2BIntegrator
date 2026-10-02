using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using B2BIntegrator.App.Views;
using B2BIntegrator.App.Services;
using B2BIntegrator.App.ViewModels;

namespace B2BIntegrator.App;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    //global container
    public IServiceProvider Services { get; }

    public App()
    {
        var services = new ServiceCollection();

        //singleton
        services.AddSingleton<INbpApiService, NbpApiService>();
        services.AddSingleton<IViesApiService, ViesApiService>();

        //new instance with every request
        services.AddTransient<MainViewModel>();
        services.AddTransient<MainWindow>();

        //buil container
        Services = services.BuildServiceProvider();
    }

    //this method runs when app starts
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        //first use of DependencyInjection
        var mainWindow = Services.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }
}
