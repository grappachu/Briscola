using Grappachu.Briscola.Interfaces;
using Grappachu.Briscola.Model;
using Grappachu.Briscola.Players.Human;
using Moq;
using SharpTestsEx;
using Xunit;

namespace Grappachu.Briscola.Test.Logic.Strategies
{
    // Step 4: Strategia Umana,Verifico solo l'eredità poichè l'interazione non è testabile e la strategia è banale
    public class HumanStrategyTest
    {
        public HumanStrategyTest()
        {
            var strategyMock = new Mock<IStrategy>();

            _p1 = new Player(strategyMock.Object, "p1");
            _p2 = new Player(strategyMock.Object, "p2");
            _p3 = new Player(strategyMock.Object, "p3");
            _p4 = new Player(strategyMock.Object, "p4");
            _userInterfaceMock = new Mock<IUserInterface>();
            _sut = new HumanStrategy(_userInterfaceMock.Object, "pippo");
        }


        private readonly IStrategy _sut;
        private readonly Player _p1;
        private readonly Player _p2;
        private readonly Player _p3;
        private readonly Player _p4;
        private readonly Mock<IUserInterface> _userInterfaceMock;

        [Fact]
        public void Play_Should_return_choose_a_card()
        {
            var briscola = new Card("Danari", 10);
            var state = new GameState(new IPlayer[] {_p1, _p2, _p3, _p4}, briscola);
            _p1.Take(new Card("Bastoni", 1));
            _p1.Take(new Card("Danari", 3));

            _userInterfaceMock.Setup(x => x.Send("request"));
            _userInterfaceMock.Setup(x => x.GetChoice()).Returns("1");

            var res = _sut.Choose(_p1, state);

            res.Should().Not.Be.Null();
        }
    }
}