using OpenTK.Mathematics;
using System;
using System.Windows;
using System.Windows.Input;

namespace MouseAndCreate.Editor
{
    class WPFControlInputQueryService : IControlInputQueryService
    {
        private readonly FrameworkElement _element;

        public WPFControlInputQueryService(FrameworkElement element)
        {
            _element = element ?? throw new ArgumentNullException(nameof(element));
        }

        public Vector2 GetMousePosition()
        {
            Point p = Mouse.GetPosition(_element);
            return new Vector2((float)p.X, (float)p.Y);
        }
    }
}
