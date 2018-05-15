using System;
using Grappachu.Briscola.Interfaces;

namespace Grappachu.Briscola.Utils
{
    public class ConsoleUserInterface : IUserInterface
    {
        public string Get()
        {
            return Console.ReadLine();
        }

        public string GetChoice()
        {
            var key = Console.ReadKey();
            Console.WriteLine("");
            return key.KeyChar.ToString();
        }

        public void Send(string message)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
        }

        public void Strong(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
        }

        public void Custom(ConsoleColor color, string message)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}