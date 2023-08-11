using MouseAndCreate.Buffers;
using MouseAndCreate.Play;
using MouseAndCreate.Shaders;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.IO;
using System.Reflection;

namespace MouseAndCreate.Rendering.OpenGL
{
    class OpenGLRenderer : IRenderer
    {
        private IShader _quadShaderColor = null;
        private IShader _quadShaderTexture = null;
        private IBuffer _quadBufferColor = null;
        private IBuffer _quadBufferTexture = null;
        private IVertexArray _quadVAColor = null;
        private IVertexArray _quadVATexture = null;

        private IShader _lineShader = null;
        private IBuffer _lineBufferOne = null;
        private IBuffer _lineBufferRect = null;
        private IVertexArray _lineVAOne = null;
        private IVertexArray _lineVARect = null;

        private ITexture _testTexture = null;

        private Vector2 _viewportOrigin = Vector2.Zero;
        private Vector2 _viewportSize = Vector2.Zero;

        public OpenGLRenderer()
        {
        }

        public static Stream GetResourceStream(string name)
        {
            Assembly assembly = typeof(IGame).Assembly;
            var names = assembly.GetManifestResourceNames();
            string resourceName = $"{nameof(MouseAndCreate)}.{name}";
            Stream result = assembly.GetManifestResourceStream(resourceName);
            return result;
        }

        private static IShader LoadShader(string name, params ShaderSource[] sources)
        {
            OpenGLShader result = new OpenGLShader(name);
            result.Attach(sources);
            return result;
        }

        private static IBuffer LoadBuffer(string name, float[] vertices, uint[] indices, BufferUsage usage = BufferUsage.Static)
        {
            OpenGLBuffer result = new OpenGLBuffer(name, vertices, indices, usage);
            return result;
        }

        public ITexture LoadTexture(string name, byte[] data, TextureFormat format, bool flipY = true)
        {
            ITexture result = new OpenGLTexture(name);
            result.Upload(data, format, flipY);
            return result;
        }

        public ITexture LoadTexture(string name, Stream stream, TextureFormat format, bool flipY = true)
        {
            ITexture result = new OpenGLTexture(name);
            result.Upload(stream, format, flipY);
            return result;
        }

        public ITexture LoadTexture(ITextureSource source, TextureFormat format, bool flipY = true)
        {
            if (source is ResourceTextureSource resourceSource)
            {
                Stream stream = resourceSource.GetStream();
                return LoadTexture(source.Name, stream, format, flipY);
            }
            return null;
        }

        public void Init()
        {
            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcColor, BlendingFactor.OneMinusSrcAlpha);

            // Quad texture
            {
                IShader shader = _quadShaderTexture = LoadShader(QuadShaderTexture.Name, QuadShaderTexture.Vertex, QuadShaderTexture.Fragment);
                IBuffer buffer = _quadBufferTexture = LoadBuffer(QuadBufferTexture.Name, QuadBufferTexture.Vertices, QuadBufferTexture.Indices);

                IVertexArray va = _quadVATexture = new OpenGLVertexArray(QuadBufferTexture.Name);
                va.Bind();
                buffer.UseElements();
                buffer.UseVertices();
                int quadVertexAttribLocation = shader.GetAttribLocation("aPosition");
                int quadTexcoordAttribLocation = shader.GetAttribLocation("aTexcoord");
                va.VertexAttribFloat(0, quadVertexAttribLocation, 3, QuadBufferTexture.Stride, 0);
                va.VertexAttribFloat(1, quadTexcoordAttribLocation, 2, QuadBufferTexture.Stride, 3 * sizeof(float));
                va.Unbind();
            }

            // Quad color
            {
                IShader shader = _quadShaderColor = LoadShader(QuadShaderColor.Name, QuadShaderColor.Vertex, QuadShaderColor.Fragment);
                IBuffer buffer = _quadBufferColor = LoadBuffer(QuadBufferColor.Name, QuadBufferColor.Vertices, QuadBufferColor.Indices);

                IVertexArray va = _quadVAColor = new OpenGLVertexArray(QuadBufferColor.Name);
                va.Bind();
                buffer.UseElements();
                buffer.UseVertices();
                int quadVertexAttribLocation = shader.GetAttribLocation("aPosition");
                va.VertexAttribFloat(0, quadVertexAttribLocation, 3, QuadBufferColor.Stride, 0);
                va.Unbind();
            }


            // Line
            {
                _lineShader = LoadShader(LineShader.Name, LineShader.Vertex, LineShader.Fragment, LineShader.Geometry);

                int positionAttribLocation = _lineShader.GetAttribLocation("aPosition");

                float[] verticesSingle = new float[3 * 2];
                _lineBufferOne = LoadBuffer("LineOne", verticesSingle, null, BufferUsage.Stream);

                float[] verticesRect = new float[3 * 4];
                _lineBufferRect = LoadBuffer("LineRect", verticesRect, null, BufferUsage.Stream);

                _lineVAOne = new OpenGLVertexArray("LineOne");
                _lineVAOne.Bind();
                _lineBufferOne.UseVertices();
                _lineVAOne.VertexAttribFloat(0, positionAttribLocation, 3, 3 * sizeof(float), 0);
                _lineVAOne.Unbind();

                _lineVARect = new OpenGLVertexArray("LineRect");
                _lineVARect.Bind();
                _lineBufferRect.UseVertices();
                _lineVARect.VertexAttribFloat(0, positionAttribLocation, 3, 3 * sizeof(float), 0);
                _lineVARect.Unbind();
            }

            
        }

