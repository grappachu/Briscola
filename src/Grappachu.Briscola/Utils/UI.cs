using System;
using Grappachu.Briscola.Interfaces;

namespace Grappachu.Briscola.Utils
{
    public static class Chat
    {
        private static readonly IUserInterface XUserInterface = new ConsoleUserInterface();
        private static readonly IUserInterface XMuteWrapper = new MuteChatWrapper(XUserInterface);

        static Chat()
        {
            Enabled = true;
        }

        public static IUserInterface GetUI()
        {
            return Enabled ? XUserInterface : XMuteWrapper;
        }


        /// <summary>
        /// Attiva o disattiva l'invio dei messaggi
        /// </summary>
        public static bool Enabled { get; set; }


        /// <summary>
        /// Un wrapper per bloccare i messaggi della chat
        /// </summary>
        private class MuteChatWrapper : IUserInterface
        {
            private readonly IUserInterface _innerChat;

            public MuteChatWrapper(IUserInterface innerChat)
            {
                _innerChat = innerChat;
            }
            public void Custom(ConsoleColor color, string message)
            {
            }

            public string Get()
            {
                return _innerChat.Get();
            }

            public string GetChoice()
            {
                return _innerChat.GetChoice();
            }

            public int GetInt(string message, int? defaultValue, ConsoleColor color = ConsoleColor.DarkYellow)
            {
                return _innerChat.GetInt(message, defaultValue, color);
            }

            public void Send(string message)
            {

            }

            public void Strong(string message)
            {
            }
        }
    }
}