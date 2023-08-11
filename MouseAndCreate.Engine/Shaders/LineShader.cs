using MouseAndCreate.Rendering;

namespace MouseAndCreate.Shaders
{
    static class LineShader
    {
        public const string Name = "Line";

        public static readonly ShaderSource Vertex = new ShaderSource(Name + "." + nameof(Vertex), ShaderSourceType.Vertex, nameof(Shaders) + ".shader_line.vert");
        public static readonly ShaderSource Fragment = new ShaderSource(Name + "." + nameof(Fragment), ShaderSourceType.Fragment, nameof(Shaders) + ".shader_line.frag");
        public static readonly ShaderSource Geometry = new ShaderSource(Name + "." + nameof(Geometry), ShaderSourceType.Geometry, nameof(Shaders) + ".shader_line.geom");
    }
}
