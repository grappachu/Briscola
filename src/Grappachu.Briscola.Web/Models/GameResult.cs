using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Grappachu.Briscola.Web.Models
{
    public class GameResult
    {
        public GameType GameType { get; set; }
        public List<Tuple<int,int>> Score { get; set; }

        public List<string> TeamOneMembers { get; set; }
        public List<string> TeamTwoMembers { get; set; }

        public int FirstTeamFinalScore { get; set; }
        public int SecondTeamFinalScore { get; set; }
        public int NumberOfGames { get; set; }
        public int NumberOfPlayers { get; set; }
        public int NumberOfMatchesPerGame { get; set; }

        public IOrderedEnumerable<Tuple<string, int>> TournamentRanking { get; set; }
    }
}