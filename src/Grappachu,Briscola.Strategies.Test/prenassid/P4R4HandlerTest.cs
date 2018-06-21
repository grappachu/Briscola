using System.Linq;
using Grappachu.Briscola.Model;
using Grappachu.Briscola.Players.prenassid;
using SharpTestsEx;
using Xunit;

namespace Grappachu.Briscola.Strategies.Test.prenassid
{
    public class P4R4HandlerTest
    {
        [Fact]
        public void P4R4_deve_andare_liscio_se_tagliano()
        {
            var sut = new GrappachuStrategy();
            var briscola = new Card("Spade", 3);
            var state = StateFixture.Create4P(sut, briscola, new[]
                {
                    new Card("Bastoni", 8),
                    new Card("Bastoni", 10),
                    new Card("Spade", 2)
                },
                new[]
                {
                    new Card("Bastoni", 7),
                    new Card("Danari", 1),
                    new Card("Spade", 5)
                });
            var me = state.Players.Single(x => x.Strategy == sut);

            var card = sut.Choose(me, state);

            card.Should().Be.EqualTo(new Card("Bastoni", 7));
        }

        [Fact]
        public void P4R4_deve_andare_su_preferibilmente_di_carico()
        {
            var sut = new GrappachuStrategy();
            var briscola = new Card("Spade", 3);
            var state = StateFixture.Create4P(sut, briscola, new[]
                {
                    new Card("Bastoni", 2),
                    new Card("Coppe", 1),
                    new Card("Bastoni", 9)
                },
                new[]
                {
                    new Card("Spade", 1),
                    new Card("Bastoni", 10),
                    new Card("Spade", 5)
                });
            var me = state.Players.Single(x => x.Strategy == sut);

            var card = sut.Choose(me, state);

            card.Should().Be.EqualTo(new Card("Bastoni", 10));
        }

        [Fact]
        public void P4R4_deve_dare_carico_al_compagno()
        {
            var sut = new GrappachuStrategy();
            var briscola = new Card("Spade", 3);
            var state = StateFixture.Create4P(sut, briscola, new[]
                {
                    new Card("Bastoni", 2),
                    new Card("Bastoni", 9),
                    new Card("Coppe", 1)
                },
                new[]
                {
                    new Card("Bastoni", 10),
                    new Card("Danari", 1),
                    new Card("Spade", 5)
                });
            var me = state.Players.Single(x => x.Strategy == sut);

            var card = sut.Choose(me, state);

            card.Should().Be.EqualTo(new Card("Danari", 1));
        }

        [Fact]
        public void P4R4_deve_prendere_il_carico_paesano()
        {
            var sut = new GrappachuStrategy();
            var briscola = new Card("Spade", 3);
            var state = StateFixture.Create4P(sut, briscola, new[]
                {
                    new Card("Bastoni", 10),
                    new Card("Coppe", 10),
                    new Card("Spade", 2)
                },
                new[]
                {
                    new Card("Bastoni", 7),
                    new Card("Danari", 1),
                    new Card("Spade", 4)
                });
            var me = state.Players.Single(x => x.Strategy == sut);

            var card = sut.Choose(me, state);

            card.Should().Be.EqualTo(new Card("Spade", 4));
        }


        [Fact]
        public void P4R4_per_pochi_punti_non_deve_sprecare_l_asso()
        {
            var sut = new GrappachuStrategy();
            var briscola = new Card("Spade", 3);
            var state = StateFixture.Create4P(sut, briscola, new[]
                {
                    new Card("Bastoni", 10),
                    new Card("Coppe", 10),
                    new Card("Spade", 2)
                },
                new[]
                {
                    new Card("Danari", 1),
                    new Card("Spade", 1),
                    new Card("Bastoni", 7)
                });
            var me = state.Players.Single(x => x.Strategy == sut);

            var card = sut.Choose(me, state);

            card.Should().Be.EqualTo(new Card("Bastoni", 7));
        }
    }
}