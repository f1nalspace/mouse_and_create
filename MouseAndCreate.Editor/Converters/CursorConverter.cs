using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Input;

namespace MouseAndCreate.Editor.Converters;

public class CursorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Platform.CursorType cursor)
        {
            return cursor switch
            {
                Platform.CursorType.Arrow => Cursors.Arrow,
                Platform.CursorType.Hand => Cursors.Hand,
                Platform.CursorType.Crosshair => Cursors.Cross,
                Platform.CursorType.HResize => Cursors.SizeWE,
                Platform.CursorType.VResize => Cursors.SizeNS,
                Platform.CursorType.IBeam => Cursors.IBeam,
                Platform.CursorType.Move => Cursors.SizeAll,
                _ => Cursors.Arrow
            };
        }
        return Cursors.Arrow;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Cursor cursor)
        {
            if (cursor == Cursors.Arrow)
                return Platform.CursorType.Arrow;
            else if (cursor == Cursors.Hand)
                return Platform.CursorType.Hand;
            else if (cursor == Cursors.Cross)
                return Platform.CursorType.Crosshair;
            else if (cursor == Cursors.SizeWE)
                return Platform.CursorType.HResize;
            else if (cursor == Cursors.SizeNS)
                return Platform.CursorType.VResize;
            else if (cursor == Cursors.IBeam)
                return Platform.CursorType.IBeam;
            else if (cursor == Cursors.SizeAll)
                return Platform.CursorType.Move;
            else
                return Platform.CursorType.Arrow;
        }
        return Platform.CursorType.Arrow;
    }
}
