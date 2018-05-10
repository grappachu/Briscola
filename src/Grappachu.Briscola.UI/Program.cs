using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        private static IUserInterface _ui;

        private static void Main(string[] args)
        {
            _strategyFactory = new StrategyFactory();
            _ui = Chat.GetUI();
            var version = Assembly.GetEntryAssembly().GetName().Version;

            _ui.Send("===============================================================================");
            _ui.Send("---------------------------------- Briscola -----------------------------------");
            _ui.Send("-------------------------------- v. " + version + " ---------------------------------");
            _ui.Send("===============================================================================");
            string cmd;
            do
            {
                _ui.Send("");
                _ui.Send("Cosa vuoi fare?");
                _ui.Send("");
                _ui.Send("  (1) | Partita singola a 2 giocatori (umano/pc)");
                _ui.Send("  (2) | Partita singola a 4 giocatori (umano/pc)");
                _ui.Send("  (3) | Partita singola a 4 tra robot");
                _ui.Send("  (4) | Gran Torneo dei Robot");
                _ui.Send("  (x) | Esci");

                cmd = _ui.Get();

                switch (cmd)
                {
                    case "1":
                        Run2PlayerGame();
                        break;
                    case "2":
                        Run4PlayerGame();
                        break;
                    case "3":
                        RunTournament(_ui);
                        break;
                    case "4":
                        _ui.Strong("NON IMPLEMENTATO");
                        //   RunTournament(_ui);
                        break;
                }

            } while (!IsExit(cmd));

        }

        private static bool IsExit(string command)
        {
            return string.Equals(command, "Q", StringComparison.OrdinalIgnoreCase);
        }

        private static void RunTournament(IUserInterface ui, int matches = 1000)
        {
            int matchId = 1;

            List<Tuple<int, int>> scores = new List<Tuple<int, int>>();
            for (int i = 0; i < 120; i++)
            {

                var g = new BriscolaGame(new ItalianDeckFactory(), new PlayerFactory(), new GameEvaluator());
                var runner = new GameRunner(g, ui, _strategyFactory);
                runner.InitializeRoboMatch("random", "random", 4);
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

        private static void Run2PlayerGame()
        {
            var g = new BriscolaGame(new ItalianDeckFactory(), new PlayerFactory(), new GameEvaluator());
            var runner = new GameRunner(g, _ui, _strategyFactory);
            runner.InitializeHumanGame(2);
            runner.Play();
        }

        private static void Run4PlayerGame()
        {
            var g = new BriscolaGame(new ItalianDeckFactory(), new PlayerFactory(), new GameEvaluator());
            var runner = new GameRunner(g, _ui, _strategyFactory);
            runner.InitializeHumanGame(4);
            runner.Play();
        }
    }
}
