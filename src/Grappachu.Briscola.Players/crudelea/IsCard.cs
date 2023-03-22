using Grappachu.Briscola.Model;

namespace Grappachu.Briscola.Players.crudelea
{
    public static class IsCard
    {
        // collegamento alle funzioni
        public static bool IsHighCard(this Card card)
        {
            return card.IsAce() || card.IsThree();
        }
        public static bool IsLowCard(this Card card)
        {
            return card.Value >= 8 && card.Value <= 10;
        }
        public static bool IsNoValueCard(this Card card)
        {
            return card.Value == 2 || card.Value >= 4 && card.Value <= 7;
        }
        public static bool IsAce(this Card card)
        {
            return card.Value == 1;
        }
        public static bool IsEmpty(this Card card)
        {
            return card.Value == 0;
        }

        public static bool IsThree(this Card card)
        {
            return card.Value == 3;
        }


        public static int CompareCards(this Card card, Card cardToCompare)
        {
            var cardPoints = ConvertToPoints(card.Value);
            var cardToComparePoints = ConvertToPoints(cardToCompare.Value);

            return cardPoints > cardToComparePoints? 1 : cardPoints < cardToComparePoints? -1 : 0;    // come if else
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


