using Grappachu.Briscola.Interfaces;
using Grappachu.Briscola.Model;
using System;
using System.Linq;
using Grappachu.Briscola.Players.depratoa;

namespace Grappachu.Briscola.Players.suerzg
{
  static class PlayerExtension
  {
    public static bool HasCard(this IPlayer myself, Func<Card, bool> predicate)
    {
      return myself.HandCards.Any(predicate);
    }

    public static Card GetCard(this IPlayer myself, Func<Card, bool> predicate, ChooseAlgorithm type = ChooseAlgorithm.Random) // TODO same seed
    {
      var cards = myself.HandCards.Where(predicate).ToList();
      switch (type)
      {
        case ChooseAlgorithm.Highest:
          cards.Sort((a, b) => a.Value < b.Value ? 1 : -1);
          return cards.ElementAt(0);
        case ChooseAlgorithm.Lowest:
          cards.Sort((a, b) => a.Value < b.Value ? -1 : 1);
          return cards.ElementAt(0);
        default:
          var index = new Random().Next(0, cards.Count) % cards.Count;
          return cards.ElementAt(index);
      }
    }

    public static bool CheckAndGetCard(this IPlayer myself, Func<Card, bool> predicate, ChooseAlgorithm type, out Card? choosen)
    {
      if (HasCard(myself, predicate))
      {
        choosen = GetCard(myself, predicate, type);
        return true;
      }

      choosen = null;
      return false;
    }

  }
}
