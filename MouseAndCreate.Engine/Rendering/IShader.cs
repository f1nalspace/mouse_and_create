using MouseAndCreate.Types;
using System;

namespace MouseAndCreate.Rendering
{
    public interface IShader : IResource, IDisposable
    {
        void Bind();
        void Unbind();
        bool Attach(params IShaderSource[] sources);
        int GetAttribLocation(string attribName);
        int GetUniformLocation(string uniformName);
    }
}
