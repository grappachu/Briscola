using System.Collections.Generic;

namespace Grappachu.Briscola.Interfaces
{
    public interface IStrategyFactory
    {
        IStrategy GetHumanStrategy(IUserInterface ui, string playerName);

        IEnumerable<IStrategy> GetAllRobots();

        IStrategy GetStrategy(string strategyName);
    }
}