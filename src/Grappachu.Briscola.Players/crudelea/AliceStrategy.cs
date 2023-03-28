using Grappachu.Briscola.Interfaces;
using Grappachu.Briscola.Model;
using Grappachu.Briscola.Players.Default;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Grappachu.Briscola.Players.crudelea
{
    public class AliceStrategy : StrategyBase
    {
        public static readonly string StrategyName = "AliExpress";
       
        private IEnumerable<Card> _deck;

        public AliceStrategy() : base(StrategyName)
        {
            _deck = new List<Card>();
        }

        public IEnumerable<Card> Deck { get => _deck;  }

        protected override Card OnChoose(IPlayer myself, GameState state)
        {
            Card cardToPlay = CardToPlay(myself, state);

            return cardToPlay;
        }

        protected override void OnWatch(IPlayer myself, GameState state)
        {
            // fai un ciclo per ricordare quali carte sono già state giocate 

            foreach (var dishCard in state.Dish)
            {

                if (!_deck.Contains(dishCard))
                {
                    _deck = _deck.Append(dishCard);
                }
            }

        }

        private Card CardToPlay(IPlayer myself, GameState state)
        {
            var pointsCard      = new Func<Card, bool>(hc => hc.IsLowCard() || hc.IsHighCard());
            var neatCard        = new Func<Card, bool>(hc => hc.IsNoValueCard() && hc.Seed != state.Briscola.Seed);
            var briscola        = new Func<Card, bool>(hc => hc.Seed == state.Briscola.Seed);
            var noValueBriscola = new Func<Card, bool>(hc => hc.Seed == state.Briscola.Seed && hc.IsNoValueCard());
            var ace             = new Func<Card, bool>(hc => hc.Seed == state.Briscola.Seed && hc.IsAce());
            var three           = new Func<Card, bool>(hc => hc.Seed == state.Briscola.Seed && hc.IsThree());
            var king            = new Func<Card, bool>(hc => hc.Seed == state.Briscola.Seed && hc.IsKing());
 

            Card cardToPlay = new Card();

            // ordinamento delle carte che ho in mano
            var cards = myself.HandCards.ToList();
            cards.Sort(delegate (Card x, Card y)
            {
                return x.CompareCards(y);
            });
            cardToPlay = cards.FirstOrDefault();


            if (state.Turn == 0 && !state.Dish.Any()) // quando sei al primo giro e come primo giocatore
            {
                cardToPlay = cards.FirstOrDefault(hc => hc.IsHighCard());           // lancia un carico

                if (cardToPlay.IsEmpty())
                {
                    cardToPlay = cards.FirstOrDefault(hc => hc.IsNoValueCard());    // lancia un liscio
                }

            }
            else if (state.Turn != 0 && state.Dish.Any()) // quando stai giocando e non sei di prima mano
            {

                // se sul tavolo non c'è briscola, c'è un carico e sono di ultima, salgo sul seme della prima carta giocata
                if (!state.Dish.Any(briscola) && state.Dish.Count == state.Players.Count - 1
                    && state.Dish.Any(hc => hc.IsHighCard()))
                {
                    var firstCard = state.Dish.First();
                    cardToPlay = cards.FirstOrDefault(hc => hc.Seed == firstCard.Seed && hc.CompareCards(firstCard) == 1);
                }


                // se sul tavolo c'è una figura, prendo con una briscola piccola
                if (state.Dish.Any(hc => hc.IsLowCard()) && cardToPlay.IsEmpty())   
                {
                    cards = cards.Where(noValueBriscola).ToList();
                    
                    cardToPlay = cards.FirstOrDefault();
                }


                // se sei di ultimo e non ci sono briscole sali della prima carta lanciata 
                if (!state.Dish.Any(briscola) && state.Dish.Count == state.Players.Count - 1
                    && state.Dish.Any(pointsCard) && cardToPlay.IsEmpty())
                {
                    var firstCard = state.Dish.First();
                    cardToPlay = cards.FirstOrDefault(hc => hc.Seed == firstCard.Seed && hc.CompareCards(firstCard) == 1);
                }


                // se sul tavolo c'è un carico, prendo con l'asso                       isEmpty() deve rimanere 
                if (state.Dish.Any(hc => hc.IsHighCard()) && cardToPlay.IsEmpty())
                {
                    cards = cards.Where(ace).ToList();
                    
                    cardToPlay = cards.LastOrDefault();

                // se sul tavolo c'è un carico, l'asso è già stato giocato, prendo con il tre
                }
                else if (state.Dish.Any(hc => hc.IsHighCard()) && cardToPlay.IsEmpty())
                {
                    cards = cards.Where(three).ToList();

                    cardToPlay = cards.LastOrDefault();

                // se sul tavolo c'è un carico, l'asso e il tre sono già stati giocati, prendi con il re
                }else if (state.Dish.Any(hc => hc.IsHighCard()) && cardToPlay.IsEmpty())
                {
                    cards = cards.Where(king).ToList();
                    
                    cardToPlay = cards.LastOrDefault();

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
