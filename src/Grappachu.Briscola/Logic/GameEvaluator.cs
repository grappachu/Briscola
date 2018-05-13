using System.Collections.Generic;
using System.Linq;
using Grappachu.Briscola.Model;

namespace Grappachu.Briscola.Logic
{
    /// <summary>
    ///     Fornisce un'insieme di metodi per la valutazione dello stato di gioco per la Briscola
    /// </summary>
    public static class BriscolaUtils
    {

        /// <summary>
        ///     Valuta lo stato del piatto e restituisce l'indice del giocatore che si aggiudica la mano
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public static int Evaluate(this GameState state)
        {
            var briscola = state.Briscola.Seed;
            Card winCard; // cerco la carta vincente della mano

            if (state.Dish.Any(x => x.Seed == briscola))
            {
                // se ci sono briscole sarà quella di maggior valore
                winCard = state.Dish.Where(x => x.Seed == briscola).OrderByDescending(Order).First();
            }
            else
            {
                // altrimenti guardo il primo seme giocato
                var first = state.Dish.First().Seed;
                winCard = state.Dish.Where(x => x.Seed == first).OrderByDescending(Order).First();
            }

            // dall'ordine di gioco delle carte e dal giocatore di turno 
            // determino chi ha giocato la carta vincente 
            var winIdx = state.Dish.IndexOf(winCard);
            var playerIdx = (winIdx + state.Turn) % state.Players.Count;
            return playerIdx;
        }


        //Step 3: Una funzione statica che assegna un valore diverso ad asso e 3 
        //        in modo da poterli ordinare facilmente
        private static int Order(Card card)
        {
            if (card.Value == 1)
                return 12;
            if (card.Value == 3)
                return 11;
            return card.Value;
        }
        

        /// <summary>
        ///     Somma i punti di un insieme di carte secondo le regole del gioco della Briscola
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static int Totalize(IEnumerable<Card> cards)
        {
            return cards.Select(x => x.Value).Sum(ValueToScore);
        }

        private static int ValueToScore(int value)
        {
            switch (value)
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
    }
}