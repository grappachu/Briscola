using System;
using System.Collections.Generic;
using Grappachu.Briscola.Interfaces;
using Grappachu.Briscola.Logic;
using Grappachu.Briscola.Model;
using Grappachu.Briscola.Players.Default;

namespace Grappachu.Briscola.Strategies.Test.suerzg
{
    public static class CreateDish
    {
        public static GameState CreateHand(IStrategy sut, Card briscola, Card[] dish, Card[] handCards, int turn = 5)
        {
            var deck = new ItalianDeckFactory().CreateDeck();
            turn = turn != 5 ? turn : new Random().Next(0, 4);
            var players = new List<IPlayer>();
            for (int i = 0; i < 4; i++)
            {
                players.Add((4 - turn + i) % 4 != dish.Length
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