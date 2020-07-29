using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grappachu.Briscola.Model;
using Grappachu.Briscola.Players.suerzg;
using Grappachu.Briscola.Strategies.Test.prenassid;
using SharpTestsEx;
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

		[Fact(DisplayName = "Alla prima mano se c'è una carta con grandi punti in mano viene giocata ma non se è di briscola")]
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

		[Fact(DisplayName = "Alla prima mano se c'è una carta con grandi punti in mano viene giocata ma non se è di briscola")]
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

		[Fact(DisplayName = "Deve giocare liscio")]
		public void DeveGiocareLiscio()
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
