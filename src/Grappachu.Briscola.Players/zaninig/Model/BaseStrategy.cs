using Grappachu.Briscola.Interfaces;
using Grappachu.Briscola.Model;
using Grappachu.Briscola.Players.Default;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grappachu.Briscola.Players.zaninig.Model
{
    public class IlmiorStrategy : StrategyBase
    {
        protected override Card OnChoose(IPlayer myself, GameState state) {
            return myself.HandCards.OrderBy(x => new Guid()).First();
        }

        protected override void OnWatch(IPlayer myself, GameState state) {
            // la strategia stupida non pensa
        }

        public IlmiorStrategy() : base("ilmior")
        {
        }
    }
}
