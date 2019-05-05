using System;
using System.Collections.Generic;
using Grappachu.Briscola.Interfaces;
using Grappachu.Briscola.Logic;
using Grappachu.Briscola.Model;
using Grappachu.Briscola.Players.Default;

namespace Grappachu.Briscola.Strategies.Test.prenassid
{
    public static class StateFixture
    {
        private static readonly IDeckFactory XDeckFactory = new ItalianDeckFactory();
        private static readonly Random XRandom = new Random();
         

        public static GameState Create4P(IStrategy sut, bool withPartner, Card briscola, Card[] dish, Card[] handCards)
        {
            var deck = XDeckFactory.CreateDeck();
            var turn = XRandom.Next(0, 4);
            var players = new List<IPlayer>();
            for (int i = 0; i < 4; i++)
            {
                var div = withPartner ? 2 : 4;
                players.Add((4 - turn + i) % div != dish.Length
                    ? new Player(new RandomStrategy(), "p" + i + 1)
                    : new Player(sut, "p" + i + 1));
            }

            GameState gameState = new GameState(players, briscola) { Turn = turn };
            foreach (var player in players)
            {
                if (player.Strategy == sut)
                {
                    foreach (var card in handCards)
                    {
                        player.Take(card);
                    }
                }
                else
                {
                    player.Take(deck.Pop());
                    player.Take(deck.Pop());
                    player.Take(deck.Pop());
                }
            }

            foreach (var card in dish)
            {
                gameState.Dish.Add(card);
            }

            return gameState;
        }
    }
}