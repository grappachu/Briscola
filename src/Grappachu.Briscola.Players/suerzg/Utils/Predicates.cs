using Grappachu.Briscola.Model;
using System;

namespace Grappachu.Briscola.Players.suerzg
{
  static class Predicates
  {
    public static Func<Card, bool> RandomPredicate
    {
      get { return hc => true; }
    }

    public static Func<Card, bool> TrumpPredicate
    {
      get { return hc => hc.IsPoints(); }
    }
  }
}