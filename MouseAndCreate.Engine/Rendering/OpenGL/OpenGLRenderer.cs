using MouseAndCreate.Buffers;
using MouseAndCreate.Fonts;
using MouseAndCreate.Graphics;
using MouseAndCreate.Play;
using MouseAndCreate.Shaders;
using MouseAndCreate.Types;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

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

        private Vector2 _viewportOrigin = Vector2.Zero;
        private Vector2 _viewportSize = Vector2.Zero;
        private Vector2 _lineWidthRange = Vector2.One;

        private readonly CoordinateSystem _coordinateSystem;

        private readonly List<IResource> _allResources = new List<IResource>();
        private readonly List<IResource> _internalResources = new List<IResource>();

        public OpenGLRenderer(CoordinateSystem coordinateSystem)
        {
            _coordinateSystem = coordinateSystem;
        }

        public static Stream GetResourceStream(string name)
        {
            Assembly assembly = typeof(IGame).Assembly;
            var names = assembly.GetManifestResourceNames();
            string resourceName = $"{nameof(MouseAndCreate)}.{name}";
            Stream result = assembly.GetManifestResourceStream(resourceName);
            return result;
        }

        private IShader LoadShader(Guid id, string name, params IShaderSource[] sources)
        {
            OpenGLShader result = new OpenGLShader(id, name);
            result.Attach(sources);
            _allResources.Add(result);
            return result;
        }

        private IBuffer LoadBuffer(Guid id, string name, float[] vertices, uint[] indices, BufferUsage usage = BufferUsage.Static)
        {
            OpenGLBuffer result = new OpenGLBuffer(id, name, vertices, indices, usage);
            _allResources.Add(result);
            return result;
        }

        public ITexture LoadTexture(Guid id, string name, TextureData data)
        {
            OpenGLTexture result = new OpenGLTexture(id, name, data.Width, data.Height, data.Format, data.Data);
            _allResources.Add(result);
            return result;
        }

        public ITexture LoadTexture(Guid id, ITextureSource source, TextureFormat format, ImageFlags flags)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            TextureData data = source.Load(format, flags);
            return LoadTexture(id, source.Name, data);
        }

        public IFontTexture LoadFont(Guid id, string name, IFont font, TextureData textureData)
        {
            OpenGLFontTexture result = new OpenGLFontTexture(id, name, textureData.Width, textureData.Height, textureData.Format, textureData.Data, font);
            _allResources.Add(result);
            return result;
        }

        public void Init()
        {
            string version = GL.GetString(StringName.Version);
            string vendor = GL.GetString(StringName.Vendor);
            string renderer = GL.GetString(StringName.Renderer);
            string glsl = GL.GetString(StringName.ShadingLanguageVersion);

            GL.GetFloat(GetPName.LineWidthRange, out Vector2 lineWidthRange);
            _lineWidthRange = lineWidthRange;

            GL.GetInteger(GetPName.NumExtensions, out int numExtensions);

            List<string> extensions = new List<string>(2048);
            for (int i = 0; i < numExtensions; ++i)
            {
                string ext = GL.GetString(StringNameIndexed.Extensions, i);
                extensions.Add(ext);
            }
            extensions.Sort();

            CheckForErrors();

            Debug.WriteLine("OpenGL infos:");
            Debug.WriteLine($"\tVersion: {version}");
            Debug.WriteLine($"\tVendor: {vendor}");
            Debug.WriteLine($"\tRenderer: {renderer}");
            Debug.WriteLine($"\tGLSL Version: {glsl}");
            Debug.WriteLine($"\tLine Width Range: {lineWidthRange}");
            Debug.WriteLine($"\t{extensions.Count} Extensions:");
            foreach (var ext in extensions)
                Debug.WriteLine($"\t\t{ext}");

            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcColor, BlendingFactor.OneMinusSrcAlpha);

            GL.Enable(EnableCap.LineSmooth);

            CheckForErrors();

            // Quad texture
            {
                IShader shader = _quadShaderTexture = LoadShader(new Guid("600B19F4-572C-4297-B195-5BBA00944DCA"), QuadShaderTexture.Name, QuadShaderTexture.Vertex, QuadShaderTexture.Fragment);
                _internalResources.Add(shader);

                IBuffer buffer = _quadBufferTexture = LoadBuffer(new Guid("666C93D1-62B2-4332-8325-CBCBEB7A137C"), QuadBufferTexture.Name, QuadBufferTexture.Vertices, QuadBufferTexture.Indices);
                _internalResources.Add(buffer);

                IVertexArray va = _quadVATexture = new OpenGLVertexArray(new Guid("0E90FBF5-9ADB-47B8-BC24-B55F0A29565D"), QuadBufferTexture.Name);
                va.Bind();
                buffer.UseElements();
                buffer.UseVertices();
                int quadVertexAttribLocation = shader.GetAttribLocation("aPosition");
                int quadTexcoordAttribLocation = shader.GetAttribLocation("aTexcoord");
                va.VertexAttribFloat(0, quadVertexAttribLocation, 3, QuadBufferTexture.Stride, 0);
                va.VertexAttribFloat(1, quadTexcoordAttribLocation, 2, QuadBufferTexture.Stride, 3 * sizeof(float));
                va.Unbind();
                _internalResources.Add(va);

                CheckForErrors();
            }

            // Quad color
            {
                IShader shader = _quadShaderColor = LoadShader(new Guid("1E25C87A-9689-43FF-8763-E5CD809DB78F"), QuadShaderColor.Name, QuadShaderColor.Vertex, QuadShaderColor.Fragment);
                _internalResources.Add(shader);

                IBuffer buffer = _quadBufferColor = LoadBuffer(new Guid("20568D8F-9EAA-4560-9D2F-1462C20E9E8A"), QuadBufferColor.Name, QuadBufferColor.Vertices, QuadBufferColor.Indices);
                _internalResources.Add(buffer);

                IVertexArray va = _quadVAColor = new OpenGLVertexArray(new Guid("09811424-4B88-48D4-B596-396177B41314"), QuadBufferColor.Name);
                va.Bind();
                buffer.UseElements();
                buffer.UseVertices();
                int quadVertexAttribLocation = shader.GetAttribLocation("aPosition");
                va.VertexAttribFloat(0, quadVertexAttribLocation, 3, QuadBufferColor.Stride, 0);
                va.Unbind();
                _internalResources.Add(va);

                CheckForErrors();
            }


            // Line
            {
                _lineShader = LoadShader(new Guid("B08BAF79-A431-438D-BA8C-252B184E1A90"), LineShader.Name, LineShader.Vertex, LineShader.Fragment);
                _internalResources.Add(_lineShader);

                int positionAttribLocation = _lineShader.GetAttribLocation("aPosition");

                float[] verticesSingle = new float[3 * 2];
                _lineBufferOne = LoadBuffer(new Guid("33FBFD53-F180-4896-B730-0814508E62B3"), "LineOne", verticesSingle, null, BufferUsage.Stream);
                _internalResources.Add(_lineBufferOne);

                float[] verticesRect = new float[3 * 4];
                _lineBufferRect = LoadBuffer(new Guid("5D83ADB3-1AE7-4060-9B6F-6AA80D9BA2A3"), "LineRect", verticesRect, null, BufferUsage.Stream);
                _internalResources.Add(_lineBufferRect);

                _lineVAOne = new OpenGLVertexArray(new Guid("7F172B1F-B4A0-41AE-90D4-9910131EC1E8"), "LineOne");
                _lineVAOne.Bind();
                _lineBufferOne.UseVertices();
                _lineVAOne.VertexAttribFloat(0, positionAttribLocation, 3, 3 * sizeof(float), 0);
                _lineVAOne.Unbind();
                _internalResources.Add(_lineVAOne);

                _lineVARect = new OpenGLVertexArray(new Guid("B89BF248-C5AB-4D5E-8B20-FCD51B0BBF9C"), "LineRect");
                _lineVARect.Bind();
                _lineBufferRect.UseVertices();
                _lineVARect.VertexAttribFloat(0, positionAttribLocation, 3, 3 * sizeof(float), 0);
                _lineVARect.Unbind();
                _internalResources.Add(_lineVARect);

                CheckForErrors();
            }
        }

        public void Release()
        {
            List<IResource> missingResources = _allResources.Except(_internalResources).ToList();
            if (missingResources.Count > 0)
            {
                InvalidDataException exception = new InvalidDataException($"{missingResources.Count} external resources was not disposed!");
                foreach (IResource missingResource in missingResources)
                    exception.Data.Add(missingResource.Id, missingResource.Name);
            }

            List<IResource> reversedInternalResources = new List<IResource>(_internalResources);
            reversedInternalResources.Reverse();

            foreach (IResource internalResource in reversedInternalResources)
                internalResource.Dispose();
        }

        public void SetViewport(int x, int y, int width, int height)
        {
            _viewportOrigin = new Vector2(x, y);
            _viewportSize = new Vector2(width, height);
            GL.Viewport(x, y, width, height);
            CheckForErrors();
        }

        public void Clear(Color4 clearColor)
        {
            GL.ClearColor(clearColor);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            CheckForErrors();
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

            CheckForErrors();
        }

        public void DrawQuad(Matrix4 viewProjection, Vector3 translation, Vector3 scale, ITexture texture, Color4? color = null, Vector4? uvAdjustment = null)
        {
            if (texture is null)
                return;

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

            CheckForErrors();
        }

        public void DrawLine(Matrix4 viewProjection, Vector3 p0, Vector3 p1, float thickness, Color4 color, LinePattern pattern = LinePattern.Solid, float stippleFactor = 2.0f)
        {
            var actualThickness = Math.Max(Math.Min(_lineWidthRange.Y, thickness), _lineWidthRange.X);
            GL.LineWidth(actualThickness);

            float[] vertices = new float[] { p0.X, p0.Y, p0.Z, p1.X, p1.Y, p1.Z };
            _lineBufferOne.UpdateVertices(0, vertices.Length, vertices);

            CheckForErrors();

            _lineShader.Bind();

            int viewportLocation = _lineShader.GetUniformLocation("uViewport");
            GL.Uniform2(viewportLocation, _viewportSize);

            int colorLocation = _lineShader.GetUniformLocation("uColor");
            GL.Uniform4(colorLocation, color);

            int patternLocation = _lineShader.GetUniformLocation("uPattern");
            GL.Uniform1(patternLocation, (uint)pattern);

            int factorLocation = _lineShader.GetUniformLocation("uFactor");
            GL.Uniform1(factorLocation, stippleFactor);

            int viewProjectionLocation = _lineShader.GetUniformLocation("uViewProjectionMat");
            GL.UniformMatrix4(viewProjectionLocation, true, ref viewProjection);

            _lineVAOne.Bind();
            GL.DrawArrays(PrimitiveType.Lines, 0, vertices.Length);
            _lineVAOne.Unbind();

            _lineShader.Unbind();

            GL.LineWidth(_lineWidthRange.X);

            CheckForErrors();
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

        public void DrawString(Matrix4 viewProjection, Vector3 translation, string text, IFontTexture fontTexture, float scale = 1.0f, Color4? color = null)
        {
            if (text is null || text.Length == 0)
                return;
            if (fontTexture is null)
                return;

            Vector2 boxSize = MeasureString(text, fontTexture, scale);

            Vector4 up4 = Vector4.UnitY * viewProjection;
            Vector2 up = new Vector2(0, Math.Sign(up4.Y));

            Color4 actualColor = color ?? Color4.White;

            Vector2 offset = Vector2.Zero;
            bool firstGlyphOnLine = true;

            float additionalSpacingPerChar = fontTexture.Spacing * scale;

            float lineAdvance = fontTexture.LineAdvance * scale;

            void DrawGlyph(Glyph glyph, int repeat = 1)
            {
                Vector3 glyphAdvance = glyph.Advance;

                float leftSideBearing = glyphAdvance.X * scale;
                float width = glyphAdvance.Y * scale;
                float rightSideBearing = glyphAdvance.Z * scale;

                Rect4 glyphUV = glyph.UV;

                Vector3 glyphOffset = new Vector3(glyph.Bounds.Offset) * scale;
                Vector3 glyphSize = new Vector3(glyph.Bounds.Size) * scale;

                for (int i = 0; i < repeat; ++i)
                {
                    if (firstGlyphOnLine)
                    {
                        offset.X = Math.Max(leftSideBearing, 0);
                        firstGlyphOnLine = false;
                    }
                    else
                        offset.X += additionalSpacingPerChar + leftSideBearing;

                    Vector3 quadTranslation = translation;
                    quadTranslation += new Vector3(offset); // Move to current position
                    quadTranslation += new Vector3(glyphOffset) * scale; // Adjust by offset
                    quadTranslation += new Vector3(glyphSize) * 0.5f; // Adjust by half the glyph size, so quads are drawn by left/top

                    // When pixel coordinate system, we it down to fit into the text box
                    // When cartesian coordinate system, we it down to the very bottom
                    quadTranslation += new Vector3(0, -up.Y, 0) * new Vector3(0, lineAdvance, 0);

                    quadTranslation += new Vector3(up.X, Math.Max(0, up.Y), 0) * new Vector3(boxSize);

                    DrawRectangle(viewProjection, quadTranslation, glyphSize, 1.0f, Color4.Cyan);

                    DrawQuad(viewProjection, quadTranslation, glyphSize, fontTexture, actualColor, glyphUV.ToVec4());

                    offset.X += width + rightSideBearing;
                }
            }

            foreach (char c in text)
            {
                int codePoint = (int)c;

                if (codePoint == ' ' || codePoint == '\t')
                {
                    if (!fontTexture.Glyphs.TryGetValue(32, out Glyph spaceGlyph))
                        throw new InvalidDataException($"No space glyph in font texture '{fontTexture}' found!");
                    int numSpaces = codePoint == '\t' ? 4 : 1;
                    DrawGlyph(spaceGlyph, numSpaces);
                    continue;
                }

                if (codePoint == '\n')
                {
                    offset.X = 0;
                    offset += -up * new Vector2(0, lineAdvance);
                    firstGlyphOnLine = true;
                    continue;
                }

                if (char.IsWhiteSpace((char)codePoint))
                    continue;

                if (!fontTexture.Glyphs.TryGetValue(codePoint, out Glyph glyph))
                    throw new InvalidDataException($"No glyph for code point '{codePoint}' / char '{(char)codePoint}' in font texture '{fontTexture}' found");

                DrawGlyph(glyph);
            }
        }

        public Vector2 MeasureString(string text, IFont fontTexture, float scale = 1.0f)
        {
            if (text is null || text.Length == 0)
                return Vector2.Zero;
            if (fontTexture is null)
                return Vector2.Zero;

            Vector2 offset = Vector2.Zero;
            float width = 0.0f;
            float height = 0.0f;
            float lineAdvance = fontTexture.LineAdvance * scale;
            float finalLineHeight = lineAdvance;
            bool firstGlyphOnLine = false;

            float additionalSpacingPerChar = fontTexture.Spacing * scale;

            void ProcessGlyph(Glyph glyph, int repeat = 1)
            {
                Vector3 advance = glyph.Advance;

                float leftSideBearing = advance.X * scale;
                float charWidth = advance.Y * scale;
                float rightSideBearing = advance.Z * scale;

                for (int i = 0; i < repeat; ++i) 
                {
                    if (firstGlyphOnLine)
                    {
                        offset.X = Math.Max(leftSideBearing, 0);
                        firstGlyphOnLine = false;
                    }
                    else
                        offset.X += additionalSpacingPerChar + leftSideBearing;

                    offset.X += charWidth;

                    float proposedWidth = offset.X + rightSideBearing;
                    if (proposedWidth > width)
                        width = proposedWidth;

                    offset.X += rightSideBearing;

                    float height = glyph.Bounds.Height * scale;

                    if (height > finalLineHeight)
                        finalLineHeight = height;
                }
            }

            foreach (char c in text)
            {
                int codePoint = (int)c;

                if (codePoint == ' ' || codePoint == '\t')
                {
                    if (!fontTexture.Glyphs.TryGetValue(32, out Glyph spaceGlyph))
                        throw new InvalidDataException($"No space glyph in font texture '{fontTexture}' found!");
                    int numSpaces = codePoint == '\t' ? 4 : 1;
                    ProcessGlyph(spaceGlyph, numSpaces);
                    continue;
                }

                if (codePoint == '\n')
                {
                    finalLineHeight = lineAdvance;
                    firstGlyphOnLine = true;
                    height += lineAdvance;
                    offset.X = 0;
                    offset.Y += lineAdvance;
                    continue;
                }

                if (char.IsWhiteSpace((char)codePoint))
                    continue;

                if (!fontTexture.Glyphs.TryGetValue(codePoint, out Glyph glyph))
                    throw new InvalidDataException($"No glyph for code point '{codePoint}' / char '{(char)codePoint}' in font texture '{fontTexture}' found!");

                ProcessGlyph(glyph);
            }

            Vector2 result = new Vector2(width, height + finalLineHeight);

            return result;
        }

        public void CheckForErrors()
        {
            StringBuilder s = new StringBuilder();
            ErrorCode err;

            while ((err = GL.GetError()) != ErrorCode.NoError)
            {
                if (s.Length > 0)
                    s.Append(", ");
                s.Append(err);
            }

            if (s.Length > 0)
                throw new Exception($"OpenGL Error(s): {s}");
        }

        public void Dispose()
        {
        }


    }
}

