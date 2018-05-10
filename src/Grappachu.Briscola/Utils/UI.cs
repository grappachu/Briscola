using Grappachu.Briscola.Interfaces;

namespace Grappachu.Briscola.Utils
{
    public static class Chat
    {
        private static readonly IUserInterface UserInterface = new ConsoleUserInterface();

        public static IUserInterface GetUI()
        {
            return UserInterface;
        }
    }
}