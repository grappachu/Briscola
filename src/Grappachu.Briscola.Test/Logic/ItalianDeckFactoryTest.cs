using System.Linq;
using Grappachu.Briscola.Interfaces;
using Grappachu.Briscola.Logic;
using SharpTestsEx;
using Xunit;

namespace Grappachu.Briscola.Test.Logic
{
   
    public class ItalianDeckFactoryTest
    {
        public ItalianDeckFactoryTest()
        {
            _sut = new ItalianDeckFactory();
        }

        private readonly IDeckFactory _sut;

        [Fact]
        public void CreateDeck_should_create_a_40_cards_deck()
        {
            var deck = _sut.CreateDeck();

            // Verifico che
            deck.Count().Should().Be.EqualTo(40); // le carte siano 40
            deck.Distinct().Count().Should().Be(40); // tutte distinte
            deck.Select(c => c.Seed).Distinct().Count().Should().Be(4); // che ci siano 4 semi
            deck.All(c => c.Value >= 1 && c.Value <= 10).Should().Be.True(); // e che i valori siano tutti tra 1 e 10
        }
    }
}