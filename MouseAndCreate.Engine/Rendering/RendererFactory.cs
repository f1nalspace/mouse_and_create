using MouseAndCreate.Rendering.OpenGL;
using MouseAndCreate.Types;
using System;

namespace MouseAndCreate.Rendering
{
    class RendererFactory : IRendererFactory
    {
        public IRenderer Create(RendererType type, CoordinateSystem coordinateSystem)
        {
            IRenderer result = type switch
            {
                RendererType.OpenGL => new OpenGLRenderer(coordinateSystem),
                _ => throw new NotSupportedException($"The renderer type '{type}' is not supported")
            };
            return result;
        }
    }
}
