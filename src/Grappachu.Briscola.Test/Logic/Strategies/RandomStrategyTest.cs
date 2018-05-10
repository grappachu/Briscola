using Grappachu.Briscola.Interfaces;
using Grappachu.Briscola.Model;
using Grappachu.Briscola.Players.Default;
using Moq;
using SharpTestsEx;
using Xunit;

namespace Grappachu.Briscola.Test.Logic.Strategies
{
    // Step 4: Strategia Random, non perdo molto tempo in questa fase ma verifico solo che scegla una carta
    //         Rimando la creazione di casi complessi a quando avrò finito
    public class RandomStrategyTest
    {
        public RandomStrategyTest()
        {
            var strategyMock = new Mock<IStrategy>();

            _p1 = new Player(strategyMock.Object, "p1");
            _p2 = new Player(strategyMock.Object, "p2");
            _p3 = new Player(strategyMock.Object, "p3");
            _p4 = new Player(strategyMock.Object, "p4");
            _sut = new RandomStrategy();
        }

        private readonly IStrategy _sut;
        private readonly Player _p1;
        private readonly Player _p2;
        private readonly Player _p3;
        private readonly Player _p4;

        [Fact]
        public void Choose_Should_return_select_a_card_from_player_hands()
        {
            var briscola = new Card("Danari", 10);
            var state = new GameState(new IPlayer[] {_p1, _p2, _p3, _p4}, briscola);
            _p1.Take(new Card("Bastoni", 1));
            _p1.Take(new Card("Danari", 6));

            var res = _sut.Choose(_p1, state);

            res.Should().Not.Be.Null();
        }
    }
}