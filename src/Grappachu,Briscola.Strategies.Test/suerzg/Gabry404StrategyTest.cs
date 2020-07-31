using Grappachu.Briscola.Model;
using Grappachu.Briscola.Players.suerzg;
using SharpTestsEx;
using System.Linq;
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
			var state = CreateDish.CreateHand(_gabry404Strategy,  briscola, new Card[] { },
				new[]
				{
					new Card("Bastoni", 3),
					new Card("Danari", 2),
					new Card("Spade", 4)
				}, 0);
			var me = state.Players.Single(x => x.Strategy == _gabry404Strategy);

			var card = _gabry404Strategy.Choose(me, state);

			card.Should().Be.EqualTo(new Card("Bastoni", 3));
		}

		[Fact(DisplayName = "Alla prima mano se c'è un asso o tre di briscola in mano, non viene giocato")]
		public void PrimaManoSpecialGabry404Not()
		{
			var briscola = new Card("Spade", 3);
			var state = CreateDish.CreateHand(_gabry404Strategy,  briscola, new Card[] { },
				new[]
				{
					new Card("Spade", 3),
					new Card("Danari", 2),
					new Card("Spade", 4)
				}, 0);
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
				});

      Card? card = null;
      var me = state.Players.Single(x => x.Strategy == _gabry404Strategy);
			foreach (var player in state.Players)
      {
        if (player.Strategy == _gabry404Strategy)
        {
          card = _gabry404Strategy.Choose(me, state);
        }
        else
				{
          _gabry404Strategy.Watch(me, state);
        }
      }

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

		}

	}
}
