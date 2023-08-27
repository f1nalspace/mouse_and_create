using MouseAndCreate.Rendering;

namespace MouseAndCreate.Shaders
{
    static class LineShader
    {
        public const string Name = "Line";

        public static readonly ResourceShaderSource Vertex = new ResourceShaderSource(Name + "." + nameof(Vertex), ShaderSourceType.Vertex, nameof(Shaders) + ".shader_line.vert");
        public static readonly ResourceShaderSource Fragment = new ResourceShaderSource(Name + "." + nameof(Fragment), ShaderSourceType.Fragment, nameof(Shaders) + ".shader_line.frag");
    }
}
