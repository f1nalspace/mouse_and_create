namespace MouseAndCreate.Fonts
{
    public class DefaultFontBuilderFactory : IFontBuilderFactory
    {
        public IFontBuilder Create() => new STBFontBuilder();
    }
}
