using B2BIntegrator.App.Services;
using B2BIntegrator.App.ViewModels;
using B2BIntegrator.App.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Windows;

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
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        var services = new ServiceCollection();

        //----------------------------------------------------------------------------
        //wyjasnienie lukru syntaktycznwego (funkcje lambda oraz metody rozszerzajace

        //    public static IServiceCollection AddLogging(this IServiceCollection services, Action<ILoggingBuilder> configure)
        //{
        // czyli to jest metoda rozszerzajaca IServiceCollection

        //    //najpierw microsoft dorzuca swoje domyslne klasy do kontenera DI
        //    services.AddOptions();
        //    services.TryAdd(ServiceDescriptor.Singleton<ILoggerFactory, LoggerFactory>());
        //    services.TryAdd(ServiceDescriptor.Singleton(typeof(ILogger<>), typeof(Logger<>)));

        //    //tutaj powstaje obiekt za pomocą buildera
        //    // LoggingBuilder to zwykła, prosta klasa wewnątrz .NET
        //    ILoggingBuilder builder = new LoggingBuilder(services);
        //    
        //    // metoda wywoluje funkcje (lub lambde), ktora mu przekazalem - Action<ILoggingBuilder> configure

        //tutaj wywolujemy kolejna metode rozszerzajaca ILoggerBuilder
        //    configure(builder);
        // builder to tylko wrapper, wiec wewnatrz kodu przekazanej przeze mnie metody
        // wykonywane sa jakies dzialania konfiguracyjne na polu services klasu LoggingBuilder
        // bo chyba jedyna funkcja klasy LoggingBuilder jest wlasnie przechowywanie referencji do services
        // po co to? chodzi o podpowiedzi IntelliSense, czyli builder dla logowania bedzie mial podpowiedzi
        // metod, ktore dotyczna konfigurowania logowania itd

        //    return services;
        //}

        _ = services.AddLogging(builder =>
        {
            builder.AddSerilog(dispose: true);
        });

        //----------------------------------------------------------------------------

        //singleton
        _ = services.AddHttpClient<INbpApiService, NbpApiService>(static client =>
        {
            client.BaseAddress = new Uri("http://api.nbp.pl/api/");
            client.Timeout = TimeSpan.FromSeconds(10); // Dobra praktyka: globalny timeout
        });
        _ = services.AddSingleton<IViesApiService, ViesApiService>();

        //new instance with every request
        _ = services.AddTransient<MainViewModel>();
        _ = services.AddTransient<MainWindow>();

        //buil container
        Services = services.BuildServiceProvider();
    }

    //this method runs when app starts
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        Log.Information("Aplikacja B2BIntegrator została uruchomiona.");

        var mainWindow = Services.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        Log.Information("Aplikacja B2BIntegrator jest zamykana.");
        Log.CloseAndFlush();

        base.OnExit(e);
    }
}
