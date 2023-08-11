using MouseAndCreate.Rendering;

namespace MouseAndCreate.Shaders
{
    static class QuadShaderColor
    {
        public const string Name = "QuadColor";

        public static readonly ShaderSource Vertex = new ShaderSource(Name + "." + nameof(Vertex), ShaderSourceType.Vertex, nameof(Shaders) + ".shader_quad_color.vert");
        public static readonly ShaderSource Fragment = new ShaderSource(Name + "." + nameof(Fragment), ShaderSourceType.Fragment, nameof(Shaders) + ".shader_quad_color.frag");
    }
}
