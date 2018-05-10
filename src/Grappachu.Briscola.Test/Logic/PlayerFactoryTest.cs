using Grappachu.Briscola.Interfaces;
using Grappachu.Briscola.Logic;
using Moq;
using SharpTestsEx;
using Xunit;

namespace Grappachu.Briscola.Test.Logic
{
    public class PlayerFactoryTest
    {
        [Fact]
        public void CreatePlayer_should_return_a_player_with_specified_strategy()
        {
            var sut = new PlayerFactory();
            var strategyMock = new Mock<IStrategy>();

            var p = sut.CreatePlayer("pippo", strategyMock.Object);

            p.Name.Should().Be.EqualTo("pippo");
            p.Strategy.Should().Be.EqualTo(strategyMock.Object);
        }
    }
}