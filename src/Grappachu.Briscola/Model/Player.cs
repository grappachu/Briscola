using System.Collections.Generic;
using Grappachu.Briscola.Interfaces;
using Grappachu.Briscola.Utils;

namespace Grappachu.Briscola.Model
{
    public class Player : IPlayer
    {
        private readonly List<Card> _handCards;
        private readonly List<Card> _stack;

        public Player(IStrategy strategy, string name)
        {
            Name = name;
            Strategy = strategy;
            _handCards = new List<Card>();
            _stack = new List<Card>();
        }

        public Player(IStrategy strategy, string name, IEnumerable<Card> cards) : this(strategy, name)
        {
            _handCards.AddRange(cards);
        }

        public string Name { get; }

        public IEnumerable<Card> HandCards => _handCards;

        public IEnumerable<Card> Stack => _stack;

        public IStrategy Strategy { get; }


        public Card Play(GameState state)
        {
            var card = Strategy.Choose(this, state);
            _handCards.Remove(card);
            Chat.GetUI().Send(string.Format("{0} | GIOCA: {1} di {2}", Name.PadRight(8), card.Value, card.Seed));
            return card;
        }

        public void Take(Card card)
        {
            if (Strategy.IsHuman)
            {
                Chat.GetUI().Strong(string.Format("{0} | HAI PESCATO: {1} di {2}", Name.PadRight(8), card.Value, card.Seed));
            }
            _handCards.Add(card);
        }

        public void Save(IEnumerable<Card> cards)
        {
            _stack.AddRange(cards);
        }

        public void Look(GameState state)
        {
            Strategy.Watch(this, state);
        }
    }
}