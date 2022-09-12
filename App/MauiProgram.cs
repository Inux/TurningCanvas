using App.Pages.Settings;
using App.Pages.TurningCanvas;
using Microsoft.Extensions.Logging;

namespace App;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {

        var builder = MauiApp.CreateBuilder();

        // Create services and register them

        builder.Services.AddLogging(configure =>
        {
            configure.AddDebug()
                .AddFilter("App", LogLevel.Trace)
                .AddFilter("Microsoft", LogLevel.Warning);

            configure.AddSimpleConsole(options => {
                options.IncludeScopes = true;
                options.SingleLine = true;
                options.TimestampFormat = "hh:mm:ss ";
            });
        });


        builder.Services.AddSingleton<SettingsPage>();
        builder.Services.AddSingleton<TurningCanvasPage>();

        builder.Services.BuildServiceProvider();

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
