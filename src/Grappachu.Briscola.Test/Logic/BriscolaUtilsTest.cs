using Grappachu.Briscola.Logic;
using Grappachu.Briscola.Model;
using SharpTestsEx;
using Xunit;

namespace Grappachu.Briscola.Test.Logic
{
    public class BriscolaUtilsTest
    {
        [Fact]
        public void Score_of_deck_should_be_120()
        {
            var deck = new Deck(new[] {"A", "B", "C", "D"}, new Range(1, 10));

            var res = BriscolaUtils.Totalize(deck);

            res.Should().Be(120);
        }
    }
}