using System.Linq;
using Grappachu.Briscola.Interfaces;
using Grappachu.Briscola.Model;

namespace Grappachu.Briscola.Players.prenassid.Handlers
{
    public class P4R3Handler : RoundHandler
    {
        protected override Card OnHandle(IPlayer myself, GameState state, Watcher watcher)
        {
            return myself.HandCards.First();
        }

        protected override bool OnCanHandle(IPlayer myself, GameState state)
        {
            return state.Players.Count == 4 && state.Dish.Count == 2;
        }
    }
}