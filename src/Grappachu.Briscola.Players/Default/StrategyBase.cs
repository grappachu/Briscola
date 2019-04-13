using System.ComponentModel;
using Grappachu.Briscola.Interfaces;
using Grappachu.Briscola.Model;

namespace Grappachu.Briscola.Players.Default
{
    /// <summary>
    ///     Rappresenta la classe base per l'implementazione delle strategie automatizzate
    /// </summary>
    public abstract class StrategyBase : IStrategy
    {

        protected StrategyBase(string strategyUniqueName)
        {
            Name = strategyUniqueName;
            IsHuman = false;
        }

        public string Name { get; } 

        public Card Choose(IPlayer player, GameState state)
        {
            return OnChoose(player, state);
        }


        public void Watch(IPlayer player, GameState state)
        {
            OnWatch(player, state);
        }

        public bool IsHuman { get; }

        protected abstract Card OnChoose(IPlayer myself, GameState state);

        protected abstract void OnWatch(IPlayer myself, GameState state);
    }
}