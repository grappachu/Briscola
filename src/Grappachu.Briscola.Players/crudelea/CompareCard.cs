using Grappachu.Briscola.Model;

namespace Grappachu.Briscola.Players.crudelea
{
    public static class CompareCard
    {
        public static int CompareCards(this Card card, Card cardToCompare)
        {
            var cardPoints = ConvertToPoints(card.Value);
            var cardToComparePoints = ConvertToPoints(cardToCompare.Value);

            return cardPoints > cardToComparePoints ? 1 : cardPoints < cardToComparePoints ? -1 : 0;    
        }

        private static int ConvertToPoints(int value)
        {
            switch (value)
            {
                case 1:
                    return 11;
                case 3:
                    return 10;
                case 8:
                    return 2;
                case 9:
                    return 3;
                case 10:
                    return 4;
                default: return 0;
            }
        }
    }
}
