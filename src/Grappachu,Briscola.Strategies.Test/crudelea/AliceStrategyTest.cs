using Grappachu.Briscola.Interfaces;
using Grappachu.Briscola.Model;
using Grappachu.Briscola.Players.crudelea;
using SharpTestsEx;
using System.Collections.Generic;
using Xunit;

namespace Grappachu.Briscola.Strategies.Test.crudelea
{
    public class AliceStrategyTest
    {

        [Fact(DisplayName = "DATO che la partita è appena iniziata " +
            "QUANDO sei di prima mano e hai un carico disponibile " +
            "ALLORA lancia un carico ")]
        public void TestHighCard()
        {
            // Arrange
            var sut = new AliceStrategy();

            IStrategy strategy = new AliceStrategy();
            IPlayer player = new Player(strategy, AliceStrategy.StrategyName, new List<Card> {
                new Card("Spade", 1),
                new Card("Bastoni", 5),
                new Card("Denari", 10)
            });
            IList<IPlayer> players = new List<IPlayer> { player };
            Card briscola = new Card("denari", 10);
            GameState gameState = new GameState(players, briscola);

            // Act
            var res = sut.Choose(player, gameState);

            // Verify
            res.Value.Should().Be.EqualTo(1);

        }

        [Fact(DisplayName = "DATO che la partita è appena iniziata " +
            "QUANDO sei di prima mano " +
            "ALLORA lancia liscio")]
        public void TestNeat()
        {
            // Arrange
            var sut = new AliceStrategy();

            IStrategy strategy = new AliceStrategy();
            IPlayer player = new Player(strategy, AliceStrategy.StrategyName, new List<Card> {
                new Card("Spade", 10),
                new Card("Bastoni", 5),
                new Card("Denari", 10)
            });
            IList<IPlayer> players = new List<IPlayer> { player };
            Card briscola = new Card("denari", 10);
            GameState gameState = new GameState(players, briscola);

            // Act
            var res = sut.Choose(player, gameState);

            // Verify
            res.Value.Should().Not.Be.EqualTo(1);
            res.Value.Should().Not.Be.EqualTo(3);
            res.Value.Should().Not.Be.EqualTo(8);
            res.Value.Should().Not.Be.EqualTo(9);
            res.Value.Should().Not.Be.EqualTo(10);
        }

        [Fact(DisplayName = "DATO che hai il tre di briscola e l'asso è già giocato " +
            "QUANDO sul tavolo c'è un carico " +
            "ALLORA lancia il TRE ")]
        public void TestThree()
        {
            // Arrange
            var sut = new AliceStrategy();

            IStrategy strategy = new AliceStrategy();
            IPlayer player = new Player(strategy, AliceStrategy.StrategyName, new List<Card> {
                new Card("Spade", 4),
                new Card("Bastoni", 5),
                new Card("Denari", 3)
            });
            IList<IPlayer> players = new List<IPlayer> { player };
            Card briscola = new Card("Denari", 10);
            GameState gameState = new GameState(players, briscola);

            gameState.Dish.Add(new Card("Coppe", 3));
            gameState.Turn++;

            // Act
            var res = sut.Choose(player, gameState);

            // Verify
            res.Value.Should().Be.EqualTo(3);
        }

        [Fact(DisplayName = "DATO che l'asso e il tre di briscola sono già stati giocati " +
            "QUANDO sul tavolo c'è un carico " +
            "ALLORA lancia il re ")]
        public void TestKing()
        {
            // Arrange
            var sut = new AliceStrategy();

            IStrategy strategy = new AliceStrategy();
            IPlayer player = new Player(strategy, AliceStrategy.StrategyName, new List<Card> {
                new Card("Spade", 3),
                new Card("Bastoni", 5),
                new Card("Denari", 10)
            });
            IList<IPlayer> players = new List<IPlayer> { player };
            Card briscola = new Card("Denari", 10);
            GameState gameState = new GameState(players, briscola);

            gameState.Dish.Add(new Card("Coppe", 3));
            gameState.Turn++;

            // Act
            var res = sut.Choose(player, gameState);

            // Verify
            res.Value.Should().Be.EqualTo(10);
        }

        [Fact(DisplayName = "DATO una figura sul tavolo " +
            "QUANDO sul tavolo non ci sono briscole " +
            "ALLORA lancia una briscola bassa ")]

        public void TestLowCard()
        {
            // Arrange
            var sut = new AliceStrategy();

            IStrategy strategy = new AliceStrategy();
            IPlayer player = new Player(strategy, AliceStrategy.StrategyName, new List<Card> {
                new Card("Bastoni", 5),
                new Card("Denari", 4),
                new Card("Spade", 6)
            });
            IList<IPlayer> players = new List<IPlayer> { player };
            Card briscola = new Card("Denari", 10);
            GameState gameState = new GameState(players, briscola);

            gameState.Dish.Add(new Card("Spade", 8));
            gameState.Turn++;

            // Act
            var res = sut.Choose(player, gameState);

            // Verify
            res.Value.Should().Be.EqualTo(4);
        }

        [Fact(DisplayName = "DATO che sul tavolo ci sono dei punti " +
            "QUANDO hai più briscole " +
            "ALLORA lancia la più piccola di seguito")]
        public void TestBriscolaPiccola()
        {
            // Arrange
            var sut = new AliceStrategy();

            IStrategy strategy = new AliceStrategy();
            IPlayer player = new Player(strategy, AliceStrategy.StrategyName, new List<Card> {
                new Card("Spade", 3),
                new Card("Denari", 2),
                new Card("Denari", 10)
            });
            IList<IPlayer> players = new List<IPlayer> { player };
            Card briscola = new Card("Denari", 10);
            GameState gameState = new GameState(players, briscola);

            gameState.Dish.Add(new Card("Coppe", 9));
            gameState.Turn++;

            // Act
            var res = sut.Choose(player, gameState);

            // Verify
            res.Value.Should().Be.EqualTo(2);
        }

        [Fact(DisplayName = "DATO che non ci sono briscole in tavola " +
            "QUANDO sei di ultimo e ci sono punti in tavola " +
            "ALLORA sali della prima carta lanciata")]
        public void TestSali()
        {
            // Arrange
            var sut = new AliceStrategy();

            IStrategy strategy = new AliceStrategy();
            IPlayer player = new Player(strategy, AliceStrategy.StrategyName, new List<Card> {
                    new Card("Spade", 3),
                    new Card("Denari", 5),
                    new Card("Bastoni", 1)
            });
            IPlayer player2 = new Player(strategy, AliceStrategy.StrategyName, new List<Card>());
            IPlayer player3 = new Player(strategy, AliceStrategy.StrategyName, new List<Card>());
            IPlayer player4 = new Player(strategy, AliceStrategy.StrategyName, new List<Card>());

            IList <IPlayer> players = new List<IPlayer> { player, player2, player3, player4 };
            Card briscola = new Card("Denari", 10);
            GameState gameState = new GameState(players, briscola);

            gameState.Dish.Add(new Card("Bastoni", 3));
            gameState.Dish.Add(new Card("Spade", 4));
            gameState.Dish.Add(new Card("Coppe", 5));
            gameState.Turn++;

            // Act
            var res = sut.Choose(player, gameState);

            // Verify
            res.Value.Should().Be.EqualTo(1);
        }
    }
}
