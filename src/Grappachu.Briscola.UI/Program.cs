using System;
using System.Collections.Generic;
using System.Linq;
using Grappachu.Briscola.Interfaces;
using Grappachu.Briscola.Logic;
using Grappachu.Briscola.Players;
using Grappachu.Briscola.UI.Util;
using Grappachu.Briscola.Utils;

namespace Grappachu.Briscola.UI
{
    internal class Program
    {
        private static StrategyFactory _strategyFactory;
        private static IUserInterface _ui = Chat.GetUI();

        private static void Main(string[] args)
        {
            _strategyFactory = new StrategyFactory();

            //  RunGame(ui);
            RunTournament(_ui);

            _ui.Get();
        }

        private static void RunTournament(IUserInterface ui)
        {
            int matchId = 1;

            List<Tuple<int, int>> scores = new List<Tuple<int, int>>();
            for (int i = 0; i < 120; i++)
            {

                var strategy1 = _strategyFactory.GetAllRobots().First();
                var strategy2 = _strategyFactory.GetAllRobots().First();
                var strategy3 = _strategyFactory.GetAllRobots().First();
                var strategy4 = _strategyFactory.GetAllRobots().First();

                var g = new BriscolaGame(new ItalianDeckFactory(), new PlayerFactory(), new GameEvaluator());
                var runner = new GameRunner(g, ui, _strategyFactory);
                runner.InitializeTournamentMatch(strategy1, strategy2, strategy3, strategy4);

                var result = runner.Play();
                scores.Add(result);
            }

            foreach (var score in scores)
            {
                ui.Send("MATCH " + matchId + ": " + score.Item1 + " - " + score.Item2);
                matchId++;
            }

            var team1Score = scores.Count(x => x.Item1 > 60) * 2 + scores.Count(x => x.Item1 == 60);
            var team2Score = scores.Count(x => x.Item2 > 60) * 2 + scores.Count(x => x.Item2 == 60);

            ui.Send("Squadra 1: " + team1Score);
            ui.Send("Squadra 2: " + team2Score);
        }

        private static void RunGame()
        {
            var g = new BriscolaGame(new ItalianDeckFactory(), new PlayerFactory(), new GameEvaluator());
            var runner = new GameRunner(g, _ui, _strategyFactory);

            runner.Initialize();
            runner.Play();
        }
    }
}
