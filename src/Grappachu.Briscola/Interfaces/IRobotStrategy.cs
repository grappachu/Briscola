using System;

namespace Grappachu.Briscola.Interfaces
{
    /// <inheritdoc />
    /// <summary>
    ///     Rappresenta una strategia di tipo automatico
    /// </summary>
    public interface IRobotStrategy : IStrategy
    {
        /// <summary>
        ///     Ottiene l'autore dell'algoritmo implementato da questa strategia
        /// </summary>
        string Author { get; }

        /// <summary>
        ///     Ottiene o imposta la versione dell'algoritmo impelemtato da questa strategia
        /// </summary>
        Version Version { get; }
    }
}