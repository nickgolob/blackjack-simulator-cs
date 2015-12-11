using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    public class Hand
    {
        private List<Card> cards;

        public Player owner { get; private set; }

        public int total { get; private set; }
        public bool soft_hand { get; private set; }
        public bool doubled_down { get; set; }

        public Hand(Player owner) 
        { 
            cards = new List<Card>();
            total = 0;
            soft_hand = false;
            this.owner = owner;
        }
        public ~Hand() { if (cards.Count > 0) throw new Exception("Destructing non empty hand"); }

        public void AddCard(Card card)
        {
            if (doubled_down) throw new Exception("Hand is already doubled down");

            cards.Add(card);
            total += Blackjack.values[card.value];
            if (card.value == Card.Value.Ace && total + 10 <= 21)
            {
                total += 10;
                soft_hand = true;
            }
            else if (soft_hand && total > 21)
            {
                total -= 10;
                soft_hand = false;
            }
        }

        public bool CanSplit() 
        {
            if (cards.Count != 2) return false;
            return cards[0].value == cards[1].value;
        }

        /* Returns cards and resets hand to initial state */
        public List<Card> Reclaim()
        {
            List<Card> cards = this.cards;
            this.cards.Clear();

            total = 0;
            soft_hand = false;

            return cards;
        }
    }
}
