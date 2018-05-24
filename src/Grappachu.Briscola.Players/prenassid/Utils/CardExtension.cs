using System.Collections.Generic;
using System.Linq;
using Grappachu.Briscola.Model;

namespace Grappachu.Briscola.Players.prenassid.Utils
{
    public static class CardExtension
    {
        /// <summary>
        /// Ottiene un valore che indica che, data la briscola, la carta è vincente sul piatto
        /// </summary>
        /// <param name="myCard">La carta per cui viene effettuata la valutazione</param>
        /// <param name="dish">Elenco ordinato delle carte sul piatto</param>
        /// <param name="briscola">La briscola corrente</param>
        /// <returns></returns>
        public static bool IsWinning(this Card myCard, IEnumerable<Card> dish, Card briscola)
        {
            var candidateDish = dish.ToList();
            candidateDish.Add(myCard);
            var myIndex = candidateDish.Count - 1;

            Card winCard; // cerco la carta vincente della mano 
            if (candidateDish.Any(x => x.Seed == briscola.Seed))
            {
                // se ci sono briscole sarà quella di maggior valore
                winCard = candidateDish.Where(x => x.Seed == briscola.Seed).GetHighest();
            }
            else
            {
                // altrimenti guardo il primo seme giocato
                var first = candidateDish.First().Seed;
                winCard = candidateDish.Where(x => x.Seed == first).GetHighest();
            }

            // dall'ordine di gioco delle carte e dal giocatore di turno 
            // determino chi ha giocato la carta vincente 
            var winIdx = candidateDish.IndexOf(winCard);

            return winIdx == myIndex;
        }

        /// <summary>
        /// Ottiene un valore che indica che, data la briscola, la carta è vincente sul piatto
        /// </summary>
        /// <param name="myCard">La carta per cui viene effettuata la valutazione</param>
        /// <param name="state">Stato del gioco su cui viene effettuato il confronto</param>
        public static bool IsWinning(this Card myCard, GameState state)
        {
            return IsWinning(myCard, state.Dish, state.Briscola);
        }

        /// <summary>
        /// Ottiene i punti di una carta
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public static int GetScore(this Card card)
        {
            switch (card.Value)
            {
                case 1:
                    return 11;
                case 3:
                    return 10;
                case 10:
                    return 4;
                case 9:
                    return 3;
                case 8:
                    return 2;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Ottiene la carta di minor valore da un elenco
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static Card GetLowest(this IEnumerable<Card> cards)
        {
            return cards.OrderBy(GetValueOrder).First();
        }

        /// <summary>
        /// Ottiene la carta di maggior valore da un elenco
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static Card GetHighest(this IEnumerable<Card> cards)
        {
            return cards.OrderBy(GetValueOrder).Last();
        }

        private static int GetValueOrder(Card card)
        {
            if (card.Value == 1)
                return 12;
            if (card.Value == 3)
                return 11;
            return card.Value;
        }
    }
}