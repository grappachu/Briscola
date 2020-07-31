using System;
using Grappachu.Briscola.Interfaces;
using Grappachu.Briscola.Model;
using Grappachu.Briscola.Players.Default;
using System.Collections.Generic;
using System.Linq;

namespace Grappachu.Briscola.Players.suerzg
{
  public class Gabry404Strategy : StrategyBase
  {
    public const string StrategyName = "gabry404";

    public Dictionary<string, List<Card>> playedCards;

    public Gabry404Strategy() : base(StrategyName)
    {
      playedCards = new Dictionary<string, List<Card>>();
    }

    protected override Card OnChoose(IPlayer myself, GameState state)
    {
      var specialMovePredicate = new Func<Card, bool>(hc => hc.Seed != state.Briscola.Seed && hc.IsLoad() && !state.BriscolaInDish());
      var lowBriscolaPredicate = new Func<Card, bool>(hc => hc.Seed == state.Briscola.Seed && hc.IsInsignificant());
      var insignificantPredicate = new Func<Card, bool>(hc => hc.Seed != state.Briscola.Seed && hc.IsInsignificant());

      if (playedCards.Count == 0 && myself.HasCard(specialMovePredicate))
      {
        return myself.GetCard(specialMovePredicate);
      }

      if (state.Dish.Count == 0)
      {
        return myself.HasCard(insignificantPredicate) ? myself.GetCard(insignificantPredicate) : myself.GetCard(Predicates.RandomPredicate);
      }

      if ((state.PointsInDish() || state.TrumpInDish()) && myself.CheckAndGetCard(lowBriscolaPredicate, ChooseAlgorithm.Lowest, out Card? card))
      {
        return card.Value;
      }

      return myself.HasCard(insignificantPredicate) ? myself.GetCard(insignificantPredicate) : myself.GetCard(Predicates.RandomPredicate);
    }

    protected override void OnWatch(IPlayer myself, GameState state)
    {
      foreach (var card in state.Dish)
      {
        if (!playedCards.ContainsKey(card.Seed))
        {
          playedCards[card.Seed] = new List<Card>();
        }

        playedCards[card.Seed].Add(card);
        playedCards[card.Seed] = playedCards[card.Seed].OrderBy(point => point.Value).ToList();
      }
    }
  }
}