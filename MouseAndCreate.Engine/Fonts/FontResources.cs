using System.IO;
using System.Reflection;

namespace MouseAndCreate.Fonts;

static class FontResources
{
    public static Stream SulphurPointRegular => GetStream($"{nameof(Fonts)}.SulphurPointRegular.ttf");

    private static Stream GetStream(string link)
    {
        Assembly assembly = typeof(FontResources).Assembly;
        string resourceName = $"{nameof(MouseAndCreate)}.{link}";
        Stream stream = assembly.GetManifestResourceStream(resourceName);
        return stream;
    }
}
