using Grappachu.Briscola.Model;

namespace Grappachu.Briscola.Interfaces
{
    public interface IGameEvaluator
    {
        int Assign(GameState state);
    }
}