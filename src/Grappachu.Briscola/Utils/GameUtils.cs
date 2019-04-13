using System;
using System.Collections.Generic;
using System.Linq;
using Grappachu.Briscola.Model;

namespace Grappachu.Briscola.Utils
{
    /// <summary>
    ///     Fornisce un'insieme di estensioni e metodi di utilità per l'interazione con lo stato di gioco
    /// </summary>
    public static class GameUtils
    {
        public static IReadOnlyCollection<string> GetTeamNames(this GameState state)
        {
            var pls = state.Players.ToArray();
            switch (pls.Length)
            {
                case 2:
                case 3:
                    return pls.Select(p => p.Name).ToArray();
                case 4:
                    if (state.IsRobotMatch())
                    {
                        // Usa i nomi delle strategie
                        return new[] {pls[0].Strategy.Name, pls[1].Strategy.Name};
                    }
                    else
                    {
                        // compone i nomi dei giocatori
                        var team1 = $"{pls[0].Name} e {pls[2].Name}";
                        var team2 = $"{pls[1].Name} e {pls[3].Name}";
                        return new[] {team1, team2};
                    }
                default:
                    throw new NotSupportedException("Il numero di giocatori non consente di determinare le squadre");
            }
        }


        /// <summary>
        ///     Ottiene un valore che indica che nel gioco non sono presenti giocatori umani
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public static bool IsRobotMatch(this GameState state)
        {
            return state.Players.All(p => !p.Strategy.IsHuman);
        }
    }
}