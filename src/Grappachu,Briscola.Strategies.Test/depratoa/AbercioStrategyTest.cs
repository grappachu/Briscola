using Grappachu.Briscola.Interfaces;
using Grappachu.Briscola.Logic;
using Grappachu.Briscola.Model;
using Grappachu.Briscola.Players.depratoa;
using Moq;
using SharpTestsEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;
using static Grappachu.Briscola.Players.depratoa.AbercioStrategy;

namespace Grappachu.Briscola.Strategies.Test.depratoa
{
    public class AbercioStrategyTest
    {
        private AbercioStrategy Strategy;
        private IGame<GameState> Game;
        private IDeck Deck;
        private GameState State;
        private IPlayer Player1;
        private IPlayer Player2;

        public AbercioStrategyTest()
        {
            Strategy = new AbercioStrategy();
            Deck = new RandomizedDeck();
            Game = new BriscolaGame(new RandomizedDeckFactory(Deck), new PlayerFactory());
            Game.Join("Gianni", Strategy);
            Game.Join("Pinotto", Strategy);

            FieldInfo deckField = Game.GetType().GetField("_deck", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance);
            deckField.SetValue(Game, Deck);

            FieldInfo subscribersField = Game.GetType().GetField("_subscribers", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance);
            List<IPlayer> players = (List<IPlayer>)subscribersField.GetValue(Game);
            foreach (Player player in players)
            {
                for (var i = 0; i < 3; i++)
                {
                    var card = Deck.Pop();
                    player.Take(card);
                }
            }
            Card briscola = Deck.PeekLast();
            State = new GameState(players, briscola);
            PropertyInfo stateProperty = Game.GetType().GetProperty("State");
            stateProperty.SetValue(Game, State);

            Player1 = State.Players.ElementAt(0);
            Player2 = State.Players.ElementAt(1);
        }

        [Fact(DisplayName = "Ad inizio partita, con briscola D9, le mani ordinate di Player1 e Player2 saranno [B1,B8,S8] e [D1,S3,S5]")]
        public void TurnInitOperations()
        {
            Strategy.InitTurnData(Player1, State);
            Strategy.Briscola.Should().Be(State.Briscola);
            Strategy.BriscolaSeed.Should().Be(State.Briscola.Seed);
            Player1.HandCards.ElementAt(0).Satisfies(x => ! Strategy.IsBriscola(x));
            Player1.HandCards.ElementAt(1).Satisfies(x => ! Strategy.IsBriscola(x));
            Player1.HandCards.ElementAt(2).Satisfies(x => Strategy.IsBriscola(x));
            Player2.HandCards.ElementAt(0).Satisfies(x => Strategy.IsBriscola(x));
            Player2.HandCards.ElementAt(1).Satisfies(x => Strategy.IsBriscola(x));
            Player2.HandCards.ElementAt(2).Satisfies(x => ! Strategy.IsBriscola(x));
            IEnumerable<Card> hand1 = new List<Card>(Player1.HandCards);
            Strategy.SortCards(Player1, Strategy.EvaluateCardWeight);
            Player1.HandCards.ElementAt(0).Should().Be(hand1.ElementAt(2));
            Player1.HandCards.ElementAt(1).Should().Be(hand1.ElementAt(1));
            Player1.HandCards.ElementAt(2).Should().Be(hand1.ElementAt(0));
            IEnumerable<Card> hand2 = new List<Card>(Player2.HandCards);
            Strategy.SortCards(Player2, Strategy.EvaluateCardWeight);
            Player2.HandCards.ElementAt(0).Should().Be(hand2.ElementAt(0));
            Player2.HandCards.ElementAt(1).Should().Be(hand2.ElementAt(1));
            Player2.HandCards.ElementAt(2).Should().Be(hand2.ElementAt(2));
        }

