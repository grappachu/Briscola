﻿using System;
using System.Collections.Generic;
using System.Linq;
using Grappachu.Briscola.Interfaces;
using Grappachu.Briscola.Players.Default;
using Grappachu.Briscola.Players.Human;

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
                {"random2", () => new RandomStrategy()},
                {"random3", () => new RandomStrategy()}
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