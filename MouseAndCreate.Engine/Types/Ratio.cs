namespace MouseAndCreate.Types
{
    public readonly struct Ratio
    {
        public float Numerator { get; }
        public float Denominator { get; }

        public float Value => Denominator != 0 ? Numerator / Denominator : 0;

        public Ratio(float numerator, float denumerator) : this()
        {
            Numerator = numerator;
            Denominator = denumerator;
        }

        public override string ToString() => $"{Numerator} / {Denominator}";
    }
}
