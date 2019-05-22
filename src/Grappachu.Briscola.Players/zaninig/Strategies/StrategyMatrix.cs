using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grappachu.Briscola.Players.zaninig.Strategies
{
    public class StrategyMatrix
    {
        /// <summary>
        /// Associa la carta al suo valore
        /// </summary>
        public static readonly Dictionary<int, int> CardWeigth = new Dictionary<int, int>() {
            {1, 12},
            {3, 11},
            {10, 4},
            {9, 3},
            {8, 2},
            {7, 1},
            {6, 1},
            {5, 1},
            {4, 1},
            {2, 1}
        };
    }
}
