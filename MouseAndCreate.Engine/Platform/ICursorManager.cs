namespace MouseAndCreate.Platform
{
    public interface ICursorManager
    {
        CursorType GetCursor();
        void SetCursor(CursorType cursor);
    }
}
