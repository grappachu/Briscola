using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using Grappachu.Briscola.Interfaces;
using Grappachu.Briscola.Logic;
using Grappachu.Briscola.Model;
using Grappachu.Briscola.Players.Default;
using Grappachu.Briscola.Players.prenassid.Utils;
using Grappachu.Briscola.Players.zaninig.Model;

namespace Grappachu.Briscola.Players.zaninig.Strategies
{
    public class GabStrategy : StrategyBase
    {
        private readonly IUserInterface _userInterface;
        private readonly Model.Deck _playedCards;
        private readonly Model.Deck _winnedCards;
        private readonly Model.Deck _loosedCards;
        private readonly Model.Deck _missingCards;

        private readonly League _league;

        public GabStrategy()
            : base("ilmior")
        {
            _playedCards = new Model.Deck();
            _winnedCards = new Model.Deck();
            _loosedCards = new Model.Deck();
            _missingCards = new Model.Deck(new[] { "Danari", "Coppe", "Bastoni", "Spade" }, new Range(1, 10));

            _league = new League();
        }

        protected override Card OnChoose(IPlayer myself, GameState state)
        {            
            var briscola = state.Briscola;
            SetLeagueComponents(state.Dish.Count, state.Players);

            var probabilityPoints = new List<MoveValue>();
            var strategyCalculator = new BriscolaCalculator(state, _league);

            var currentDish = new Model.Deck(state.Dish);

            if (state.Dish.Count == 0)
            {
                // Penalizzare la briscola
                // Meglio non mettere carichi
                // Favorire le carte forti
                Debug.WriteLine("CARTE DI PRIMA MANO ");

                var cardsInfo = new List<CardInfo>();
                foreach (var handCard in new Model.Deck(myself.HandCards))
                {                    
                    var baseWeight = strategyCalculator.GetCardWeight(handCard, _missingCards);
                    var weight = baseWeight;

                    if (handCard.IsCaricoOBriscola(briscola))
                        weight *= (decimal)-1;

                    cardsInfo.Add(new CardInfo() { Card = handCard, Weight = weight });
                    Debug.WriteLine($"Weight del {handCard.Value} di {handCard.Seed} : {baseWeight:000.000} -> {weight:000.000}");
                }

                // Seleziona la carta che massimizza il peso
                var selectedCard = cardsInfo.Where(x => x.Weight == cardsInfo.Max(y => y.Weight))
                                            .Select(x => x.Card)
                                            .First();

                if (myself.HandCards.Count(x => x.IsCaricoOBriscola(briscola)) == myself.HandCards.Count())
                    Debug.WriteLine("Non posso far altro che giocare carico o briscola");

                if (myself.HandCards.Count(x => x.IsCaricoOBriscola(briscola)) < myself.HandCards.Count()
                    && (selectedCard.IsCaricoOBriscola(briscola)))
                    Debug.WriteLine("******* Ho buttato carico o briscola ad cazzum");

                return selectedCard;
            }

            if (state.Dish.Count == 1)
            {
                // Vai su se possibile ma non di carico
                // Non buttare carico
                // Non butto butta la briscola se ci sono pochi punti oppure un'altra briscola
                var firstDishCard = state.Dish.First();

                Debug.WriteLine("CARTE DI SECONDA MANO ");

                var cardsInfo = new List<CardInfo>();
                foreach (var handCard in new Model.Deck(myself.HandCards))
                {
                    var baseWeight = strategyCalculator.GetCardWeight(handCard, _missingCards);
                    var weight = baseWeight;

                    if (handCard.IsBriscola(briscola))
                    {
                        if (!firstDishCard.IsCarico() || firstDishCard.IsBriscola(briscola))
                            weight = Math.Abs(weight) * (decimal)-1;
                    }

                    if (handCard.IsCaricoNonBriscola(briscola) && 
                        (!strategyCalculator.IsBestCardInDish(currentDish, handCard)
                         || strategyCalculator.GetCardWinningProbabilty(handCard, _missingCards) < 1))
                    {
                        weight = Math.Abs(weight) * (decimal)-1;
                    }

                    cardsInfo.Add(new CardInfo() { Card = handCard, Weight = weight });
                    Debug.WriteLine($"Weight del {handCard.Value} di {handCard.Seed} : {baseWeight:000.000} -> {weight:000.000}");
                }

                // Seleziona la carta che massimizza il peso
                var selectedCard = cardsInfo.Where(x => x.Weight == cardsInfo.Max(y => y.Weight))
                    .Select(x => x.Card)
                    .First();              

                if (!selectedCard.IsBriscola(briscola) && selectedCard.IsCarico() && strategyCalculator.GetCardWinningProbabilty(selectedCard, _missingCards) == 1
                    && strategyCalculator.IsBestCardInDish(currentDish, selectedCard))
                    Debug.WriteLine("Butto carico perchè vince");

                if (selectedCard.IsBriscola(briscola) && firstDishCard.GetScore() >= 10 && !firstDishCard.IsBriscola(briscola))
                    Debug.WriteLine("Butto briscola perchè c'è un carico");

                if (myself.HandCards.Count(x => (x.IsBriscola(briscola) && x.GetScore() < 10) || x.IsCarico()) == myself.HandCards.Count())
                    Debug.WriteLine("Non posso far altro che giocare carico o briscola");

                return selectedCard;
            }

            if (state.Dish.Count() == 2)
            {
                Debug.WriteLine("CARTE DI TERZA MANO ");
                
                var iWin = _league.Contains(strategyCalculator.Winner(new Model.Deck(state.Dish)));

                var cardsInfo = new List<CardInfo>();
                foreach (var handCard in new Model.Deck(myself.HandCards))
                {
                    var baseWeight = strategyCalculator.GetCardWeight(handCard, _missingCards);                    
                    var weight = baseWeight;

                    var tempDish = new Model.Deck(state.Dish);
                    var points = tempDish.GetPoints(); 
                    tempDish.Add(handCard); 

                    //prendere di carico
                    var baseProbability = strategyCalculator.GetCardWinningProbabilty(handCard, _missingCards);
                    if (handCard.IsCaricoNonBriscola(state.Briscola) && baseProbability < 1)
                    {
                        weight = Math.Abs(weight) * (decimal)-1;
                    }

                    //prendere di carico di briscola
                    if (handCard.IsCaricoDiBriscola(state.Briscola) && (points < 10 || !strategyCalculator.IsBestCardInDish(tempDish, handCard)))
                    {
                        weight = Math.Abs(weight) * (decimal)-1;
                    }

                    // prende di briscola
                    if (handCard.IsBriscola(state.Briscola) && !handCard.IsCarico() && (points < 10 || !strategyCalculator.IsBestCardInDish(tempDish, handCard)))
                    {
                        weight = Math.Abs(weight) * (decimal)-1;
                    }

                    cardsInfo.Add(new CardInfo() { Card = handCard, Weight = weight });
                    Debug.WriteLine($"Weight del {handCard.Value} di {handCard.Seed} : {baseWeight:000.000} -> {weight:000.000}");
                }

                var selectedCard = cardsInfo.Where(x => x.Weight == cardsInfo.Max(y => y.Weight))
                    .Select(x => x.Card)
                    .First();

                return selectedCard;
            }

            if (state.Dish.Count() == 3)
            {
                Debug.WriteLine("CARTE DI QUARTA MANO");

                var iWin = _league.Contains(strategyCalculator.Winner(new Model.Deck(state.Dish)));

                var cardsInfo = new List<CardInfo>();
                foreach (var handCard in new Model.Deck(myself.HandCards))
                {
                    var baseWeight = strategyCalculator.GetCardWeight(handCard, _missingCards);
                    var weight = baseWeight;

                    // Non buttare briscola
                    if (iWin && handCard.IsCaricoNonBriscola(state.Briscola))
                    {                        
                            weight = Math.Abs(weight) * (decimal)-1;
                    }

                    if (!iWin)
                    {
                        var tempDish = new Model.Deck(state.Dish);
                        var points = tempDish.GetPoints();        
                                                
                        tempDish.Add(handCard);                    

                        //prendere di carico
                        if (handCard.IsCaricoNonBriscola(state.Briscola) && !strategyCalculator.IsBestCardInDish(tempDish, handCard))
                        {
                            weight = Math.Abs(weight) * (decimal)-1;
                        }
                        
                        //prendere di carico di briscola
                        if (handCard.IsCaricoDiBriscola(state.Briscola) && (points < 10 || !strategyCalculator.IsBestCardInDish(tempDish, handCard)))
                        {
                            weight = Math.Abs(weight) * (decimal)-1;
                        }

                        // prende di briscola
                        if (handCard.IsBriscola(state.Briscola) && !handCard.IsCarico() && (points < 10 || !strategyCalculator.IsBestCardInDish(tempDish, handCard)))
                        {
                            weight = Math.Abs(weight) * (decimal)-1;
                        }

                        // prende senza briscola
                        if (!handCard.IsBriscola(state.Briscola) && (points < 6 || !strategyCalculator.IsBestCardInDish(tempDish, handCard)))
                        {
                            weight = Math.Abs(weight) * (decimal)-1;
                        }
                                               
                    }

                    cardsInfo.Add(new CardInfo() { Card = handCard, Weight = weight });
                    Debug.WriteLine($"Weight del {handCard.Value} di {handCard.Seed} : {baseWeight:000.000} -> {weight:000.000}");
                }

                // Seleziona la carta che massimizza il peso
                var selectedCard = cardsInfo.Where(x => x.Weight == cardsInfo.Max(y => y.Weight))
                    .Select(x => x.Card)
                    .First();               
                
                return selectedCard;
            }

            // random
            return myself.HandCards.OrderBy(x => new Guid()).First();
        }

