using Grappachu.Briscola.Model;
using Grappachu.Briscola.Players.depratoa;
using SharpTestsEx;
using Xunit;

namespace Grappachu.Briscola.Strategies.Test.depratoa
{
    public class CardExtensionsTest
    {
        Card c1 = new Card(CardSeed.CLUBS, 8);
        Card c2 = new Card(CardSeed.COINS, 1);
        Card c3 = new Card(CardSeed.CUPS, 2);
        Card c4 = new Card(CardSeed.COINS, 9);
        Card c5 = new Card(CardSeed.SWORDS, 3);
        Card c6 = new Card(CardSeed.CLUBS, 10);
        Card c7 = new Card(CardSeed.CUPS, 7);
        Card c8 = new Card(CardSeed.SWORDS, 4);

        [Fact(DisplayName = "Le figure devono essere riconosciute come punti, asso e 3 come carichi, il resto scartine")]
        public void PointsArePoints()
        {
            c1.GetCardType().Should().Be(CardType.Points);
            c2.GetCardType().Should().Be(CardType.Trump);
            c3.GetCardType().Should().Be(CardType.LowValue);
            c3.GetCardType().Should().Not.Be(CardType.Points);
            c3.GetCardType().Should().Not.Be(CardType.Trump);
            c4.GetCardType().Should().Be(CardType.Points);
            c5.GetCardType().Should().Be(CardType.Trump);
            c6.GetCardType().Should().Be(CardType.Points);
            c7.GetCardType().Should().Be(CardType.LowValue);
            c8.GetCardType().Should().Not.Be(CardType.Trump);
        }

        [Fact(DisplayName = "La valutazione delle carte deve essere indipendente dal seme")]
        public void SeedDoesntMatter()
        {
            c1.GetCardType().Should().Be(c6.GetCardType());
            c2.GetCardType().Should().Be(c5.GetCardType());
            c3.GetCardType().Should().Be(c7.GetCardType());
            c4.GetCardType().Should().Be(c1.GetCardType());
            c4.GetCardType().Should().Not.Be(c2.GetCardType());
            c4.GetCardType().Should().Not.Be(c3.GetCardType());
        }

        [Fact(DisplayName = "Il peso di gioco assegnato ai tipi deve essere Load > Points > LowValue")]
        public void TypesAreComparable()
        {
            c1.Satisfies(x => x.GetCardType().CompareTo(c2.GetCardType()) < 0);
            c2.Satisfies(x => x.GetCardType().CompareTo(c1.GetCardType()) > 0);
            c1.Satisfies(x => x.GetCardType().CompareTo(c6.GetCardType()) == 0);
            c3.Satisfies(x => x.GetCardType().CompareTo(c8.GetCardType()) == 0);
            c3.Satisfies(x => x.GetCardType().CompareTo(c6.GetCardType()) < 0);
            c5.Satisfies(x => x.GetCardType().CompareTo(c6.GetCardType()) < 0);
        }
    }
}
