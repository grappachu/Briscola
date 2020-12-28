using Grappachu.Briscola.Interfaces;
using Grappachu.Briscola.Model;
using Grappachu.Briscola.Players.Default;
using Grappachu.Briscola.Players.deiannia.handlers;
using System;
using System.Collections.Generic;

namespace Grappachu.Briscola.Players.deiannia
{
    public class CapitanOvvioStrategy : StrategyBase, IRobotStrategy
    {

        public const string StrategyName = "capitan-ovvio";
        private const string StrategyAuthor = "Antonio De Ianni";
        private const string StrategyVersion = "1.0.0";

        public string Author => StrategyAuthor;
        public Version Version => Version.Parse(StrategyVersion);

        private IList<Card> _playedCards;

        public CapitanOvvioStrategy() : base(StrategyName)
        {
            _playedCards = new List<Card>();
        }

        protected override Card OnChoose(IPlayer myself, GameState state)
        {
            var first = new FirstRoundHandler();
            var second = new SecondRoundHandler();
            var third = new ThirdRoundHandler();
            var fourth = new FourthRoundHandler();

            first.SetNext(second).SetNext(third).SetNext(fourth);

            var choosed = first.ChooseCard(myself, state);

            return choosed;
        }

        protected override void OnWatch(IPlayer myself, GameState state)
        {
            foreach (Card card in state.Dish)
            {
                _playedCards.Add(card);
            }
        }

        public IList<Card> GetPlayedCards()
        {
            return _playedCards;
        }

    }
}
