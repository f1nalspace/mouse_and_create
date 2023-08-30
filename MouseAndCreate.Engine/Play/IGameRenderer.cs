using OpenTK.Mathematics;

namespace MouseAndCreate.Play
{
    public interface IGameRenderer
    {
        void Init();
        void Release();
        void Render(Vector2i windowSize);
    }
}
