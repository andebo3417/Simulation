using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation
{
    public class CardPack
    {
        private List<Card> pack;
        private Random random;
        private System.Windows.Controls.Label amountLabel;
        public CardPack(System.Windows.Controls.Label cardpackLabel)
        {
            pack = new List<Card>();
            random = new Random();
            int suit = 1;
            int strength = 1;

            for (int i = 0; i < 52; i++)
            {
                pack.Add(new Card(suit, strength + 1));
                strength++;
                if (strength == 14)
                {
                    strength = 1;
                    suit++;
                }
            }

            amountLabel = cardpackLabel;
            updateAmount();
        }

        public Card takeTop()
        {
            try
            {
                if (pack[pack.Count - 1] != null)
                {
                    Card card = pack[pack.Count - 1];
                    pack.RemoveAt(pack.Count - 1);
                    updateAmount();
                    return card;
                }
            }
            catch (InvalidOperationException)
            {
                return null;
            }
            return null;
        }

        public bool putBottom(Card card)
        {
            if (pack.Count < 52)
            {
                pack = pack.Prepend(card).ToList();
                updateAmount();
                return true;

            }
            return false;
        }

        private void updateAmount()
        {
            amountLabel.Content = pack.Count.ToString();
        }

        public void mixRandom()
        {
            int index = 0;

            for (int i = 0; i < pack.Count; i++)
            {
                do index = random.Next(0, pack.Count);
                while (index == i);

                Card tmp = pack[i];
                pack[i] = pack[index];
                pack[index] = tmp;

            }
        }

    }
}
