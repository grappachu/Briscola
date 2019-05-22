using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grappachu.Briscola.Model;

namespace Grappachu.Briscola.Players.zaninig.Model
{
    public static class GameStateExtension
    {
        public static bool IsBriscola(this GameState gameState, Card card)
        {
            return gameState.Briscola.Seed == card.Seed;
        }
    }
}
