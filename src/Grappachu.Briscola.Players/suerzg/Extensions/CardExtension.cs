using Grappachu.Briscola.Model;

namespace Grappachu.Briscola.Players.suerzg.Extensions
{
  static class CardExtension
  {

    public static bool IsInsignificant(this Card card)
    {
      return card.Value == 2 || card.Value > 3 && card.Value < 8;
    }

    public static bool IsPoints(this Card card)
    {
      return card.Value > 7;
    }

    public static bool IsTrump(this Card card)
    {
      return card.Value == 3 || card.Value == 1;
    }

    public static int GetValueInPoints(this Card card)
    {
      return card.Value == 1 ? 11 :
        card.Value == 3 ? 10 :
        card.Value == 8 ? 2 :
        card.Value == 9 ? 3 :
        card.Value == 10 ? 4 :
        0;
    }

  }
}
