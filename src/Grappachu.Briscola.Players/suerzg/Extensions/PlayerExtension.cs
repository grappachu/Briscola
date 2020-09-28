using Grappachu.Briscola.Interfaces;
using Grappachu.Briscola.Model;
using Grappachu.Briscola.Players.suerzg.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Grappachu.Briscola.Players.suerzg.Extensions
{
  static class PlayerExtension
  {
    /// <summary>
    /// Controlla se esiste una carta secondo uno specifico criterio
    /// </summary>
    /// <param name="myself">Il giocatore chiamante</param>
    /// <param name="predicate">Il criterio da soddisfare</param>
    /// <returns>Se il criterio è soddisfatto o meno</returns>
    public static bool HasCard(this IPlayer myself, Func<Card, bool> predicate)
    {
      return myself.HandCards.Any(predicate);
    }

    /// <summary>
    /// Restituisce una carta in base ad uno specifico criterio in base al algoritmo scelto 
    /// </summary>
    /// <param name="myself">Il giocatore chiamante</param>
    /// <param name="predicate">Il criterio da soddisfare</param>
    /// <param name="type">Il tipo di algoritmo da utilizzare <seealso cref="ChooseAlgorithm" /></param>
    /// <returns>Se il criterio è soddisfatto o meno</returns>
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

    public static bool CheckAndGetCard(this IPlayer myself, List<Func<Card, bool>> predicates, ChooseAlgorithm type, out Card? chosen)
    {
      foreach (var predicate in predicates)
      {
        if (HasCard(myself, predicate))
        {
          chosen = GetCard(myself, predicate, type);
          return true;
        }
      }

      chosen = null;
      return false;
    }

  }
}
