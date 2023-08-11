using OpenTK.Mathematics;

namespace MouseAndCreate.Input
{
    public interface IMouseInputManager
    {
        void MouseEnter();
        void MouseLeave();
        void MouseMove(Vector2 position);
        void MouseButtonDown(Vector2 position, MouseButton button);
        void MouseButtonUp(Vector2 position, MouseButton button);
        void MouseWheel(Vector2 position, Vector2 offset);
    }
}