        [Fact(DisplayName = "La strategia deve assegnare un valore di gioco decrescente per ogni carta in mano al giocatore in base alla briscola, alle carte uscite, al piatto ed alla sua mano")]
        public void RankAssignation()
        {
            Strategy.SortCards(Player1, Strategy.EvaluateCardWeight);
            Strategy.SortCards(Player2, Strategy.EvaluateCardWeight);
            IEnumerable<Card> hand1 = new List<Card>(Player1.HandCards);
            IEnumerable<Card> hand2 = new List<Card>(Player2.HandCards);
            Strategy.EvaluateBaseRank(hand1.ElementAt(0)).Should().Be(4);
            Strategy.EvaluateBaseRank(hand1.ElementAt(1)).Should().Be(0);
            Strategy.EvaluateBaseRank(hand1.ElementAt(2)).Should().Be(4);
            Strategy.EvaluateBaseRank(hand2.ElementAt(0)).Should().Be(1);
            Strategy.EvaluateBaseRank(hand2.ElementAt(1)).Should().Be(7);
            Strategy.EvaluateBaseRank(hand2.ElementAt(2)).Should().Be(0);
            Strategy.EvaluateUnknownTrumpSituation(Player1, State, CardSeed.CLUBS).Should().Be(UnknownTrumpSituation.THREE);
            Strategy.EvaluateUnknownTrumpSituation(Player1, State, CardSeed.SWORDS).Should().Be(UnknownTrumpSituation.ALL);
            Strategy.EvaluateUnknownTrumpSituation(Player1, State, CardSeed.CUPS).Should().Be(UnknownTrumpSituation.ALL);
            Strategy.EvaluateUnknownTrumpSituation(Player1, State, CardSeed.COINS).Should().Be(UnknownTrumpSituation.ALL);
            Strategy.EvaluateUnknownTrumpSituation(Player2, State, CardSeed.CLUBS).Should().Be(UnknownTrumpSituation.ALL);
            Strategy.EvaluateUnknownTrumpSituation(Player2, State, CardSeed.SWORDS).Should().Be(UnknownTrumpSituation.ACE);
            Strategy.EvaluateUnknownTrumpSituation(Player2, State, CardSeed.CUPS).Should().Be(UnknownTrumpSituation.ALL);
            Strategy.EvaluateUnknownTrumpSituation(Player2, State, CardSeed.COINS).Should().Be(UnknownTrumpSituation.THREE);
            Strategy.EvaluateFallenTrumpAdjustment(Player1, State, hand1.ElementAt(0)).Should().Be(0);
            Strategy.EvaluateFallenTrumpAdjustment(Player1, State, hand1.ElementAt(1)).Should().Be(0);
            Strategy.EvaluateFallenTrumpAdjustment(Player1, State, hand1.ElementAt(2)).Should().Be(-1);
            Strategy.EvaluateFallenTrumpAdjustment(Player2, State, hand2.ElementAt(0)).Should().Be(0);
            Strategy.EvaluateFallenTrumpAdjustment(Player2, State, hand2.ElementAt(1)).Should().Be(-1);
            Strategy.EvaluateFallenTrumpAdjustment(Player2, State, hand2.ElementAt(2)).Should().Be(0);
            Strategy.AssignRank(Player1, State, hand1.ElementAt(0)).Should().Be(2);
            Strategy.AssignRank(Player1, State, hand1.ElementAt(1)).Should().Be(0);
            Strategy.AssignRank(Player1, State, hand1.ElementAt(2)).Should().Be(3);
            Strategy.AssignRank(Player2, State, hand2.ElementAt(0)).Should().Be(-1);
            Strategy.AssignRank(Player2, State, hand2.ElementAt(1)).Should().Be(2);
            Strategy.AssignRank(Player2, State, hand2.ElementAt(2)).Should().Be(0);
            Strategy.ChooseBestLower(Player1, State).Should().Be(hand1.ElementAt(2));
            Strategy.ChooseBestLower(Player2, State).Should().Be(hand2.ElementAt(1));
            Strategy.ChooseWorstLower(Player1, State).Should().Be(hand1.ElementAt(2));
            Strategy.ChooseWorstLower(Player2, State).Should().Be(hand2.ElementAt(1));
        }

        [Fact(DisplayName = "Turno1 - [S8,B1,(B8)], [S3,(S5),D1]*")]
        public void FirstTurn()
        {
            Strategy.SortCards(Player1, Strategy.EvaluateCardWeight);
            Strategy.SortCards(Player2, Strategy.EvaluateCardWeight);
            IEnumerable<Card> hand1 = new List<Card>(Player1.HandCards);
            IEnumerable<Card> hand2 = new List<Card>(Player2.HandCards);
            Card choice1;
            Card choice2;
            choice1 = Strategy.DoChoose(Player1, State);
            choice1.Should().Be(hand1.ElementAt(2));
            State.Dish.Add(Player1.Play(State));
            choice2 = Strategy.DoChoose(Player2, State);
            choice2.Should().Be(hand2.ElementAt(1));
            State.Dish.Add(Player2.Play(State));
            Strategy.DoWatch(State);
            Strategy.FallenTrumps[CardSeed.CLUBS].AllInPlay.Should().Be(true);
            Strategy.FallenTrumps[CardSeed.SWORDS].AllInPlay.Should().Be(true);
            Strategy.FallenTrumps[CardSeed.CUPS].AllInPlay.Should().Be(true);
            Strategy.FallenTrumps[CardSeed.COINS].AllInPlay.Should().Be(true);
            int winnerIdx = Game.AssignHand(State);
            winnerIdx.Should().Be(1);
            State.Turn = winnerIdx;
        }

        [Fact(DisplayName = "Turno2 - [S8,B1,(C6)], [S3,D1,(D9)]*")]
        public void SecondTurn()
        {
            SimulateNextTurn(1, 2, 2, 1);
        }

