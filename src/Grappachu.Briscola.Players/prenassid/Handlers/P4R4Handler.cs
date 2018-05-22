using System.Linq;
using Grappachu.Briscola.Interfaces;
using Grappachu.Briscola.Model;
using Grappachu.Briscola.Players.prenassid.Utils;

namespace Grappachu.Briscola.Players.prenassid.Handlers
{
    public class P4R4Handler : RoundHandler
    {
        protected override Card OnHandle(IPlayer myself, GameState state, Watcher watcher)
        {
            IPlayer candidateWinner = state.GetWinning();
            if (state.ArePartners( candidateWinner,myself))
            {
                return myself.Carica(state);
            }
            else
            {

            }
            return myself.HandCards.Last();
        }

        protected override bool OnCanHandle(IPlayer myself, GameState state)
        {
            return state.Players.Count == 4 && state.Dish.Count == 3;
        }
    }
}