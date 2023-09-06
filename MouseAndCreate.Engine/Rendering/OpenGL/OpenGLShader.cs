using OpenTK.Graphics.OpenGL4;
using System;
using System.Diagnostics;

namespace MouseAndCreate.Rendering.OpenGL
{
    class OpenGLShader : IShader
    {
        public Guid Id { get; }
        public bool IsDisposed => _disposed;

        private readonly int _programId;

        public OpenGLShader(Guid id, string name)
        {
            Id = id;
            Name = name;
            _programId = GL.CreateProgram();
        }

        public bool Attach(params IShaderSource[] sources)
        {
            if (sources is null || sources.Length == 0)
                return false;

            // Load and compile temporary shaders
            Span<int> shaderIds = stackalloc int[sources.Length];
            int index = 0;
            foreach (IShaderSource source in sources)
            {
                ShaderType type = source.Type switch
                {
                    ShaderSourceType.Vertex => ShaderType.VertexShader,
                    ShaderSourceType.Fragment => ShaderType.FragmentShader,
                    ShaderSourceType.Geometry => ShaderType.GeometryShader,
                    _ => throw new NotSupportedException($"In shader '{Name}', the shader source type '{source.Type}' is not supported!")
                };

                string content = source.GetContent();

                int shaderId = GL.CreateShader(type);
                GL.ShaderSource(shaderId, content);
                GL.CompileShader(shaderId);
                GL.GetShader(shaderId, ShaderParameter.CompileStatus, out int compileStatus);
                if (compileStatus == 0)
                {
                    string info = GL.GetShaderInfoLog(shaderId);
                    Debug.WriteLine($"{type} compilation of '{source}' failed [Status: {compileStatus}]:");
                    Debug.WriteLine(info);

                    GL.DeleteShader(shaderId);
                    shaderId = 0;
                }

                shaderIds[index] = shaderId;

                ++index;
            }

            // Early out when one shader compilation failed
            for (int i = 0; i < shaderIds.Length; ++i)
            {
                if (shaderIds[i] == 0)
                    return false;
            }

            // Attach shaders and link it against the program
            for (int i = 0; i < shaderIds.Length; ++i)
                GL.AttachShader(_programId, shaderIds[i]);

            GL.LinkProgram(_programId);
            GL.GetProgram(_programId, GetProgramParameterName.LinkStatus, out int linkStatus);
            if (linkStatus == 0)
            {
                string info = GL.GetProgramInfoLog(_programId);
                Debug.WriteLine($"Shader linking of '{Name}' failed [Status: {linkStatus}]:");
                Debug.WriteLine(info);
            }

            // Remove shaders
            for (int i = 0; i < shaderIds.Length; ++i)
            {
                GL.DetachShader(_programId, shaderIds[i]);
                GL.DeleteShader(shaderIds[i]);
            }

            return linkStatus != 0;
        }

        public string Name { get; }

        public void Bind()
        {
            GL.UseProgram(_programId);
        }

        public void Unbind()
        {
            GL.UseProgram(0);
        }

        public int GetAttribLocation(string attribName) => GL.GetAttribLocation(_programId, attribName);

        public int GetUniformLocation(string uniformName) => GL.GetUniformLocation(_programId, uniformName);

        public override string ToString() => $"[Shader/{Id}] {Name}";

        private bool _disposed = false;

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                GL.DeleteProgram(_programId);
                _disposed = true;
            }
        }

        ~OpenGLShader()
        {
            if (!_disposed)
                Debug.WriteLine($"[WARNING] Shader '{Name}' not disposed!");
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
