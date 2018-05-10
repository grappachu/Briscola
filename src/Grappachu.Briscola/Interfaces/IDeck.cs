using System.Collections.Generic;
using Grappachu.Briscola.Model;

namespace Grappachu.Briscola.Interfaces
{
    /// <summary>
    /// Rappresenta un generico mazzo di carte
    /// </summary>
    public interface IDeck : IEnumerable<Card>
    {
        /// <summary>
        /// Mescola il mazzo modificando l'ordine delle carte in modo causale
        /// </summary>
        void Shuffle();

        /// <summary>
        /// Restituisce la prima carta del mazzo rimuovendola (rappresenta la "pesca")
        /// </summary>
        /// <returns></returns>
        Card Pop();

        /// <summary>
        /// Ottiene l'ultima carta del mazzo ma senza rimuoverla
        /// </summary>
        /// <returns></returns>
        Card PeekLast();

        /// <summary>
        /// Ottiene un valore che indica il numero di carte nel mazzo
        /// </summary>
        int Count { get; }
    }
}