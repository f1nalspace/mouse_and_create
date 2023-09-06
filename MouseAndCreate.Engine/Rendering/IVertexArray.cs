using MouseAndCreate.Types;
using System;

namespace MouseAndCreate.Rendering
{
    interface IVertexArray : IResource, IDisposable
    {
        void VertexAttribFloat(int index, int location, int size, int stride, int offset);
        void Bind();
        void Unbind();
    }
}
