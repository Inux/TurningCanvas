using System.ComponentModel;

namespace App.Lib.Settings;

public interface ISettings
{
    public Property<float> RotationSpeed { get; }
    public Property<Color> DrawingColor { get; }
    public Property<float> DrawingSize { get; }
}
