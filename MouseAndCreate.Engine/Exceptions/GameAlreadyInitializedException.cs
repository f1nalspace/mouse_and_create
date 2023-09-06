using System;

namespace MouseAndCreate.Exceptions
{
    public class GameAlreadyInitializedException : Exception
    {
        public GameAlreadyInitializedException() : base("The game is already initialized!") { }
        public GameAlreadyInitializedException(string message) : base(message + Environment.NewLine + "The game is already initialized!") { }
    }
}
