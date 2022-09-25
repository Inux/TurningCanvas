using App.Lib.Settings;
using App.Pages.Settings;
using App.Pages.TurningCanvas;
using Microsoft.Extensions.Logging;
using TinyIoC;

namespace App;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        // Create logging service
        builder.Services.AddLogging(configure =>
        {
            configure.AddDebug()
                .AddFilter("App", LogLevel.Trace)
                .AddFilter("Microsoft", LogLevel.Warning);
        });
        var serviceProvider = builder.Services.BuildServiceProvider();

        // Use TinyIoC because TurningCanvasDrawable cannot use built in dependency injection
        TinyIoCContainer.Current.Register(serviceProvider.GetService<ILoggerFactory>());
        TinyIoCContainer.Current.Register<ISettings>(new SettingsProvider());

        // Create Maui App
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        return builder.Build();
    }
}