        //private Card BestChoice4(List<MoveValue> probabilityPoints) {
        //    Model.Deck bestDeck;

        //    var bestProbability = probabilityPoints.Max(x => x.VictoryProbability);

        //    var bestCards = probabilityPoints.Where(x => x.VictoryProbability == bestProbability).ToArray();

        //    if (bestProbability > 0)
        //        bestDeck = bestCards.OrderByDescending(x => x.EstimatedPoints).Take(1).First().Deck;
        //    else
        //        bestDeck = bestCards.OrderBy(x => x.EstimatedPoints).Take(1).First().Deck;

        //    return bestDeck.Last();
        //}

        //private MoveValue EvaluateTurno4(StrategyCalculator strategyCalculator, Model.Deck dish, bool iWinBeforeLastCard) {
        //    var result = new MoveValue();
        //    result.Deck = dish;
        //    result.EstimatedPoints = dish.GetPoints();

        //    var iWin = _league.Contains(strategyCalculator.Winner(dish));

        //    if (iWin)
        //    {
        //        // Vittoria 
        //        result.VictoryProbability = (decimal)0.9;

        //        // Vittoria senza briscola
        //        if (dish.All(x => x.Seed != strategyCalculator.Briscola.Seed))
        //            result.VictoryProbability = 1;

