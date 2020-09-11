using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Grappachu.Briscola.Interfaces;
using Grappachu.Briscola.Logic;
using Grappachu.Briscola.UI.Util.Pairing;

namespace Grappachu.Briscola.Web.Util
{
    public class BergerPairing
    {
        private readonly IStrategyFactory _strategyFactory;

        public BergerPairing(IStrategyFactory strategyFactory, int singleRounds)
        {
            _strategyFactory = strategyFactory;
            SingleRounds = singleRounds;
        }

        private int SingleRounds { get; }

        public IOrderedEnumerable<Tuple<string, int>> Run()
        {
            var allStrategies = _strategyFactory.GetAllRobots().ToArray();
            var totalTurns = BergerTables.GetRounds(allStrategies.Length);
            int[] allScores = new int[allStrategies.Length];

            for (int i = 0; i < totalTurns; i++)
            {
                var games = BergerTables.GetRoundMatches(allStrategies.Length, i);

                foreach (var game in games)
                {
                    var homeIndex = game.Home - 1;
                    var visitorIndex = game.Visitor - 1;

                    // Vittorie a Forfait con numero dispari di giocatori
                    if (allStrategies.Length <= visitorIndex)
                    {
                        allScores[homeIndex] += 2;
                        continue;
                    }
                    if (allStrategies.Length <= homeIndex)
                    {
                        allScores[visitorIndex] += 2; // Vittoria a Forfait
                        continue;
                    }

                    var homeStrategy = allStrategies.ElementAt(homeIndex);
                    var visitorStrategy = allStrategies.ElementAt(visitorIndex);


                    int partialHome = 0, partialVisitor = 0;
                    for (int j = 0; j < SingleRounds; j++)
                    {
                        var outcome = DoSingleMatch(j, homeStrategy, visitorStrategy);
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
            var scoreChart = new List<Tuple<string, int>>();
            for (int i = 0; i < allStrategies.Length; i++)
            {
                var s = allStrategies.ElementAt(i);
                var strategyName = s is IRobotStrategy strategy ? $"{strategy.Name} {strategy.Version} by {strategy.Author}" : s.Name;
                scoreChart.Add(new Tuple<string, int>(strategyName, allScores.ElementAt(i)));
            }
            var orderedChart = scoreChart.OrderByDescending(x => x.Item2);

            return orderedChart;
        }
        private int DoSingleMatch(int turnNumber, IStrategy homeStrategy, IStrategy visitorStrategy)
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
            var runner = new GameRunner(g, _strategyFactory);
            runner.InitializeRobotMatch(s1.Name, s2.Name, 4);
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