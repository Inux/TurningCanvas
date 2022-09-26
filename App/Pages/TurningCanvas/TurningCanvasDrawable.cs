using TinyIoC;
using App.Lib.Settings;
using Microsoft.Extensions.Logging;
using System.ComponentModel;

namespace App.Pages.TurningCanvas;

public partial class TurningCanvasDrawable : IDrawable
{
    private readonly ILogger logger;
    private readonly ISettings settings;

    private const double TargetFps = 60;
    private const double IntervalInMs = 1000 / TargetFps;

    private readonly System.Timers.Timer rotationTimer;
    private float rotationSpeedPerMillisecond = 0.03f;
    private float rotationDegrees = 0.0f;

    private float drawingSize = 3.0f;

    private readonly IList<(float x, float y, float size, Color color, float rotation)> drawRequests = new List<(float x, float y, float size, Color color, float rotation)>();

    public TurningCanvasDrawable()
    {
        var factory = TinyIoCContainer.Current.Resolve<ILoggerFactory>();
        this.logger = factory.CreateLogger<TurningCanvasDrawable>();
        this.settings = TinyIoCContainer.Current.Resolve<ISettings>();

        this.rotationTimer = new System.Timers.Timer(IntervalInMs);
        this.rotationTimer.Elapsed += OnRotationTimerElapsed;
        this.rotationTimer.AutoReset = true;
        this.rotationTimer.Start();

        this.rotationSpeedPerMillisecond = this.settings.RotationSpeed.Value;
        this.settings.RotationSpeed.PropertyChanged +=
            (_, _) => this.rotationSpeedPerMillisecond = this.settings.RotationSpeed.Value;

        this.drawingSize = this.settings.DrawingSize.Value;
        this.settings.DrawingSize.PropertyChanged +=
            (_, _) => this.drawingSize = this.settings.DrawingSize.Value;
    }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        canvas.StrokeSize = 1;
        canvas.Rotate(this.rotationDegrees, dirtyRect.Center.X, dirtyRect.Center.Y);
        canvas.StrokeColor = Colors.Black;
        canvas.DrawLine(dirtyRect.Left, dirtyRect.Top, dirtyRect.Right, dirtyRect.Bottom);

        foreach (var drawRequest in drawRequests)
        {
            var centerX = dirtyRect.Center.X;
            var centerY = dirtyRect.Center.Y;
            var rotation = -(drawRequest.rotation * Math.PI / 180);
            var newX = Math.Cos(rotation) * (drawRequest.x - centerX) - Math.Sin(rotation) * (drawRequest.y - centerY) + centerX;
            var newY = Math.Sin(rotation) * (drawRequest.x - centerX) + Math.Cos(rotation) * (drawRequest.y - centerY) + centerY;

            canvas.FillColor = drawRequest.color;
            canvas.FillCircle((float)newX, (float)newY, drawRequest.size);
        }
    }

    private void OnRotationTimerElapsed(object sender, System.Timers.ElapsedEventArgs args)
    {
        this.rotationDegrees += (float)(rotationSpeedPerMillisecond * IntervalInMs);
        if (this.rotationDegrees > 360f)
        {
            this.rotationDegrees = 0;
        }
    }

    public void OnStartInteraction(object sender, TouchEventArgs args)
    {
        var touch = args.Touches[0];
        drawRequests.Add((touch.X, touch.Y, this.drawingSize, this.settings.DrawingColor.Value, this.rotationDegrees));
    }

    public void OnDragInteraction(object sender, TouchEventArgs args)
    {
        var touch = args.Touches[0];
        drawRequests.Add((touch.X, touch.Y, this.drawingSize, this.settings.DrawingColor.Value, this.rotationDegrees));
    }

    public void OnEndInteraction(object sender, TouchEventArgs args)
    {
        var touch = args.Touches[0];
        drawRequests.Add((touch.X, touch.Y, this.drawingSize, this.settings.DrawingColor.Value, this.rotationDegrees));
    }
}
