using System;
using System.Collections.Generic;
using System.Linq;
using Grappachu.Briscola.Interfaces;
using Grappachu.Briscola.Logic;
using Grappachu.Briscola.Utils;

namespace Grappachu.Briscola.UI.Util.Pairing
{
    public class BergerPairing
    {
        private readonly IStrategyFactory _strategyFactory;

        public BergerPairing(IStrategyFactory strategyFactory, int singleRounds)
        {
            _strategyFactory = strategyFactory;
            SingleRounds = singleRounds;
        }

        /// <summary>
        ///     Ottiene o imposta il numero ti partite per ogni match
        /// </summary>
        private int SingleRounds { get; }

        public void Run()
        {
            var allStrategies = _strategyFactory.GetAllRobots().ToArray();
            var totalTurns = BergerTables.GetRounds(allStrategies.Length);
            int[] allScores = new int[allStrategies.Length];

            Chat.GetUI().Send(string.Empty);
            var detail = 9 == Chat.GetUI().GetInt("Premi invio per contiunare in modalità rapida oppure 9 per la modalità dettagliata", 0);

            Chat.GetUI().Strong("  HA INIZIO IL GRANDE TORNEO DEI ROBOT!");
            Chat.GetUI().Strong($"  Oggi {DateTime.Now:dd MMMM yyyy alle HH:mm}");
            Chat.GetUI().Strong($"  si giocheranno: {totalTurns} turni");
            Chat.GetUI().Strong($"  ad incontri da {SingleRounds} partite");

            for (int i = 0; i < totalTurns; i++)
            {
                Chat.GetUI().Send(string.Empty);
                Chat.GetUI().Strong(" >>>>>>>> TURNO " + (i + 1) + " <<<<<<<<<");
                var games = BergerTables.GetRoundMatches(allStrategies.Length, i);

                foreach (var game in games)
                {
                    var homeIndex = game.Home - 1;
                    var visitorIndex = game.Visitor - 1;

                    // Vittorie a Forfait con numero dispari di giocatori
                    if (allStrategies.Length <= visitorIndex)
                    {
                        allScores[homeIndex] += 2;
                        Chat.GetUI().Send(string.Format(" => {0} vince a forfait", allStrategies.ElementAt(homeIndex).Name));
                        continue;
                    }
                    if (allStrategies.Length <= homeIndex)
                    {
                        allScores[visitorIndex] += 2; // Vittoria a Forfait
                        Chat.GetUI().Send(string.Format(" => {0} vince a forfait", allStrategies.ElementAt(visitorIndex).Name));
                        continue;
                    }
                    // -----

                    var homeStrategy = allStrategies.ElementAt(homeIndex);
                    var visitorStrategy = allStrategies.ElementAt(visitorIndex);

                    Chat.Enabled = detail;

                    int partialHome = 0, partialVisitor = 0;
                    for (int j = 0; j < SingleRounds; j++)
                    {
                        var outcome = DoSingleMatch(Chat.GetUI(), j, homeStrategy, visitorStrategy);
                        switch (outcome)
                        {
                            case 0:
                                partialVisitor += 1;
                                partialHome += 1;
                                break;
                            case 1:
                                partialHome += 2;
                                break;
                            case 2:
                                partialVisitor += 2;
                                break;
                        }
                    }
                    var allGamesOutcome = GetOutcome(partialHome, partialVisitor);

                    Chat.Enabled = true;

                    Chat.GetUI().Send(string.Format(" => {0} vs. {1} | {2:####} - {3:####}",
                        homeStrategy.Name.PadRight(15),
                        visitorStrategy.Name.PadRight(15),
                        partialHome, partialVisitor));

                    switch (allGamesOutcome)
                    {
                        case 0:
                            allScores[homeIndex] += 1;
                            allScores[visitorIndex] += 1;
                            break;
                        case 1:
                            allScores[homeIndex] += 2;
                            break;
                        case 2:
                            allScores[visitorIndex] += 2;
                            break;
                    }
                }
            }

            Chat.GetUI().Send(string.Empty);
            Chat.GetUI().Strong(" >>>>>>>> CLASSIFICA FINALE  <<<<<<<<<");

            var scoreChart = new List<Tuple<string, int>>();
            for (int i = 0; i < allStrategies.Length; i++)
            {
                var s = allStrategies.ElementAt(i);
                var strategyName = s is IRobotStrategy strategy ? $"{strategy.Name} {strategy.Version} by {strategy.Author}" : s.Name;
                scoreChart.Add(new Tuple<string, int>(strategyName, allScores.ElementAt(i)));
            }
            var orderedChart = scoreChart.OrderByDescending(x => x.Item2);
            foreach (var tuple in orderedChart)
            {
                Chat.GetUI().Send(string.Format("  {0} | {1}", tuple.Item2, tuple.Item1));
            }

        }


        /// <summary>
        /// Gioca una singola partita e restituisce il risultato
        /// </summary>
        /// <param name="ui"></param>
        /// <param name="turnNumber"></param>
        /// <param name="homeStrategy"></param>
        /// <param name="visitorStrategy"></param>
        /// <returns>Ritorna
        /// 0 = pareggio
        /// 1 = vince <see cref="homeStrategy"/>
        /// 2 = vince <see cref="visitorStrategy"/>
        /// </returns>
        private int DoSingleMatch(IUserInterface ui, int turnNumber, IStrategy homeStrategy, IStrategy visitorStrategy)
        {
            IStrategy s1, s2;
            bool switched = turnNumber % 2 == 1;
            if (switched)
            {
                s1 = homeStrategy;
                s2 = visitorStrategy;
            }
            else
            {
                s2 = homeStrategy;
                s1 = visitorStrategy;
            }

            var g = new BriscolaGame(new ItalianDeckFactory(), new PlayerFactory());
            var runner = new GameRunner(g, ui, _strategyFactory);
            runner.InitializeRoboMatch(s1.Name, s2.Name, 4);
            var result = runner.Play();

            if (switched)
            {
                return GetOutcome(result.Item1, result.Item2);
            }
            else
            {
                return GetOutcome(result.Item2, result.Item1);
            }
        }

        private static int GetOutcome(int scoreA, int scoreB)
        {
            if (scoreA == scoreB) return 0;
            return (scoreA > scoreB) ? 1 : 2;
        }
    }
}