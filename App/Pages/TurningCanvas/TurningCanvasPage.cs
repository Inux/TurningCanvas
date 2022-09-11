using Microsoft.Extensions.Logging;

namespace App.Pages.TurningCanvas;

public partial class TurningCanvasPage : ContentPage
{
	private const double FramesPerSec = 60;
	private const double FramesIntervallMs = 1000 / FramesPerSec; 

	public TurningCanvasPage(ILogger<TurningCanvasPage> logger)
	{
		InitializeComponent();

		logger.LogInformation("Creating instance of {Type}", nameof(TurningCanvasPage));

		var timer = new System.Timers.Timer(FramesIntervallMs);
		timer.Elapsed += OnUpdateCanvasTimerElapsed;
		timer.AutoReset = true;
		timer.Enabled = true;
		timer.Start();
	}

	private void OnUpdateCanvasTimerElapsed(object sender, System.Timers.ElapsedEventArgs args)
	{
		this.Dispatcher.Dispatch(() => this.turningCanvasDrawable.Invalidate());
    }
}