        [Fact(DisplayName = "Turno3 - [S8,S2,(B1)]*, [S3,D1,(B5)]")]
        public void ThirdTurn()
        {
            SimulateNextTurn(1, 1, 2, 0);
            Strategy.FallenTrumps[CardSeed.CLUBS].AllInPlay.Should().Be(false);
            Strategy.FallenTrumps[CardSeed.CLUBS].FallenAce.Should().Be(true);
            Strategy.FallenTrumps[CardSeed.CLUBS].FallenThree.Should().Be(false);
            Strategy.FallenTrumps[CardSeed.CLUBS].AllFallen.Should().Be(false);
        }

        [Fact(DisplayName = "Turno4 - [S8,S2,(D8)], [S3,C3,(D1)]*")]
        public void ForthTurn()
        {
            SimulateNextTurn(0, 2, 2, 1);
            Strategy.FallenTrumps[CardSeed.COINS].AllInPlay.Should().Be(false);
            Strategy.FallenTrumps[CardSeed.COINS].FallenAce.Should().Be(true);
            Strategy.FallenTrumps[CardSeed.COINS].FallenThree.Should().Be(false);
            Strategy.FallenTrumps[CardSeed.COINS].AllFallen.Should().Be(false);
        }

        [Fact(DisplayName = "Turno5 - [S9,S8,(S2)]*, [S3,C3,(B9)]")]
        public void FifthTurn()
        {
            SimulateNextTurn(1, 2, 2, 0);
        }

        [Fact(DisplayName = "La strategia deve operare senza errori fino a fine partita, anche quando la mano si riduce a meno di 3 carte")]
        public void ToEndGameWithNoErrors()
        {
            var totalTurns = 40 / Game.State.Players.Count - 5;
            for (var i = 0; i < totalTurns; i++)
            {
                Game.PlayHand();
                Game.Refill();
            }
        }

        internal void SimulateNextTurn(int startingTurn, int expectedChoice1, int expectedChoice2, int expectedWinnerIdx)
        {
            Game.Refill();
            Strategy.SortCards(Player1, Strategy.EvaluateCardWeight);
            Strategy.SortCards(Player2, Strategy.EvaluateCardWeight);
            IEnumerable<Card> hand1 = new List<Card>(Player1.HandCards);
            IEnumerable<Card> hand2 = new List<Card>(Player2.HandCards);
            
            int index;
            IPlayer player;
            Card choice; 
            for (int i = 0; i < State.Players.Count; i++)
            {
                index = (i + startingTurn) % State.Players.Count;
                player = State.Players.ElementAt(index);
                choice = Strategy.DoChoose(player, State);
                if (index == 0)
                {
                    choice.Should().Be(hand1.ElementAt(expectedChoice1));
                }
                else
                {
                    choice.Should().Be(hand2.ElementAt(expectedChoice2));
                }
                State.Dish.Add(player.Play(State));
            }

            Strategy.DoWatch(State);
            int winnerIdx = Game.AssignHand(State);
            winnerIdx.Should().Be(expectedWinnerIdx);
            State.Turn = winnerIdx;
        }
    }


    internal class RandomizedDeckFactory : IDeckFactory
    {
        IDeck Deck;
        public RandomizedDeckFactory(IDeck deck)
        {
            Deck = deck;
        }
        public IDeck CreateDeck()
        {
            throw new NotImplementedException();
        }
    }

    internal class RandomizedDeck : Deck
    {
        public RandomizedDeck() : base(new string[] { CardSeed.CLUBS, CardSeed.SWORDS, CardSeed.COINS, CardSeed.CUPS }, new Model.Range(1, 10))
        {
            FieldInfo field = typeof(Deck).GetField("_cards", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance); 
            List<Card> cards = (List<Card>)field.GetValue(this);
            Card swap;
            int randomizedIndex;
            for (int i = 0; i < 40; i++)
            {
                randomizedIndex = RandomizedIndexes[i];
                swap = cards[i];
                cards[i] = cards[randomizedIndex];
                cards[randomizedIndex] = swap;
            }
        }

        private int[] RandomizedIndexes
        {
            get
            {
                if (_indexes == null) {
                    _indexes = new List<int>();
                    for (int i=0; i<40; i++)
                    {
                        _indexes.Add(i);
                    }
                    PseudoRandom random = PseudoRandom.Instance;
                    int randIdx;
                    int swap;
                    for (int i = 39; i >= 0; i--)
                    {
                        randIdx = random.Next(i);
                        swap = _indexes[randIdx];
                        _indexes[randIdx] = _indexes[i];
                        _indexes[i] = swap;
                    }
                }
                return _indexes.ToArray();
            }
        }
        private IList<int> _indexes;
    }

    internal class PseudoRandom
    {
        public static PseudoRandom Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PseudoRandom(31654);
                }
                return _instance;
            }
        }
        private static PseudoRandom _instance;

        private const int MAX = 1000000;

        private int Seed;

        private PseudoRandom(int seed)
        {
            Seed = seed;
        }

        public int Next()
        {
            Seed = Math.Abs(Seed * (Seed - 43) - (Seed * 12 / 27)) % MAX;
            return Seed;
        }

        public int Next(int module)
        {
            if (module == 0)
            {
                return 0;
            }
            return Next() % module;
        }
    }
}
