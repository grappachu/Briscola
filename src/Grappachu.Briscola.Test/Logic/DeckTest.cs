using System.Collections.Generic;
using System.Linq;
using Grappachu.Briscola.Interfaces;
using Grappachu.Briscola.Logic;
using Grappachu.Briscola.Model;
using SharpTestsEx;
using Xunit;

namespace Grappachu.Briscola.Test.Logic
{
    public class DeckTest
    {
        public DeckTest()
        {
            var seeds = new[] { "Cuori", "Picche", "Quadri", "Fiori" };
            var valueRange = new Range(1, 13);
            _sut = new Deck(seeds, valueRange);
        }

        private readonly IDeck _sut;


        [Fact]
        public void Deck_should_be_IEnumerable_of_cards()
        {
            var enumerable = _sut as IEnumerable<Card>;

            enumerable.Should().Not.Be.Null();
            _sut.Count.Should().Be(52);
        }

        [Fact(DisplayName = "L'inizializzazione del mazzo deve essere dinamica, per semi e valori")]
        public void Deck_should_initialize_dynamilcally_with_seeds_and_values()
        {
            var seeds = new[] { "Coppe", "Spade" };
            var valueRange = new Range(1, 3);

            // uso un nuovo sut in modo da non dipendere dagli altri test
            IDeck sut = new Deck(seeds, valueRange);

            sut.Should()
                .Have.SameValuesAs(new Card("Coppe", 1),
                    new Card("Coppe", 2),
                    new Card("Coppe", 3),
                    new Card("Spade", 1),
                    new Card("Spade", 2),
                    new Card("Spade", 3));
        }

        //  Creo un metodo per vedere l'ultima carta del mazzo senza rimuoverla  (Briscola)
        //         Si poteva usare direttamente il Last() dal momento che il deck è IEnumerable 
        //         ma è utile fare un metodo nell'interfaccia per poterlo mockare facilmente
        [Fact(DisplayName = "Deve poter vedere l'ultima carta del mazzo che sarà la briscola")]
        public void PeekLast_should_look_at_last_card_without_removing_it()
        {
            var seeds = new[] { "Coppe", "Spade" };
            var valueRange = new Range(1, 3);
            IDeck sut = new Deck(seeds, valueRange);
            var expected = sut.Last();

            var card = sut.PeekLast();

            card.Should().Be(expected);
            sut.Should().Contain(card);
            sut.Count().Should().Be(6);
        }


        [Fact(DisplayName = "Pesca di una carta")]
        public void Pop_should_take_first_card_of_Deck_removing_it()
        {
            var seeds = new[] { "Coppe", "Spade" };
            var valueRange = new Range(1, 3);
            IDeck sut = new Deck(seeds, valueRange);
            var expected = sut.First();

            var card = sut.Pop();

            card.Should().Be(expected);
            sut.Should().Not.Contain(card);
            sut.Count().Should().Be(5);
        }
        
        [Fact(DisplayName = "Il mescolamento non deve essere idempotente")]
        public void Shuffle_should_change_cards_order_at_every_time()
        {
            _sut.Shuffle();
            var seq1 = _sut.ToArray();

            _sut.Shuffle();
            var seq2 = _sut.ToArray();

            seq1.Should().Not.Have.SameSequenceAs(seq2);
        }
    }
}