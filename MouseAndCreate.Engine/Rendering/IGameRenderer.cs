using OpenTK.Mathematics;

namespace MouseAndCreate.Rendering
{
    public interface IGameRenderer
    {
        void Init();
        void Release();
        void Render(Vector2i windowSize);
    }
}
