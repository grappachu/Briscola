namespace Grappachu.Briscola.Interfaces
{
    /// <summary>
    ///     Rappresenta l'interfaccia per un generico gioco di carte a turni
    /// </summary>
    public interface IGame<out T>
    {
        /// <summary>
        ///     Ottiene un valore che indica lo stato di gioco
        /// </summary>
        T State { get; }

        /// <summary>
        ///     Permette ad un giocatore di unirsi al gioco prima dell'inizio
        /// </summary>
        /// <param name="playerName"></param>
        /// <param name="strategy"></param>
        void Join(string playerName, IStrategy strategy);

        /// <summary>
        ///     Avvia l'inizio del gioco
        /// </summary>
        void Start();

        /// <summary>
        ///     Esegue la logica che governa un intero turno di gioco
        /// </summary>
        void PlayHand();

        /// <summary>
        ///     Richiamato alla fine di ogni turno di gioco permette di pescare le carte necessarie per il prossimo turno
        /// </summary>
        void Refill();
    }
}