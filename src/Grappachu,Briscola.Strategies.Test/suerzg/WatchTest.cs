using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grappachu.Briscola.Model;
using Grappachu.Briscola.Players.suerzg;
using Grappachu.Briscola.Strategies.Test.suerzg.Utils;
using SharpTestsEx;
using Xunit;

namespace Grappachu.Briscola.Strategies.Test.suerzg
{
  public class WatchTest
  {

    private readonly Gabry404Strategy _gabry404Strategy;

    public WatchTest()
    {
      _gabry404Strategy = new Gabry404Strategy();
    }

    [Fact(DisplayName = "Aggiunge i semi al 'Cimitero'")]
    public void AggiungeLeCarteDellaManoAllaListaDiCarteGiocate()
    {
      var briscola = new Card("Spade", 3);
      var state = CreateDish.CreateHand(_gabry404Strategy, briscola, new Card[] {
          new Card("Bastoni", 3),
          new Card("Danari", 2),
          new Card("Spade", 4)},
        new[]
        {
          new Card("Coppe", 3),
          new Card("Coppe", 2),
          new Card("Coppe", 4)
        }, 3);

      var me = state.Players.Single(x => x.Strategy == _gabry404Strategy);
      foreach (var player in state.Players)
      {
        if (player.Strategy != _gabry404Strategy)
        {
          player.Strategy.Choose(player, state);
          me.Strategy.Watch(me, state);
        }
      }

      var myStrategy = (Gabry404Strategy) me.Strategy;
      myStrategy.PlayedCards.Count.Should().Be.EqualTo(3);
      myStrategy.PlayedCards.ContainsKey("Bastoni").Should().Be.True();
      myStrategy.PlayedCards.ContainsKey("Danari").Should().Be.True();
      myStrategy.PlayedCards.ContainsKey("Spade").Should().Be.True();
    }

    [Fact(DisplayName = "Aggiunge le carte al seme giusto")]
    public void AggiungeLeCarteDellaManoAllaListaDiCarteGiocateNelSemeGiusto()
    {
      var briscola = new Card("Spade", 3);
      var state = CreateDish.CreateHand(_gabry404Strategy, briscola, new Card[] {
          new Card("Bastoni", 3),
          new Card("Danari", 2),
          new Card("Spade", 4)},
        new[]
        {
          new Card("Coppe", 3),
          new Card("Coppe", 2),
          new Card("Coppe", 4)
        }, 3);

      var me = state.Players.Single(x => x.Strategy == _gabry404Strategy);
      foreach (var player in state.Players)
      {
        if (player.Strategy != _gabry404Strategy)
        {
          player.Strategy.Choose(player, state);
          me.Strategy.Watch(me, state);
        }
      }

      var myStrategy = (Gabry404Strategy)me.Strategy;
      myStrategy.PlayedCards.ContainsKey("Bastoni").Should().Be.True();
      myStrategy.PlayedCards["Bastoni"].Count.Should().Be(1);
      myStrategy.PlayedCards.ContainsKey("Danari").Should().Be.True();
      myStrategy.PlayedCards["Danari"].Count.Should().Be(1);
      myStrategy.PlayedCards.ContainsKey("Spade").Should().Be.True();
      myStrategy.PlayedCards["Spade"].Count.Should().Be(1);
    }
  }
}
