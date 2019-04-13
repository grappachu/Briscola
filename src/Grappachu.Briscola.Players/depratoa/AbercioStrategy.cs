using System;
using System.Collections.Generic;
using System.Linq;
using Grappachu.Briscola.Interfaces;
using Grappachu.Briscola.Model;
using Grappachu.Briscola.Players.Default;
using System.Reflection;

namespace Grappachu.Briscola.Players.depratoa
{
    public class AbercioStrategy : StrategyBase
    {
        public static string StrategyName = "Abercio10";

        public GameState State;
        public IPlayer Player;
        public Card Briscola;
        public string BriscolaSeed;
        public IDictionary<string, TrumpStatus> FallenTrumps;
        public ICollection<Card> FallenBriscola;

        public AbercioStrategy() : base(StrategyName)
        {
            FallenTrumps = new Dictionary<string, TrumpStatus>()
            {
                { CardSeed.CLUBS, new TrumpStatus() },
                { CardSeed.SWORDS, new TrumpStatus() },
                { CardSeed.CUPS, new TrumpStatus() },
                { CardSeed.COINS, new TrumpStatus() },
            };
            FallenBriscola = new SortedSet<Card>(new ValueComparer());
        }

        protected override Card OnChoose(IPlayer myself, GameState state)
        {
            return DoChoose(myself, state);
        }

        protected override void OnWatch(IPlayer myself, GameState state)
        {
            DoWatch(state);
        }





        public Card DoChoose(IPlayer myself, GameState state)
        {
            InitTurnData(myself, state);
            SortCards(myself, EvaluateCardWeight);

            Card choice = myself.HandCards.Last();
            if (state.Dish.Count == 0)
            {
                choice = ChooseBestLower(myself, state);
            }
            else
            {
                Card leadingCard = state.Dish.First();
                if (IsBriscola(leadingCard))
                {
                    choice = ChooseWorstLower(myself, state);
                }
                else
                {
                    bool chosen = false;
                    if (leadingCard.GetCardType() == CardType.Trump)
                    {
                        IEnumerable<Card> handBriscole = myself.HandCards.Where(x => IsBriscola(x));
                        if (handBriscole.Count() > 0)
                        {
                            int maxRank = handBriscole.Select(x => EvaluateBaseRank(x)).Max();
                            choice = handBriscole.Single(x => EvaluateBaseRank(x) == maxRank);
                            chosen = true;
                        }
                    }
                    if (!chosen)
                    {
                        IEnumerable<Card> seedMatchingCards = myself.HandCards.Where(x => x.Seed.Equals(leadingCard.Seed));
                        if (seedMatchingCards.Count() == 0)
                        {
                            choice = ChooseWorstLower(myself, state);
                        }
                        else
                        {
                            Card card;
                            for (int i = 0; i < seedMatchingCards.Count() && !chosen; i++)
                            {
                                card = seedMatchingCards.ElementAt(i);
                                if (card.GetCardType() > leadingCard.GetCardType())
                                {
                                    choice = card;
                                    chosen = true;
                                }
                                else if (card.GetCardType() == leadingCard.GetCardType())
                                {
                                    if (card.Value == 1 || (card.GetCardType() != CardType.Trump && card.Value > leadingCard.Value))
                                    {
                                        choice = card;
                                        chosen = true;
                                    }
                                }
                                else if (card.GetCardType() == CardType.LowValue && card.Value < leadingCard.Value)
                                {
                                    choice = card;
                                    chosen = true;
                                }
                            }
                            if (!chosen)
                            {
                                choice = ChooseWorstLower(myself, state);
                            }
                        }
                    }
                }
            }

            return choice;
        }

        public void DoWatch(GameState state)
        {
            foreach (Card card in state.Dish)
            {
                if (card.GetCardType() == CardType.Trump)
                {
                    if (card.Value == 1)
                    {
                        FallenTrumps[card.Seed].FallenAce = true;
                    }
                    else
                    {
                        FallenTrumps[card.Seed].FallenThree = true;
                    }
                }
                if (IsBriscola(card))
                {
                    FallenBriscola.Add(card);
                }
            }
        }

