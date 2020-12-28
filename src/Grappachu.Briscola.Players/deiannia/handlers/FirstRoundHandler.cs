using Grappachu.Briscola.Interfaces;
using Grappachu.Briscola.Model;

namespace Grappachu.Briscola.Players.deiannia.handlers
{

    public class FirstRoundHandler : AbstractRoundHandler
    {

        public override Card ChooseCard(IPlayer myself, GameState state)
        {
            Card? choosed = null;
            if (state.Dish.Count == 0)
            {
                choosed = TakeLiscio(myself.HandCards, state.Briscola);
                //choosed = choosed ?? TakePunti(myself.HandCards, state.Briscola);

                return choosed.Value;
            }
            else
            {
                return base.ChooseCard(myself, state);
            }
        }
    }
}
