using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WestCoastEducationDesktop.Services.Interfaces;
using WestCoastEducationDesktop.Services.RestDb;
using WestCoastEducationDesktop.ViewModels;
using WestCoastEducationDesktop.Views;

namespace WestCoastEducationDesktop;

public partial class App : Application
{
    public static IServiceProvider Services { get; private set; } = null!;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var services = new ServiceCollection();
        ConfigureServices(services);
        Services = services.BuildServiceProvider();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var mainWindowViewModel = Services.GetRequiredService<MainWindowViewModel>();

            desktop.MainWindow = new MainWindow
            {
                DataContext = mainWindowViewModel
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    /// <summary>
    /// Dependency Injection setup (VG-requirement): the HTTP clients for
    /// RestDB are registered against their interfaces, and every
    /// ViewModel receives its dependencies through its constructor
    /// rather than constructing them itself.
    /// </summary>
    private static void ConfigureServices(IServiceCollection services)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
            .Build();

        var restDbSettings = configuration.GetSection("RestDb").Get<RestDbSettings>() ?? new RestDbSettings();
        services.AddSingleton(restDbSettings);

        services.AddHttpClient<ICourseService, RestDbCourseService>(client =>
        {
            client.BaseAddress = new Uri(restDbSettings.BaseUrl);
            client.DefaultRequestHeaders.Add("x-apikey", restDbSettings.ApiKey);
            client.DefaultRequestHeaders.Add("cache-control", "no-cache");
        });

        services.AddHttpClient<IStudentService, RestDbStudentService>(client =>
        {
            client.BaseAddress = new Uri(restDbSettings.BaseUrl);
            client.DefaultRequestHeaders.Add("x-apikey", restDbSettings.ApiKey);
            client.DefaultRequestHeaders.Add("cache-control", "no-cache");
        });

        services.AddHttpClient<ITeacherService, RestDbTeacherService>(client =>
        {
            client.BaseAddress = new Uri(restDbSettings.BaseUrl);
            client.DefaultRequestHeaders.Add("x-apikey", restDbSettings.ApiKey);
            client.DefaultRequestHeaders.Add("cache-control", "no-cache");
        });

        services.AddSingleton<CoursesViewModel>();
        services.AddSingleton<StudentsViewModel>();
        services.AddSingleton<TeachersViewModel>();
        services.AddSingleton<MainWindowViewModel>();
    }
}
