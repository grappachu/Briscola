using System;

namespace Grappachu.Briscola.Interfaces
{
    /// <summary>
    ///     Rappresenta una interfaccia utente per il gioco
    /// </summary>
    public interface IUserInterface
    {
        /// <summary>
        ///     Richiede un input da parte del giocatore
        /// </summary>
        /// <returns></returns>
        string Get();

        /// <summary>
        ///     Richiede al giocatore di operare una scelta
        /// </summary>
        /// <returns></returns>
        string GetChoice();

        /// <summary>
        ///     Invia un messaggio al giocatore
        /// </summary>
        /// <param name="message"></param>
        void Send(string message);

        /// <summary>
        ///     Invia un messaggio in evidenza al giocatore
        /// </summary>
        /// <param name="message"></param>
        void Strong(string message);

        /// <summary>
        ///     Invia un messaggio personalizzato al giocatore
        /// </summary>
        /// <param name="color"></param>
        /// <param name="message"></param>
        void Custom(ConsoleColor color, string message);

        /// <summary>
        /// Richiede all'utente l'input di un valore intero
        /// </summary>
        /// <param name="message">Il messaggio da visualizzare</param>
        /// <param name="defaultValue">Un valore di default che, se specificato, permette all'utente di non inserire nulla.</param>
        /// <param name="color">Il colore del messaggio visualizzato</param>
        /// <returns></returns>
        int GetInt(string message, int? defaultValue, ConsoleColor color = ConsoleColor.DarkYellow);
         
    }
}