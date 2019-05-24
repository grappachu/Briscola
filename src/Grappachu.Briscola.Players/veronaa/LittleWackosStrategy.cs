using System.Collections.Generic;
using System.Linq;
using Grappachu.Briscola.Interfaces;
using Grappachu.Briscola.Logic;
using Grappachu.Briscola.Model;
using Grappachu.Briscola.Players.Default;

namespace Grappachu.Briscola.Players.veronaa
{
    public class LittleWackosStrategy : StrategyBase
    {
        public const string StrategyName = "littlewackos";

        private readonly IList<Card> _unplayedCards = new List<Card>();

        public LittleWackosStrategy() : base(StrategyName)
        {
            var deck = new ItalianDeckFactory().CreateDeck();
            while (deck.Count > 0)
            {
                _unplayedCards.Add(deck.Pop());
            }
        }

        protected override Card OnChoose(IPlayer myself, GameState state) {

            var assiNonGiocati = _unplayedCards.Where(card => card.Value == 1).ToList();

            var chosenCardWithSeed = myself.HandCards
                .Where(card => card.Seed == state.Briscola.Seed)
                .OrderBy(card => card.Value).FirstOrDefault();
        
            var chosenCard = myself.HandCards.OrderBy(card => card.Value).First();

            if (chosenCardWithSeed.Value == 0)
                return chosenCard;
            if (chosenCard.Value > chosenCardWithSeed.Value && assiNonGiocati.Count > 2) 
                return chosenCard;
            else
                return chosenCardWithSeed;
        }

        protected override void OnWatch(IPlayer myself, GameState state) {
            foreach (var card in state.Dish)
            {
                _unplayedCards.Remove(card);
            }
        }

       
    }
}
