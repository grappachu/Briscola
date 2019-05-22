using System.Collections.Generic;
using System.Linq;
using Grappachu.Briscola.Model;

namespace Grappachu.Briscola.Players.zaninig.Model
{
    public class Deck : List<Card>
    {
        public Deck()
        {
        }

        public Deck(string[] seeds, Range valueRange) {            
            foreach (var seed in seeds)
                for (var value = valueRange.Min; value <= valueRange.Max; value++)
                    this.Add(new Card(seed, value));
        }
        
        public Deck(IEnumerable<Card> cards)
        {
            this.AddRange(cards);
        }

        public int GetPoints()
        {
            return this.Select(x => x.Value).Sum(ValueToScore);
        }

        private int ValueToScore(int value) {
            switch (value)
            {
                case 1:
                    return 11;
                case 3:
                    return 10;
                case 10:
                    return 4;
                case 9:
                    return 3;
                case 8:
                    return 2;
                default:
                    return 0;
            }
        }
    }
}
