using Grappachu.Briscola.Interfaces;
using Grappachu.Briscola.Model;

namespace Grappachu.Briscola.Logic
{
    public class PlayerFactory : IPlayerFactory
    {
        public IPlayer CreatePlayer(string playerName, IStrategy strategy)
        {
            var p = new Player(strategy, playerName);
            return p;
        }
    }
}