using OpenTK.Graphics.OpenGL4;
using System;
using System.Diagnostics;

namespace MouseAndCreate.Rendering.OpenGL
{
    class OpenGLVertexArray : IVertexArray
    {
        public Guid Id { get; }
        public string Name { get; }
        public bool IsDisposed => _disposed;

        private readonly int _va;

        public OpenGLVertexArray(Guid id, string name)
        {
            Id = id;
            Name = name;
            _va = GL.GenVertexArray();
        }

        public void VertexAttribFloat(int index, int location, int size, int stride, int offset)
        {
            GL.VertexAttribPointer(location, size, VertexAttribPointerType.Float, false, stride, offset);
            GL.EnableVertexAttribArray(index);
        }

        public void Bind()
        {
            GL.BindVertexArray(_va);
        }

        public void Unbind()
        {
            GL.BindVertexArray(0);
        }

        public override string ToString() => $"[VertexArray/{Id}] {Name}";

        private bool _disposed = false;

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                GL.DeleteVertexArray(_va);
                _disposed = true;
            }
        }

        ~OpenGLVertexArray()
        {
            if (!_disposed)
                Debug.WriteLine($"[WARNING] VertexArray '{Name}' not disposed!");
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
