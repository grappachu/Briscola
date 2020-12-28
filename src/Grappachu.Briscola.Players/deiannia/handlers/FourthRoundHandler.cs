using Grappachu.Briscola.Interfaces;
using Grappachu.Briscola.Model;
using System.Linq;

namespace Grappachu.Briscola.Players.deiannia.handlers
{

    public class FourthRoundHandler : AbstractRoundHandler
    {

        public override Card ChooseCard(IPlayer myself, GameState state)
        {
            if (state.Dish.Count == 3)
            {
                Card choosed = TakeLiscio(myself.HandCards, state.Briscola);

                if (Winner(state).Equals(state.Dish[1]))
                {
                    choosed = TakeCarico(myself.HandCards, state.Briscola);
                }
                if (state.Dish.Sum(c => c.ValueInPoints()) > 11)
                {
                    if (TakeTaglio(myself.HandCards, state.Briscola).WinsOn(Winner(state), state.Briscola.Seed))
                        choosed = TakeTaglio(myself.HandCards, state.Briscola);
                    choosed = TakeBriscola(myself.HandCards, state.Briscola);
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
