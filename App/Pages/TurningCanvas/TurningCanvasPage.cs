using Microsoft.Extensions.Logging;

namespace App.Pages.TurningCanvas;

public partial class TurningCanvasPage : ContentPage
{
    private readonly ILogger logger;

    private const double FramesPerSec = 60;
    private const double FramesIntervallMs = 1000 / FramesPerSec;

    public TurningCanvasPage() { }

    public TurningCanvasPage(ILogger<TurningCanvasPage> logger)
    {
        InitializeComponent();

        this.logger = logger;
        this.logger.LogInformation("Creating instance of {Type}", nameof(TurningCanvasPage));

        var timer = new System.Timers.Timer(FramesIntervallMs);
        timer.Elapsed += OnUpdateCanvasTimerElapsed;
        timer.AutoReset = true;
        timer.Enabled = true;
        timer.Start();
    }

    private void OnUpdateCanvasTimerElapsed(object sender, System.Timers.ElapsedEventArgs args)
    {
        this.Dispatcher.Dispatch(() => this.turningCanvasGraphicsView.Invalidate());
    }

    private void OnStartInteraction(object sender, TouchEventArgs args)
    {
        this.logger.LogInformation("StartInteraction: {Args}", args.Touches);
        this.Dispatcher.Dispatch(() => this.turningCanvasDrawable.OnStartInteraction(this, args));
    }

    private void OnEndInteraction(object sender, TouchEventArgs args)
    {
        this.logger.LogInformation("EndInteraction: {Args}", args.Touches);
        this.Dispatcher.Dispatch(() => this.turningCanvasDrawable.OnEndInteraction(this, args));
    }
}
