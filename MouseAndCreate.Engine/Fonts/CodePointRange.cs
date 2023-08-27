namespace MouseAndCreate.Fonts
{
    public readonly struct CodePointRange
    {
        public static readonly CodePointRange BasicLatin = new CodePointRange(0x0020, 0x007F);
        public static readonly CodePointRange Latin1Supplement = new CodePointRange(0x00A0, 0x00FF);
        public static readonly CodePointRange LatinExtendedA = new CodePointRange(0x0100, 0x017F);
        public static readonly CodePointRange LatinExtendedB = new CodePointRange(0x0180, 0x024F);
        public static readonly CodePointRange Cyrillic = new CodePointRange(0x0400, 0x04FF);
        public static readonly CodePointRange CyrillicSupplement = new CodePointRange(0x0500, 0x052F);
        public static readonly CodePointRange Hiragana = new CodePointRange(0x3040, 0x309F);
        public static readonly CodePointRange Katakana = new CodePointRange(0x30A0, 0x30FF);
        public static readonly CodePointRange Greek = new CodePointRange(0x0370, 0x03FF);
        public static readonly CodePointRange CjkSymbolsAndPunctuation = new CodePointRange(0x3000, 0x303F);
        public static readonly CodePointRange CjkUnifiedIdeographs = new CodePointRange(0x4e00, 0x9fff);
        public static readonly CodePointRange HangulCompatibilityJamo = new CodePointRange(0x3130, 0x318f);
        public static readonly CodePointRange HangulSyllables = new CodePointRange(0xac00, 0xd7af);

        public int Start { get; }
        public int End { get; }
        public int Length => (End - Start) + 1;

        public CodePointRange(int start, int end)
        {
            Start = start;
            End = end;
        }

        public CodePointRange(int single) : this(single, single) { }
    }
}
