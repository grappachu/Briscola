using System.Linq;

namespace Grappachu.Briscola.Players.zaninig.Model
{
    public class Turn
    {
        private readonly Deck _deck;

        public Turn(Deck deck)
        {
            _deck = deck;
        }

        bool IsFirst()
        {
            return _deck.Count() == 1;
        }

        bool isLast()
        {
            return _deck.Count() == 4;
        }
    }
}
