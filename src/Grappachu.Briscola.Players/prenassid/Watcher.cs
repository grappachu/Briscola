using System.Collections.Generic;
using System.Linq;
using Grappachu.Briscola.Interfaces;
using Grappachu.Briscola.Logic;
using Grappachu.Briscola.Model;
using Grappachu.Briscola.Players.prenassid.Utils;

namespace Grappachu.Briscola.Players.prenassid
{
    public class Watcher
    {
        private readonly IList<Card> _unplayedCards = new List<Card>();

        public Watcher()
        {
            var deck = new ItalianDeckFactory().CreateDeck();
            while (deck.Count > 0)
            {
                _unplayedCards.Add(deck.Pop());
            }
        }

        public void Watch(IPlayer myself, GameState state)
        {
            foreach (var card in state.Dish)
            {
                _unplayedCards.Remove(card);
            }

        }

        /// <summary>
        /// Ottiene tutte le carte non giocate escluse le mie
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Card> GetUnknownCards(IPlayer myself)
        {
            var res = _unplayedCards.Where(x => !myself.HandCards.Contains(x)).ToArray();
            return res;
        }
    }
}