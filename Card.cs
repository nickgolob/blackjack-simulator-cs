using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    public class Card
    {
        #region [ CONSTANTS ]
        public enum Suit  { Heart, Diamond, Club, Spade }
        public enum Value { Ace, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King }
        #endregion

        public Value value { get; private set; }
        public Suit suit { get; private set; }

        public Card(Value value, Suit suit)
        {
            this.suit = suit;
            this.value = value;
        }
    }
}
