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
            if (state.ArePartners(candidateWinner, myself))
            {
                // Quando prende il compagno carica
                return myself.Carica(state);
            }
            else
            {
                // Va su o prende se ci sono sufficienti punti
                var takingChoice = GetMyBestTakingChoice(myself, state);
                if (takingChoice.HasValue && IsWorth(takingChoice.Value, state))
                {
                    return takingChoice.Value;
                }

                // se non vale la pena sta fuori
                var resighChoice = myself.StaiFuori(state);
                if (resighChoice.HasValue)
                {
                    return resighChoice.Value;
                }
            }

            // Non dovrebbe arrivare qui
            return myself.HandCards.Last();
        }

        /// <summary>
        /// Ottiene un valore che indica che la carta è degna di essere utilizzata per prendere il piatto
        /// </summary>
        /// <param name="card"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        private static bool IsWorth(Card card, GameState state)
        {
            var points = state.Dish.Sum(c => c.GetScore());
            if (card.Seed == state.Briscola.Seed)
            {
                switch (card.Value)
                {
                    case 2:
                    case 4:
                    case 5:
                        return points > 7;

                    case 6:
                    case 7:
                        return points > 7;
                    case 8:
                        return points > 7;
                    case 9:
                        return points > 8;
                    case 10:
                        return points > 8;
                    case 3:
                    case 1:
                        return points > 11;
                }
            }

            return true;

        }

        /// <summary>
        /// Ottiene la carta migliore per prendere il piatto. 
        /// La carta sarà la carta più alta non di briscola o alternativamente la briscola di minor valore
        /// </summary>
        /// <param name="myself"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        private static Card? GetMyBestTakingChoice(IPlayer myself, GameState state)
        {
            var winningCards = myself.GetWinningCards(state);
            if (winningCards.Any())
            {
                var winningCardWithoutBriscola = winningCards.Where(x => x.Seed != state.Briscola.Seed).ToArray();
                if (winningCardWithoutBriscola.Any())
                {
                    return winningCardWithoutBriscola.OrderBy(x => x.Value).Last();
                }

                return winningCards.OrderBy(x => x.Value).Last();
            }

            return null;
        }

       
        protected override bool OnCanHandle(IPlayer myself, GameState state)
        {
            return state.Players.Count == 4 && state.Dish.Count == 3;
        }
    }
}