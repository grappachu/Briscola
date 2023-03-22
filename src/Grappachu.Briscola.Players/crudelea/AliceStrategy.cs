using Grappachu.Briscola.Interfaces;
using Grappachu.Briscola.Model;
using Grappachu.Briscola.Players.Default;
using System;
using System.Linq;

namespace Grappachu.Briscola.Players.crudelea
{
    public class AliceStrategy : StrategyBase
    {
        public static readonly string StrategyName = "AliExpress";
        private int[] NoPointsValues = { 2, 4, 5, 6, 7 };
        private int[] HighPointsValues = { 1, 3 };
        private int[] LowPointsValues = { 8, 9, 10 };

        public AliceStrategy() : base(StrategyName)
        {

        }

        public Card AddRimemberCard { get; private set; }

        protected override Card OnChoose(IPlayer myself, GameState state)
        {

            Card cardToPlay = CardToPlay(myself, state);


            return cardToPlay;
        }

        protected override void OnWatch(IPlayer myself, GameState state)
        {
            // fai un ciclo per ricordare quali carte sono state giocate

            foreach (var card in state.Dish)
            {
                AddRimemberCard = card;
            }
        }

        private Card CardToPlay(IPlayer myself, GameState state)
        {
            var pointsCard = new Func<Card, bool>(hc => hc.IsLowCard() || hc.IsHighCard());
            var neatCard   = new Func<Card, bool>(hc => hc.IsNoValueCard() && hc.Seed != state.Briscola.Seed);
            var briscola   = new Func<Card, bool>(hc => hc.Seed == state.Briscola.Seed);
            var ace        = new Func<Card, bool>(hc => hc.Seed == state.Briscola.Seed && hc.IsAce());
          //var points     = new Func<Card, bool>(hc => hc.Value.CompareTo(hc => hc.IsPoints) /* il valore della carta uguale al punteggio */);


            Card cardToPlay = new Card();

            if (state.Turn == 0 && !state.Dish.Any()) // quando sei al primo giro e come primo giocatore
            {
                cardToPlay = myself.HandCards.FirstOrDefault(hc => hc.IsHighCard());           // lancia un carico
                    
                if (cardToPlay.IsEmpty())
                {
                    cardToPlay = myself.HandCards.FirstOrDefault(hc => hc.IsNoValueCard());    // lancia un liscio
                }

            }
            else if (state.Turn != 0 && state.Dish.Any()) // quando stai giocando e non sei di prima mano
            {
                // se sul tavolo c'è un carico ma non ci sono briscole prendi salendo con l'asso
                if (!state.Dish.Any(briscola) && state.Dish.Count == state.Players.Count - 1
                    && state.Dish.Any(hc => hc.IsHighCard()) && cardToPlay.IsEmpty())
                {
                    var firstCard = state.Dish.First();
                    cardToPlay = myself.HandCards.FirstOrDefault(hc => hc.Seed == firstCard.Seed && hc.CompareCards(firstCard) == 1);

                } else cardToPlay = myself.HandCards.FirstOrDefault(briscola); // se sul tavolo c'è un carico ma non riesci a prenderlo salendo lancia la briscola più alta che hai


                if (state.Dish.Any(hc => hc.IsLowCard()) && cardToPlay.IsEmpty())   // se sul tavolo c'è una figura lancia una briscola bassa
                {
                    cardToPlay = myself.HandCards.FirstOrDefault(briscola);
                }

                // se sei di ultimo e non ci sono briscole sali della prima carta lanciata 
                if (!state.Dish.Any(briscola) && state.Dish.Count == state.Players.Count - 1
                    && state.Dish.Any(pointsCard) && cardToPlay.IsEmpty())
                {
                    var firstCard = state.Dish.First();
                    cardToPlay = myself.HandCards.FirstOrDefault(hc => hc.Seed == firstCard.Seed && hc.CompareCards(firstCard) == 1);
                }
            }

            // se il cardToPlay è vuoto
            if (cardToPlay.IsEmpty())
            {
                cardToPlay = myself.HandCards.First();
            }



            return cardToPlay;

        }
    }
}
