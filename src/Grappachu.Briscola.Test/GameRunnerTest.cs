using System.Collections.Generic;
using Grappachu.Briscola.Interfaces;
using Grappachu.Briscola.Model;
using Grappachu.Briscola.Players.Default;
using Grappachu.Briscola.UI.Util;
using Moq;
using Xunit;

namespace Grappachu.Briscola.Test
{
    public class GameRunnerTest
    {
        public GameRunnerTest()
        {
            var uiMock = new Mock<IUserInterface>();
            _gameMock = new Mock<IGame<GameState>>();
            _strategyFactory = new Mock<IStrategyFactory>();
            _sut = new GameRunner(_gameMock.Object, uiMock.Object, _strategyFactory.Object);
        }

        private readonly Mock<IGame<GameState>> _gameMock;
        private readonly GameRunner _sut;
        private readonly Mock<IStrategyFactory> _strategyFactory;

        [Fact]
        public void Initialize_should_run_game()
        {
            _gameMock.Setup(x => x.Join(It.IsAny<string>(), It.IsAny<IStrategy>()));
            _strategyFactory.Setup(x => x.GetAllRobots()).Returns(new[] {new RandomStrategy()});

            _sut.InitializeHumanGame(4);

            _gameMock.Verify(m => m.Join(It.IsAny<string>(), It.IsAny<IStrategy>()), Times.AtLeast(2));
        }

        [Fact]
        public void Run_should_run_game()
        {
            _gameMock.Setup(x => x.Start());
            _gameMock.Setup(x => x.PlayHand());
            _gameMock.SetupGet(x => x.State).Returns(new GameState(new List<IPlayer>
                {
                    new Player(null, "A", new Card[0]),
                    new Player(null, "B", new Card[0]),
                    new Player(null, "C", new Card[0]),
                    new Player(null, "D", new Card[0])
                },
                new Card("Brisocla", 1)));

            _sut.Play();

            _gameMock.Verify(x => x.Start(), Times.Once);
            _gameMock.Verify(x => x.PlayHand(), Times.Exactly(10));
            _gameMock.Verify(x => x.Refill(), Times.AtLeast(7));
        }
    }
}