        //        // Vittoria con spreco di briscola
        //        if (dish.Last().Seed == strategyCalculator.Briscola.Seed && iWinBeforeLastCard)
        //            result.VictoryProbability = (decimal)0.8;
        //    }
        //    else
        //    {
        //        result.VictoryProbability = (decimal)-0.9;

        //        // Perdita con spreco di briscola
        //        if (dish.Last().Seed == strategyCalculator.Briscola.Seed)
        //            result.VictoryProbability = -1;
        //    }

        //    return result;
        //}

        //private IPlayer GetMyPartner(IPlayer player, GameState state)
        //{
        //    Debug.Assert(state.Players.Count == 4);

        //    var ioIndex = Array.IndexOf(state.Players.ToArray(), player);
        //    var partnerIndex = (ioIndex + 2) % 4;

        //    return state.Players.ElementAt(partnerIndex);
        //}

        private void SetLeagueComponents(int turn, IEnumerable<IPlayer> players)
        {

            if (_league.Count == 0)
            {
                switch (turn)
                {
                    case 0:
                    case 2:
                        _league.Add(players.ElementAt(0));
                        _league.Add(players.ElementAt(2));
                        break;
                    case 1:
                    case 3:
                        _league.Add(players.ElementAt(1));
                        _league.Add(players.ElementAt(3));
                        break;
                }
            }
        }

        protected override void OnWatch(IPlayer myself, GameState state)
        {
            _playedCards.AddRange(state.Dish);
            state.Dish.Any(x => _missingCards.Remove(x));

            // Calcola l'indentificativo del giocatore ma vale solo se la mano è completata.
            int winnerPlayerIdentifier = state.Evaluate();

            if (_league.Any(x => x == state.Players.ElementAt(winnerPlayerIdentifier)))
            {
                _winnedCards.AddRange(state.Dish);
            }
            else
            {
                _loosedCards.AddRange(state.Dish);
            }
        }
    }
}
