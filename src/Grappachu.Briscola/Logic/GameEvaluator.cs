using System.Collections.Generic;
using System.Linq;
using Grappachu.Briscola.Interfaces;
using Grappachu.Briscola.Model;
using Grappachu.Briscola.Utils;

namespace Grappachu.Briscola.Logic
{
    // Step 3: Raccolgo la logica "complessa" del gioco in una classe
    public class GameEvaluator : IGameEvaluator
    {
        public GameEvaluator()
        {
            
        }
        
        public int Assign(GameState state)
        {
            var playerIdx = state.Evaluate();
            var player = state.Players.ElementAt(playerIdx);

            Chat.GetUI().Strong(string.Format("{0} | VINCE ({1} punti)\n", player.Name.PadRight(8), Totalize(state.Dish)));

            // Un comodo metodo per far si che il giocatore raccolga le carte e le metta nel suo stack
            player.Save(state.Dish);

            //Pulisco il piatto
            state.Dish.Clear();

            return playerIdx;
        }





        public static int Totalize(IEnumerable<Card> cards)
        {
            return cards.Select(x => x.Value).Sum(ValueToScore);
        }

        private static int ValueToScore(int value)
        {
            switch (value)
            {
                case 1:
                    return 11;
                case 3:
                    return 10;
                case 10:
                    return 4;
                case 9:
                    return 3;
                case 8:
                    return 2;
                default:
                    return 0;
            }
        }
    }
}