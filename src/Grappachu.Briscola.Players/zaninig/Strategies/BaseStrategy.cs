using Grappachu.Briscola.Interfaces;
using Grappachu.Briscola.Model;
using Grappachu.Briscola.Players.Default;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grappachu.Briscola.Players.zaninig.Model;

namespace Grappachu.Briscola.Players.zaninig.Strategies
{
    //public class IlmiorStrategy : StrategyBase
    //{
        //private readonly IUserInterface _userInterface;
        //private readonly Deck _playedCards;
        //private readonly Deck _winnedCards;
        //private readonly Deck _loosedCards;
        //private readonly Deck _unknownCards;

        //private readonly League _league;

        //private void SetLeagueComponents(int turn, IEnumerable<IPlayer> players) {

        //    if (_league.Count == 0)
        //    {
        //        switch (turn)
        //        {
        //            case 0:
        //            case 2:
        //                _league.Add(players.ElementAt(0));
        //                _league.Add(players.ElementAt(2));
        //                break;
        //            case 1:
        //            case 3:
        //                _league.Add(players.ElementAt(1));
        //                _league.Add(players.ElementAt(3));
        //                break;
        //        }
        //    }
        //}

        //protected override Card OnChoose(IPlayer myself, GameState state) {
        //    SetLeagueComponents(state.Dish.Count, state.Players);

        //    var probabilityPoints = new List<MoveValue>();
        //    var strategyCalculator = new StrategyCalculator(state, _league, state.Turn);

        //    var currentDish = new Model.Deck(state.Dish);

        //    if (state.Dish.Count() == 3)
        //    {
        //        var iWinBeforeLastCard = _league.Contains(strategyCalculator.Winner(currentDish));

        //        foreach (var handCard in new Model.Deck(player.HandCards))
        //        {
        //            var afterDish = new Model.Deck(state.Dish);
        //            afterDish.Add(handCard);

        //            probabilityPoints.Add(EvaluateTurno4(strategyCalculator, afterDish, iWinBeforeLastCard));
        //        }

        //        foreach (var probabilityPoint in probabilityPoints)
        //        {
        //            var card = probabilityPoint.Deck.Last();
        //            Debug.WriteLine(string.Format("{0} di {1}: prob.{2} -  punti.{3}", card.Value, card.Seed, probabilityPoint.VictoryProbability, probabilityPoint.EstimatedPoints));
        //        }

        //        var bestCard = BestChoice4(probabilityPoints);
        //        Debug.WriteLine("BEST: " + bestCard.Value + " di " + bestCard.Seed);
        //        Debug.WriteLine("----------------------------------");
        //        return bestCard;
        //    }

        //    // random
        //    return player.HandCards.OrderBy(x => new Guid()).First();
        //}

        //protected override void OnWatch(IPlayer myself, GameState state) {
        //    // la strategia stupida non pensa
        //}

        //public IlmiorStrategy() : base("ilmior")
        //{
        //    _playedCards = new Model.Deck();
        //    _winnedCards = new Model.Deck();
        //    _loosedCards = new Model.Deck();
        //    _unknownCards = new Model.Deck(new[] { "Danari", "Coppe", "Bastoni", "Spade" }, new Range(1, 10));

        //    _league = new League();
        //}
    //}
}
