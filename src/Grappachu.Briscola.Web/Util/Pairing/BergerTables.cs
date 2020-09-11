using System.Collections.Generic;
using System.Diagnostics;

namespace Grappachu.Briscola.UI.Util.Pairing
{
    public static class BergerTables
    {
        private const char Separator = '|';

        private static readonly string[] X3 = {
            "2,3",
            "1,2",
            "3,1"
        };

        private static readonly string[] X4 = {
            "1,4|2,3",
            "4,3|1,2",
            "2,4|3,1"
        };

        private static readonly string[] X5 = {
            "2,5|3,4",
            "5,3|1,2",
            "3,1|4,5",
            "1,4|2,3",
            "4,2|5,1"
        };

        private static readonly string[] X6 = {
            "1,6|2,5|3,4",
            "6,4|5,3|1,2",
            "2,6|3,1|4,5",
            "6,5|1,4|2,3",
            "3,6|4,2|5,1"
        };

        private static readonly string[] X7 = {
            "2,7|3,6|4,5",
            "6,4|7,3|1,2",
            "3,1|4,7|5,6",
            "7,5|1,4|2,3",
            "4,2|5,1|6,7",
            "1,6|2,5|3,4",
            "5,3|6,2|7,1"
        };

        private static readonly string[] X8 = {
            "1,8|2,7|3,6|4,5",
            "8,5|6,4|7,3|1,2",
            "2,8|3,1|4,7|5,6",
            "8,6|7,5|1,4|2,3",
            "3,8|4,2|5,1|6,7",
            "8,7|1,6|2,5|3,4",
            "4,8|5,3|6,2|7,1"
        };

        private static readonly string[] X9 = {
            "2,9|3,8|4,7|5,6",
            "7,5|8,4|9,3|1,2",
            "3,1|4,9|5,8|6,7",
            "8,6|9,5|1,4|2,3",
            "4,2|5,1|6,9|7,8",
            "9,7|1,6|2,5|3,4",
            "5,3|6,2|7,1|8,9",
            "1,8|2,7|3,6|4,5",
            "6,4|7,3|8,2|9,1"
        };

        private static readonly string[] X10 = {
            "1,10|2,9|3,8|4,7|5,6",
            "10,6|7,5|8,4|9,3|1,2",
            "2,10|3,1|4,9|5,8|6,7",
            "10,7|8,6|9,5|1,4|2,3",
            "3,10|4,2|5,1|6,9|7,8",
            "10,8|9,7|1,6|2,5|3,4",
            "4,10|5,3|6,2|7,1|8,9",
            "10,9|1,8|2,7|3,6|4,5",
            "5,10|6,4|7,3|8,2|9,1"
        };

        private static readonly string[] X11 = {
            "2,11|3,10|4,9|5,8|6,7",
            "8,6|9,5|10,4|11,3|1,2",
            "3,1|4,11|5,10|6,9|7,8",
            "9,7|10,6|11,5|1,4|2,3",
            "4,2|5,1|6,11|7,10|8,9",
            "10,8|11,7|1,6|2,5|3,4",
            "5,3|6,2|7,1|8,11|9,10",
            "11,9|1,8|2,7|3,6|4,5",
            "6,4|7,3|8,2|9,1|10,11",
            "1,10|2,9|3,8|4,7|5,6",
            "7,5|8,4|9,3|10,2|11,1"
        };

        private static readonly string[] X12 = {
            "1,12|2,11|3,10|4,9|5,8|6,7",
            "12,7|8,6|9,5|10,4|11,3|1,2",
            "2,12|3,1|4,11|5,10|6,9|7,8",
            "12,8|9,7|10,6|11,5|1,4|2,3",
            "3,12|4,2|5,1|6,11|7,10|8,9",
            "12,9|10,8|11,7|1,6|2,5|3,4",
            "4,12|5,3|6,2|7,1|8,11|9,10",
            "12,10|11,9|1,8|2,7|3,6|4,5",
            "5,12|6,4|7,3|8,2|9,1|10,11",
            "12,11|1,10|2,9|3,8|4,7|5,6",
            "6,12|7,5|8,4|9,3|10,2|11,1"
        };

        private static readonly string[] X13 = {
            "2,13|3,12|4,11|5,10|6,9|7,8",
            "9,7|10,6|11,5|12,4|13,3|1,2",
            "3,1|4,13|5,12|6,11|7,10|8,9",
            "10,8|11,7|12,6|13,5|1,4|2,3",
            "4,2|5,1|6,13|7,12|8,11|9,10",
            "11,9|12,8|13,7|1,6|2,5|3,4",
            "5,3|6,2|7,1|8,13|9,12|10,11",
            "12,10|13,9|1,8|2,7|3,6|4,5",
            "6,4|7,3|8,2|9,1|10,13|11,12",
            "13,11|1,10|2,9|3,8|4,7|5,6",
            "7,5|8,4|9,3|10,2|11,1|12,13",
            "1,12|2,11|3,10|4,9|5,8|6,7",
            "8,6|9,5|10,4|11,3|12,2|13,1"
        };

        private static readonly string[] X14 = {
            "1,14|2,13|3,12|4,11|5,10|6,9|7,8",
            "14,8|9,7|10,6|11,5|12,4|13,3|1,2",
            "2,14|3,1|4,13|5,12|6,11|7,10|8,9",
            "14,9|10,8|11,7|12,6|13,5|1,4|2,3",
            "3,14|4,2|5,1|6,13|7,12|8,11|9,10",
            "14,10|11,9|12,8|13,7|1,6|2,5|3,4",
            "4,14|5,3|6,2|7,1|8,13|9,12|10,11",
            "14,11|12,10|13,9|1,8|2,7|3,6|4,5",
            "5,14|6,4|7,3|8,2|9,1|10,13|11,12",
            "14,12|13,11|1,10|2,9|3,8|4,7|5,6",
            "6,14|7,5|8,4|9,3|10,2|11,1|12,13",
            "14,13|1,12|2,11|3,10|4,9|5,8|6,7",
            "7,14|8,6|9,5|10,4|11,3|12,2|13,1"
        };

