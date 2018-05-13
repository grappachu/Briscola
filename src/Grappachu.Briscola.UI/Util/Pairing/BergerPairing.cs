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

            Chat.GetUI().Strong("  HA INIZIO IL GRANDE TORNEO DEI ROBOT!");
            Chat.GetUI().Strong("  Sono previsti: " + totalTurns + " scontri");

            for (int i = 0; i < totalTurns; i++)
            {
                Chat.GetUI().Strong(" >>>>>>>> TURNO " + (i + 1) + " <<<<<<<<<");
                var games = BergerTables.GetRoundMatches(allStrategies.Length, i);

                foreach (var game in games)
                {
                    // Vittorie a Forfait con numero dispari di giocatori
                    if (allStrategies.Length <= game.Visitor)
                    {
                        allScores[game.Home] += 2;
                        Chat.GetUI().Strong(string.Format(" => {0} vince a forfait", allStrategies.ElementAt(game.Home)));
                        continue;
                    }
                    if (allStrategies.Length <= game.Home)
                    {
                        allScores[game.Visitor] += 2; // Vittoria a Forfait
                        Chat.GetUI().Strong(string.Format(" => {0} vince a forfait", allStrategies.ElementAt(game.Visitor)));
                        continue;
                    }
                    // -----

                    var homeStrategy = allStrategies.ElementAt(game.Home);
                    var visitorStrategy = allStrategies.ElementAt(game.Visitor);
                    Chat.GetUI().Strong(string.Format(" => {0} vs. {1}", homeStrategy.Name, visitorStrategy.Name));

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
                    Chat.GetUI().Strong(string.Format(" => RISULTATO: {0} ({1}-{2})", allGamesOutcome, partialHome, partialVisitor));

                    switch (allGamesOutcome)
                    {
                        case 0:
                            allScores[game.Home] += 1;
                            allScores[game.Visitor] += 1;
                            break;
                        case 1:
                            allScores[game.Home] += 2;
                            break;
                        case 2:
                            allScores[game.Visitor] += 2;
                            break;
                    }
                }

            }

            Chat.GetUI().Strong(" >>>>>>>> CLASSIFICA FINALE  <<<<<<<<<");
            for (int i = 0; i < allStrategies.Length; i++)
            {
                Chat.GetUI().Send(string.Format("  {0} | {1}", allScores.ElementAt(i), allStrategies.ElementAt(i).Name));
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