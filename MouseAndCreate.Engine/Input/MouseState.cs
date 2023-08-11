using OpenTK.Mathematics;

namespace MouseAndCreate.Input
{
    public class MouseState
    {
        public Vector2 Position { get; set; }
        public ButtonState Left { get; set; }
        public ButtonState Middle { get; set; }
        public ButtonState Right { get; set; }
        public float WheelDelta { get; set; }
    }
}
