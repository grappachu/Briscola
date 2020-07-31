using Grappachu.Briscola.Model;

namespace Grappachu.Briscola.Players.suerzg
{
  static class GameStateExtension
  {

    public static bool PointsInDish(this GameState state)
    {
      foreach (var card in state.Dish)
      {
        if (card.Seed != state.Briscola.Seed && card.Value > 8)
        {
          return true;
        }
      }

      return false;
    }

    public static bool TrumpInDish(this GameState state)
    {
      foreach (var card in state.Dish)
      {
        if (card.Seed != state.Briscola.Seed && (card.Value == 1 || card.Value == 3))
        {
          return true;
        }
      }

      return false;
    }

    public static bool BriscolaInDish(this GameState state)
    {
      foreach (var card in state.Dish)
      {
        if (card.Seed == state.Briscola.Seed)
        {
          return true;
        }
      }

      return false;
    }

  }
}
