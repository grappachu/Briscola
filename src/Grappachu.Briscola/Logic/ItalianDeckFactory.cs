using Grappachu.Briscola.Interfaces;
using Grappachu.Briscola.Model;

namespace Grappachu.Briscola.Logic
{
    public class ItalianDeckFactory : IDeckFactory
    {
        public IDeck CreateDeck()
        {
            return new Deck(new[] {"Danari", "Coppe", "Bastoni", "Spade"}, new Range(1, 10));
        }
    }
}