using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Grappachu.Briscola.Model;
using Grappachu.Briscola.Players.zaninig.Strategies;

namespace Grappachu.Briscola.Players.zaninig.Model
{
    public static class CardExtensions
    {
        public static int BaseWeight(this Card card)
        {
            return StrategyMatrix.CardWeigth[card.Value];
        }

        private static bool IsCarico(this Card card)
        {
            if (card.Value == 1 || card.Value == 3)
                return true;

            return false;
        }

        public static bool IsCaricoDiBriscola(this Card card, Card briscola)
        {
            return card.IsCarico() && card.IsBriscola(briscola);
        }

        public static bool IsCaricoNonBriscola(this Card card, Card briscola)
        {
            return card.IsCarico() && !card.IsBriscola(briscola);
        }

        public static bool IsBriscola(this Card card, Card briscola)
        {
            return card.Seed == briscola.Seed;
        }

        public static bool IsCaricoOBriscola(this Card card, Card briscola)
        {
            return IsCarico(card) || IsBriscola(card, briscola);
        }

    }
}
