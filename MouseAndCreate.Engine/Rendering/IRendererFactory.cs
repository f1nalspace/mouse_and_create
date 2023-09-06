using MouseAndCreate.Types;

namespace MouseAndCreate.Rendering
{
    public interface IRendererFactory
    {
        IRenderer Create(RendererType type, CoordinateSystem coordinateSystem);
    }
}
