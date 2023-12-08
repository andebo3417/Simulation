using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Simulation
{
    public class Player
    {
        public System.Windows.Controls.Image playerAv;
        public System.Windows.Controls.Image cardLeftImg;
        public System.Windows.Controls.Image cardRightImg;
        public System.Windows.Controls.Label wealthLabel;
        public System.Windows.Controls.Image smallChipsImg;
        public System.Windows.Controls.Label tableBetLabel;

        public static bool cardsOpenAtStart = true;

        public Card cardLeft;
        public Card cardRight;
        public double cardsWeight;
        public int bestCardStrength;
        public CardCombination cardCombination;
        private CardCombination lookingFor;
        public PlayerAction currentAction;

        public int Wealth;

        private int maxBetInStage;
        public int alreadyBetInStage;
        public int alredyBetInCircle;

        public bool isPlaying;

        private Random random;

        public static double cardWeightModifier = 0.1;
        public static double cardWeightRandomnessModifier = 0.1;
        public Player(int Wealth,
                        System.Windows.Controls.Image playerAv,
                        System.Windows.Controls.Image cardLeftImg, 
                        System.Windows.Controls.Image cardRightImg, 
                        System.Windows.Controls.Label wealthLabel,
                        System.Windows.Controls.Image smallChipsImg,
                        System.Windows.Controls.Label tableBetLabel)
        {
            this.playerAv = playerAv;
            this.cardLeftImg = cardLeftImg;
            this.cardRightImg = cardRightImg;
            this.wealthLabel = wealthLabel;
            this.smallChipsImg = smallChipsImg;
            this.tableBetLabel = tableBetLabel;
            cardLeft = null; 
            cardRight = null;
            cardsWeight = 0;
            maxBetInStage = 0;
            alreadyBetInStage = 0;
            alredyBetInCircle = 0;
            bestCardStrength = 2;
            cardCombination = CardCombination.None;
            currentAction = PlayerAction.None;
            this.Wealth = Wealth;
            isPlaying = true;
            random= new Random();
            updateWealth();
        }

        //State updates
        public void updateWealth()
        {
            wealthLabel.Content = Math.Round((double)Wealth / 1000, 2).ToString() + "K";
        }
        public void UpdateMaxBetInStage(int playerMinWealth)
        {
            maxBetInStage = (int)(Wealth * cardsWeight * random.NextDouble() * 0.3 * 0.75);
            if (maxBetInStage > playerMinWealth) maxBetInStage = playerMinWealth;
        }
        public void updateBet()
        {
            tableBetLabel.Content = Math.Round((double)alredyBetInCircle / 1000, 2).ToString() + "K";
        }

        //Cards and chips actions
        public void takeLeftCard(Card card)
        {
            cardLeft = card;
            if (cardsOpenAtStart == true)
            {
                cardLeftImg.Source = cardLeft.img;
            }
        }
        public void takeRightCard(Card card) 
        { 
            cardRight = card;
            if (cardsOpenAtStart == true)
            {
                cardRightImg.Source = cardRight.img;
            }
        }
        public void takeChips(int amount)
        {
            Wealth += amount;
            updateWealth();
        }
        
        //Analysys
        public double UpdateCardsWeight()
        {

            cardsWeight = 0;

            if (cardLeft.strength > bestCardStrength) bestCardStrength = cardLeft.strength;
            if (cardRight.strength > bestCardStrength) bestCardStrength = cardRight.strength;

            if (cardLeft.strength == cardRight.strength)
            {
                cardsWeight += 0.42;
                cardCombination = CardCombination.OnePair;
            }

            if (cardLeft.suit == cardRight.suit) cardsWeight += 0.08;

            if (Math.Abs(cardLeft.strength - cardRight.strength) == 1) cardsWeight += 0.11;

            cardsWeight += (cardLeft.strength + cardRight.strength) * 0.008 * cardWeightModifier;

            cardsWeight += random.NextDouble() * cardWeightRandomnessModifier - cardWeightRandomnessModifier / 2;

            if (cardsWeight > 1) cardsWeight = 1;
            if (cardsWeight < 0) cardsWeight = 0;


            return Math.Round(cardsWeight, 2);
        }
        public double UpdateCardsWeight(Card cardFlop)
        {
            //cardsWeight -= 0.15;

            if (cardFlop.strength > bestCardStrength) bestCardStrength = cardFlop.strength;

            if (cardCombination == CardCombination.OnePair && cardFlop.strength == cardLeft.strength)
            {
                cardsWeight += 0.4 * cardWeightModifier;
                cardCombination = CardCombination.ThreeOfAKind;
            }

            if (cardCombination == CardCombination.None && (cardLeft.strength == cardFlop.strength ||
                                                            cardRight.strength == cardFlop.strength))
            {
                cardsWeight += 0.25 * cardWeightModifier;
                cardCombination = CardCombination.OnePair;
            }

            if (isOneSuit(new Card[] { cardLeft, cardRight, cardFlop }))
            {
                lookingFor = CardCombination.Flush;
                cardsWeight += 0.15 * cardWeightModifier;
            }

            if (areInRow(new Card[] { cardLeft, cardRight, cardFlop }))
            {
                lookingFor = CardCombination.Straight;
                cardsWeight += 0.18 * cardWeightModifier;
            }

            cardsWeight += (cardFlop.strength) * 0.01 * cardWeightModifier;

            cardsWeight += random.NextDouble() * cardWeightRandomnessModifier - cardWeightRandomnessModifier / 2;

            if (cardsWeight > 1) cardsWeight = 1;
            if (cardsWeight < 0) cardsWeight = 0;


            return Math.Round(cardsWeight, 2);
        }
        public double UpdateCardsWeight(Card cardFlop, Card cardTern)
        {
            //cardsWeight -= 0.15;

            if (cardTern.strength > bestCardStrength) bestCardStrength = cardTern.strength;

            if (cardCombination == CardCombination.ThreeOfAKind && cardFlop.strength == cardTern.strength)
            {
                cardCombination = CardCombination.FourOfAKind;
                cardsWeight += 0.65 * cardWeightModifier;
            }

            if (cardCombination == CardCombination.OnePair && cardFlop.strength == cardTern.strength)
            {
                cardsWeight += 0.45 * cardWeightModifier;
                cardCombination = CardCombination.ThreeOfAKind;
            }

            if (cardCombination == CardCombination.None)
            {
                if (cardLeft.strength == cardTern.strength || cardRight.strength == cardTern.strength)
                {
                    cardsWeight += 0.3 * cardWeightModifier;
                    cardCombination = CardCombination.OnePair;
                }
            }

            if (isOneSuit(new Card[] { cardLeft, cardRight, cardFlop, cardTern }))
            {
                cardsWeight += 0.35 * cardWeightModifier;
            }
            else if (lookingFor == CardCombination.Flush) cardsWeight -= 0.2 * cardWeightModifier;

            if (areInRow(new Card[] { cardLeft, cardRight, cardFlop, cardTern }))
            {
                cardsWeight += 0.35 * cardWeightModifier;
            }
            else if (lookingFor == CardCombination.Straight) cardsWeight -= 0.25 * cardWeightModifier;

            cardsWeight += (cardTern.strength) * 0.005 * cardWeightModifier;

            cardsWeight += random.NextDouble() * cardWeightRandomnessModifier - cardWeightRandomnessModifier / 2;

            if (cardsWeight > 1) cardsWeight = 1;
            if (cardsWeight < 0) cardsWeight = 0;


            return Math.Round(cardsWeight, 2);
        }
        public double UpdateCardsWeight(Card cardFlop, Card cardTern, Card cardRiver)
        {
            //cardsWeight -= 0.1;

            if (cardRiver.strength > bestCardStrength) bestCardStrength = cardRiver.strength;

            cardCombination = analyzeCards(cardFlop, cardTern, cardRiver);

            if (cardCombination == CardCombination.OnePair && cardsWeight < 0.2) cardsWeight += 0.11 * cardWeightModifier;
            if (cardCombination == CardCombination.TwoPair && cardsWeight < 0.2) cardsWeight += 0.18 * cardWeightModifier;
            if (cardCombination == CardCombination.ThreeOfAKind && cardsWeight < 0.2) cardsWeight += 0.22 * cardWeightModifier;

            if (cardCombination == CardCombination.Straight) cardsWeight += 0.1 * cardWeightModifier;
            else if (lookingFor == CardCombination.Straight) cardsWeight -= 0.3 * cardWeightModifier;
            if (cardCombination == CardCombination.Flush) cardsWeight += 0.13 * cardWeightModifier;
            else if (lookingFor == CardCombination.Flush) cardsWeight -= 0.3 * cardWeightModifier;
            if (cardCombination == CardCombination.FullHouse) cardsWeight += 0.67 * cardWeightModifier;
            if (cardCombination == CardCombination.StraightFlush) cardsWeight += 0.9 * cardWeightModifier;
            if (cardCombination == CardCombination.StraightFlush) cardsWeight += 1 * cardWeightModifier;

            cardsWeight += random.NextDouble() * cardWeightRandomnessModifier - cardWeightRandomnessModifier / 2;

            if (cardsWeight > 1) cardsWeight = 1;
            if (cardsWeight < 0) cardsWeight = 0;


            return Math.Round(cardsWeight, 2);
        }
        public CardCombination analyzeCards(Card TableCardFlop, Card TableCardTern, Card TableCardRiver)
        {
            Card[] cards = new Card[5];

            cards[0] = cardLeft;
            cards[1] = cardRight;
            cards[2] = TableCardFlop;
            cards[3] = TableCardTern;
            cards[4] = TableCardRiver;

            if (cards.Length != 5) throw new ArgumentException();

            bool flagOneSuit = true;
            bool flagIsFourOfAKind = false;
            bool flagIsThreeOfAKind = false;
            bool flagIsPair = false;
            bool flagIsTwoPair = false;

            Array.Sort(cards);

            //Is one suit
            for (int i = 1; i < 5; i++)
            {
                if (cards[0].suit != cards[i].suit)
                {
                    flagOneSuit = false;
                    break;
                }
            }

            if (flagOneSuit == true)
            {
                //RoyalF
                if (cards[0].strength == 10 && cards[4].strength == 14) return CardCombination.RoyalFlush;

                //StraightF
                if (cards[4].strength - cards[0].strength == 4) return CardCombination.StraightFlush;

                return CardCombination.Flush;
            }

            //Count amount of duplicates
            Dictionary<int, int> dupCounts = new Dictionary<int, int>();

            for (int i = 0; i < 5; i++)
            {
                if (dupCounts.ContainsKey(cards[i].strength) == false)
                {
                    dupCounts.Add(cards[i].strength, 1);
                }
                else dupCounts[cards[i].strength]++;
            }

            foreach (var value in dupCounts)
            {
                if (value.Value == 4)
                {
                    flagIsFourOfAKind = true;
                    break;
                }
                if (value.Value == 3) flagIsThreeOfAKind = true;
                if (flagIsPair == true && value.Value == 2)
                {
                    flagIsTwoPair = true;
                    break;
                }
                if (value.Value == 2) flagIsPair = true;
            }

            if (flagIsFourOfAKind == true) return CardCombination.FourOfAKind;

            if (flagIsThreeOfAKind == true && flagIsPair == true) return CardCombination.FullHouse;

            if (cards[0].strength - cards[1].strength == -1 &&
                cards[1].strength - cards[2].strength == -1 &&
                cards[2].strength - cards[3].strength == -1 &&
                cards[3].strength - cards[4].strength == -1) return CardCombination.Straight;

            if (flagIsThreeOfAKind == true) return CardCombination.ThreeOfAKind;

            if (flagIsTwoPair == true) return CardCombination.TwoPair;

            if (flagIsPair == true) return CardCombination.OnePair;

            return CardCombination.None;
        }
        public PlayerAction getPlayerAction(int tableMaxBet, bool wasBetmade)
        {
            if (wasBetmade == false)
            {
                if (cardsWeight > 0.4)
                {
                    return PlayerAction.Bet;
                }
                else return PlayerAction.Check;
            }
            else
            {
                if (tableMaxBet > maxBetInStage + random.Next(0, maxBetInStage)) return PlayerAction.Fold;
                else
                {
                    if (tableMaxBet < maxBetInStage * 0.9) return PlayerAction.Raise;
                    else return PlayerAction.Call;
                }
            }
        }

        
        //Game Actions
        public void Fold(CardPack cardPack)
        {
            cardPack.putBottom(cardLeft);
            cardPack.putBottom(cardRight);
            cardLeftImg.Visibility = System.Windows.Visibility.Hidden;
            cardRightImg.Visibility = System.Windows.Visibility.Hidden;
        }
        public int Blind(int blindValue)
        {
            Wealth -= blindValue;
            updateWealth();
            alredyBetInCircle += blindValue;
            updateBet();
            ShowSmallChips();
            return blindValue;
        }
        public int Bet()
        {
            Wealth -= maxBetInStage;
            updateWealth();
            alreadyBetInStage += maxBetInStage;
            alredyBetInCircle += maxBetInStage;
            ShowSmallChips();
            updateBet();
            return maxBetInStage;
        }
        public int Call(int tableMaxBet, int playerBet)
        {
            int betValue = tableMaxBet - playerBet;
            Wealth -= betValue;
            updateWealth();
            alreadyBetInStage += betValue;
            alredyBetInCircle += betValue;
            ShowSmallChips();
            updateBet();
            return betValue;
        }
        public int Raise(ref int tableMaxBet, int playerBet)
        {
            int betValue = maxBetInStage;
            Wealth -= betValue;
            updateWealth();
            alreadyBetInStage += betValue;
            alredyBetInCircle += betValue;
            ShowSmallChips();
            updateBet();
            tableMaxBet = maxBetInStage + playerBet;
            return betValue;
        }

        //Reset
        public void gameover()
        {
            isPlaying = false;
            currentAction = PlayerAction.Fold;
            playerAv.Source = new BitmapImage(new Uri("/Resources/playerKilled.png", UriKind.Relative));
        }
        public void resetForStage()
        {
            currentAction = PlayerAction.None;
            maxBetInStage = 0;
            alreadyBetInStage = 0;
        }
        public void resetForCircle(CardPack cardpack)
        {
            Fold(cardpack);
            cardsWeight = 0;
            smallChipsImg.Visibility = System.Windows.Visibility.Hidden;
            tableBetLabel.Visibility = System.Windows.Visibility.Hidden;
            cardCombination = CardCombination.None;
            lookingFor = CardCombination.None;
            currentAction = PlayerAction.None;
            maxBetInStage = 0;
            alreadyBetInStage = 0;
            alredyBetInCircle = 0;
        }

        //Logic Checks help func
        public bool areInRow(params Card[] cards)
        {
            bool flag = true;

            Array.Sort(cards);

            for (int i = 0; i < cards.Length - 1; i++)
            {
                if (cards[i].strength - cards[i+1].strength != -1)
                {
                    flag= false;
                    break;
                }
            }

            return flag;
        }
        public bool isOneSuit(params Card[] cards)
        {
            bool flag = true;

            Array.Sort(cards);

            for (int i = 0; i < cards.Length - 1; i++)
            {
                if (cards[i].suit != cards[i + 1].suit)
                {
                    flag = false;
                    break;
                }
            }

            return flag;
        }

        //Graphic
        public void Card_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (cardsOpenAtStart == false)
            {
                cardsOpenAtStart = true;
                cardLeftImg.Source = cardLeft.img;
                cardRightImg.Source = cardRight.img;
            }
            else
            {
                cardsOpenAtStart = false;
                cardLeftImg.Source = new BitmapImage(new Uri("/Resources/backside.png", UriKind.Relative));
                cardRightImg.Source = new BitmapImage(new Uri("/Resources/backside.png", UriKind.Relative));
            }
        }
        private void ShowSmallChips()
        {
            if (smallChipsImg.IsVisible == false)
                smallChipsImg.Visibility = System.Windows.Visibility.Visible;

            if (tableBetLabel.IsVisible == false)
                tableBetLabel.Visibility = System.Windows.Visibility.Visible;
        }
        public void Card_MouseEnter(object sender, MouseEventArgs e)
        {

        }
        public void Card_MouseLeave(object sender, MouseEventArgs e)
        {

        }
    }
}
