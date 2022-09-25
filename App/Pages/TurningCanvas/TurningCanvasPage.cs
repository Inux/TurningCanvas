using System.Reflection;
using App.Lib.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using TinyIoC;

namespace App.Pages.TurningCanvas;

public partial class TurningCanvasPage : ContentPage
{
    private readonly ILogger logger;
    private readonly ISettings settings;

    private const double FramesPerSec = 60;
    private const double FramesIntervallMs = 1000 / FramesPerSec;
    private DateTime lastUpdate = DateTime.Now.AddYears(-1);

    public TurningCanvasPage()
    {
        InitializeComponent();

        var factory = TinyIoCContainer.Current.Resolve<ILoggerFactory>();
        this.logger = factory.CreateLogger<TurningCanvasPage>();
        this.settings = TinyIoCContainer.Current.Resolve<ISettings>();

        this.logger.LogInformation("Creating instance of {Type}", nameof(TurningCanvasPage));

        var timer = new System.Timers.Timer(FramesIntervallMs);
        timer.Elapsed += OnUpdateCanvasTimerElapsed;
        timer.AutoReset = true;
        timer.Enabled = true;
        timer.Start();

        this.sliderSpeed.Value = this.settings.RotationSpeed.Value;
        this.sliderSize.Value = this.settings.DrawingSize.Value;
        var colorsType = typeof(Colors);
        var allColorNames = colorsType.GetMembers().Where(mi => mi.MemberType == System.Reflection.MemberTypes.Field).Select(mi => mi.Name).ToList();

        this.pickerColor.ItemsSource = new List<object>();
        foreach (var colorName in allColorNames)
        {
            this.pickerColor.ItemsSource.Add(colorsType.GetMembers().First(mi => mi.Name == colorName));
        }
        this.pickerColor.ItemDisplayBinding = new Binding("Name");
        this.labelColor.Text = $"Color: {this.settings.DrawingColor.Value.ToString()}";
    }

    private void OnUpdateCanvasTimerElapsed(object sender, System.Timers.ElapsedEventArgs args)
    {
        this.Dispatcher.Dispatch(() => this.turningCanvasGraphicsView.Invalidate());
    }

    private void OnStartInteraction(object sender, TouchEventArgs args)
    {
        this.logger.LogInformation("StartInteraction: {Args}, Count: {Count}", args.Touches, args.Touches.Length);
        this.turningCanvasDrawable.OnStartInteraction(this, args);
    }

    private void OnDragInteraction(object sender, TouchEventArgs args)
    {
        var current = DateTime.Now;
        var diff = current - lastUpdate;
        if (diff > TimeSpan.FromMilliseconds(50))
        {
            this.logger.LogInformation("DragInteraction: {Args}, Count: {Count}", args.Touches, args.Touches.Length);
            this.Dispatcher.Dispatch(() => this.turningCanvasDrawable.OnDragInteraction(this, args));
            this.lastUpdate = current;
        }
    }

    private void OnEndInteraction(object sender, TouchEventArgs args)
    {
        this.logger.LogInformation("EndInteraction: {Args}, Count: {Count}", args.Touches, args.Touches.Length);
    }

    private void OnSpeedChanged(object sender, ValueChangedEventArgs args)
    {
        this.logger.LogInformation("Speed value changed to {Value}", args.NewValue);
        this.labelSpeed.Text = $"Speed: {String.Format("{0,0:0.000}", args.NewValue)}";
        this.Dispatcher.Dispatch(() => this.settings.RotationSpeed.Value = Convert.ToSingle(args.NewValue));
    }

    private void OnColorChanged(object sender, EventArgs args)
    {
        var picker = (Picker)sender;
        int selectedIndex = picker.SelectedIndex;

        if (selectedIndex != -1)
        {
            MemberInfo mi = (MemberInfo)picker.ItemsSource[selectedIndex];
            this.settings.DrawingColor.Value = (Color)((FieldInfo)mi).GetValue(null);
            this.labelColor.Text = $"Color {mi.Name}";
        }
    }

    private void OnSizeChanged(object sender, ValueChangedEventArgs args)
    {
        this.logger.LogInformation("Size value changed to {Value}", args.NewValue);
        this.labelSize.Text = $"Size: {String.Format("{0,0:0.000}", args.NewValue)}";
        this.Dispatcher.Dispatch(() => this.settings.DrawingSize.Value = Convert.ToSingle(args.NewValue));
    }

    private void OnSavePressed(object sender, EventArgs args)
    {
        this.logger.LogInformation("Saving Canvas...");
    }
}
