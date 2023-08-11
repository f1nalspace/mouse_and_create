using DevExpress.Mvvm.UI;
using OpenTK.Mathematics;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MouseAndCreate.Editor;

public class SizeEventArgsConverter : IEventArgsConverter
{
    public object Convert(object sender, object args)
    {
        if (args is SizeChangedEventArgs actualArgs)
        {
            return new Vector2i((int)actualArgs.NewSize.Width, (int)actualArgs.NewSize.Width);
        }
        return Vector2i.Zero;
    }
}
