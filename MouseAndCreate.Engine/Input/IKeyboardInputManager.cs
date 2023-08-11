namespace MouseAndCreate.Input
{
    public interface IKeyboardInputManager
    {
        void KeyDown(Key key);
        void KeyUp(Key key);
        void Input(string input);
    }
}
