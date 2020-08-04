using Grappachu.Briscola.Interfaces;
using Grappachu.Briscola.Model;
using Grappachu.Briscola.Players.Default;
using Grappachu.Briscola.Players.suerzg.Extensions;
using Grappachu.Briscola.Players.suerzg.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Grappachu.Briscola.Players.suerzg
{
  public class Gabry404Strategy : StrategyBase
  {
    public const string StrategyName = "gabry404";

    public Dictionary<string, List<Card>> PlayedCards;

    public Gabry404Strategy() : base(StrategyName)
    {
      PlayedCards = new Dictionary<string, List<Card>>();
    }

    protected override Card OnChoose(IPlayer myself, GameState state)
    {
      Card card = GetCardToPlay(myself, state);

      AddCardToPlayedCards(card);

      return card;
    }

    protected override void OnWatch(IPlayer myself, GameState state)
    {
      foreach (var card in state.Dish)
      {
        AddCardToPlayedCards(card);
      }
    }

    private Card GetCardToPlay(IPlayer myself, GameState state)
    {
      var specialMove = new Func<Card, bool>(hc => hc.Seed != state.Briscola.Seed && hc.IsTrump() && !state.BriscolaInDish());
      var lowBriscola = new Func<Card, bool>(hc => hc.Seed == state.Briscola.Seed && hc.IsInsignificant());
      var highBriscola = new Func<Card, bool>(hc => hc.Seed == state.Briscola.Seed && hc.IsPoints());
      var highestBriscola = new Func<Card, bool>(hc => hc.Seed == state.Briscola.Seed && hc.IsPoints());
      var insignificant = new Func<Card, bool>(hc => hc.Seed != state.Briscola.Seed && hc.IsInsignificant());

      if (PlayedCards.Count == 0 && myself.HasCard(specialMove))
      {
        return myself.GetCard(specialMove);
      }

      if (state.Dish.Count == 0)
      {
        return myself.HasCard(insignificant) ? myself.GetCard(insignificant) : myself.GetCard(Predicates.RandomPredicate);
      }

      if ((state.PointsInDish() || state.TrumpInDish()))
      {
        Card? c;
        if (!state.BriscolaInDish() &&
            myself.CheckAndGetCard(new List<Func<Card, bool>> { lowBriscola }, ChooseAlgorithm.Lowest, out c))
        {
          return c.Value;
        }

        if (myself.CheckAndGetCard(new List<Func<Card, bool>> { highBriscola, highestBriscola }, ChooseAlgorithm.Lowest, out c))
        {
          return c.Value;
        }
      }

      return myself.HasCard(insignificant) ? myself.GetCard(insignificant) : myself.GetCard(Predicates.RandomPredicate);
    }

    private void AddCardToPlayedCards(Card card)
    {
      if (!PlayedCards.ContainsKey(card.Seed))
      {
        PlayedCards[card.Seed] = new List<Card>();
      }

      if (!PlayedCards[card.Seed].Contains(card))
      {
        PlayedCards[card.Seed].Add(card);
        PlayedCards[card.Seed] = PlayedCards[card.Seed].OrderBy(point => point.Value).ToList();
      }
    }

    private Card? CheckHighestBriscola(GameState state, IPlayer me)
    {
      return null;
    }

    private bool AceInDeck(string seed)
    {
      return PlayedCards.ContainsKey(seed) && PlayedCards[seed].Contains(new Card(seed, 1));
    }

    private bool ThreeInDeck(string seed)
    {
      return PlayedCards.ContainsKey(seed) && PlayedCards[seed].Contains(new Card(seed, 1));
    }

    private bool WinningProbability(GameState state)
    {
      int remainingPoints = 120;
      int dishPoints = 0;
      int cards = 0;

      foreach (var card in PlayedCards)
      {
        remainingPoints += card.Value.Select(c => c.GetValueInPoints()).Aggregate((a, b) => a + b);
        cards++;
      }

      foreach (var card in state.Dish)
      {
        dishPoints += card.GetValueInPoints();
      }

      int threshold = remainingPoints / (cards / state.Players.Count);

      return dishPoints > threshold;
    }
  }
}