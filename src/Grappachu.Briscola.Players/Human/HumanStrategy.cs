using System;
using System.Linq;
using System.Text;
using Grappachu.Briscola.Interfaces;
using Grappachu.Briscola.Model;

namespace Grappachu.Briscola.Players.Human
{
    public class HumanStrategy : IStrategy
    {
        private readonly IUserInterface _userInterface;

        public HumanStrategy(IUserInterface userInterface, string name)
        {
            _userInterface = userInterface;
            Name = name;
        }

        public string Name { get; } 

        public Card Choose(IPlayer playFor, GameState state)
        {
            do
            {
                var sb = new StringBuilder();
                sb.AppendLine("Gioca una carta tra:\n");
                var count = 1;
                foreach (var card in playFor.HandCards)
                    sb.AppendFormat("  ({2}) | Il {0} di {1}\n", card.Value, card.Seed, count++);
                sb.AppendFormat("  (x) | Abbandona la partita\n");

                _userInterface.Send(sb.ToString());
                var val = _userInterface.GetChoice();

                if (string.Equals(val, "x", StringComparison.OrdinalIgnoreCase))
                    Environment.Exit(0);
                int idx;
                if (int.TryParse(val, out idx) && idx > 0 && idx <= playFor.HandCards.Count())
                    return playFor.HandCards.ElementAt(idx - 1);
            } while (true);
        }

        public void Watch(IPlayer player, GameState state)
        {
            // l'umano pensa per la macchina
        }

        public bool IsHuman => true;
    }
}