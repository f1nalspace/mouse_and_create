using MouseAndCreate.Rendering;

namespace MouseAndCreate.Textures
{
    public static class DefaultTextures
    {
        public static readonly ResourceTextureSource MouseArrow = new ResourceTextureSource(nameof(MouseArrow), nameof(Textures) + ".mouse_arrow.png");

        public static readonly ResourceTextureSource OpenGLTestTexture = new ResourceTextureSource(nameof(OpenGLTestTexture), nameof(Textures) + ".opengl_test_texture.png");
    }
}
