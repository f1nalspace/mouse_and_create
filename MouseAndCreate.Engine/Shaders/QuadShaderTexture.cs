using MouseAndCreate.Rendering;

namespace MouseAndCreate.Shaders
{
    static class QuadShaderTexture
    {
        public const string Name = "QuadTexture";

        public static readonly ShaderSource Vertex = new ShaderSource(Name + "." + nameof(Vertex), ShaderSourceType.Vertex, nameof(Shaders) + ".shader_quad_texture.vert");
        public static readonly ShaderSource Fragment = new ShaderSource(Name + "." + nameof(Fragment), ShaderSourceType.Fragment, nameof(Shaders) + ".shader_quad_texture.frag");
    }
}
