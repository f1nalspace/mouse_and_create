namespace MouseAndCreate.Input
{
    public interface IKeyboardInputManager
    {
        void KeyDown(Key key, KeyModifiers modifiers, bool isRepeat);
        void KeyUp(Key key, KeyModifiers modifiers, bool wasRepeat);
        void Input(string input);
    }
}
