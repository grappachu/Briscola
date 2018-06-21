using Grappachu.Briscola.Model;
using SharpTestsEx;
using Xunit;

namespace Grappachu.Briscola.Test.Logic
{
    public class CardTest
    {
        [Fact]
        public void Should_be_comparable()
        {
            var c1 = new Card("Spade", 7);
            var c2 = new Card("Spade", 5);
            var c3 = new Card("Spade", 7);
            var c4 = new Card("Bastoni", 5);

            c1.Equals(c2).Should().Be.False();
            c1.Equals(c3).Should().Be.True();
            c2.Equals(c3).Should().Be.False();
            c2.Equals(c4).Should().Be.False();
        }
    }
}