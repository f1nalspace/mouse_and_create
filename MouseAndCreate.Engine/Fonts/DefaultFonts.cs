using System.IO;
using System.Reflection;

namespace MouseAndCreate.Fonts;

public static class DefaultFonts
{
    public static Stream SulphurPointRegular => GetStream($"{nameof(Fonts)}.SulphurPointRegular.ttf");

    public static Stream ModernDOS8x16 => GetStream($"{nameof(Fonts)}.ModernDOS8x16.ttf");

    public static Stream BitstreamVeraSansMono => GetStream($"{nameof(Fonts)}.BitstreamVeraSansMono.ttf");

    private static Stream GetStream(string link)
    {
        Assembly assembly = typeof(DefaultFonts).Assembly;
        string resourceName = $"{nameof(MouseAndCreate)}.{link}";
        Stream stream = assembly.GetManifestResourceStream(resourceName);
        return stream;
    }
}
