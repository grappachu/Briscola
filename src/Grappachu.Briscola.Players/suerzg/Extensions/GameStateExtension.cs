using Grappachu.Briscola.Model;

namespace Grappachu.Briscola.Players.suerzg.Extensions
{
  static class GameStateExtension
  {

    /// <summary>
    /// Controlla se nel piatto ci sono punti
    /// </summary>
    /// <param name="state">Il GameState chiamante</param>
    /// <returns></returns>
    public static bool PointsInDish(this GameState state)
    {
      foreach (var card in state.Dish)
      {
        if (card.Seed != state.Briscola.Seed && card.Value > 7)
        {
          return true;
        }
      }

      return false;
    }

    /// <summary>
    /// Controlla se nel piatto ci sono carichi
    /// </summary>
    /// <param name="state">Il GameState chiamante</param>
    /// <returns></returns>
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

    /// <summary>
    /// Controlla se nel piatto c'è una briscola
    /// </summary>
    /// <param name="state">Il GameState chiamante</param>
    /// <returns></returns>
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

    /// <summary>
    /// Prende il fattore di rischio dettato da quale turno giocando
    /// </summary>
    /// <param name="state">Il GameState chiamante</param>
    /// <returns></returns>
    public static double ConversionRate(this GameState state)
    {
      return (1 - 0.25 * (state.Turn + 1)) * 100;
    }

  }
}
