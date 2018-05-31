using System.Linq;
using Grappachu.Briscola.Model;
using Grappachu.Briscola.Players.prenassid;
using SharpTestsEx;
using Xunit;

namespace Grappachu.Briscola.Strategies.Test.prenassid
{
    public class P4R1HandlerTest
    {
        private readonly GrappachuStrategy _sut;

        public P4R1HandlerTest()
        {
            _sut = new GrappachuStrategy();
        }

        [Fact]
        public void P1R4_deve_andare_liscio()
        {
            var briscola = new Card("Spade", 3);
            var state = StateFixture.Create4P(_sut, briscola, new Card[] { },
                new[]
                {
                    new Card("Bastoni", 3),
                    new Card("Danari", 2),
                    new Card("Spade", 4)
                });
            var me = state.Players.Single(x => x.Strategy == _sut);

            var card = _sut.Choose(me, state);

            card.Should().Be.EqualTo(new Card("Danari", 2));
        }

        [Fact]
        public void P1R4_deve_preferire_carta_franca()
        {
            var briscola = new Card("Spade", 3);
            var previousHand = StateFixture.Create4P(_sut, briscola, new[] 
                {
                    new Card("Coppe", 2),
                    new Card("Danari", 9),
                    new Card("Coppe", 10),
                    new Card("Spade", 8) 
                },
                new Card[] { });
            var state = StateFixture.Create4P(_sut, briscola, new Card[] { },
                new[]
                {
                    new Card("Bastoni", 5),
                    new Card("Danari", 2),
                    new Card("Coppe", 4)
                });
            var me = state.Players.Single(x => x.Strategy == _sut);
            _sut.Watch(me, previousHand);

            var card = _sut.Choose(me, state);

            card.Should().Be.EqualTo(new Card("Coppe", 4));
        }



    }
}