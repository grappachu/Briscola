using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Grappachu.Briscola.Interfaces;
using Grappachu.Briscola.Logic;
using Grappachu.Briscola.Model;
using Grappachu.Briscola.Players.Human;
using Grappachu.Briscola.Utils;

namespace Grappachu.Briscola.Web.Util
{
    public class GameRunner
    {
        private readonly IGame<GameState> _game;
        private readonly IStrategyFactory _strategyFactory;

        public GameRunner(IGame<GameState> game, IStrategyFactory strategyFactory)
        {
            _game = game;
            _strategyFactory = strategyFactory;
        }

        public void InitializeHumanGame(int totalPlayers, string name)
        {
            var teamMate = "";
            var players = new[] { "Davide", "Nello", "Sergio", "Giulia", "Silvio" };
            var toPlay = players.Where(x => x != name).OrderBy(p => Guid.NewGuid()).Take(totalPlayers - 1).ToArray();

            if (totalPlayers == 4)
            {
                teamMate = toPlay.ElementAt(1);
            }

            _game.Join(name, new HumanStrategy(null, name));

            foreach (var pl in toPlay)
            {
                var strategy = GetRandomStrategy();
                _game.Join(pl, strategy);
            }
        }

        private IStrategy GetRandomStrategy()
        {
            return _strategyFactory.GetAllRobots().OrderBy(x => Guid.NewGuid()).First();
        }

        public void InitializeRobotMatch(List<string> strategy1, List<string> strategy2, int num)
        {
            var strategyA1 = _strategyFactory.GetStrategy(strategy1[0]);
            var strategyB1 = _strategyFactory.GetStrategy(strategy2[0]);

            switch (num)
            {
                case 2:
                    _game.Join(strategy1[0], strategyA1);
                    _game.Join(strategy2[0], strategyB1);
                    break;

                case 4:
                    var strategyA2 = _strategyFactory.GetStrategy(strategy2[0]);
                    var strategyB2 = _strategyFactory.GetStrategy(strategy2[1]);
                    var a1Name = strategy1[0];
                    var b1Name = strategy1[1];
                    var a2Name = strategy2[0];
                    var b2Name = strategy2[1];
                    _game.Join(a1Name, strategyA1);
                    _game.Join(b1Name, strategyB1);
                    _game.Join(a2Name, strategyA2);
                    _game.Join(b2Name, strategyB2);
                    break;

                default:
                    throw new NotSupportedException("Only 2 or 4 player game is supported");
            }
        }

        public void InitializeRobotMatch(string strategy1, string strategy2, int num)
        {
            var strategyA1 = _strategyFactory.GetStrategy(strategy1);
            var strategyB1 = _strategyFactory.GetStrategy(strategy2);

            switch (num)
            {
                case 2:
                    _game.Join(strategy1, strategyA1);
                    _game.Join(strategy2, strategyB1);
                    break;

                case 4:
                    var strategyA2 = _strategyFactory.GetStrategy(strategy1);
                    var strategyB2 = _strategyFactory.GetStrategy(strategy2);

                    var a1Name = $"{strategy1}-P1";
                    var a2Name = $"{strategy1}-P2";
                    var b1Name = $"{strategy2}-P1";
                    var b2Name = $"{strategy2}-P2";

                    _game.Join(a1Name, strategyA1);
                    _game.Join(b1Name, strategyB1);
                    _game.Join(a2Name, strategyA2);
                    _game.Join(b2Name, strategyB2);
                    break;

                default:
                    throw new NotSupportedException("Only 2 or 4 player game is supported");
            }
        }

        public Tuple<int, int> Play()
        {
            _game.Start();
            var totalTurns = 40 / _game.State.Players.Count;
            for (int i = 0; i < totalTurns; i++)
            {
                _game.PlayHand();
                _game.Refill();
            }

            var pl = _game.State.Players.ToArray();
            var teams = _game.State.GetTeamNames();

            int yourScore;
            int opponentScore;
            if (pl.Length == 4)
            {
                yourScore = BriscolaUtils.Totalize(pl[0].Stack) + BriscolaUtils.Totalize(pl[2].Stack);
                opponentScore = BriscolaUtils.Totalize(pl[1].Stack) + BriscolaUtils.Totalize(pl[3].Stack);
            }
            else
            {
                yourScore = BriscolaUtils.Totalize(pl[0].Stack);
                opponentScore = pl.Where(x => x != pl[0])
                    .Select(p => BriscolaUtils.Totalize(p.Stack)).Max();
            }

            return new Tuple<int, int>(yourScore, opponentScore);
        }
    }
}