using System;
using System.Collections.Generic;
using System.Linq;
using Grappachu.Briscola.Interfaces;
using Grappachu.Briscola.Model;
using Grappachu.Briscola.Players.zaninig.Model;

namespace Grappachu.Briscola.Players.zaninig.Strategies
{
    public class BriscolaCalculator
    {
        private readonly GameState _gameState;
        private readonly League _league;

        public BriscolaCalculator(GameState gameState, League league)
        {
            _gameState = gameState;
            _league = league;
        }

        /// <summary>
        /// Ritorna il peso della carta. Il peso è proporzionale ai punti della carta
        /// per alla probabilità di essere una carta vincente (che prende con più probabilità)
        /// </summary>
        /// <remarks>Usata per dare un peso alla singola carta calcolata al momento di utilizzo</remarks>
        /// <param name="card"></param>
        /// <param name="missingCards"></param>
        /// <returns></returns>
        public decimal GetCardWeight(Card card, Deck missingCards)
        {
            var cardWeight = card.BaseWeight();
          
            return GetCardWinningProbabilty(card, missingCards) * cardWeight;
        }

       /// <summary>
       /// Ritorna la probabilità di vittoria della carta 1 se nessuna altra delle carte rimanenti la supera
       /// </summary>
       /// <param name="card"></param>
       /// <param name="missingCards"></param>
       /// <returns></returns>
        public decimal GetCardWinningProbabilty(Card card, Deck missingCards)
        {
            var cardPositionWeights = missingCards.Count(x => x.Seed == card.Seed && Order(x) > Order(card));
            if (card.Seed != _gameState.Briscola.Seed)
            {
                cardPositionWeights += missingCards.Count(x => x.Seed == _gameState.Briscola.Seed);
            }

            var numeroDiCarteMancanti = (decimal)missingCards.Count;

            var weight = 1 * (numeroDiCarteMancanti - cardPositionWeights) / numeroDiCarteMancanti;

            return weight;
        }

       /// <summary>
       /// Ritorna true se la carta è la carta che vince la mano corrente
       /// </summary>
       /// <param name="dish"></param>
       /// <param name="card"></param>
       /// <returns></returns>
       public bool IsBestCardInDish(Deck dish, Card card)
       {
           var winCard = BestCardInDish(dish); // cerco la carta vincente della mano

           return winCard.Seed == card.Seed && winCard.Value == card.Value;
       }

       private Card BestCardInDish(Deck dish)
       {
           Card winCard; // cerco la carta vincente della mano

           if (dish.Any(x => x.Seed == _gameState.Briscola.Seed))
           {
               // se ci sono briscole sarà quella di maggior valore
               winCard = dish.Where(x => x.Seed == _gameState.Briscola.Seed).OrderByDescending(Order).First();
           }
           else
           {
               // altrimenti guardo il primo seme giocato
               var first = dish.First().Seed;
               winCard = dish.Where(x => x.Seed == first).OrderByDescending(Order).First();
           }

           return winCard;
       }

        public IPlayer Winner(Deck dish) {
            var winCard = BestCardInDish(dish); // cerco la carta vincente della mano
           
            // dall'ordine di gioco delle carte e dal giocatore di turno 
            // determino chi ha giocato la carta vincente 
            var winIdx = dish.IndexOf(winCard);
            var playerIdx = (winIdx + _gameState.Turn) % _gameState.Players.Count();
            return _gameState.Players.ElementAt(playerIdx);
        }

        private static int Order(Card card) {
            if (card.Value == 1)
                return 12;
            if (card.Value == 3)
                return 11;
            return card.Value;
        }
    }
}
