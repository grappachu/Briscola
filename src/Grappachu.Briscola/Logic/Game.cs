using System;
using System.Collections.Generic;
using System.Linq;
using Grappachu.Briscola.Interfaces;
using Grappachu.Briscola.Model;
using Grappachu.Briscola.Utils;

namespace Grappachu.Briscola.Logic
{
    public class BriscolaGame : IGame<GameState>
    {
        private readonly IDeckFactory _deckFactory;
        private readonly IPlayerFactory _playerFactory;
        private readonly List<IPlayer> _subscribers;
        private IDeck _deck;

        public BriscolaGame(IDeckFactory deckFactory, IPlayerFactory playerFactory)
        {
            _deckFactory = deckFactory;
            _playerFactory = playerFactory;
            _subscribers = new List<IPlayer>();
        }


        public GameState State { get; private set; }

        public void Start()
        {
            _deck = _deckFactory.CreateDeck();
            _deck.Shuffle();
            var briscola = _deck.PeekLast();

            if (_subscribers.Count != 2 && _subscribers.Count != 4)
                throw new InvalidOperationException("You can start a new game only with 2 or 4 players. Please Join");

            var state = new GameState(_subscribers, briscola);
            foreach (var pl in state.Players)
                for (var i = 0; i < 3; i++)
                {
                    var card = _deck.Pop();
                    pl.Take(card);
                }
            state.Briscola = _deck.PeekLast();
            Chat.GetUI().Strong(string.Format("BRISCOLA: {0} di {1}", state.Briscola.Value, state.Briscola.Seed));

            State = state;
        }

        public void PlayHand()
        {
            var pCount = State.Players.Count;
            for (var i = 0; i < pCount; i++)
            {
                var playerIdx = (i + State.Turn) % pCount;
                var toPlay = State.Players.ElementAt(playerIdx);

                var playedCard = toPlay.Play(State);

                State.Dish.Add(playedCard);
            }

            for (var i = 0; i < pCount; i++)
            {
                var playerIdx = (i + State.Turn) % pCount;
                var loLook = State.Players.ElementAt(playerIdx);
                loLook.Look(State);
            }

            var winnerIdx = AssignHand(State);
            State.Turn = winnerIdx;
        }



        public int AssignHand(GameState state)
        {
            var playerIdx = state.Evaluate();
            var player = state.Players.ElementAt(playerIdx);

            Chat.GetUI()
                .Strong(string.Format("{0} | VINCE ({1} punti)\n", player.Name.PadRight(8), BriscolaUtils.Totalize(state.Dish)));

            // Un comodo metodo per far si che il giocatore raccolga le carte e le metta nel suo stack
            player.Save(state.Dish);

            //Pulisco il piatto
            state.Dish.Clear();

            return playerIdx;
        }

        public void Refill()
        {
            var pCount = State.Players.Count;
            if (_deck.Count >= pCount)
                for (var i = 0; i < pCount; i++)
                {
                    var playerIdx = (i + State.Turn) % pCount;
                    var toRefill = State.Players.ElementAt(playerIdx);

                    var card = _deck.Pop();
                    toRefill.Take(card);
                }
        }


        public void Join(string playerName, IStrategy strategy)
        {
            var pl = _playerFactory.CreatePlayer(playerName, strategy);
            _subscribers.Add(pl);
        }
    }
}