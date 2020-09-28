using Grappachu.Briscola.Interfaces;
using Grappachu.Briscola.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Grappachu.Briscola.Players.deiannia.handlers
{
    public abstract class AbstractRoundHandler : IRoundHandler
    {
        private IRoundHandler _nextHandler;

        public IRoundHandler SetNext(IRoundHandler handler)
        {
            this._nextHandler = handler;
            return handler;
        }

        public virtual Card ChooseCard(IPlayer myself, GameState state)
        {
            if (this._nextHandler != null)
            {
                return this._nextHandler.ChooseCard(myself, state);
            }
            else
            {
                return myself.HandCards.OrderBy(x => new Guid()).First();
            }
        }

        protected static Card Winner(GameState state)
        {
            Card winner = state.Dish[0];
            if (state.Dish.Count > 1 && state.Dish[1].WinsOn(winner, state.Briscola.Seed))
            {
                winner = state.Dish[1];
            }
            if (state.Dish.Count > 2 && state.Dish[2].WinsOn(winner, state.Briscola.Seed))
            {
                winner = state.Dish[2];
            }
            if (state.Dish.Count > 3 && state.Dish[3].WinsOn(winner, state.Briscola.Seed))
            {
                winner = state.Dish[3];
            }
            return winner;
        }

        protected Card TakeLiscio(IEnumerable<Card> handCards, Card briscola)
        {
            Card chosen = handCards.Where(c => c.IsLiscio(briscola.Seed)).OrderBy(c => c.Value).FirstOrDefault();

            if (chosen.Seed == null)
                return TakePunti(handCards, briscola);

            return chosen;
        }

        protected Card TakeBriscola(IEnumerable<Card> handCards, Card briscola)
        {
            Card chosen = handCards.Where(c => c.IsBriscola(briscola.Seed)).OrderBy(c => c.Value).FirstOrDefault();

            if (chosen.Seed == null)
                return TakeTaglio(handCards, briscola);

            return chosen;
        }

        protected Card TakeTaglio(IEnumerable<Card> handCards, Card briscola)
        {
            Card chosen = handCards.Where(c => c.IsTaglio(briscola.Seed)).OrderBy(c => c.Value).FirstOrDefault();

            if (chosen.Seed == null)
                return TakeLiscio(handCards, briscola);

            return chosen;
        }

        protected Card TakeCarico(IEnumerable<Card> handCards, Card briscola)
        {
            Card chosen = handCards.Where(c => c.IsCarico(briscola.Seed)).OrderBy(c => c.Value).FirstOrDefault();

            if (chosen.Seed == null)
                return TakePunti(handCards, briscola);

            return chosen;
        }

        protected Card TakePunti(IEnumerable<Card> handCards, Card briscola)
        {
            Card chosen = handCards.Where(c => c.IsPunti(briscola.Seed)).OrderBy(c => c.Value).FirstOrDefault();

            if (chosen.Seed == null)
                return handCards.OrderBy(x => new Guid()).First();

            return chosen;
        }
    }
}
