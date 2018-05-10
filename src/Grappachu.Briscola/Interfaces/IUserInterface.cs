using System;

namespace Grappachu.Briscola.Interfaces
{
    /// <summary>
    ///     Rappresenta una interfaccia utente per il gioco
    /// </summary>
    public interface IUserInterface
    {
        string Get();
        void Send(string message);
        void Strong(string message);
        void Custom(ConsoleColor color, string message);
    }
}