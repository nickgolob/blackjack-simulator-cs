using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    class Player
    {
        public int bank { get; private set; }

        public Player()
        {
            bank = 0;
        }

        public void ExchangeMoney(int amount) 
        {
            bank += amount;
        }

        virtual public Blackjack.Action Act(Hand hand, Card house_showing_card);
    }
}
