using MouseAndCreate.Rendering;

namespace MouseAndCreate.Shaders
{
    static class QuadShaderTexture
    {
        public const string Name = "QuadTexture";

        public static readonly ResourceShaderSource Vertex = new ResourceShaderSource(Name + "." + nameof(Vertex), ShaderSourceType.Vertex, nameof(Shaders) + ".shader_quad_texture.vert");
        public static readonly ResourceShaderSource Fragment = new ResourceShaderSource(Name + "." + nameof(Fragment), ShaderSourceType.Fragment, nameof(Shaders) + ".shader_quad_texture.frag");
    }
}
