using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    // Tracks an active and discard deck, and cards in play
    public class DeckManager
    {
        private Deck active, discard;
        private List<Card> in_play;

        #region [ CONSTRUCTORS ]

        public DeckManager()
        {
            active = DeckFactory.StandardDeck();
            discard = DeckFactory.EmptyDeck();
            in_play = new List<Card>();
            active.Shuffle();
        }

        #endregion

        /* Reclaim all cards, resttore to active pile, and shuffle */
        public void Reset()
        {
            // TODO: Reclaim cards
            active.Claim(discard);
            if (active.Size() != 52) throw new Exception("Missing cards");
            active.Shuffle();
        }

        /* Draw a card from active deck, and add it to in play */
        public Card Draw()
        {
            Card top = active.Draw();
            in_play.Add(top);
            return top;
        }

        /* Move a card(s) to the discard pile, and remove from play */
        public void Discard(Card card)
        {
            if (!in_play.Contains(card)) throw new Exception("Card not in play.");
            in_play.Remove(card);
            discard.Append(card);
        }
        public void Discard(IEnumerable<Card> cards)
        {
            foreach (Card card in cards) Discard(card);
        }

    }
}
