using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    class Blackjack
    {
        #region [ CONSTANTS ]
        public static Dictionary<Card.Value, int> values = new Dictionary<Card.Value, int>()
        {
            { Card.Value.Ace,   1  },
            { Card.Value.Two,   2  },
            { Card.Value.Three, 3  },
            { Card.Value.Four,  4  },
            { Card.Value.Five,  5  },
            { Card.Value.Six,   6  },
            { Card.Value.Seven, 7  },
            { Card.Value.Eight, 8  },
            { Card.Value.Nine,  9  },
            { Card.Value.Ten,   10 },
            { Card.Value.Jack,  10 },
            { Card.Value.Queen, 10 },
            { Card.Value.King,  10 },
        };

        public enum Action { Hit, Stand, Split, DoubleDown }

        public int bet = 2;
        #endregion

        private DeckManager deck_manager;

        private Hand house_hand;
        private Card house_showing_card;

        private List<Player> players;
        private List<Hand> player_hands;

        public Blackjack(IEnumerable<Player> players)
        {
            deck_manager = new DeckManager();
            house_hand = new Hand(null);
            this.players = new List<Player>(players);
            player_hands = new List<Hand>();
        }

        public void GiveCard(Hand hand) { hand.AddCard(deck_manager.Draw()); }

        public void PlayTurn(Hand hand)
        {
            if (hand.total > 21) return;

            Action action = hand.owner.Act(hand, house_showing_card);
            if (action == Action.Hit)
            {
                GiveCard(hand);
                PlayTurn(hand);
            }
            else if (action == Action.DoubleDown)
            {
                GiveCard(hand);
                hand.doubled_down = true;
            }
            if (action == Action.Split)
            {
                if (!hand.CanSplit()) throw new Exception("Hand not splittable");
                List<Card> cards = hand.Reclaim();

                // First hand
                hand.AddCard(cards[0]);
                PlayTurn(hand);

                // Second hand
                Hand new_hand = new Hand(hand.owner);
                new_hand.AddCard(cards[1]);
                player_hands.Add(new_hand);
                PlayTurn(new_hand);
            }
        }

        public void PlayRound()
        {
            // burn card
            deck_manager.Discard(deck_manager.Draw());

            // deal
            for (int i = 0; i < players.Count; i++) player_hands.Insert(i, new Hand(players[i]));
            for (int i = 0; i < players.Count; i++) GiveCard(player_hands[i]);
            GiveCard(house_hand);
            for (int i = 0; i < players.Count; i++) GiveCard(player_hands[i]);
            house_showing_card = deck_manager.Draw();
            house_hand.AddCard(house_showing_card);

            // Play if dealer does not have blackjack
            if (house_hand.total < 21)
            {
                // Players go
                for (int i = 0; i < players.Count; i++)
                {
                    // Check for blackjack
                    if (player_hands[i].total == 21)
                    {
                        // give winnings, collect cards
                        player_hands[i].owner.ExchangeMoney(3);
                        deck_manager.Discard(player_hands[i].Reclaim());
                    }
                    else
                    {
                        PlayTurn(player_hands[i]);
                    }
                }

                // Dealer go
                while (house_hand.total < 21) GiveCard(house_hand);
            }

            // distribute winnings, reset game state
            foreach (Hand hand in player_hands)
            {
                if (hand.total > 21)
                {
                    hand.owner.ExchangeMoney(hand.doubled_down ? -4 : -2);
                }
                else if (hand.total > house_hand.total || house_hand.total > 21)
                {
                    hand.owner.ExchangeMoney(hand.doubled_down ? 4 : 2);
                    deck_manager.Discard(hand.Reclaim());
                }
                if (hand.total < house_hand.total)
                {
                    hand.owner.ExchangeMoney(hand.doubled_down ? -4 : -2);
                }
                else if (hand.total != house_hand.total)
                {
                    throw new Exception("Error in this algorithm");
                }

                deck_manager.Discard(hand.Reclaim());
            }
        }
    }
}
