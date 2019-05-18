using System.Collections.Generic;
using System.Linq;
using Grappachu.Briscola.Exceptions;
using Grappachu.Briscola.Interfaces;
using Grappachu.Briscola.Model;
using Moq;
using SharpTestsEx;
using Xunit;

namespace Grappachu.Briscola.Test.Logic
{
    public class PlayerTest
    {
        public PlayerTest()
        {
            _strategyMock = new Mock<IStrategy>();
        }

        private readonly Mock<IStrategy> _strategyMock;


        [Fact]
        public void Play_should_choose_a_card_based_on_strategy()
        {
            var handCards = new[]
            {
                new Card("Danari", 5),
                new Card("Coppe", 8)
            };
            _strategyMock.Setup(m => m.Choose(It.IsAny<IPlayer>(), It.IsAny<GameState>()))
                .Returns(handCards[0]);


            var sut = new Player(_strategyMock.Object, "testPlayer", handCards);
            var gameState = new GameState(new IPlayer[] { sut }, new Card());

            var card = sut.Play(gameState);

            sut.HandCards.Should().Not.Contain(card);
            sut.HandCards.Count().Should().Be(1);
        }

        [Fact]
        public void Play_should_return_a_card_from_hand_removing_it()
        {
            var handCards = new[]
            {
                new Card("Danari", 5),
                new Card("Coppe", 8)
            };
            _strategyMock.Setup(m => m.Choose(It.IsAny<IPlayer>(), It.IsAny<GameState>()))
                .Returns(handCards[0]);
            var state = new GameState(new List<IPlayer>(), new Card());
            var sut = new Player(_strategyMock.Object, "testPlayer", handCards);

            var card = sut.Play(state);

            sut.HandCards.Should().Not.Contain(card);
            sut.HandCards.Count().Should().Be(1);
        }

        // Step 3: Aggiungo un test sul metodo creato
        [Fact]
        public void Save_should_get_cards_and_put_the_in_the_Stack()
        {
            var sut = new Player(_strategyMock.Object, "testPlayer");
            var cardset1 = new[]
            {
                new Card("Danari", 5),
                new Card("Coppe", 8)
            };
            var cardset2 = new[]
            {
                new Card("Danari", 3),
                new Card("Spade", 1)
            };

            // Eseguo due volte per verificare che la funzione sia incrementale
            sut.Save(cardset1);
            sut.Save(cardset2);

            sut.Stack.Should().Contain(cardset1.ElementAt(0));
            sut.Stack.Should().Contain(cardset1.ElementAt(1));
            sut.Stack.Should().Contain(cardset2.ElementAt(0));
            sut.Stack.Should().Contain(cardset2.ElementAt(1));
            sut.Stack.Count().Should().Be(4);
        }

        [Fact]
        public void Take_should_pick_a_card_into_hand()
        {
            var sut = new Player(_strategyMock.Object, "testPlayer");
            var card = new Card("Cuori", 5);

            sut.Take(card);

            sut.HandCards.Should().Have.SameSequenceAs(new[] { card });
        }

        [Fact]
        public void Look_should_notify_strategy()
        {
            var sut = new Player(_strategyMock.Object, "testPlayer");
            var briscola = new Card("Cuori", 5);
            var state = new GameState(new List<IPlayer>(), briscola);

            sut.Look(state);

            _strategyMock.Verify(x => x.Watch(sut, state));
        }


        [Fact(DisplayName = "A player can choose only cards from his hands")]
        public void Play_should_choose_cards_from_hands_only()
        {
            var state = new GameState(new List<IPlayer>(), new Card());
            var handCards = new[]
             {
                new Card("Danari", 5),
                new Card("Coppe", 8),
                new Card("Spade", 2)
            };
            var sut = new Player(_strategyMock.Object, "testPlayer", handCards);
            _strategyMock.Setup(m => m.Choose(It.IsAny<IPlayer>(), It.IsAny<GameState>())).Returns(new Card("Spade", 5));

            var ex = Record.Exception(() =>
            {
                var card = sut.Play(state);
            });

            ex.Should().Be.OfType<InvalidCardException>();
        }

    }

   
}