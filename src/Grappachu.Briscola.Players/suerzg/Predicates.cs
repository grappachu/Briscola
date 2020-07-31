using System;
using Grappachu.Briscola.Model;

namespace Grappachu.Briscola.Players.suerzg
{
  static class Predicates
  {
    public static Func<Card, bool> RandomPredicate
    {
      get { return hc => true; }
    }

    public static Func<Card, bool> InsignificantPredicate
    {
      get { return hc => hc.IsInsignificant(); }
    }

    public static Func<Card, bool> TrumpPredicate
    {
      get { return hc => hc.IsTrump(); }
    }
  }
}