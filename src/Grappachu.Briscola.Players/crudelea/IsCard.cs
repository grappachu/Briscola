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
            return card.Value == 8 || card.Value == 9 || card.IsKing();
        }

        public static bool IsNoValueCard(this Card card)
        {
            return card.Value == 2 || card.Value >= 4 && card.Value <= 7;
        }

        public static bool IsAce(this Card card)
        {
            return card.Value == 1;
        }

        public static bool IsThree(this Card card)
        {
            return card.Value == 3;
        }

        public static bool IsKing(this Card card)
        {
            return card.Value == 10;
        }

        public static bool IsEmpty(this Card card)
        {
            return card.Value == 0;
        }
    }
}


