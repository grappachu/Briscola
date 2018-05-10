using System.Collections.Generic;
using Grappachu.Briscola.Interfaces;
using Grappachu.Briscola.Players.Default;
using Grappachu.Briscola.Players.Human;

namespace Grappachu.Briscola.Players
{
    public class StrategyFactory : IStrategyFactory
    {
        public IStrategy GetHumanStrategy(IUserInterface ui)
        {
            return new HumanStrategy(ui);
        }

        public IEnumerable<IStrategy> GetAllRobots()
        {
            return new[]
            {
                 new RandomStrategy()
            };
        }
    }
}
