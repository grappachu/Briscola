using Grappachu.Briscola.Interfaces;
using Grappachu.Briscola.Model;
using Grappachu.Briscola.Players.Default;
using System.Collections.Generic;
using System.Linq;

namespace Grappachu.Briscola.Players.suerzg
{
    public class Gabry404Strategy : StrategyBase
    {
        public const string StrategyName = "gabry404";

        public Dictionary<string, List<Card>> playedCards;

        public Gabry404Strategy() : base(StrategyName)
        {
        }

        protected override Card OnChoose(IPlayer myself, GameState state)
        {
            if (state.Dish.Count > 0)
            {
                Card currentHighest = state.Dish[0];
                for (var index = 1; index < state.Dish.Count; index++)
                {
                    var c = state.Dish[index];

                    if (c.Seed == currentHighest.Seed && c.Value > currentHighest.Value)
                    {
                        currentHighest = c;
                    }
                    else if (c.Seed == state.Briscola.Seed)
                    {
                        currentHighest = c;
                    }
                }
            }
            else if (state.Turn == 0 && playedCards.Count == 0 &&
                     myself.HandCards.Any(c => c.Seed != state.Briscola.Seed && (c.Value == 3 || c.Value == 1)))
            {
                return myself.HandCards.First(c => c.Seed != state.Briscola.Seed && (c.Value == 3 || c.Value == 1));
            }
            else
            {
                if (myself.HandCards.Any(c =>
                    c.Seed != state.Briscola.Seed && c.Value != 3 && c.Value != 1 && c.Value < 8))
                {
                    return myself.HandCards.First(c =>
                        c.Seed != state.Briscola.Seed && c.Value != 3 && c.Value != 1 && c.Value < 8);
                }
                else if (myself.HandCards.Any(c =>
                    c.Seed != state.Briscola.Seed && c.Value != 3 && c.Value != 1 && c.Value > 8))
                {
                    return myself.HandCards.First(c =>
                        c.Seed != state.Briscola.Seed && c.Value != 3 && c.Value != 1 && c.Value > 8);
                }
                else
                {
                    return myself.HandCards.First();
                }
            }

            return myself.HandCards.First();
        }

        protected override void OnWatch(IPlayer myself, GameState state)
        {
            foreach (var card in state.Dish)
            {
                if (!playedCards.ContainsKey(card.Seed))
                {
                    playedCards[card.Seed] = new List<Card>();
                }

                playedCards[card.Seed].Add(card);
                playedCards[card.Seed] = playedCards[card.Seed].OrderBy(point => point.Value).ToList();
            }
        }
    }
}