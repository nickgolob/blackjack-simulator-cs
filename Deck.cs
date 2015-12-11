using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    public static class DeckFactory
    {
        public static Deck EmptyDeck()
        {
            return new Deck();
        }

        public static Deck StandardDeck()
        {
            return new Deck(from Card.Suit suit in Enum.GetValues(typeof(Card.Suit))
                            from Card.Value val in Enum.GetValues(typeof(Card.Value))
                            select new Card(val, suit));
        }
    }

    public class Deck
    {
        protected List<Card> cards;

        #region [ CONSTRUCTORS ]

        public Deck() { }
        public Deck(IEnumerable<Card> cards) { this.cards = new List<Card>(cards); }

        #endregion

        public int Size() { return cards.Count; }
        public bool Empty() { return Size() == 0; }

        /* Removes card from top of deck and returns */
        public Card Draw() 
        {
            if (Empty()) throw new Exception("Deck is empty");
            Card top = cards[0];
            cards.RemoveAt(0);
            return top;
        }

        /* Adds card to bottom of deck */
        public void Append(Card card) { cards.Add(card); }

        /* Randomly reordered deck */
        public void Shuffle() 
        {
            Random random = new Random();
            cards.OrderBy((item) => random.Next());
        }

        /* Take ownership of given decks cards */
        public void Claim(Deck deck)
        {
            cards.AddRange(deck.cards);
            deck.cards.Clear();
        }
    }
}
