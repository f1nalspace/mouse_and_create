namespace MouseAndCreate.Fonts
{
    public class DefaultIFontBuilderFactory : IFontBuilderFactory
    {
        public IFontBuilder Create() => new STBFontBuilder();
    }
}
