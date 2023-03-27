using Grappachu.Briscola.Interfaces;
using Grappachu.Briscola.Model;
using Grappachu.Briscola.Players.crudelea;
using SharpTestsEx;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Grappachu.Briscola.Strategies.Test.crudelea
{
    public class AliceStrategyTest
    {

        [Fact(DisplayName = "DATO che la partita è appena iniziata " +
            "QUANDO il giocatore è di prima mano e ha un carico disponibile " +
            "ALLORA il giocatore lancerà il CARICO ")]
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
            "QUANDO il giocatore è di prima mano e non ha nessuna carta alta " +
            "ALLORA il giocatore lancerà un LISCIO")]
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

        [Fact(DisplayName = "DATO che il giocatore ha il tre di briscola e l'asso è già stato giocato " +
            "QUANDO sul tavolo c'è un carico " +
            "ALLORA il giocatore lancerà il TRE ")]
        public void TestThree()
        {
            // Arrange
            //var sut = new AliceStrategy();

            //IStrategy strategy = new AliceStrategy();
            //IPlayer player = new Player(strategy, AliceStrategy.StrategyName, new List<Card> {
            //    new Card("Denari", 4),
            //    new Card("Bastoni", 5),
            //    new Card("Denari", 3)
            //});
            //IList<IPlayer> players = new List<IPlayer> { player };
            //Card briscola = new Card("Denari", 10);
            //GameState gameState = new GameState(players, briscola);

            //var previousGameState = new GameState(players, briscola);
            //previousGameState.Dish.Add(new Card("Denari", 1));
            //sut.Watch(player, previousGameState);

            //gameState.Turn++;
            //gameState.Dish.Add(new Card("Coppe", 3));

            //// Act
            //var res = sut.Choose(player, gameState);

            //// Verify
            //res.Value.Should().Be.EqualTo(3);
        }

        [Fact(DisplayName = "DATO che l'asso e il tre di briscola sono già stati giocati " +
            "QUANDO sul tavolo c'è un carico " +
            "ALLORA il giocatore lancerà il RE ")]
        public void TestKing()
        {
            // Arrange
            //var sut = new AliceStrategy();

            //IStrategy strategy = new AliceStrategy();
            //IPlayer player = new Player(strategy, AliceStrategy.StrategyName, new List<Card> {
            //    new Card("Denari", 10),
            //    new Card("Bastoni", 5),
            //    new Card("Denari", 7)
            //});
            //IList<IPlayer> players = new List<IPlayer> { player };
            //Card briscola = new Card("Denari", 10);
            //GameState gameState = new GameState(players, briscola);

            //var previousGameState = new GameState(players, briscola);
            //previousGameState.Dish.Add(new Card("Denari", 1));
            //previousGameState.Dish.Add(new Card("Denari", 3));
            //sut.Watch(player, previousGameState);

            //gameState.Dish.Add(new Card("Coppe", 3));
            //gameState.Turn++;

            //// Act
            //var res = sut.Choose(player, gameState);

            //// Verify
            //res.Value.Should().Be.EqualTo(10);
        }

        [Fact(DisplayName = "DATO che sul tavolo è stata lanciata una figura " +
            "QUANDO sul tavolo non ci sono briscole " +
            "ALLORA il giocatore lancerà una BRISCOLA BASSA ")]

        public void TestLowCard()
        {
            // Arrange
            var sut = new AliceStrategy();

            IStrategy strategy = new AliceStrategy();
            IPlayer player = new Player(strategy, AliceStrategy.StrategyName, new List<Card> {
                new Card("Denari", 8),
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
            "QUANDO il giocatore ha più briscole " +
            "ALLORA il giocatore lancerà la briscola più piccola se di seguito ad un altra")]
        public void TestBriscolaPiccola()
        {
            // Arrange
            var sut = new AliceStrategy();

            IStrategy strategy = new AliceStrategy();
            IPlayer player = new Player(strategy, AliceStrategy.StrategyName, new List<Card> {
                new Card("Denari", 10),
                new Card("Spade", 3),
                new Card("Denari", 2)
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

        [Fact(DisplayName = "DATO che nessun giocatore ha lanciato una briscola e ci sono punti in tavola " +
            "QUANDO il giocatore è di ultimo " +
            "ALLORA il giocatore giocherà una carta più alta ma dello stesso seme della prima giocata")]
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

            IList<IPlayer> players = new List<IPlayer> { player, player2, player3, player4 };
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

        [Fact(DisplayName = "DATO una carta " +
            "QUANDO viene lanciata nel tavolo " +
            "ALLORA viene salvata e ricordata nel mazzo delle carte lanciate ")]
        public void RimemberCard()
        {
            // Arrange
            var sut = new AliceStrategy();
            IPlayer player = new Player(sut, AliceStrategy.StrategyName, new List<Card> {
                    new Card("Spade", 3),
                    new Card("Denari", 5),
                    new Card("Bastoni", 1)
            });

            IList<IPlayer> players = new List<IPlayer> { player };
            Card briscola = new Card("Denari", 10);
            GameState gameState = new GameState(players, briscola);

            var playedCard = new Card("Bastoni", 3);
            gameState.Dish.Add(playedCard);

            // Act

            sut.Watch(player, gameState);

            // Verify
            sut.Deck.Count().Should().Be.EqualTo(1);
            sut.Deck.First().Should().Be.EqualTo(playedCard);
        }

    }
}
