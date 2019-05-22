using System;
using System.Diagnostics;
using System.Linq;
using Grappachu.Briscola.Interfaces;
using Grappachu.Briscola.Logic;
using Grappachu.Briscola.Model;
using Grappachu.Briscola.Players.Human;
using Grappachu.Briscola.Utils;

namespace Grappachu.Briscola.UI.Util
{
    public class GameRunner
    {
        private readonly IGame<GameState> _game;
        private readonly IStrategyFactory _strategyFactory;
        private readonly IUserInterface _ui;

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
            var yourName = _ui.Get();

            var players = new[] { "Davide", "Nello", "Sergio", "Giulia", "Silvio" };
            var toPlay = players.Where(x => x != yourName)
                .OrderBy(p => Guid.NewGuid()).Take(totalPlayers - 1).ToArray();

            if (totalPlayers == 4)
            {
                _ui.Send("Giocherai assieme a: " + toPlay.ElementAt(1));
            }

            _ui.Send("Buona Partita!\n");

            _game.Join(yourName, new HumanStrategy(_ui, yourName));
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


        /// <summary>
        ///     Inizializza un gioco tra due strategie
        /// </summary>
        /// <param name="strategy1"></param>
        /// <param name="strategy2"></param>
        /// <param name="num">Il numero di giocatori della partita. Sono supportati solo 2 o 4 giocatori</param>
        public void InitializeRoboMatch(string strategy1, string strategy2, int num)
        {
            _ui.Send($"Briscola a {num} giocatori");

            var strategyA1 = _strategyFactory.GetStrategy(strategy1);
            var strategyB1 = _strategyFactory.GetStrategy(strategy2);

            switch (num)
            {
                case 2:
                    _ui.Send($"{strategy1} vs. {strategy2}");
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

                    _ui.Send($"Squadra {strategy1}: {a1Name} e {a2Name}");
                    _ui.Send($"Squadra {strategy2}: {b1Name} e {b2Name}");

                    _game.Join(a1Name, strategyA1);
                    _game.Join(b1Name, strategyB1);
                    _game.Join(a2Name, strategyA2);
                    _game.Join(b2Name, strategyB2);
                    break;
                default:
                    throw new NotSupportedException("Only 2 or 4 player game is supported");
            }

            _ui.Send("Buona Partita!\n");
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
                Debug.WriteLine("-----------------------");
            }

            _ui.Send("\nPARTITA TERMINATA!");
            foreach (var player in _game.State.Players)
            {
                _ui.Send(player.Name + " => " + BriscolaUtils.Totalize(player.Stack));
            }

            var pl = _game.State.Players.ToArray();
            var teams = _game.State.GetTeamNames();
            
            int yourScore;
            int opponentScore;
            if (pl.Length == 4)
            {
                yourScore = BriscolaUtils.Totalize(pl[0].Stack) + BriscolaUtils.Totalize(pl[2].Stack);
                opponentScore = BriscolaUtils.Totalize(pl[1].Stack) + BriscolaUtils.Totalize(pl[3].Stack);

                _ui.Send(string.Empty);
                _ui.Send(teams.ElementAt(0) + " => " + yourScore);
                _ui.Send(teams.ElementAt(1) + " => " + opponentScore);
            }
            else
            {
                yourScore = BriscolaUtils.Totalize(pl[0].Stack);
                opponentScore = pl.Where(x => x != pl[0])
                    .Select(p => BriscolaUtils.Totalize(p.Stack)).Max();
            }


            var pron = _game.State.Players.Count == 4 ? "AVETE" : "HAI";
            if (yourScore == opponentScore)
            {
                _ui.Custom(ConsoleColor.Yellow, "\n >>> PAREGGIO <<<");
            }
            else
            {
                if (_game.State.IsRobotMatch())
                {
                    _ui.Custom(ConsoleColor.Green,
                        "\n >>> VINCITORI: " + (yourScore > opponentScore ? teams.ElementAt(0) : teams.ElementAt(1)) + " <<<");
                }
                else
                {
                    // Stampa i risultati per gli umani
                    if (yourScore > opponentScore)
                    {
                        _ui.Custom(ConsoleColor.Green, "\n >>> " + pron + " VINTO! <<<");
                    }
                    else
                    {
                        _ui.Custom(ConsoleColor.Red, "\n >>> PECCATO! " + pron + " PERSO <<<");
                    }
                }
            }


            return new Tuple<int, int>(yourScore, opponentScore);
        }


    }
}