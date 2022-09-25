using System.ComponentModel;

namespace App.Lib.Settings;

public class SettingsProvider : ISettings
{
    public SettingsProvider()
    {
    }

    public Property<float> RotationSpeed { get; private set; } = new Property<float>(0.02f);
    public Property<Color> DrawingColor { get; private set; } = new Property<Color>(Colors.Black);
    public Property<float> DrawingSize { get; private set; } = new Property<float>(6.0f);
}
