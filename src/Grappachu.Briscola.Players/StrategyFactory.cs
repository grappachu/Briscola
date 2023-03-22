using System;
using System.Collections.Generic;
using System.Linq;
using Grappachu.Briscola.Interfaces;
using Grappachu.Briscola.Players.depratoa;
using Grappachu.Briscola.Players.Default;
using Grappachu.Briscola.Players.Human;
using Grappachu.Briscola.Players.prenassid;
using Grappachu.Briscola.Players.veronaa;
using Grappachu.Briscola.Players.zaninig.Model;
using Grappachu.Briscola.Players.zaninig.Strategies;
using Grappachu.Briscola.Players.deiannia;
using Grappachu.Briscola.Players.suerzg;
using Grappachu.Briscola.Players.crudelea;

namespace Grappachu.Briscola.Players
{
    public class StrategyFactory : IStrategyFactory
    {
        private readonly Dictionary<string, Func<IStrategy>> _roboFactories;

        public StrategyFactory()
        {
            _roboFactories = new Dictionary<string, Func<IStrategy>>
            {
                {"random", () => new RandomStrategy()},
                {"ilmior", () => new GabStrategy()},
                {GrappachuStrategy.StrategyName, () => new GrappachuStrategy()},
                {AbercioStrategy.StrategyName, () => new AbercioStrategy()},
                {LittleWackosStrategy.StrategyName, () => new LittleWackosStrategy()},
                {Gabry404Strategy.StrategyName, () => new Gabry404Strategy()},
                {CapitanOvvioStrategy.StrategyName, () => new CapitanOvvioStrategy()},
                {AliceStrategy.StrategyName, () => new AliceStrategy()}
            };
        }

        public IStrategy GetHumanStrategy(IUserInterface ui, string playerName)
        {
            return new HumanStrategy(ui, playerName);
        }

        public IEnumerable<IStrategy> GetAllRobots()
        {
            return _roboFactories.Select(x => x.Value.Invoke()).ToArray();
        }

        public IStrategy GetStrategy(string strategyName)
        {
            return _roboFactories[strategyName].Invoke();
        }
    }
}