        private static readonly string[] X15 = {
            "2,15|3,14|4,13|5,12|6,11|7,10|8,9",
            "10,8|11,7|12,6|13,5|14,4|15,3|1,2",
            "3,1|4,15|5,14|6,13|7,12|8,11|9,10",
            "11,9|12,8|13,7|14,6|15,5|1,4|2,3",
            "4,2|5,1|6,15|7,14|8,13|9,12|10,11",
            "|12,10|13,9|14,8|15,7|1,6|2,5|3,4",
            "5,3|6,2|7,1|8,15|9,14|10,13|11,12",
            "13,11|14,10|15,9|1,8|2,7|3,6|4,5",
            "6,4|7,3|8,2|9,1|10,15|11,14|12,13",
            "14,12|15,11|1,10|2,9|3,8|4,7|5,6",
            "7,5|8,4|9,3|10,2|11,1|12,15|13,14",
            "15,13|1,12|2,11|3,10|4,9|5,8|6,7",
            "8,6|9,5|10,4|11,3|12,2|13,1|14,15",
            "1,14|2,13|3,12|4,11|5,10|6,9|7,8",
            "9,7|10,6|11,5|12,4|13,3|14,2|15,1"
        };


        private static readonly string[] X16 = {
            "1,16|2,15|3,14|4,13|5,12|6,11|7,10|8,9",
            "16,9|10,8|11,7|12,6|13,5|14,4|15,3|1,2",
            "2,16|3,1|4,15|5,14|6,13|7,12|8,11|9,10",
            "16,10|11,9|12,8|13,7|14,6|15,5|1,4|2,3",
            "3,16|4,2|5,1|6,15|7,14|8,13|9,12|10,11",
            "16,11|12,10|13,9|14,8|15,7|1,6|2,5|3,4",
            "4,16|5,3|6,2|7,1|8,15|9,14|10,13|11,12",
            "16,12|13,11|14,10|15,9|1,8|2,7|3,6|4,5",
            "5,16|6,4|7,3|8,2|9,1|10,15|11,14|12,13",
            "16,13|14,12|15,11|1,10|2,9|3,8|4,7|5,6",
            "6,16|7,5|8,4|9,3|10,2|11,1|12,15|13,14",
            "16,14|15,13|1,12|2,11|3,10|4,9|5,8|6,7",
            "7,16|8,6|9,5|10,4|11,3|12,2|13,1|14,15",
            "16,15|1,14|2,13|3,12|4,11|5,10|6,9|7,8",
            "8,16|9,7|10,6|11,5|12,4|13,3|14,2|15,1"
        };

        /// <summary>
        /// Ottiene il numero dei turni previsti per il torneo
        /// </summary>
        /// <param name="playersCount"></param>
        /// <returns></returns>
        public static int GetRounds(int playersCount)
        {
            Debug.Assert(playersCount >= 3);
            Debug.Assert(playersCount <= 16);

            var n = playersCount % 2 == 0 ? playersCount - 1 : playersCount;

            return n;
        }

        /// <summary>
        /// Ottiene tutti gli incontri per un turno di gioco
        /// </summary>
        /// <param name="playersCount">In numero dei giocatori iscritti</param>
        /// <param name="roundNumber">Il numero in base 0 del turno da generare</param>
        /// <returns></returns>
        public static IEnumerable<Match> GetRoundMatches(int playersCount, int roundNumber)
        {
            var round = new List<Match>();
            var roundIndex = roundNumber;
            string[] turno = null;

            switch (playersCount)
            {
                case 3:
                    turno = X3[roundIndex % 3].Split(Separator);
                    break;
                case 4:
                    turno = X4[roundIndex % 3].Split(Separator);
                    break;
                case 5:
                    turno = X5[roundIndex % 5].Split(Separator);
                    break;
                case 6:
                    turno = X6[roundIndex % 5].Split(Separator);
                    break;
                case 7:
                    turno = X7[roundIndex % 7].Split(Separator);
                    break;
                case 8:
                    turno = X8[roundIndex % 7].Split(Separator);
                    break;
                case 9:
                    turno = X9[roundIndex % 9].Split(Separator);
                    break;
                case 10:
                    turno = X10[roundIndex % 9].Split(Separator);
                    break;
                case 11:
                    turno = X11[roundIndex % 11].Split(Separator);
                    break;
                case 12:
                    turno = X12[roundIndex % 11].Split(Separator);
                    break;
                case 13:
                    turno = X13[roundIndex % 13].Split(Separator);
                    break;
                case 14:
                    turno = X14[roundIndex % 13].Split(Separator);
                    break;
                case 15:
                    turno = X15[roundIndex % 15].Split(Separator);
                    break;
                case 16:
                    turno = X16[roundIndex % 15].Split(Separator);
                    break;
            }

            if ((turno != null))
            {
                foreach (string coppia in turno)
                {
                    string[] c = coppia.Split(',');
                    var p1 = int.Parse(c[0]);
                    var p2 = int.Parse(c[1]);
                    var game = new Match
                    {
                        Home = p1,
                        Visitor = p2
                    };

                    round.Add(game);
                }
            }

            return round;
        }
    }

   
}