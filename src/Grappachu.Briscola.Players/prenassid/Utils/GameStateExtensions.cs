using System.Linq;
using Grappachu.Briscola.Interfaces;
using Grappachu.Briscola.Model;

namespace Grappachu.Briscola.Players.prenassid.Utils
{
    internal static class GameStateExtensions
    {
        /// <summary>
        /// Ottiene il giocatore che allo stato attuale si sta aggiudicando il piatto
        /// </summary>
        public static IPlayer GetWinning(this GameState state)
        {
            return state.Players.ElementAt(1);
        }

        /// <summary>
        /// Indica se due giocatori sono compagni
        /// </summary>
        /// <param name="state"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static bool ArePartners(this GameState state, IPlayer p1, IPlayer p2)
        {
            if (state.Players.Count != 4) return false;
            for (int i = 0; i < 4; i++)
            {
                // prende i giocatori a due a due
                if (state.Players.ElementAt(i) == p1 && state.Players.ElementAt((i + 2) % 4) == p2)
                {
                    return true;
                }
            }
            return false;
        }
    }
}