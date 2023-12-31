﻿using System.IO;
using System.Reflection;
using MouseAndCreate.Graphics;
using MouseAndCreate.Play;

namespace MouseAndCreate.Rendering
{
    public class ResourceTextureSource : ITextureSource
    {
        public string Name { get; }
        public string Link { get; }

        public ResourceTextureSource(string name, string link)
        {
            Name = name;
            Link = link;
        }

        public Stream GetStream()
        {
            Assembly assembly = typeof(IGame).Assembly;
            string resourceName = $"{nameof(MouseAndCreate)}.{Link}";
            Stream stream = assembly.GetManifestResourceStream(resourceName);
            return stream;
        }

        public override string ToString() => $"{Name} => {Link}";

        public TextureData Load(TextureFormat target, ImageFlags flags)
        {
            Stream stream = GetStream();
            return TextureLoaderFactory.Instance.Load(stream, target, flags);
        }
    }
}
