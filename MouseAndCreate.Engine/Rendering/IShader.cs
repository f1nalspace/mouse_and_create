using System;

namespace MouseAndCreate.Rendering
{
    public interface IShader : IDisposable
    {
        string Name { get; }
        void Bind();
        void Unbind();
        bool Attach(params IShaderSource[] sources);
        int GetAttribLocation(string attribName);
        int GetUniformLocation(string uniformName);
    }
}
