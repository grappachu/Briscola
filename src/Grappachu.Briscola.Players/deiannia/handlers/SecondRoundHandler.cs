using Grappachu.Briscola.Interfaces;
using Grappachu.Briscola.Model;
using System.Linq;

namespace Grappachu.Briscola.Players.deiannia.handlers
{

    public class SecondRoundHandler : AbstractRoundHandler
    {

        public override Card ChooseCard(IPlayer myself, GameState state)
        {
            if (state.Dish.Count == 1)
            {
                Card choosed = TakeLiscio(myself.HandCards, state.Briscola);

                if (state.Dish[0].IsPunti(state.Briscola.Seed))
                {
                    // provare a mettere punti dello stesso seme prima di usare un taglio
                    if (myself.HandCards.Where(c => c.IsSameSeed(state.Dish[0]) && c.Order() > state.Dish[0].Order()).Any())
                        choosed = myself.HandCards.Where(c => c.IsSameSeed(state.Dish[0]) && c.Order() > state.Dish[0].Order()).First();

                    choosed = TakeTaglio(myself.HandCards, state.Briscola);
                }
                if (state.Dish[0].IsCarico(state.Briscola.Seed))
                {
                    // provare a mettere carico maggiore dello stesso seme prima di usare una briscola
                    if (myself.HandCards.Where(c => c.IsSameSeed(state.Dish[0]) && c.Order() > state.Dish[0].Order()).Any())
                        choosed = myself.HandCards.Where(c => c.IsSameSeed(state.Dish[0]) && c.Order() > state.Dish[0].Order()).First();

                    choosed = TakeBriscola(myself.HandCards, state.Briscola);
                }
                if (state.Dish[0].IsTaglio(state.Briscola.Seed))
                {
                    choosed = TakeLiscio(myself.HandCards, state.Briscola);
                }
                if (state.Dish[0].IsBriscola(state.Briscola.Seed))
                {
                    choosed = TakeLiscio(myself.HandCards, state.Briscola);
                }
                if (state.Dish[0].IsLiscio(state.Briscola.Seed))
                {
                    choosed = TakeLiscio(myself.HandCards, state.Briscola);
                }

                return choosed;
            }
            else
            {
                return base.ChooseCard(myself, state);
            }
        }
    }
}
