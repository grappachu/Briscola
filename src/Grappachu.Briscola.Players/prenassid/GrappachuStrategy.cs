using System;
using Grappachu.Briscola.Interfaces;
using Grappachu.Briscola.Model;
using Grappachu.Briscola.Players.Default;
using Grappachu.Briscola.Players.prenassid.Handlers;

namespace Grappachu.Briscola.Players.prenassid
{
    public class GrappachuStrategy : StrategyBase, IRobotStrategy
    {
        public const string StrategyName = "tinto-briss";
        private const string StrategyAuthor = "Davide Prenassi";
        private const string StrategyVersion = "1.0.0";

        private readonly RoundHandler _roundHandler;
        private readonly Watcher _watcher;

        public GrappachuStrategy()
            : base(StrategyName)
        {
            _watcher = new Watcher();
            _roundHandler = new P4R4Handler();
            _roundHandler
                .SetSuccessor(new P4R3Handler())
                .SetSuccessor(new P4R2Handler())
                .SetSuccessor(new P4R1Handler())
                .SetSuccessor(new DefaultRoundHandler());
        }

        public string Author => StrategyAuthor;
        public Version Version => Version.Parse(StrategyVersion);

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