using Grappachu.Briscola.Interfaces;
using Grappachu.Briscola.Logic;
using Grappachu.Briscola.Model;
using Moq;
using SharpTestsEx;
using Xunit;

namespace Grappachu.Briscola.Test.Logic
{
    // Step 3:
    public class EvaluatorTest
    {
        public EvaluatorTest()
        {
            _strategyMock = new Mock<IStrategy>();
            _p1 = new Player(_strategyMock.Object, "Tizio");
            _p2 = new Player(_strategyMock.Object, "Caio");
            _p3 = new Player(_strategyMock.Object, "Sempronio");
            _p4 = new Player(_strategyMock.Object, "Io");

            _sut = new GameEvaluator();
        }

        private readonly Player _p1;
        private readonly Player _p2;
        private readonly Player _p3;
        private readonly Player _p4;
        private readonly Mock<IStrategy> _strategyMock;

        private readonly IGameEvaluator _sut;

        // Step 3: Crea un test per la valutazione del piatto quando ci sono briscole
        [Fact]
        public void Assign_should_give_cards_to_winner_player()
        {
            var briscola = new Card("Danari", 4);

            var state = new GameState(new IPlayer[] {_p1, _p2, _p3, _p4}, briscola);
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

            _sut.Assign(state);

            _p1.Stack.Should().Be.Empty();
            _p2.Stack.Should().Be.Empty();
            _p3.Stack.Should().Be.Empty();
            _p4.Stack.Should().Have.SameValuesAs(cards);
        }


        // Step 3: Crea un test per la valutazione del piatto quando briscole non ce n'è
        [Fact]
        public void Assign_should_give_cards_to_winner_player_when_no_briscola()
        {
            var briscola = new Card("Danari", 4);

            var state = new GameState(new IPlayer[] {_p1, _p2, _p3, _p4}, briscola);
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

            _sut.Assign(state);

            _p4.Stack.Should().Be.Empty();
            _p2.Stack.Should().Be.Empty();
            _p3.Stack.Should().Be.Empty();
            _p1.Stack.Should().Have.SameValuesAs(cards);
        }

        [Fact]
        public void Score_of_deck_should_be_120()
        {
            var deck = new Deck(new[] {"A", "B", "C", "D"}, new Model.Range(1, 10));

            var res = GameEvaluator.Totalize(deck);

            res.Should().Be(120);
        }
    }
}