        public void InitTurnData(IPlayer myself, GameState state)
        {
            State = state;
            Player = myself;
            if (BriscolaSeed == null)
            {
                Briscola = state.Briscola;
                BriscolaSeed = Briscola.Seed;
            }
        }

        public void SortCards(IPlayer myself, Func<Card, int> sortingFunc)
        {
            IComparer<Card> comparer = new CardComparer(this, myself.HandCards, sortingFunc);
            List<Card> sortedCards = new List<Card>(myself.HandCards.OrderByDescending(x => x, comparer));
            FieldInfo field = myself.GetType().GetField("_handCards", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance);
            List<Card> playerHand = (List<Card>)field.GetValue(myself);
            playerHand.Clear();
            playerHand.AddRange(sortedCards);
        }

        public bool IsBriscola(Card card)
        {
            return card.Seed.Equals(BriscolaSeed);
        }

        public int EvaluateCardWeight(Card card) {
            return ((int)card.GetCardType()) * 10 + (IsBriscola(card) ? 20 : 0) + card.Value;
        }
        
        public Card ChooseBestLower(IPlayer myself, GameState state)
        {
            SortCards(myself, AssignRank);
            IEnumerable<Card> handCards = myself.HandCards;
            IDictionary<Card, int> rankedCards = handCards.ToDictionary((x) => x, x => AssignRank(myself, state, x));
            Card bestCard = handCards.First();
            CardType bestType = bestCard.GetCardType();
            int bestIndex = 0;
            int bestRank = rankedCards[bestCard];
            int index = bestIndex + 1;
            int rank;
            Card card;
            CardType type;
            while (index < handCards.Count())
            {
                card = handCards.ElementAt(index);
                type = card.GetCardType();
                rank = rankedCards[card];
                if ((IsBriscola(bestCard) || type <= bestType) && rank < bestRank && !IsBriscola(card) && card.GetCardType() != CardType.Trump)
                {
                    bestIndex = index;
                    bestRank = rank;
                    bestType = type;
                    bestCard = card;
                }
                index++;
            }
            return bestCard;
        }

        public Card ChooseWorstLower(IPlayer myself, GameState state)
        {
            SortCards(myself, AssignRank);
            IEnumerable<Card> handCards = myself.HandCards;
            IDictionary<Card, int> rankedCards = handCards.ToDictionary((x) => x, x => AssignRank(myself, state, x));
            Card worstCard = handCards.First();
            CardType worstType = worstCard.GetCardType();
            int worstIndex = 0;
            int worstRank = rankedCards[worstCard];
            int index = worstIndex + 1;
            int rank;
            Card card;
            CardType type;
            while (index < handCards.Count())
            {
                card = myself.HandCards.ElementAt(index);
                type = card.GetCardType();
                rank = rankedCards[card];
                if ((IsBriscola(worstCard) || type <= worstType) && (rank > worstRank || (IsBriscola(worstCard) && !IsBriscola(card) && card.GetCardType() != CardType.Trump)))
                {
                    worstIndex = index;
                    worstRank = rank;
                    worstType = type;
                    worstCard = card;
                }
                index++;
            }
            return worstCard;
        }

        public int AssignRank(Card card)
        {
            return AssignRank(Player, State, card);
        }

        public int AssignRank(IPlayer myself, GameState state, Card card)
        {
            IDictionary<Card, int> rankedCards = new Dictionary<Card, int>();
            int rank;
            int trumpAdjustment;
            int briscolaAdjustment;
            rank = EvaluateBaseRank(card);
            trumpAdjustment = EvaluateFallenTrumpAdjustment(myself, state, card);
            briscolaAdjustment = 0;
            if (IsBriscola(card))
            {
                switch (card.GetCardType())
                {
                    case CardType.Trump:
                        briscolaAdjustment = -2;
                        break;
                    case CardType.Points:
                        briscolaAdjustment = -2;
                        break;
                    default:
                        briscolaAdjustment = 3 - rank;
                        break;
                }
            }
            return rank + trumpAdjustment + briscolaAdjustment;
        }

