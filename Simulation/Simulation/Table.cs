using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum GameStage
{
    None,
    Preflop,
    Flop,
    Tern,
    River,
    Final
}

public enum CardCombination
{
    None,
    OnePair,
    TwoPair,
    ThreeOfAKind,
    Straight,
    Flush,
    FullHouse,
    FourOfAKind,
    StraightFlush,
    RoyalFlush
}

public enum PlayerAction
{
    None,
    Fold,
    Check,
    Bet,
    Call,
    Raise
}

namespace Simulation
{
    public class Table
    {
        Simulation.Views.SimView parentView;

        public Player[] players;
        private List<Player> winners;
        public int playerAmount;
        public CardPack cardPack;
        public Card cardTableFlop;
        public Card cardTableTern;
        public Card cardTableRiver;

        public int[] playerBets;
        public int tableWealth;
        public int tableMaxBet;
        public bool isBalanced;
        public bool isFirstBetCircle;
        public bool wasBetMade;
        public int currentPlayerIndex;
        public int playerMinWealth;

        public int Blind;
        public int BlindIndex;

        public GameStage stage;
        public int button;

        private int circle;

        public static int blindSize = 10;
        public static double blindInc = 1.25;

        public Table(Simulation.Views.SimView parentView)
        {
            this.parentView = parentView;
            circle = 0;
        }
        public void startGame(Player[] players, int startingWealth, CardPack cardPack)
        {
            parentView.showFLopCard(null);
            parentView.showTernCard(null);
            parentView.showRiverCard(null);
            this.cardPack = cardPack;
            this.players = players;
            playerAmount = players.Length;
            playerBets = new int[playerAmount];
            for (int i = 0; i < playerAmount; i++) { playerBets[i] = 0; }
            tableWealth = 0;
            tableMaxBet = 0;
            isBalanced = false;
            isFirstBetCircle = true;
            wasBetMade = false;
            Blind = startingWealth / blindSize;
            BlindIndex = 1;
            playerMinWealth = startingWealth - Blind;
            stage = GameStage.None;
            button = 0;
            circle = 0;
        }
        public GameStage nextStage()
        {
            if (stage == GameStage.Preflop)
            {
                tableWealth += players[BlindIndex].Blind(Blind);
                setCurrentIndex();
                cardPack.mixRandom();

                for (int i = 0; i < playerAmount; i++)
                {
                    if (players[i].isPlaying == true) 
                    {
                        Card tmpCard = cardPack.takeTop();
                        players[i].takeLeftCard(tmpCard);
                        tmpCard = cardPack.takeTop();
                        players[i].takeRightCard(tmpCard);
                        players[i].UpdateCardsWeight();
                        players[i].UpdateMaxBetInStage(playerMinWealth);
                        players[i].currentAction = PlayerAction.None;
                        parentView.activateCards(i);
                    }
                }

                wasBetMade = false;
                isFirstBetCircle = true;

                return stage - 1;
            }

            if (stage == GameStage.Flop)
            {
                setCurrentIndex();
                cardTableFlop = cardPack.takeTop();
                parentView.showFLopCard(cardTableFlop);

                for (int i = 0; i < playerAmount; i++)
                {
                    if (players[i].isPlaying == true)
                    {
                        players[i].UpdateCardsWeight(cardTableFlop);
                        players[i].UpdateMaxBetInStage(playerMinWealth);
                    }
                }

                wasBetMade = false;
                isFirstBetCircle = true;

                return stage - 1;
            }

            if (stage == GameStage.Tern)
            {
                setCurrentIndex();
                cardTableTern = cardPack.takeTop();
                parentView.showTernCard(cardTableTern);

                for (int i = 0; i < playerAmount; i++)
                {
                    if (players[i].isPlaying == true)
                    {
                        players[i].UpdateCardsWeight(cardTableFlop, cardTableTern);
                        players[i].UpdateMaxBetInStage(playerMinWealth);
                    }
                }

                wasBetMade = false;
                isFirstBetCircle = true;

                return stage - 1;
            }

            if (stage == GameStage.River)
            {
                setCurrentIndex();
                cardTableRiver = cardPack.takeTop();
                parentView.showRiverCard(cardTableRiver);

                for (int i = 0; i < playerAmount; i++)
                {
                    if (players[i].isPlaying == true)
                    {
                        players[i].UpdateCardsWeight(cardTableFlop, cardTableTern, cardTableRiver);
                        players[i].UpdateMaxBetInStage(playerMinWealth);
                        CardCombination combination = players[i].analyzeCards(cardTableFlop, cardTableTern, cardTableRiver);
                    }
                }

                wasBetMade = false;
                isFirstBetCircle = true;

                return GameStage.River;
            }

            if (stage == GameStage.Final)
            {
                decideWinners();
                ShareTableWealth();
                resetForCircle();

                return GameStage.Final;
            }

            return GameStage.Preflop;
        }
        public void nextPlayerAction()
        {
            int betValue = 0;

            while (players[currentPlayerIndex].currentAction == PlayerAction.Fold)
            {
                currentPlayerIndex++;
                if (currentPlayerIndex == BlindIndex + 1)
                {
                    isFirstBetCircle = false;
                }
                if (currentPlayerIndex == playerAmount) currentPlayerIndex = 0;
            }

            if (isFirstBetCircle == false)
            {
                if (isTableBalanced() == true)
                {
                    if (stage == GameStage.River)
                    {
                        stage = GameStage.Final;
                        decideWinners();
                        ShareTableWealth();
                        resetForCircle();
                    }
                }
            }

            //Check if only 1 player left
            if (checkForWinner() == true)
            {
                decideWinners();
                ShareTableWealth();
                resetForCircle();
            }

            Player player = players[currentPlayerIndex];
            player.currentAction = player.getPlayerAction(tableMaxBet, wasBetMade);
            parentView.CardWeights[currentPlayerIndex].Content = player.currentAction.ToString();
            //parentView.CardWeights[currentPlayerIndex].Content = player.cardsWeight.ToString();



            switch (player.currentAction)
            {
                case PlayerAction.Fold:
                    player.Fold(cardPack);
                    break;
                case PlayerAction.Check:
                    break;
                case PlayerAction.Bet:
                    wasBetMade = true;
                    betValue = player.Bet();
                    playerBets[currentPlayerIndex] += betValue;
                    updateMaxBet();
                    tableWealth += betValue;
                    break;
                case PlayerAction.Call:
                    betValue = player.Call(tableMaxBet, playerBets[currentPlayerIndex]);
                    tableWealth += betValue;
                    playerBets[currentPlayerIndex] += betValue;
                    break;
                case PlayerAction.Raise:
                    betValue = player.Raise(ref tableMaxBet, playerBets[currentPlayerIndex]);
                    playerBets[currentPlayerIndex] += betValue;
                    updateMaxBet();
                    tableWealth += betValue;
                    break;
            }

            if (player.Wealth < playerMinWealth) playerMinWealth = player.Wealth;

            currentPlayerIndex++;
            if (currentPlayerIndex == BlindIndex + 1)
            {
                isFirstBetCircle = false;
            }

            if (currentPlayerIndex == playerAmount) currentPlayerIndex = 0;

            if (isFirstBetCircle == false)
            {
                if (isTableBalanced() == true)
                {
                    if (stage == GameStage.River)
                    {
                        stage = GameStage.Final;
                        decideWinners();
                        ShareTableWealth();
                        resetForCircle();
                    }
                }
            }

            //Check if only 1 player left
            if (checkForWinner() == true)
            {
                decideWinners();
                ShareTableWealth();
                resetForCircle();
            }
        }
        public void updateMaxBet()
        {
            for (int i = 0; i < playerAmount; i++)
            {
                if (playerBets[i] > tableMaxBet) tableMaxBet = playerBets[i];
            }
        }
        public void decideWinners()
        {
            this.winners = new List<Player>();
            CardCombination maxCombination = CardCombination.None;
            int bestCardStrength = 2;

            for (int i = 0; i < playerAmount; i++) 
            {
                if (players[i].currentAction != PlayerAction.Fold)
                {
                    if (players[i].cardCombination >= maxCombination)
                    {
                        maxCombination = players[i].cardCombination;
                        if (players[i].bestCardStrength > bestCardStrength)
                            bestCardStrength = players[i].bestCardStrength;
                    }
                }
            }

            for (int i = 0; i < playerAmount; i++)
            {
                if (players[i].currentAction != PlayerAction.Fold)
                {
                    if (players[i].cardCombination == maxCombination && players[i].bestCardStrength == bestCardStrength)
                    {
                        winners.Add(players[i]);
                        parentView.CardWeights[i].Content = "Winner!";
                        parentView.CardWeights[i].Foreground = System.Windows.Media.Brushes.Orange;
                    } else parentView.CardWeights[i].Content = "";
                }
            }
        }
        public bool checkForWinner()
        {
            int counter = 0;

            for (int i = 0; i < playerAmount; i++)
            {
                if (players[i].currentAction != PlayerAction.Fold) counter++;
            }

            if (counter == 1) return true;
            return false;
        }
        public void ShareTableWealth()
        {
            foreach (Player player in this.winners)
            {
                player.takeChips(tableWealth / winners.Count);
            }
            tableWealth = 0;
        }
        public void resetForCircle()
        {
            for (int i = 0; i < playerAmount; i++)
            {
                players[i].resetForCircle(cardPack);
                parentView.CardWeights[i].Foreground = System.Windows.Media.Brushes.Black;
            }

            winners = null;
            cardPack.putBottom(cardTableRiver);
            cardPack.putBottom(cardTableTern);
            cardPack.putBottom(cardTableFlop);
            cardTableRiver = null;
            cardTableTern = null;
            cardTableFlop = null;
            parentView.showFLopCard(null);
            parentView.showTernCard(null);
            parentView.showRiverCard(null);
            tableMaxBet = 0;
            for (int i = 0; i < playerAmount; i++) { playerBets[i] = 0; }
            isBalanced = false;
            isFirstBetCircle = true;
            button++;
            setCurrentIndex();
            stage = GameStage.None;
            circle++;

            Blind = (int)(Blind * blindInc);
            bool flag = false;
            do
            {
                flag = false;

                if (players[BlindIndex].Wealth < Blind)
                {
                    players[BlindIndex].gameover();
                    if (countActivePlayers() == 1)
                    {
                        for (int i = 0; i < playerAmount; i++)
                        {
                            if (players[i].isPlaying == true)
                            {
                                parentView.CardWeights[i].Content = "Champion!";
                                parentView.CardWeights[i].Foreground = System.Windows.Media.Brushes.Orange;
                                parentView.onlyStart();
                            }
                            else parentView.CardWeights[i].Content = "";
                        }
                    }
                    else
                    {
                        button++;
                        setCurrentIndex();
                        flag = true;
                    }
                }
            } while (flag == true);
            if (countActivePlayers() != 1) parentView.resetForCircle();
        }
        
        public void setCurrentIndex()
        {
            if (button == playerAmount - 2)
            {
                BlindIndex = playerAmount - 1;
                currentPlayerIndex = 0;
            }
            else if (button == playerAmount)
            {
                button = 0;
                BlindIndex = 1;
                currentPlayerIndex = 2;
            }
            else if (button + 1 == playerAmount)
            {
                BlindIndex = 0;
                currentPlayerIndex = 1;
            }
            else
            {
                BlindIndex = button + 1;
                currentPlayerIndex = button + 2;
            }
        }
        public int countActivePlayers()
        {
            int count = 0;
            for (int i = 0; i < playerAmount; i++)
            {
                if (players[i].isPlaying == true)
                {
                    count++;
                }
            }
            return count;
        }
        public bool isTableBalanced()
        {
            isBalanced = true;
            if (isFirstBetCircle == true) return false;
            foreach (Player playerCheck in players)
            {
                if (playerCheck.currentAction != PlayerAction.Fold && playerCheck.alreadyBetInStage != tableMaxBet)
                {
                    isBalanced = false;
                    break;
                }
            }
            return isBalanced;
        }
    }
}
