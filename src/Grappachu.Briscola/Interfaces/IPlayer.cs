using System.Collections.Generic;
using Grappachu.Briscola.Model;

namespace Grappachu.Briscola.Interfaces
{
    /// <summary>
    ///     Rappresenta un generico giocatore di briscola
    /// </summary>
    public interface IPlayer
    {
        /// <summary>
        /// Ottiene il nome del giocatore
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Rappresenta la lista delle carte in mano a ciascun giocatore
        /// </summary>
        IEnumerable<Card> HandCards { get; }

        IEnumerable<Card> Stack { get; }

        IStrategy Strategy { get; }
        Card Play(GameState state);
        void Take(Card card);

        void Save(IEnumerable<Card> cards);
        void Look(GameState state);
         
    }
}