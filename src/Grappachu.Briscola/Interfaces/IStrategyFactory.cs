using System.Collections.Generic;

namespace Grappachu.Briscola.Interfaces
{
    public interface IStrategyFactory
    {
        IStrategy GetHumanStrategy(IUserInterface ui);

        IEnumerable<IStrategy> GetAllRobots();

    }
}