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

        public static GameState Create4P(IStrategy sut, Card briscola, Card[] dish, Card[] handCards)
        {
            var deck = XDeckFactory.CreateDeck();
            var turn = XRandom.Next(0, 4);
            List<IPlayer> players = new List<IPlayer>();
            for (int i = 0; i < 4; i++)
            {
                if ((4-turn + i) % 4 != dish.Length)
                {
                    players.Add(new Player(new RandomStrategy(), "p" + i + 1));
                }
                else
                {
                    players.Add(new Player(sut, "p" + i + 1));
                }
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