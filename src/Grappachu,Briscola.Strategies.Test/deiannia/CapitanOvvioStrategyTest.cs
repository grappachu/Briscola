using Grappachu.Briscola.Model;
using Grappachu.Briscola.Players.deiannia;
using SharpTestsEx;
using System.Linq;
using Xunit;

namespace Grappachu.Briscola.Strategies.Test.deiannia
{
    public class CapitanOvvioStrategyTest
    {
        public CapitanOvvioStrategyTest()
        {
            _sut = new CapitanOvvioStrategy();
        }

        private readonly CapitanOvvioStrategy _sut;

        [Fact(DisplayName = "Il memorizzatore di carte deve memorizzare 2 carte dopo un turno")]
        public void Played_cards_memory_should_work()
        {
            var briscola = new Card("Spade", 3);
            var state = GameStateBuilder.GetBuilder().Build(_sut, briscola);
            state.Dish.Add(new Card("Danari", 4));
            state.Dish.Add(new Card("Bastoni", 1));

            var me = state.Players.Single(x => x.Strategy == _sut);
            _sut.Watch(me, state);

            _sut.GetPlayedCards().Count().Should().Be.EqualTo(2);
            _sut.GetPlayedCards().ElementAt(0).Should().Be.EqualTo(new Card("Danari", 4));
            _sut.GetPlayedCards().ElementAt(1).Should().Be.EqualTo(new Card("Bastoni", 1));
        }

        [Fact(DisplayName = "Se è la prima mano e ho 2 carte alte e una briscola, uso la briscola")]
        public void Se_prima_mano_e_2_carte_alte_e_briscola_gioco_briscola()
        {
        }

        [Fact(DisplayName = "Se sono l'ultimo del turno e ci sono più di 11 punti sul piatto, tento di prendere")]
        public void Se_piu_di_11_punti_prendo()
        {
            var state = GameStateBuilder.GetBuilder().Build(_sut, new Card("Spade", 4), 4);
            state.Dish.Add(new Card("Bastoni", 1));
            state.Dish.Add(new Card("Bastoni", 8));
            state.Dish.Add(new Card("Danari", 4));

            var me = state.Players.First(x => x.Strategy == _sut);
            me.Take(new Card("Danari", 5));
            me.Take(new Card("Spade", 5));
            me.Take(new Card("Danari", 7));

            Card card = _sut.Choose(me, state);

            card.Should().Be.EqualTo(new Card("Spade", 5));
        }

        [Fact(DisplayName = "Se sono il terzo")]
        public void Se_sono_terzo_compagno_prende_carico()
        {
            var state = GameStateBuilder.GetBuilder().Build(_sut, new Card("Spade", 4), 4);
            state.Dish.Add(new Card("Bastoni", 3));
            state.Dish.Add(new Card("Spade", 5));
            state.Dish.Add(new Card("Danari", 7));

            var me = state.Players.First(x => x.Strategy == _sut);
            me.Take(new Card("Spade", 6));
            me.Take(new Card("Danari", 1));
            me.Take(new Card("Bastoni", 8));

            Card card = _sut.Choose(me, state);

            card.Should().Be.EqualTo(new Card("Danari", 1));
        }

        [Fact(DisplayName = "Se sono il quarto del turno e il mio compagno prende, carico")]
        public void Se_sono_quarto_e_compagno_prende_carico()
        {
            var state = GameStateBuilder.GetBuilder().Build(_sut, new Card("Spade", 4), 4);
            state.Dish.Add(new Card("Bastoni", 3));
            state.Dish.Add(new Card("Spade", 5));
            state.Dish.Add(new Card("Danari", 7));

            var me = state.Players.First(x => x.Strategy == _sut);
            me.Take(new Card("Spade", 6));
            me.Take(new Card("Danari", 1));
            me.Take(new Card("Bastoni", 8));

            Card card = _sut.Choose(me, state);

            card.Should().Be.EqualTo(new Card("Danari", 1));
        }
    }
}
