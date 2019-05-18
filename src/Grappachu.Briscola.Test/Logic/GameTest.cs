using System;
using System.Linq;
using Grappachu.Briscola.Interfaces;
using Grappachu.Briscola.Logic;
using Grappachu.Briscola.Model;
using Moq;
using SharpTestsEx;
using Xunit;

namespace Grappachu.Briscola.Test.Logic
{
    public class GameTest
    {
        public GameTest()
        {
            _deckFactoryMock = new Mock<IDeckFactory>();
            _playerFactoryMock = new Mock<IPlayerFactory>();
            _player1Mock = new Mock<IPlayer>();
            _player2Mock = new Mock<IPlayer>();
            _deckMock = new Mock<IDeck>();
            _strategyMock = new Mock<IStrategy>();

            _players = new[] { "Io", "Davide" };

            _sut = new BriscolaGame(_deckFactoryMock.Object, _playerFactoryMock.Object);
        }

        private readonly IGame<GameState> _sut;
        private readonly Mock<IDeckFactory> _deckFactoryMock;
        private readonly string[] _players;
        private readonly Mock<IPlayerFactory> _playerFactoryMock;
        private readonly Mock<IPlayer> _player1Mock;
        private readonly Mock<IPlayer> _player2Mock;
        private readonly Mock<IDeck> _deckMock;
        private readonly Mock<IStrategy> _strategyMock;


        // Step 3: Testo in modo completo una mano di gioco
        [Fact(DisplayName = "La singola una mano di gioco deve svegliare i giocatori al loro turno e alla termine della mano")]
        public void PlayHand_should_workout_a_hand_regarding_player_turn()
        {
            var p2Card = new Card("Coppe", 1);
            var p1Card = new Card("Bastoni", 5);
            var briscola = new Card("Bastoni", 3);
            _deckFactoryMock.Setup(x => x.CreateDeck()).Returns(_deckMock.Object);
            _deckMock.Setup(d => d.PeekLast()).Returns(briscola);
            _deckMock.Setup(d => d.Pop()).Returns(new Card("foo", 1));

            var strategyMock = new Mock<IStrategy>();
            _playerFactoryMock.Setup(f => f.CreatePlayer(_players[0], strategyMock.Object)).Returns(_player1Mock.Object);
            _playerFactoryMock.Setup(f => f.CreatePlayer(_players[1], strategyMock.Object)).Returns(_player2Mock.Object);

            _player1Mock.Setup(x => x.Name).Returns("Pippo");
            _player1Mock.Setup(x => x.Play(It.IsAny<GameState>())).Returns(p1Card);
            _player1Mock.Setup(x => x.Look(It.IsAny<GameState>()));

            _player2Mock.Setup(x => x.Name).Returns("Topolino");
            _player2Mock.Setup(x => x.Play(It.IsAny<GameState>())).Returns(p2Card);
            _player2Mock.Setup(x => x.Look(It.IsAny<GameState>()));

            _sut.Join(_players[0], strategyMock.Object);
            _sut.Join(_players[1], strategyMock.Object);
            _sut.Start();
            _sut.State.Turn = 1; // Inizio arbitrariamente dal giocatore 0 per testare il ciclo

            _sut.PlayHand();

            _player1Mock.Verify(x => x.Play(It.Is<GameState>(s => s == _sut.State)));
            _player2Mock.Verify(x => x.Play(It.Is<GameState>(s => s == _sut.State)));
            // la sequenza deve iniziare da giocatore 2

            // Al termine di ogni mano deve notificare i giocatori per vedere l'esito del piatto
            _player1Mock.Verify(p => p.Look(It.Is<GameState>(s => s == _sut.State)), Times.Once);
            _player2Mock.Verify(p => p.Look(It.Is<GameState>(s => s == _sut.State)), Times.Once);
         
            _sut.State.Dish.Should().Be.Empty();
            _sut.State.Turn.Should().Be(0);
        }


        [Fact]
        public void Start_cannot_be_called_when_players_are_1_or_more_than_4()
        {
            var briscola = new Card("Bastoni", 3);

            _deckFactoryMock.Setup(x => x.CreateDeck()).Returns(_deckMock.Object);
            _deckMock.Setup(d => d.PeekLast()).Returns(briscola);
            _deckMock.Setup(d => d.Pop()).Returns(new Card("foo", 1));
            var strategyMock = new Mock<IStrategy>();
            strategyMock.Setup(m => m.Choose(It.IsAny<IPlayer>(), It.IsAny<GameState>())).Returns(new Card());
            _playerFactoryMock.Setup(f => f.CreatePlayer(It.IsAny<string>(), It.IsAny<IStrategy>()))
                .Returns(_player1Mock.Object);

            var g = new BriscolaGame(_deckFactoryMock.Object, _playerFactoryMock.Object);
            g.Join("Giocatore1", strategyMock.Object);

            Executing.This(() =>
                g.Start()).Should().Throw<InvalidOperationException>();
        }