        public int EvaluateBaseRank(Card card)
        {
            int rank;
            if (card.GetCardType() == CardType.Trump)
            {
                rank = card.Value == 1 ? 0 : 1;
            }
            else if (card.GetCardType() == CardType.Points)
            {
                rank = 2 + (10 - card.Value);
            }
            else
            {
                if (card.Value == 2)
                {
                    rank = 9;
                }
                else
                {
                    rank = 5 + (7 - card.Value);
                }
            }
            return rank;
        }

        public int EvaluateFallenTrumpAdjustment(IPlayer myself, GameState state, Card card)
        {
            int adjustment = 0;
            UnknownTrumpSituation unknownTrump = EvaluateUnknownTrumpSituation(myself, state, card.Seed);
            if (card.GetCardType() == CardType.Trump)
            {
                if (card.Value == 3)
                {
                    if (unknownTrump == UnknownTrumpSituation.NONE)
                    {
                        adjustment = -1;
                    }
                }
            }
            else
            {
                if (unknownTrump == UnknownTrumpSituation.NONE)
                {
                    adjustment = -2;
                }
                else if (unknownTrump != UnknownTrumpSituation.ALL)
                {
                    adjustment = -1;
                }
            }
            return adjustment;
        }

        public UnknownTrumpSituation EvaluateUnknownTrumpSituation(IPlayer myself, GameState state, String seed)
        {
            UnknownTrumpSituation result;
            TrumpStatus trumpStatus = FallenTrumps[seed];
            if (trumpStatus.AllFallen)
            {
                result = UnknownTrumpSituation.NONE;
            }
            else
            {
                if (trumpStatus.AllInPlay)
                {
                    result = UnknownTrumpSituation.ALL;
                }
                else if (trumpStatus.FallenAce)
                {
                    result = UnknownTrumpSituation.THREE;
                }
                else
                {
                    result = UnknownTrumpSituation.ACE;
                }
                List<Card> knownTrumps = new List<Card>();
                knownTrumps.AddRange(myself.HandCards.Where(x => x.Seed.Equals(seed) && x.GetCardType() == CardType.Trump));
                knownTrumps.AddRange(state.Dish.Where(x => x.Seed.Equals(seed) && x.GetCardType() == CardType.Trump));
                foreach (Card card in knownTrumps)
                {
                    if (card.Value == 1)
                    {
                        result = (result == UnknownTrumpSituation.ALL) ? UnknownTrumpSituation.THREE : UnknownTrumpSituation.NONE;
                    }
                    else
                    {
                        result = (result == UnknownTrumpSituation.ALL) ? UnknownTrumpSituation.ACE : UnknownTrumpSituation.NONE;
                    }
                }
            }
            return result;
        }





        private class CardComparer : IComparer<Card>
        {
            AbercioStrategy _outer;
            IDictionary<Card, int> _weightedHand;
            public CardComparer(AbercioStrategy outer, IEnumerable<Card> cards, Func<Card, int> comparingFunc)
            {
                _outer = outer;
                _weightedHand = new Dictionary<Card, int>();
                int weight;
                foreach (Card card in cards)
                {
                    weight = comparingFunc(card);
                    _weightedHand.Add(card, weight);
                }
            }

            public int Compare(Card x, Card y)
            {
                int xWeight = _weightedHand[x];
                int yWeight = _weightedHand[y];
                return xWeight.CompareTo(yWeight);
            }
        }

        private class ValueComparer : IComparer<Card>
        {
            public int Compare(Card x, Card y)
            {
                return x.Value.CompareTo(y.Value);
            }
        }

        public class TrumpStatus
        {
            public bool FallenAce;
            public bool FallenThree;
            public bool AllFallen => FallenAce && FallenThree;
            public bool AllInPlay => !FallenAce && !FallenThree;
        }

        public enum UnknownTrumpSituation
        {
            NONE,
            THREE,
            ACE,
            ALL
        }
    }
}