        public void Release()
        {
            _testTexture?.Dispose();

            _lineVARect?.Dispose();
            _lineVAOne?.Dispose();
            _lineBufferRect?.Dispose();
            _lineBufferOne?.Dispose();
            _lineShader?.Dispose();

            _quadVAColor?.Dispose();
            _quadVATexture?.Dispose();
            _quadBufferColor?.Dispose();
            _quadBufferTexture?.Dispose();
            _quadShaderColor?.Dispose();
            _quadShaderTexture?.Dispose();
        }

        public void SetViewport(int x, int y, int width, int height)
        {
            _viewportOrigin = new Vector2(x, y);
            _viewportSize = new Vector2(width, height);
            GL.Viewport(x, y, width, height);
        }

        public void Clear(Color4 clearColor)
        {
            GL.ClearColor(clearColor);
            GL.Clear(ClearBufferMask.ColorBufferBit);
        }

        public void DrawQuad(Matrix4 viewProjection, Vector3 translation, Vector3 scale, Color4 color)
        {
            _quadShaderColor.Bind();

            int colorLocation = _quadShaderColor.GetUniformLocation("uColor");
            GL.Uniform4(colorLocation, color);

            Matrix4 modelMat = Matrix4.CreateScale(scale) * Matrix4.CreateTranslation(translation);

            int viewProjectionLocation = _quadShaderColor.GetUniformLocation("uViewProjectionMat");
            GL.UniformMatrix4(viewProjectionLocation, true, ref viewProjection);

            int modelLocation = _quadShaderColor.GetUniformLocation("uModelMat");
            GL.UniformMatrix4(modelLocation, true, ref modelMat);

            _quadVAColor.Bind();
            GL.DrawElements(PrimitiveType.Triangles, QuadBufferColor.Indices.Length, DrawElementsType.UnsignedInt, 0);
            _quadVAColor.Unbind();

            _quadShaderColor.Unbind();
        }

        public void DrawQuad(Matrix4 viewProjection, Vector3 translation, Vector3 scale, ITexture texture, Color4? color = null, Vector4? uvAdjustment = null)
        {
            texture.Bind();

            _quadShaderTexture.Bind();

            int colorLocation = _quadShaderTexture.GetUniformLocation("uColor");
            GL.Uniform4(colorLocation, color ?? Color4.White);

            int textureLocation = _quadShaderTexture.GetUniformLocation("texture0");
            GL.Uniform1(textureLocation, 0);

            int texcoordAdjustmentLocation = _quadShaderTexture.GetUniformLocation("uTexcoordAdjustment");
            Vector4 texcoordAdjustment = uvAdjustment ?? new Vector4(0, 0, 1, 1);
            GL.Uniform4(texcoordAdjustmentLocation, ref texcoordAdjustment);

            Matrix4 modelMat = Matrix4.CreateScale(scale) * Matrix4.CreateTranslation(translation);

            int viewProjectionLocation = _quadShaderTexture.GetUniformLocation("uViewProjectionMat");
            GL.UniformMatrix4(viewProjectionLocation, true, ref viewProjection);

            int modelLocation = _quadShaderTexture.GetUniformLocation("uModelMat");
            GL.UniformMatrix4(modelLocation, true, ref modelMat);

            _quadVATexture.Bind();
            GL.DrawElements(PrimitiveType.Triangles, QuadBufferTexture.Indices.Length, DrawElementsType.UnsignedInt, 0);
            _quadVATexture.Unbind();

            _quadShaderTexture.Unbind();

            texture.Unbind();
        }

        public void DrawLine(Matrix4 viewProjection, Vector3 p0, Vector3 p1, float thickness, Color4 color)
        {
            float[] vertices = new float[] { p0.X, p0.Y, p0.Z, p1.X, p1.Y, p1.Z };
            _lineBufferOne.UpdateVertices(0, vertices.Length, vertices);

            _lineShader.Bind();

            int thicknessLocation = _lineShader.GetUniformLocation("uThickness");
            GL.Uniform1(thicknessLocation, thickness);

            int viewportLocation = _lineShader.GetUniformLocation("uViewport");
            GL.Uniform2(viewportLocation, _viewportSize);

            int colorLocation = _lineShader.GetUniformLocation("uColor");
            GL.Uniform4(colorLocation, color);

            int viewProjectionLocation = _lineShader.GetUniformLocation("uViewProjectionMat");
            GL.UniformMatrix4(viewProjectionLocation, true, ref viewProjection);

            _lineVAOne.Bind();
            GL.DrawArrays(PrimitiveType.Lines, 0, vertices.Length);
            _lineVAOne.Unbind();

            _lineShader.Unbind();
        }

        public void DrawRectangle(Matrix4 viewProjection, Vector3 translation, Vector3 scale, float thickness, Color4 color)
        {
            Vector3 s = scale * 0.5f;
            Vector3 p0 = translation + new Vector3(-s.X, -s.Y, 0);
            Vector3 p1 = translation + new Vector3(s.X, -s.Y, 0);
            Vector3 p2 = translation + new Vector3(s.X, s.Y, 0);
            Vector3 p3 = translation + new Vector3(-s.X, s.Y, 0);
            DrawLine(viewProjection, p0, p1, thickness, color);
            DrawLine(viewProjection, p1, p2, thickness, color);
            DrawLine(viewProjection, p2, p3, thickness, color);
            DrawLine(viewProjection, p3, p0, thickness, color);
        }

        public void CheckForErrors()
        {
            ErrorCode err = GL.GetError();
            if (err != ErrorCode.NoError)
            {
                throw new Exception($"OpenGL Error: {err}");
            }
        }

        public void Dispose()
        {
        }

        
    }
}