        // Step 3: Crea un test per la valutazione del piatto quando ci sono briscole
        [Fact]
        public void Assign_should_give_cards_to_winner_player()
        {
            var briscola = new Card("Danari", 4);
            var strategyMock = new Mock<IStrategy>();
            var p1 = new Player(strategyMock.Object, "Tizio");
            var p2 = new Player(strategyMock.Object, "Caio");
            var p3 = new Player(strategyMock.Object, "Sempronio");
            var p4 = new Player(strategyMock.Object, "Io");
            var state = new GameState(new IPlayer[] { p1, p2, p3, p4 }, briscola);
            var cards = new[]
            {
                new Card("Bastoni", 8),
                new Card("Bastoni", 1),
                new Card("Danari", 3),
                new Card("Danari", 5)
            };
            state.Dish.Add(cards[0]);
            state.Dish.Add(cards[1]);
            state.Dish.Add(cards[2]);
            state.Dish.Add(cards[3]);
            state.Turn = 1; // il giocatore che ha giocato la prima carta

            int winnerIdx = _sut.AssignHand(state);

            p1.Stack.Should().Be.Empty();
            p2.Stack.Should().Be.Empty();
            p3.Stack.Should().Be.Empty();
            p4.Stack.Should().Have.SameValuesAs(cards);
            winnerIdx.Should().Be(3);
        }

        // Step 3: Crea un test per la valutazione del piatto quando briscole non ce n'è
        [Fact]
        public void Assign_should_give_cards_to_winner_player_when_no_briscola()
        {
            var briscola = new Card("Danari", 4);
            var strategyMock = new Mock<IStrategy>();
            var p1 = new Player(strategyMock.Object, "Tizio");
            var p2 = new Player(strategyMock.Object, "Caio");
            var p3 = new Player(strategyMock.Object, "Sempronio");
            var p4 = new Player(strategyMock.Object, "Io");

            var state = new GameState(new IPlayer[] { p1, p2, p3, p4 }, briscola);
            var cards = new[]
            {
                new Card("Bastoni", 8),
                new Card("Bastoni", 1),
                new Card("Spade", 3),
                new Card("Coppe", 10)
            };
            state.Dish.Add(cards[0]);
            state.Dish.Add(cards[1]);
            state.Dish.Add(cards[2]);
            state.Dish.Add(cards[3]);
            state.Turn = 3; // il giocatore che ha giocato la prima carta

            int winnerIdx = _sut.AssignHand(state);

            p4.Stack.Should().Be.Empty();
            p2.Stack.Should().Be.Empty();
            p3.Stack.Should().Be.Empty();
            p1.Stack.Should().Have.SameValuesAs(cards);
            winnerIdx.Should().Be(0);
        }


        // Step 2: Verifico che sia inizializzato lo stato di gioco
        [Fact]
        public void Start_should_initialize_GameState()
        {
            var briscola = new Card("Bastoni", 3);
            _deckFactoryMock.Setup(x => x.CreateDeck()).Returns(_deckMock.Object);
            // Verifico che la creazione del mazzo avvenga usando la Factory
            // Cos� da avere la certezza che il mazzo sia giusto

            _deckMock.Setup(d => d.PeekLast()).Returns(briscola);
            // Per comodit� ho aggiunto un metodo PeekLast per vedere l'ultima carta senza rimuoverla, 
            // visto che la biscola deve rimanere nel mazzo ed � l'ultima carta ad essere pescata

            _deckMock.Setup(d => d.Pop()).Returns(new Card("foo", 1));
            // Per ogni giocatore simulo la pesca di una carta a caso (non mi interessa la carta ma solo che venga presa dal mazzo)

            var strategyMock = new Mock<IStrategy>();
            _playerFactoryMock.Setup(f => f.CreatePlayer(_players[0], strategyMock.Object))
                .Returns(new Player(_strategyMock.Object, _players[0]));
            _playerFactoryMock.Setup(f => f.CreatePlayer(_players[1], strategyMock.Object))
                .Returns(new Player(_strategyMock.Object, _players[1]));


            _sut.Join(_players[0], strategyMock.Object);
            _sut.Join(_players[1], strategyMock.Object);
            _sut.Start();

            _deckMock.Verify(x => x.Shuffle(), Times.AtLeastOnce);
            _sut.State.Briscola.Should().Be.EqualTo(briscola);
            _sut.State.Players.Select(x => x.Name).Should().Have.SameValuesAs(_players);
            _sut.State.Players.All(p => p.HandCards.Count() == 3).Should().Be.True();
            _sut.State.Turn.Should().Be.EqualTo(0);
            _sut.State.Dish.Should().Be.Empty();
        }

        [Fact]
        public void Refill_should_give_a_card_to_each_player()
        {
            _deckFactoryMock.Setup(x => x.CreateDeck()).Returns(_deckMock.Object);
            _deckMock.Setup(d => d.Pop()).Returns(new Card("foo", 1));
            _deckMock.SetupGet(d => d.Count).Returns(40);

            var strategyMock = new Mock<IStrategy>();
            _playerFactoryMock.Setup(f => f.CreatePlayer(_players[0], strategyMock.Object))
                .Returns(new Player(_strategyMock.Object, _players[0]));
            _playerFactoryMock.Setup(f => f.CreatePlayer(_players[1], strategyMock.Object))
                .Returns(new Player(_strategyMock.Object, _players[1]));
            _sut.Join(_players[0], strategyMock.Object);
            _sut.Join(_players[1], strategyMock.Object);
            _sut.Start();

            _sut.Refill();


            _sut.State.Players.All(p => p.HandCards.Count() == 4).Should().Be.True();
            _sut.State.Turn.Should().Be.EqualTo(0);
            _sut.State.Dish.Should().Be.Empty();
        }
    }
}