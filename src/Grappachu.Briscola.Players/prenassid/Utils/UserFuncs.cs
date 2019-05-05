using System.Collections.Generic;
using System.Linq;
using Grappachu.Briscola.Interfaces;
using Grappachu.Briscola.Model;

namespace Grappachu.Briscola.Players.prenassid.Utils
{
    /// <summary>
    ///     Defnisce un'insieme di azioni generiche per il giocatore
    /// </summary>
    internal static class UserFuncs
    {
        /// <summary>
        /// Ottiene le carte che non prendono il piatto tra quelle a disposizione del giocatore
        /// </summary>
        /// <param name="player"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        internal static IReadOnlyCollection<Card> GetLooseCards(this IPlayer player, GameState state)
        {
            return player.HandCards.Where(x => !x.IsWinning(state)).ToArray();
        }



        /// <summary>
        /// Ottiene le carte che vincono il piatto tra quelle a disposizione del giocatore
        /// </summary>
        /// <param name="player"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        internal static IReadOnlyCollection<Card> GetWinningCards(this IPlayer player, GameState state)
        {
            return player.HandCards.Where(x => x.IsWinning(state)).ToArray();
        }

        /// <summary>
        ///     Gioca la carta più bassa tra quelle che non vincono sul piatto
        /// </summary>
        /// <param name="player"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        internal static Card? StaiFuori(this IPlayer player, GameState state)
        {
            var notWinningCards = player.GetLooseCards(state);
            if (notWinningCards.Any())
            {
                return notWinningCards.GetLowest();
            }

            return null;
        }


        /// <summary>
        ///     Gioca la briscola di minor valore per prendere il piatto
        /// </summary>
        /// <param name="myself"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        internal static Card? Taglia(this IPlayer myself, GameState state)
        {
            var winningCards = myself.GetWinningCards(state);
            if (winningCards.Any())
            {
                var briscole = winningCards.Where(x => x.Seed == state.Briscola.Seed).ToArray();
                if (briscole.Any())
                {
                    return briscole.GetLowest();
                }
            }

            return null;
        }


        /// <summary>
        ///  Ottiene la carta NON di briscola con il minor valore per prendere il piatto
        /// </summary>
        /// <param name="myself"></param>
        /// <param name="state"></param>
        /// <param name="conCarico">Indica che è possibile superare anche con un carico</param>
        /// <returns></returns>
        public static Card? Supera(this IPlayer myself, GameState state, bool conCarico)
        {
            var winningCards = myself.GetWinningCards(state);
            if (winningCards.Any())
            {
                var nonBriscole = winningCards.Where(x => x.Seed != state.Briscola.Seed && (conCarico || !x.IsCarico())).ToArray();
                if (nonBriscole.Any())
                {
                    return nonBriscole.GetLowest();
                }
            }

            return null;
        }



        /// <summary>
        /// Ottiene la carta NON di briscola con il maggior valore per prendere il piatto
        /// </summary>
        /// <param name="myself"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        private static Card? VaiSu(IPlayer myself, GameState state)
        {
            var winningCards = myself.GetWinningCards(state);
            if (winningCards.Any())
            {
                var winningCardWithoutBriscola = winningCards.Where(x => x.Seed != state.Briscola.Seed).ToArray();
                if (winningCardWithoutBriscola.Any())
                {
                    return winningCardWithoutBriscola.GetHighest();
                }
            }

            return null;
        }


    }
}