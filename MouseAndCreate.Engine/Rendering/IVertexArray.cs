using System;

namespace MouseAndCreate.Rendering
{
    interface IVertexArray : IDisposable
    {
        void VertexAttribFloat(int index, int location, int size, int stride, int offset);
        void Bind();
        void Unbind();
    }
}
