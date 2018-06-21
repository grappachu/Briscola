using System.Collections.Generic;
using System.Collections.ObjectModel;
using Grappachu.Briscola.Interfaces;

namespace Grappachu.Briscola.Model
{
    public class GameState
    {
        private GameState()
        {
            Dish = new List<Card>();
        }

        public GameState(IList<IPlayer> players, Card briscola) : this()
        {
            Players = new ReadOnlyCollection<IPlayer>(players);
            Briscola = briscola;
            Dish.Clear();
            Turn = 0;
        }

        public Card Briscola { get; internal set; }
        public IReadOnlyCollection<IPlayer> Players { get; private set; }
        public int Turn { get; set; }
        public IList<Card> Dish { get; } 
    }
}