using MouseAndCreate.Rendering;

namespace MouseAndCreate.Textures
{
    static class TestTextures
    {
        public static readonly ResourceTextureSource OpenGLTestTexture = new ResourceTextureSource(nameof(OpenGLTestTexture), nameof(Textures) + ".opengl_test_texture.png");
    }
}
