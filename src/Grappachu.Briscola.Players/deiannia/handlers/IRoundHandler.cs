using Grappachu.Briscola.Interfaces;
using Grappachu.Briscola.Model;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Grappachu.Briscola.Players.deiannia.handlers
{
    public interface IRoundHandler
    {
        IRoundHandler SetNext(IRoundHandler handler);

        Card ChooseCard(IPlayer myself, GameState state);
    }
}
