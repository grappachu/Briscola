using System.Linq;
using Grappachu.Briscola.Interfaces;
using Grappachu.Briscola.Model;
using Grappachu.Briscola.Players.prenassid.Utils;

namespace Grappachu.Briscola.Players.prenassid.Handlers
{
    public class P4R1Handler : RoundHandler
    {
        protected override Card OnHandle(IPlayer myself, GameState state, Watcher watcher)
        {
            int minPoints = int.MaxValue;
            Card toPlay = default(Card);
            var unknownCards = watcher.GetUnknownCards(myself).ToArray();
            foreach (var card in myself.HandCards)
            { 
                var overcards = unknownCards.Where(c => c.Seed == card.Seed && c.GetScore() > card.GetScore());

                // aggiunge un bonus pari a 2 volte il valore della carta in modo da non andare di carico
                int overPoints = overcards.GetScore() + 2 * card.GetScore();
                // aggiunge un bonus per non sprecare le briscole
                if (card.Seed == state.Briscola.Seed)
                {
                    overPoints += 5;
                }

                if (overPoints < minPoints)
                {
                    toPlay = card;
                    minPoints = overPoints;
                }
            }
         
            return toPlay;
        }

        protected override bool OnCanHandle(IPlayer myself, GameState state)
        {
            return state.Players.Count == 4 && state.Dish.Count == 0;
        }
    }
}