using Grappachu.Briscola.Interfaces;
using Grappachu.Briscola.Model;

namespace Grappachu.Briscola.Players.deiannia.handlers
{

    public class ThirdRoundHandler : AbstractRoundHandler
    {

        public override Card ChooseCard(IPlayer myself, GameState state)
        {
            if (state.Dish.Count == 2)
            {
                Card choosed = TakeLiscio(myself.HandCards, state.Briscola);

                if (Winner(state).Equals(state.Dish[0]))
                {
                    choosed = TakeCarico(myself.HandCards, state.Briscola);
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
