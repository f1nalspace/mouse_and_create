using OpenTK.Graphics.OpenGL4;
using System;
using System.Diagnostics;

namespace MouseAndCreate.Rendering.OpenGL
{
    class OpenGLBuffer : IBuffer
    {
        public string Name { get; }
        public BufferUsage Usage { get; }

        private int _vbo;
        private int _ebo;

        private static BufferUsageHint GetUsage(BufferUsage usage) => usage switch
        {
            BufferUsage.Dynamic => BufferUsageHint.DynamicDraw,
            BufferUsage.Stream => BufferUsageHint.StreamDraw,
            _ => BufferUsageHint.StaticDraw,
        };

        public OpenGLBuffer(string name, float[] vertices, uint[] indices, BufferUsage usage = BufferUsage.Static)
        {
            Name = name;
            Usage = usage;

            if (vertices is not null && vertices.Length > 0)
            {
                _vbo = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
                GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, GetUsage(usage));
                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            }

            if (indices is not null && indices.Length > 0)
            {
                _ebo = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
                GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, GetUsage(usage));
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            }
        }

        public void UpdateVertices(int offset, int length, float[] data)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.BufferSubData(BufferTarget.ArrayBuffer, offset, length * sizeof(float), data);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        public void UpdateElements(int offset, int length, uint[] data)
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _vbo);
            GL.BufferSubData(BufferTarget.ElementArrayBuffer, offset, length * sizeof(float), data);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }

        public void UseVertices()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
        }

        public void UseElements()
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
        }

        public override string ToString() => Name;

        private bool _disposed = false;

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                GL.DeleteBuffer(_ebo);
                GL.DeleteBuffer(_vbo);
                _disposed = true;
            }
        }

        ~OpenGLBuffer()
        {
            if (!_disposed)
                Debug.WriteLine($"[WARNING] Buffer '{Name}' not disposed!");
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
