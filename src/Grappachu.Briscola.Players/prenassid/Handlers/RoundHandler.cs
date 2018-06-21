using Grappachu.Briscola.Interfaces;
using Grappachu.Briscola.Model;

namespace Grappachu.Briscola.Players.prenassid.Handlers
{
    public abstract class RoundHandler
    {
        private RoundHandler _successor;

        public RoundHandler SetSuccessor(RoundHandler nextHandler)
        {
            _successor = nextHandler;
            return _successor;
        }

        public Card Choose(IPlayer myself, GameState state, Watcher watcher)
        {
            if (OnCanHandle(myself, state)) return OnHandle(myself, state, watcher);

            return _successor.Choose(myself, state, watcher);
        }

        protected abstract Card OnHandle(IPlayer myself, GameState state, Watcher watcher);


        protected abstract bool OnCanHandle(IPlayer myself, GameState state);
    }
}