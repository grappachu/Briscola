using Grappachu.Briscola.Model;

namespace Grappachu.Briscola.Players.deiannia
{
    public static class CardExtension
    {
        /// <summary>
        /// Ottiene un valore che indica se la carta <see cref="card"/> è dello stesso seme della <see cref="other"/>
        /// </summary>
        /// <param name="card"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static bool IsSameSeed(this Card card, Card other)
        {
            return card.Seed == other.Seed;
        }

        /// <summary>
        /// Ottiene un valore che indica se la carta non vale alcun punto e non è una briscola
        /// </summary>
        /// <param name="card"></param>
        /// <param name="briscolaSeed"></param>
        /// <returns></returns>
        public static bool IsLiscio(this Card card, string briscolaSeed)
        {
            return !card.IsBriscola(briscolaSeed) && !card.IsCarico(briscolaSeed) && !card.IsPunti(briscolaSeed);
        }

        /// <summary>
        /// Ottiene un valore che indica se la carta non è briscola e non è asso o 3
        /// </summary>
        /// <param name="card"></param>
        /// <param name="briscolaSeed"></param>
        /// <returns></returns>
        public static bool IsCarico(this Card card, string briscolaSeed)
        {
            return !card.IsBriscola(briscolaSeed) && (card.Value == 1 || card.Value == 3);
        }

        /// <summary>
        /// Ottiene un valore che indica se è briscola
        /// </summary>
        /// <param name="card"></param>
        /// <param name="briscolaSeed"></param>
        /// <returns></returns>
        public static bool IsBriscola(this Card card, string briscolaSeed)
        {
            return card.Seed == briscolaSeed;
        }

        /// <summary>
        /// Ottiene un valore che indica se è una briscola che non vale punti: 2, 3, 4, 5, 6, 7
        /// </summary>
        /// <param name="card"></param>
        /// <param name="briscolaSeed"></param>
        /// <returns></returns>
        public static bool IsTaglio(this Card card, string briscolaSeed)
        {
            return card.IsBriscola(briscolaSeed) && (card.Value == 2 || card.Value == 4 || card.Value == 5 || card.Value == 6 || card.Value == 7);
        }

        /// <summary>
        /// Ottiene un valore che indica se non è briscola ed è fante, cavallo o re
        /// </summary>
        /// <param name="card"></param>
        /// <param name="briscolaSeed"></param>
        /// <returns></returns>
        public static bool IsPunti(this Card card, string briscolaSeed)
        {
            return !card.IsBriscola(briscolaSeed) && (card.Value == 8 || card.Value == 9 || card.Value == 10);
        }

        /// <summary>
        /// Ottiene un valore che indica se la carta <see cref="succ"/> è vincente su <see cref="prec"/>
        /// tenendo conto dell'ordine di uscita e della briscola corrente
        /// </summary>
        /// <param name="succ"></param>
        /// <param name="prec"></param>
        /// <param name="briscolaSeed"></param>
        /// <returns></returns>
        public static bool WinsOn(this Card succ, Card prec, string briscolaSeed)
        {
            if (!prec.IsBriscola(briscolaSeed) && succ.IsBriscola(briscolaSeed))
                return true;
            if (prec.IsBriscola(briscolaSeed) && succ.IsBriscola(briscolaSeed) && succ.Order() > prec.Order())
                return true;
            if (!prec.IsBriscola(briscolaSeed) && !succ.IsBriscola(briscolaSeed) && succ.Order() > prec.Order())
                return true;

            return false;
        }

        /// <summary>
        /// Ottiene il valore in punti della carta
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public static int ValueInPoints(this Card card)
        {
            switch (card.Value)
            {
                case 1: return 11;
                case 3: return 10;
                case 10: return 4;
                case 9: return 3;
                case 8: return 2;
                default: return 0;
            }
        }

        /// <summary>
        /// Ottiene l'indice della carta nella scala di importanza
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public static int Order(this Card card)
        {
            switch (card.Value)
            {
                case 2: return 1;
                case 4: return 2;
                case 5: return 3;
                case 6: return 4;
                case 7: return 5;
                case 8: return 6;
                case 9: return 7;
                case 10: return 8;
                case 3: return 9;
                case 1: return 10;
                default: return 0;
            }
        }
    }
}
