using System.Linq;
using Grappachu.Briscola.Interfaces;
using Grappachu.Briscola.Model;
using Grappachu.Briscola.Players.prenassid.Utils;

namespace Grappachu.Briscola.Players.prenassid.Handlers
{
    public class P4R2Handler : RoundHandler
    {

        protected override Card OnHandle(IPlayer myself, GameState state, Watcher watcher)
        {
            Card? choice = null;
            if (state.IsBriscolaPlayed())
            {
                choice = myself.StaiFuori(state);
            }
            else
            {
                bool ilCompagnoHaBriscola = true; // Rand.Next(1, 3) == 1;
                choice = myself.Supera(state, ilCompagnoHaBriscola);
            }



            return choice ?? myself.HandCards.First();
        }



        //private static IPlayer GetPartner(IPlayer myself, GameState state)
        //{
        //    var meIdx = Array.IndexOf(state.Players.ToArray(), myself);
        //    var partnerIdx = (meIdx + 2) % 4;
        //    var partner = state.Players.ElementAt(partnerIdx);
        //    return partner;
        //}







        protected override bool OnCanHandle(IPlayer myself, GameState state)
        {
            return state.Players.Count == 4 && state.Dish.Count == 1;
        }
    }
}