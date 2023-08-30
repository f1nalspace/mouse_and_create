namespace MouseAndCreate.Fonts
{
    public class DefaultFontBuilderFactory : IFontBuilderFactory
    {
        public IBitmapFontBuilder Create() => new STBBitmapFontBuilder();
    }
}
