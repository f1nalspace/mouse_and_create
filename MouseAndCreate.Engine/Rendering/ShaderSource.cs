using System.IO;
using System.Reflection;
using MouseAndCreate.Play;

namespace MouseAndCreate.Rendering
{
    class ShaderSource
    {
        public string Name { get; }
        public ShaderSourceType Type { get; }
        public string Link { get; }

        public ShaderSource(string name, ShaderSourceType type, string link)
        {
            Name = name;
            Type = type;
            Link = link;
        }

        public string GetContent()
        {
            Assembly assembly = typeof(IGame).Assembly;
            var names = assembly.GetManifestResourceNames();
            string resourceName = $"{nameof(MouseAndCreate)}.{Link}";
            using Stream stream = assembly.GetManifestResourceStream(resourceName);
            using StreamReader reader = new StreamReader(stream);
            string result = reader.ReadToEnd();
            return result;
        }

        public override string ToString() => $"{Link} [{Type}]";
    }
}
