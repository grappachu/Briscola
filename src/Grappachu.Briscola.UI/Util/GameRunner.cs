using System;
using System.Linq;
using Grappachu.Briscola.Interfaces;
using Grappachu.Briscola.Logic;
using Grappachu.Briscola.Model;
using Grappachu.Briscola.Players.Human;

namespace Grappachu.Briscola.UI.Util
{
    public class GameRunner
    {
        private readonly IGame<GameState> _game;
        private readonly IUserInterface _ui;
        private readonly IStrategyFactory _strategyFactory;

        public GameRunner(IGame<GameState> game, IUserInterface ui, IStrategyFactory strategyFactory)
        {
            _game = game;
            _ui = ui;
            _strategyFactory = strategyFactory;
        }


        public void InitializeHumanGame(int totalPlayers)
        {
            _ui.Send($"Briscola a {totalPlayers} giocatori");
            _ui.Strong("Come ti chiami?");
            string yourName = _ui.Get();

            var players = new[] { "Davide", "Nello", "Sergio", "Giulia", "Silvio" };
            var toPlay = players.Where(x => x != yourName)
                .OrderBy(p => Guid.NewGuid()).Take(totalPlayers - 1).ToArray();

            if (totalPlayers == 4)
            {
                _ui.Send("Giocherai assieme a: " + toPlay.ElementAt(1));
            }
            _ui.Send("Buona Partita!\n");

            _game.Join(yourName, new HumanStrategy(_ui));
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



        public void InitializeRoboMatch(string strategy1, string strategy2, int num)
        {
            _ui.Send("Briscola a 4 giocatori");

            var strategyA1 = _strategyFactory.GetStrategy(strategy1);
            var strategyA2 = _strategyFactory.GetStrategy(strategy1);
            var strategyB1 = _strategyFactory.GetStrategy(strategy2);
            var strategyB2 = _strategyFactory.GetStrategy(strategy2);

            var players = new[] { "Bill Gates", "Steve Jobbs", "Mark Zuckerberg", "Alberto Baruzzo", "Alan Turing", "Linus Torvalds", "Elliot Alderson" };
            var toPlay = players.OrderBy(p => Guid.NewGuid()).Take(4).ToArray();

            _ui.Send("Squadra PC: " + toPlay.ElementAt(0) + " e " + toPlay.ElementAt(2));
            _ui.Send("Squadra Mac: " + toPlay.ElementAt(1) + " e " + toPlay.ElementAt(3));

            _ui.Send("Buona Partita!\n");

            _game.Join(toPlay.ElementAt(0), strategyA1);
            _game.Join(toPlay.ElementAt(1), strategyB1);
            _game.Join(toPlay.ElementAt(2), strategyA2);
            _game.Join(toPlay.ElementAt(3), strategyB2);
        }

        public Tuple<int, int> Play()
        {
            _game.Start();
            var totalTurns = 40 / _game.State.Players.Count;
            for (var i = 0; i < totalTurns; i++)
            {
                _ui.Custom(ConsoleColor.Yellow, string.Format("\n===== {0}° Mano ===== (Briscola: {1} di {2})",
                    i + 1, _game.State.Briscola.Value, _game.State.Briscola.Seed));
                _game.PlayHand();

                _game.Refill();
            }

            _ui.Send("\nPARTITA TERMINATA!");
            foreach (var player in _game.State.Players)
            {
                _ui.Send(player.Name + " => " + GameEvaluator.Totalize(player.Stack));
            }


            var pl = _game.State.Players.ToArray();
            int yourScore;
            int opponentScore;
            if (pl.Length == 4)
            {
                yourScore = GameEvaluator.Totalize(pl[0].Stack) + GameEvaluator.Totalize(pl[2].Stack);
                opponentScore = GameEvaluator.Totalize(pl[1].Stack) + GameEvaluator.Totalize(pl[3].Stack);

                _ui.Send(string.Empty);
                _ui.Send(pl[0].Name + " e " + pl[2].Name + " => " + yourScore);
                _ui.Send(pl[1].Name + " e " + pl[3].Name + " => " + opponentScore);
            }
            else
            {
                yourScore = GameEvaluator.Totalize(pl[0].Stack);
                opponentScore = pl.Where(x => x != pl[0])
                    .Select(p => GameEvaluator.Totalize(p.Stack)).Max();
            }


            var pron = _game.State.Players.Count == 4 ? "AVETE" : "HAI";
            if (yourScore == opponentScore)
            {
                _ui.Custom(ConsoleColor.Yellow, "\n >>> PAREGGIO <<<");
            }
            else
            {
                if (yourScore > opponentScore)
                {
                    _ui.Custom(ConsoleColor.Green, "\n >>> " + pron + " VINTO! <<<");
                }
                else
                {
                    _ui.Custom(ConsoleColor.Red, "\n >>> PECCATO! " + pron + " PERSO <<<");
                }
            }

            return new Tuple<int, int>(yourScore, opponentScore);
        }


    }
}