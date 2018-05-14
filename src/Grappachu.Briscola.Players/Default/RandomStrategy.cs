using System;
using System.Linq;
using Grappachu.Briscola.Interfaces;
using Grappachu.Briscola.Model;

namespace Grappachu.Briscola.Players.Default
{
    public class RandomStrategy : StrategyBase
    {
        protected override Card OnChoose(IPlayer myself, GameState state)
        {
            return myself.HandCards.OrderBy(x => new Guid()).First();
        }

        protected override void OnWatch(IPlayer myself, GameState state)
        {
            // la strategia stupida non pensa
        }

        public RandomStrategy() : base("random")
        {
        }
    }
}