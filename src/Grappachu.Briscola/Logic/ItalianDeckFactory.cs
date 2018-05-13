using Grappachu.Briscola.Interfaces;
using Grappachu.Briscola.Model;

namespace Grappachu.Briscola.Logic
{
    /// <summary>
    ///     Rappresenta una factory per la creazione di un mazzo italiano di 40 carte
    /// </summary>
    public class ItalianDeckFactory : IDeckFactory
    {
        public IDeck CreateDeck()
        {
            return new Deck(new[] {"Danari", "Coppe", "Bastoni", "Spade"}, new Range(1, 10));
        }
    }
}