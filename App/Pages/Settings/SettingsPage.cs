using Microsoft.Extensions.Logging;

namespace App.Pages.Settings;

public partial class SettingsPage : ContentPage
{
    private readonly ILogger logger;
    int count = 0;

    public SettingsPage(ILogger<SettingsPage> logger)
    {
        this.logger = logger;
        this.logger.LogInformation("Creating instance of {Type}", nameof(SettingsPage));

        InitializeComponent();
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


