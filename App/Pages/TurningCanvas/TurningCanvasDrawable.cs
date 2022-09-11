using Microsoft.Extensions.Logging;

namespace App.Pages.TurningCanvas;

public partial class TurningCanvasDrawable : IDrawable
{
    private const double TargetFps = 60;
    private const double IntervalInMs = (1 / TargetFps) * 1000; 

    private readonly System.Timers.Timer rotationTimer;
    private readonly float rotationSpeedPerMillisecond = 0.1f; 
    private float rotationDegrees = 0.0f;
    
    public TurningCanvasDrawable() {
        rotationTimer = new System.Timers.Timer(IntervalInMs);
        rotationTimer.Elapsed += OnRotationTimerElapsed;
        rotationTimer.AutoReset = true;
        rotationTimer.Start();
    }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        canvas.StrokeColor = Colors.Red;
        canvas.StrokeSize = 1;
        canvas.Rotate(this.rotationDegrees, dirtyRect.Center.X, dirtyRect.Center.Y);
        //canvas.StrokeDashPattern = new float[] { 2, 2 };
        canvas.DrawLine(dirtyRect.Left, dirtyRect.Top, dirtyRect.Right, dirtyRect.Bottom);
    }

    private void OnRotationTimerElapsed(object sender, System.Timers.ElapsedEventArgs args)
    {
        this.rotationDegrees += (float)(rotationSpeedPerMillisecond * IntervalInMs);
        if(rotationDegrees > 360f)
        {
            rotationDegrees = 0;
        }
    }
}
