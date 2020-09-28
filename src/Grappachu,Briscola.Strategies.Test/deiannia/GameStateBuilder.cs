using Grappachu.Briscola.Interfaces;
using Grappachu.Briscola.Model;
using Grappachu.Briscola.Players.Default;
using System.Collections.Generic;

namespace Grappachu.Briscola.Strategies.Test.deiannia
{
    public class GameStateBuilder
    {
        private readonly GameState _gameState;

        private GameStateBuilder()
        {
        }

        public static GameStateBuilder GetBuilder()
        {
            return new GameStateBuilder();
        }

        public GameState Build(IStrategy strategy, Card briscola, int nPlayers = 2)
        {
            var players = new List<IPlayer>();
            players.Add(new Player(strategy, "CapitanOvvio1"));
            players.Add(new Player(new RandomStrategy(), "Random1"));
            if (nPlayers == 4)
            {
                players.Add(new Player(strategy, "CapitanOvvio2"));
                players.Add(new Player(new RandomStrategy(), "Random2"));
            }

            return new GameState(players, briscola);
        }
    }
}
