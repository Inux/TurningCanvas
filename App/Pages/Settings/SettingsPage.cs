using Microsoft.Extensions.Logging;
using App.Lib.Settings;
using TinyIoC;

namespace App.Pages.Settings;

public partial class SettingsPage : ContentPage
{
    private readonly ILogger logger;
    private readonly ISettings settings;
    int count = 0;

    public SettingsPage()
    {
        InitializeComponent();

        var factory = TinyIoCContainer.Current.Resolve<ILoggerFactory>();
        this.logger = factory.CreateLogger<SettingsPage>();
        this.logger.LogInformation("Creating instance of {Type}", nameof(SettingsPage));

        this.settings = TinyIoCContainer.Current.Resolve<ISettings>();
    }

    private void OnCounterClicked(object sender, EventArgs e)
    {
        count++;

        if (count == 1)
            CounterBtn.Text = $"Clicked {count} time";
        else
            CounterBtn.Text = $"Clicked {count} times";

        this.logger.LogInformation("Button text update: {Text}", CounterBtn.Text);

        SemanticScreenReader.Announce(CounterBtn.Text);
    }
}


