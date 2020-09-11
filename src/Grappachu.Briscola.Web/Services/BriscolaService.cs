using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Grappachu.Briscola.Exceptions;
using Grappachu.Briscola.Interfaces;
using Grappachu.Briscola.Logic;
using Grappachu.Briscola.Model;
using Grappachu.Briscola.Players;
using Grappachu.Briscola.Web.Models;
using Grappachu.Briscola.Web.Util;

namespace Grappachu.Briscola.Web.Services
{
    public class BriscolaService
    {
        private static int _defaultNumberOfGames = 100;
        private static int _defaultNumberOfMatchesPerGame = 1000;
        public static StrategyFactory _strategyFactory = new StrategyFactory();
        public StartNewGame InitGame()
        {
            StartNewGame startNewGame = new StartNewGame();
            startNewGame.NumberOfGames = _defaultNumberOfGames;
            startNewGame.NumberOfMatchesPerGame = _defaultNumberOfMatchesPerGame;
            startNewGame.GameTypes = GetGameTypes();
            return startNewGame;
        }

        public StartNewGame StartGame(StartNewGame startNewGame)
        {
            switch (startNewGame.CurrentGameType)
            {
                case GameType.DirectMatch:
                    if (startNewGame.NumberOfGames == 0)
                    {
                        startNewGame.NumberOfGames = _defaultNumberOfGames;
                    }
                    startNewGame.FirstListOfRobots = GetAllRobots();
                    startNewGame.SecondListOfRobots = GetAllRobots();
                    startNewGame.ThirdListOfRobots = GetAllRobots();
                    startNewGame.FourthListOfRobots = GetAllRobots();
                    break;
                case GameType.RobotTournament:
                    if (startNewGame.NumberOfMatchesPerGame == 0)
                    {
                        startNewGame.NumberOfMatchesPerGame = _defaultNumberOfMatchesPerGame;
                    }
                    break;
                default:
                    throw new BriscolaException("Attenzione! La scelta non è corretta");
            }
            return startNewGame;
        }

        public GameResult Result(StartNewGame startNewGame)
        {
            GameResult gameResult = new GameResult
            {
                TeamOneMembers = new List<string>(),
                TeamTwoMembers = new List<string>()
            };

            if (startNewGame.CurrentGameType.ToString() == "DirectMatch")
            {
                // controllo uguaglianza con i valori e con il null!!!
                if (startNewGame.NumberOfPlayers == 4)
                {
                    gameResult.TeamOneMembers.Add(startNewGame.FirstCurrentRobot);
                    gameResult.TeamOneMembers.Add(startNewGame.SecondCurrentRobot);
                    gameResult.TeamTwoMembers.Add(startNewGame.ThirdCurrentRobot);
                    gameResult.TeamTwoMembers.Add(startNewGame.FourthCurrentRobot);
                }
                else if (startNewGame.NumberOfPlayers == 2)
                {
                    gameResult.TeamOneMembers.Add(startNewGame.FirstCurrentRobot);
                    gameResult.TeamTwoMembers.Add(startNewGame.ThirdCurrentRobot);
                }
                else
                {
                    throw new BriscolaException("Attenzione! Scegliere il numero di giocatori");
                }
                gameResult.NumberOfPlayers = startNewGame.NumberOfPlayers.Value;
            }

            gameResult.NumberOfGames = startNewGame.NumberOfGames;
            gameResult.NumberOfMatchesPerGame = startNewGame.NumberOfMatchesPerGame;
            gameResult.GameType = startNewGame.CurrentGameType;

            return gameResult;
        }

        public void Play(GameResult gameResult)
        {

            switch (gameResult.GameType.ToString())
            {
                case "DirectMatch":
                    RunRobotMatch(gameResult);
                    break;
                case "RobotTournament":
                    var tournamentRunner = new BergerPairing(_strategyFactory, gameResult.NumberOfMatchesPerGame);
                    gameResult.TournamentRanking = tournamentRunner.Run();
                    break;
                default:
                    throw new BriscolaException("Il gioco selezionato non è valido");
            }
        }

        private static void RunRobotMatch(GameResult gameResult)
        {

            gameResult.Score = new List<Tuple<int, int>>();
            gameResult.FirstTeamFinalScore = 0;
            gameResult.SecondTeamFinalScore = 0;

            for (int i = 0; i < gameResult.NumberOfGames; i++)
            {
                var g = new BriscolaGame(new ItalianDeckFactory(), new PlayerFactory());
                var runner = new GameService(g, _strategyFactory);
                runner.InitializeRobotMatch(gameResult.TeamOneMembers, gameResult.TeamTwoMembers, gameResult.NumberOfPlayers);
                var result = runner.Play();
                gameResult.Score.Add(result);
            }

            gameResult.FirstTeamFinalScore = gameResult.Score.Count(x => x.Item1 > 60) * 2 + gameResult.Score.Count(x => x.Item1 == 60);
            gameResult.SecondTeamFinalScore = gameResult.Score.Count(x => x.Item2 > 60) * 2 + gameResult.Score.Count(x => x.Item2 == 60);
        }

        public IEnumerable<SelectListItem> GetGameTypes()
        {
            var gameTypes = Enum.GetValues(typeof(GameType)).Cast<GameType>()
                .Select(s =>
                        new SelectListItem
                        {
                            Value = s.ToString(),
                            Text = s.GetDescriptionAttr()
                        });

            return new SelectList(gameTypes, "Value", "Text");
        }
        public IEnumerable<SelectListItem> GetAllRobots()
        {
            StrategyFactory strategyFactory = new StrategyFactory();
            var allStrategies = strategyFactory.GetAllRobots()
                .Select(s =>
                        new SelectListItem
                        {
                            Value = s.Name,
                            Text = s.Name
                        });

            return new SelectList(allStrategies, "Value", "Text");
        }
    }
}