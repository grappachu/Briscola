using Grappachu.Briscola.Interfaces;
using Grappachu.Briscola.Model;
using Grappachu.Briscola.Players.Default;
using Grappachu.Briscola.Players.prenassid.Handlers;

namespace Grappachu.Briscola.Players.prenassid
{
    public class GrappachuStrategy : StrategyBase
    {
        public const string StrategyName = "tinto-briss";
        private readonly RoundHandler _roundHandler;
        private readonly Watcher _watcher;

        public GrappachuStrategy()
            : base(StrategyName)
        {
            _watcher = new Watcher();
            _roundHandler = new P4R4Handler();
            _roundHandler
                .SetSuccessor(new DefaultRoundHandler());
        }

        protected override Card OnChoose(IPlayer myself, GameState state)
        {
            return _roundHandler.Choose(myself, state, _watcher);
        }

        protected override void OnWatch(IPlayer myself, GameState state)
        {
            _watcher.Watch(myself, state);
        }
    }
}
