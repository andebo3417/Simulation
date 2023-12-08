using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace Simulation
{
    public class Card : IComparable
    {
        public int suit { get; }
        public int strength { get; }
        public CroppedBitmap img { get; }

        public Card(int suit, int strength)
        {
            if (suit < 1 || suit > 4) throw new ArgumentOutOfRangeException("Suit cannot be less 0 or more 3");
            if (strength < 2 || strength > 14) throw new ArgumentOutOfRangeException("Strength cannot be less 2 or more 14");
            this.suit = suit - 1;
            this.strength = strength - 1;
            if (strength == 14) this.strength = 0;
            System.Windows.Int32Rect rectangle = new System.Windows.Int32Rect(225 * this.strength, 315 * this.suit, 225, 315);
            BitmapImage bmp = new BitmapImage(new Uri("pack://application:,,,/Resources/cards.png"));
            img = new CroppedBitmap(bmp, rectangle);
            this.strength = strength;
            this.suit++;

        }

        public int CompareTo(object obj)
        {
            Card card = obj as Card;
            if (this.strength > card.strength) return 1;
            if (this.strength < card.strength) return -1;
            return 0;
        }

    }
}
