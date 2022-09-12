namespace App.Pages.TurningCanvas;

public partial class TurningCanvasDrawable : IDrawable
{
    private const double TargetFps = 60;
    private const double IntervalInMs = 1000 / TargetFps;

    private readonly System.Timers.Timer rotationTimer;
    private float rotationSpeedPerMillisecond = 0.03f;
    private float rotationDegrees = 0.0f;

    private readonly IList<(float x, float y, float rotation)> drawRequests = new List<(float x, float y, float rotation)>();

    public TurningCanvasDrawable()
    {
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
        canvas.DrawLine(dirtyRect.Left, dirtyRect.Top, dirtyRect.Right, dirtyRect.Bottom);

        foreach (var drawRequest in drawRequests)
        {
            var centerX = dirtyRect.Center.X;
            var centerY = dirtyRect.Center.Y;
            var rotation = -(drawRequest.rotation * Math.PI / 180);
            var newX = Math.Cos(rotation) * (drawRequest.x - centerX) - Math.Sin(rotation) * (drawRequest.y - centerY) + centerX;
            var newY = Math.Sin(rotation) * (drawRequest.x - centerX) + Math.Cos(rotation) * (drawRequest.y - centerY) + centerY;

            canvas.DrawCircle((float)newX, (float)newY, 5.0f);
        }
    }

    private void OnRotationTimerElapsed(object sender, System.Timers.ElapsedEventArgs args)
    {
        this.rotationDegrees += (float)(rotationSpeedPerMillisecond * IntervalInMs);
        if (rotationDegrees > 360f)
        {
            rotationDegrees = 0;
        }
    }

    public void OnStartInteraction(object sender, TouchEventArgs args)
    {
        var touch = args.Touches[0];
        drawRequests.Add((touch.X, touch.Y, rotationDegrees));
    }

    public void OnDragInteraction(object sender, TouchEventArgs args)
    {
        var touch = args.Touches[0];
        drawRequests.Add((touch.X, touch.Y, rotationDegrees));
    }

    public void OnEndInteraction(object sender, TouchEventArgs args)
    {

    }
}
