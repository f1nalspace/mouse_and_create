using System;

namespace MouseAndCreate.Exceptions
{
    public class GameNotInitializedException : Exception
    {
        public GameNotInitializedException() : base("The game was not initialized!") { }
        public GameNotInitializedException(string message) : base(message + Environment.NewLine + "The game was not initialized!") { }
    }
}
