using Grappachu.Briscola.Model;
using System.Collections.Generic;
using System.Linq;

namespace Grappachu.Briscola.Players.depratoa
{
    public enum CardType : int
    {
        LowValue = 0,
        Points = 1,
        Trump = 2,
    }

    public static class CardSeed
    {
        public const string SWORDS = "Spade";
        public const string CLUBS = "Bastoni";
        public const string CUPS = "Coppe";
        public const string COINS = "Danari";
    }

    public static class CardExtensions
    {
        static readonly IEnumerable<int> TrumpValues = new int[] { 1, 3 };
        static readonly IEnumerable<int> PointsValues = new int[] { 8, 9, 10 };

        public static CardType GetCardType(this Card card)
        {
            CardType result;
            int value = card.Value;
            if (TrumpValues.Contains(value))
            {
                result = CardType.Trump;
            }
            else if(PointsValues.Contains(value))
            {
                result = CardType.Points;
            }
            else
            {
                result = CardType.LowValue;
            }
            return result;
        }
    }
}
