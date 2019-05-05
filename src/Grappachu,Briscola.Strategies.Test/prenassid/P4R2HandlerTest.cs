using System.Linq;
using Grappachu.Briscola.Model;
using Grappachu.Briscola.Players.prenassid;
using SharpTestsEx;
using Xunit;

namespace Grappachu.Briscola.Strategies.Test.prenassid
{
    public class P4R2HandlerTest
    {
        private readonly GrappachuStrategy _sut;

        public P4R2HandlerTest()
        {
            _sut = new GrappachuStrategy();
        }

        [Fact(DisplayName = "Alla seconda mano: deve stare fuori se mettono briscola")]
        public void P1R2_deve_stare_fuori_se_giocano_briscola()
        {
            var briscola = new Card("Spade", 3);
            var state = StateFixture.Create4P(_sut, true, briscola, new[]
                {
                    // In tavola
                    new Card("Spade", 2)
                },
                new[]
                {
                    // In mano
                    new Card("Danari", 2),
                    new Card("Bastoni", 9),
                    new Card("Spade", 4)
                });
            var me = state.Players.First(x => x.Strategy == _sut);
            
            var card = _sut.Choose(me, state);

            card.Should().Be.EqualTo(new Card("Danari", 2));
        }

        [Fact(DisplayName = "Alla seconda mano: deve andare su con i punti")]
        public void P1R2_pua_andare_su_con_qualche_punto()
        {
            var briscola = new Card("Spade", 3);
            var state = StateFixture.Create4P(_sut, true, briscola, new[]
                {
                    // In tavola
                    new Card("Bastoni", 5)
                },
                new[]
                {
                    // In mano
                    new Card("Danari", 2),
                    new Card("Bastoni", 9),
                    new Card("Spade", 4)
                });
            var me = state.Players.First(x => x.Strategy == _sut);
            
            var card = _sut.Choose(me, state);

            card.Should().Be.EqualTo(new Card("Bastoni", 9));
        }
        
    }
}