using Grappachu.Briscola.Interfaces;
using Grappachu.Briscola.Model;

namespace Grappachu.Briscola.Players.Default
{
    /// <summary>
    /// Rappresenta la classe base per l'implementazione delle strategie automatizzate
    /// </summary>
    public abstract class StrategyBase : IStrategy
    {
        protected StrategyBase()
        {
            IsHuman = false;
        }

        public Card Choose(IPlayer player, GameState state)
        {
            return OnChoose(player, state);
        }

        protected abstract Card OnChoose(IPlayer myself, GameState state);



        public void Watch(IPlayer player, GameState state)
        {
            OnWatch(player, state);
        }

        protected abstract void OnWatch(IPlayer myself, GameState state);

        public bool IsHuman { get; }


    }
}