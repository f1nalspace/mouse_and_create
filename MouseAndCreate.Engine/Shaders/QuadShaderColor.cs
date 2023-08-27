using MouseAndCreate.Rendering;

namespace MouseAndCreate.Shaders
{
    static class QuadShaderColor
    {
        public const string Name = "QuadColor";

        public static readonly ResourceShaderSource Vertex = new ResourceShaderSource(Name + "." + nameof(Vertex), ShaderSourceType.Vertex, nameof(Shaders) + ".shader_quad_color.vert");
        public static readonly ResourceShaderSource Fragment = new ResourceShaderSource(Name + "." + nameof(Fragment), ShaderSourceType.Fragment, nameof(Shaders) + ".shader_quad_color.frag");
    }
}
