using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Grappachu.Briscola.Interfaces;
using Grappachu.Briscola.Model;

namespace Grappachu.Briscola.Logic
{
    public class Deck : IDeck
    {
        private List<Card> _cards;

        public Deck(string[] seeds, Range valueRange)
        {
            _cards = new List<Card>();
            foreach (var seed in seeds)
                for (var value = valueRange.Min; value <= valueRange.Max; value++)
                    _cards.Add(new Card(seed, value));
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Shuffle()
        {
            _cards = _cards.OrderBy(x => Guid.NewGuid()).ToList();
        }

        public Card Pop()
        {
            var card = _cards.First();
            _cards.Remove(card);
            return card;
        }

        public IEnumerator<Card> GetEnumerator()
        {
            return _cards.GetEnumerator();
        }

        public Card PeekLast()
        {
            return _cards.Last();
        }

        public int Count { get { return this.Count(); } }
    }
}