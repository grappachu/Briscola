using System.Collections.Generic;
using Grappachu.Briscola.Model;
using Grappachu.Briscola.Players.suerzg;
using SharpTestsEx;
using System.Linq;
using Grappachu.Briscola.Strategies.Test.suerzg.Utils;
using Xunit;

namespace Grappachu.Briscola.Strategies.Test.suerzg
{
  public class Gabry404StrategyTest
  {

    private readonly Gabry404Strategy _gabry404Strategy;

    public Gabry404StrategyTest()
    {
      _gabry404Strategy = new Gabry404Strategy();
    }

    [Fact(DisplayName = "Alla prima mano se c'è un carico in mano, viene giocato")]
    public void PrimaManoSpecialGabry404()
    {
      var briscola = new Card("Spade", 3);
      var state = CreateDish.CreateHand(_gabry404Strategy, briscola, new Card[] { },
        new[]
        {
          new Card("Bastoni", 3),
          new Card("Danari", 2),
          new Card("Spade", 4)
        });
      var me = state.Players.Single(x => x.Strategy == _gabry404Strategy);

      var card = _gabry404Strategy.Choose(me, state);

      card.Should().Be.EqualTo(new Card("Bastoni", 3));
    }

    [Fact(DisplayName = "Alla prima mano se c'è un asso o tre di briscola in mano, non viene giocato")]
    public void PrimaManoSpecialGabry404Not()
    {
      var briscola = new Card("Spade", 3);
      var state = CreateDish.CreateHand(_gabry404Strategy, briscola, new Card[] { },
        new[]
        {
          new Card("Spade", 3),
          new Card("Danari", 2),
          new Card("Spade", 4)
        });
      var me = state.Players.Single(x => x.Strategy == _gabry404Strategy);

      var card = _gabry404Strategy.Choose(me, state);

      card.Should().Not.Be.EqualTo(new Card("Spade", 3));
    }

    [Fact(DisplayName = "Deve giocare liscio se è il primo giocatore")]
    public void DeveGiocareLiscioSePrimoGiocatore()
    {
      var briscola = new Card("Spade", 3);
      var state = CreateDish.CreateHand(_gabry404Strategy, briscola, new Card[] { },
        new[]
        {
          new Card("Bastoni", 3),
          new Card("Danari", 2),
          new Card("Spade", 4)
        }, 0);

      _gabry404Strategy.PlayedCards = new Dictionary<string, List<Card>>()
      {
        {"Coppe", new List<Card>{new Card("Coppe", 5)}}
      };

      var me = state.Players.Single(x => x.Strategy == _gabry404Strategy);
      var card = _gabry404Strategy.Choose(me, state);

      card.Should().Be.EqualTo(new Card("Danari", 2));
    }

    [Fact(DisplayName = "Deve giocare la briscola più bassa in mano se ci sono punti nel piatto")]
    public void DeveGiocareBriscolaBassaConPuntiNelPiatto()
    {
      var briscola = new Card("Spade", 3);
      var state = CreateDish.CreateHand(_gabry404Strategy, briscola, new Card[]
        {
          new Card("Danari", 3)
        },
        new[]
        {
          new Card("Spade", 7),
          new Card("Danari", 2),
          new Card("Spade", 4)
        });
      var me = state.Players.Single(x => x.Strategy == _gabry404Strategy);

      var card = _gabry404Strategy.Choose(me, state);

      card.Should().Be.EqualTo(new Card("Spade", 4));
    }

    [Fact(DisplayName = "Deve giocare tre di briscola se l'asso di briscola è già stato giocato in un'altra mano")]
    public void DeveGiocareAssoConAssoGiocatoAltraMano()
    {
      var briscola = new Card("Spade", 8);
      var state = CreateDish.CreateHand(_gabry404Strategy, briscola, new Card[] { },
        new[]
        {
          new Card("Bastoni", 3),
          new Card("Danari", 2),
          new Card("Spade", 3)
        }, 1);

      _gabry404Strategy.PlayedCards = new Dictionary<string, List<Card>>()
      {
        {"Spade", new List<Card>{new Card("Spade", 1)}}
      };

      var me = state.Players.Single(x => x.Strategy == _gabry404Strategy);
      var card = _gabry404Strategy.Choose(me, state);

      card.Should().Be.EqualTo(new Card("Danari", 2));
    }

    [Fact(DisplayName = "Se sul piatto c'è il re e carico prendi con una briscola superiore")]
    public void DeveGiocareBriscolaSeSulPiattoHoCaricoEReDiBriscola()
    {
      var briscola = new Card("Danari", 8);
      var state = CreateDish.CreateHand(_gabry404Strategy, briscola, new Card[]
        {
          new Card("Bastoni", 4),
          new Card("Bastoni", 3),
          new Card("Danari", 10)
        },
        new[]
        {
          new Card("Danari", 1),
          new Card("Danari", 2),
          new Card("Spade", 3)
        }, 4);

      _gabry404Strategy.PlayedCards = new Dictionary<string, List<Card>>()
      {
        {"Bastoni", new List<Card>{new Card("Bastoni", 3),new Card("Bastoni", 4)}},
        {"Danari", new List<Card>{new Card("Danari", 10)}}
      };

      var me = state.Players.Single(x => x.Strategy == _gabry404Strategy);
      var card = _gabry404Strategy.Choose(me, state);

      card.Should().Be.EqualTo(new Card("Danari", 1));
    }

    [Fact(DisplayName = "Se sul piatto c'è il re e carico prendi con una briscola di minor valore possibile")]
    public void DeveGiocareTreDiBriscolaSeSulPiattoHoCaricoEReDiBriscola()
    {
      var briscola = new Card("Danari", 8);
      var state = CreateDish.CreateHand(_gabry404Strategy, briscola, new Card[]
        {
          new Card("Bastoni", 4),
          new Card("Bastoni", 3),
          new Card("Danari", 10)
        },
        new[]
        {
          new Card("Danari", 1),
          new Card("Danari", 2),
          new Card("Danari", 3)
        }, 4);

      _gabry404Strategy.PlayedCards = new Dictionary<string, List<Card>>()
      {
        {"Bastoni", new List<Card>{new Card("Bastoni", 3),new Card("Bastoni", 4)}},
        {"Danari", new List<Card>{new Card("Danari", 10)}}
      };

      var me = state.Players.Single(x => x.Strategy == _gabry404Strategy);
      var card = _gabry404Strategy.Choose(me, state);

      card.Should().Not.Be.EqualTo(new Card("Danari", 3));
    }

  }
}
