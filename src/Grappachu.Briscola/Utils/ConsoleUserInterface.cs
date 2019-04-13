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

        public int GetInt(string message, int? defaultValue, ConsoleColor color)
        {
            bool userInputValid;
            int userValue;
            do
            {
                Custom(color, message);
                var userInput = Get();
                if (string.IsNullOrEmpty(userInput) && defaultValue.HasValue)
                {
                    return defaultValue.Value;
                }

                userInputValid = int.TryParse(userInput, out userValue);
                if (!userInputValid)
                {
                    Custom(ConsoleColor.Red, "Valore non valido");
                }
            } while (!userInputValid);

            return userValue;
        }
    }
}