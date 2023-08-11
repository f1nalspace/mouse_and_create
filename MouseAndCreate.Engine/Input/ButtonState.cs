namespace MouseAndCreate.Input
{
    public struct ButtonState
    {
        public int HalfTransitionCount { get; set; }
        public int EndedDown { get; set; }

        public bool WasPressed() => ((HalfTransitionCount > 1) || ((HalfTransitionCount == 1) && (EndedDown != 0)));
        public bool IsDown() => EndedDown != 0;

        public ButtonState(int endedDown, int halfTransitionCount)
        {
            EndedDown = endedDown;
            HalfTransitionCount = halfTransitionCount;
        }
    }
}
