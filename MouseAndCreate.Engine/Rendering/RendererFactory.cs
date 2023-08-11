using MouseAndCreate.Rendering.OpenGL;
using System;

namespace MouseAndCreate.Rendering
{
    class RendererFactory : IRendererFactory
    {
        public IRenderer Create(RendererType type)
        {
            IRenderer result = type switch
            {
                RendererType.OpenGL => new OpenGLRenderer(),
                _ => throw new NotSupportedException($"The renderer type '{type}' is not supported")
            };
            return result;
        }
    }
}
