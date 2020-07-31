using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grappachu.Briscola.Model;

namespace Grappachu.Briscola.Players.suerzg
{
  static class CardExtension
  {

    public static bool IsInsignificant(this Card card)
    {
      return card.Value == 2 || card.Value > 3 && card.Value < 8;
    }

    public static bool IsTrump(this Card card)
    {
      return card.Value > 7;
    }

    public static bool IsLoad(this Card card)
    {
      return card.Value == 3 || card.Value == 1;
    }

  }
}
