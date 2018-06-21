using System.Linq;
using Grappachu.Briscola.Interfaces;
using Grappachu.Briscola.Model;

namespace Grappachu.Briscola.Players.prenassid.Utils
{
    internal static class PlayerExtension
    {
        
        /// <summary>
        /// Tira la carta più alta non di briscola
        /// </summary>
        /// <param name="me"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public static Card Carica(this IPlayer me, GameState state)
        {
            var sortedCards = me.HandCards.OrderByDescending(x => OrderCarica(x, state.Briscola)).ToArray();
            return sortedCards.First();
        }

        private static int OrderCarica(Card c, Card briscola)
        {
            int val;
            if (c.Seed != briscola.Seed)
            {
                val  =c.Value + 20;
                if (c.Value == 1) val += 15;
                if (c.Value == 3) val += 10;
            }
            else
            {
                val = -c.Value;
                return val;
            } 
            return val;
        }
    }
}