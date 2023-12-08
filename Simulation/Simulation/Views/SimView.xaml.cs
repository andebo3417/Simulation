using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Simulation.Views
{
    /// <summary>
    /// Логика взаимодействия для SimView.xaml
    /// </summary>
    public partial class SimView : UserControl
    {
        private Player[] players;
        private Table table;
        private System.Windows.Controls.Image[] PlayerAvs;
        private System.Windows.Controls.Image[] Chips;
        private System.Windows.Controls.Image[] SmallChips;
        private System.Windows.Controls.Image[] CardsLeft;
        private System.Windows.Controls.Image[] CardsRight;
        private System.Windows.Controls.Label[] Wealths;
        private System.Windows.Controls.Label[] TableBets;
        public System.Windows.Controls.Label[] CardWeights;
        System.Windows.Threading.DispatcherTimer timer;

        public static int playerAmount = 8;
        public static int startingWealth = 10000;

        public bool onlyStartBool;

        private static Point[] playerCoords = new Point[8] {
            new Point(1, 4),
            new Point(1, 6),
            new Point(4, 7),
            new Point(7, 6),
            new Point(7, 4),
            new Point(7, 2),
            new Point(4, 1),
            new Point(1, 2),
        };

        private static Point[] SmallChipsCoords = new Point[8] {
            new Point(3, 4),
            new Point(3, 5),
            new Point(4, 6),
            new Point(6, 5),
            new Point(6, 4),
            new Point(6, 3),
            new Point(4, 2),
            new Point(3, 3),
        };
        public SimView()
        {
            CardWeights = new Label[playerAmount];
            PlayerAvs = new System.Windows.Controls.Image[playerAmount];
            Chips = new System.Windows.Controls.Image[playerAmount];
            CardsLeft = new System.Windows.Controls.Image[playerAmount];
            CardsRight = new System.Windows.Controls.Image[playerAmount];
            Wealths = new System.Windows.Controls.Label[playerAmount];
            TableBets = new System.Windows.Controls.Label[playerAmount];
            SmallChips = new System.Windows.Controls.Image[playerAmount];

            table = new Table(this);
            onlyStartBool = false;

            InitializeComponent();
            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Tick += new EventHandler(timer_tick);
            timer.Interval = new TimeSpan(0, 0, 2);
            createPlayers();
        }

        private void createPlayers()
        {
            players = new Player[playerAmount];
            for (int i = 0; i < playerAmount; i++)
            {
                //State labels
                CardWeights[i] = new Label();
                SimGrid.Children.Add(CardWeights[i]);
                Grid.SetRow(CardWeights[i], (int)playerCoords[i].X - 1);
                Grid.SetColumn(CardWeights[i], (int)playerCoords[i].Y);
                //Grid.SetColumnSpan(CardWeights[i], 2);
                CardWeights[i].Width = Double.NaN;
                CardWeights[i].Height = Double.NaN;
                CardWeights[i].HorizontalAlignment = HorizontalAlignment.Right;
                CardWeights[i].VerticalAlignment = VerticalAlignment.Bottom;
                CardWeights[i].Background = Brushes.Transparent;
                CardWeights[i].BorderBrush = null;
                CardWeights[i].FontWeight = FontWeights.Bold;
                //

                //PlayerAvs
                PlayerAvs[i] = new System.Windows.Controls.Image();
                SimGrid.Children.Add(PlayerAvs[i]);
                Grid.SetRow(PlayerAvs[i], (int)playerCoords[i].X);
                Grid.SetColumn(PlayerAvs[i], (int)playerCoords[i].Y);
                PlayerAvs[i].Source = new BitmapImage(new Uri("/Resources/player.png", UriKind.Relative));
                PlayerAvs[i].Width = Double.NaN;
                PlayerAvs[i].Height = Double.NaN;
                PlayerAvs[i].HorizontalAlignment = HorizontalAlignment.Right;
                PlayerAvs[i].VerticalAlignment = VerticalAlignment.Top;

                //Chips
                Chips[i] = new System.Windows.Controls.Image();
                SimGrid.Children.Add(Chips[i]);
                Grid.SetRow(Chips[i], (int)playerCoords[i].X);
                Grid.SetColumn(Chips[i], (int)playerCoords[i].Y);
                Chips[i].Source = new BitmapImage(new Uri("/Resources/chips.png", UriKind.Relative));
                Chips[i].Width = Double.NaN;
                Chips[i].Height = Double.NaN;
                Chips[i].HorizontalAlignment = HorizontalAlignment.Left;
                Chips[i].VerticalAlignment = VerticalAlignment.Top;

                //CardLeft
                CardsLeft[i] = new System.Windows.Controls.Image();
                SimGrid.Children.Add(CardsLeft[i]);
                Grid.SetRow(CardsLeft[i], (int)playerCoords[i].X + 1);
                Grid.SetColumn(CardsLeft[i], (int)playerCoords[i].Y);
                CardsLeft[i].Source = new BitmapImage(new Uri("/Resources/backside.png", UriKind.Relative));
                CardsLeft[i].HorizontalAlignment = HorizontalAlignment.Left;
                //rotateTransform= new RotateTransform(4);
                //CardsLeft[i].RenderTransform= rotateTransform;
                CardsLeft[i].Width = Double.NaN;
                CardsLeft[i].Height = Double.NaN;
                CardsLeft[i].Visibility= Visibility.Hidden;
                CardsLeft[i].IsEnabled = false;

                //CardRight
                CardsRight[i] = new System.Windows.Controls.Image();
                SimGrid.Children.Add(CardsRight[i]);
                Grid.SetRow(CardsRight[i], (int)playerCoords[i].X + 1);
                Grid.SetColumn(CardsRight[i], (int)playerCoords[i].Y);
                CardsRight[i].Source = new BitmapImage(new Uri("/Resources/backside.png", UriKind.Relative));
                CardsRight[i].HorizontalAlignment = HorizontalAlignment.Right;
                //rotateTransform = new RotateTransform(-10);
                //CardsRight[i].RenderTransform = rotateTransform;
                CardsRight[i].Width = Double.NaN;
                CardsRight[i].Height = Double.NaN;
                CardsRight[i].Visibility = Visibility.Hidden;
                CardsRight[i].IsEnabled = false;

                //Wealths
                Wealths[i] = new System.Windows.Controls.Label();
                SimGrid.Children.Add(Wealths[i]);
                Grid.SetRow(Wealths[i], (int)playerCoords[i].X);
                Grid.SetColumn(Wealths[i], (int)playerCoords[i].Y);
                Wealths[i].Width = Double.NaN;
                Wealths[i].Height = Double.NaN;
                Wealths[i].HorizontalAlignment = HorizontalAlignment.Left;
                Wealths[i].VerticalAlignment = VerticalAlignment.Bottom;
                Wealths[i].Background = Brushes.Transparent;
                Wealths[i].BorderBrush = null;
                Wealths[i].FontWeight = FontWeights.Bold;

                //ChipsIMg
                SmallChips[i] = new System.Windows.Controls.Image();
                SimGrid.Children.Add(SmallChips[i]);
                Grid.SetRow(SmallChips[i], (int)SmallChipsCoords[i].X);
                Grid.SetColumn(SmallChips[i], (int)SmallChipsCoords[i].Y);
                SmallChips[i].Source = new BitmapImage(new Uri("/Resources/chips.png", UriKind.Relative));
                SmallChips[i].Width = Double.NaN;
                SmallChips[i].Height = Double.NaN;
                SmallChips[i].HorizontalAlignment = HorizontalAlignment.Center;
                SmallChips[i].VerticalAlignment = VerticalAlignment.Center;
                if ((int)SmallChipsCoords[i].X == 4) Grid.SetRowSpan(SmallChips[i], 2);
                SmallChips[i].Visibility = Visibility.Hidden;

                //BetLabel
                TableBets[i] = new Label();
                SimGrid.Children.Add(TableBets[i]);
                Grid.SetRow(TableBets[i], (int)SmallChipsCoords[i].X);
                Grid.SetColumn(TableBets[i], (int)SmallChipsCoords[i].Y);
                TableBets[i].Width = Double.NaN;
                TableBets[i].Height = Double.NaN;
                TableBets[i].HorizontalAlignment = HorizontalAlignment.Left;
                TableBets[i].VerticalAlignment = VerticalAlignment.Center;
                if ((int)SmallChipsCoords[i].X == 4) Grid.SetRowSpan(TableBets[i], 2);
                TableBets[i].Background = Brushes.Transparent;
                TableBets[i].BorderBrush = null;
                TableBets[i].FontWeight = FontWeights.Bold;
                TableBets[i].Content = "0K";
                TableBets[i].Visibility = Visibility.Hidden;

                players[i] = new Player(startingWealth, PlayerAvs[i], CardsLeft[i], CardsRight[i], Wealths[i], SmallChips[i], TableBets[i]);
                CardsLeft[i].MouseLeftButtonDown += players[i].Card_MouseLeftButtonDown;
                CardsRight[i].MouseLeftButtonDown += players[i].Card_MouseLeftButtonDown;

                CardsLeft[i].MouseEnter += players[i].Card_MouseEnter;
                CardsRight[i].MouseLeave += players[i].Card_MouseLeave;
                //ResizeMargins();
            }
        }

        private void ResizeMargins()
        {
            int cellWidth = (int)ColumnDef1.ActualWidth;
            int cellHeight = (int)RowDef1.ActualHeight;
            double fontValue = 14 * (cellHeight + cellWidth) * 0.5 * 0.015;
            double halfCell = cellWidth * 0.5;


            for (int i = 0; i < playerAmount; i++)
            {
                PlayerAvs[i].Margin = new Thickness(halfCell * 1.1, 0, 0, 0);
                Chips[i].Margin = new Thickness(0, 0, halfCell, cellHeight * 0.5);
                Wealths[i].Margin = new Thickness(0, 0, halfCell * 0.9, 0);
                Wealths[i].FontSize = fontValue * 0.8;
                CardWeights[i].FontSize = fontValue;
                Cardpack_label.FontSize = fontValue * 2.0;
                CardsLeft[i].Margin = new Thickness(0, cellHeight * 0.1, halfCell, 0);
                CardsRight[i].Margin = new Thickness(halfCell, cellHeight * 0.1, 0, 0);
                StartButton.Height = cellHeight * 0.75;
                StartButton.Width = cellWidth * 0.75;
                NextStageButton.Height = cellHeight * 0.75;
                NextStageButton.Width = cellWidth * 0.75;
                NextActionButton.Height = cellHeight * 0.75;
                NextActionButton.Width = cellWidth * 0.75;
                SetTimerButton.Height = cellHeight * 0.75;
                SetTimerButton.Width = cellWidth * 0.75;
                KillTimerButton.Height = cellHeight * 0.75;
                KillTimerButton.Width = cellWidth * 0.75;
                if (i == 2)
                {
                    SmallChips[i].Margin = new Thickness(halfCell * 0.4, 0, halfCell * 1.1, 0);
                    TableBets[i].Margin = new Thickness(halfCell * 0.9, 0, 0, 0);
                }
                else
                {
                    SmallChips[i].Margin = new Thickness(halfCell * 0.7, 0, halfCell * 0.8, 0);
                    TableBets[i].Margin = new Thickness(halfCell * 1.2, 0, 0, 0);
                }
                
                TableBets[i].FontSize = fontValue * 0.7;
            }

        }

        private void SimView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ResizeMargins();
        }

    

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            table = new Table(this);
            table.startGame(players, startingWealth, new CardPack(Cardpack_label));
            for (int i = 0; i < playerAmount; i++)
            {
                PlayerAvs[i].Source = new BitmapImage(new Uri("/Resources/player.png", UriKind.Relative));
                SmallChips[i].Visibility = Visibility.Hidden;
                TableBets[i].Visibility = Visibility.Hidden;
                players[i].isPlaying = true;
                CardWeights[i].Content = "";
                CardWeights[i].Foreground = System.Windows.Media.Brushes.Black;
            }
            onlyStartBool = false;
            StartButton.IsEnabled = false;
            SetTimerButton.IsEnabled = true;
            NextStageButton.IsEnabled = true;
        }

        private void NextStageButton_Click(object sender, RoutedEventArgs e)
        {
            if (onlyStartBool == false)
            { 
                table.stage++;
                table.nextStage();
                for (int i = 0; i < playerAmount; i++) CardWeights[i].Content = "";
                NextStageButton.IsEnabled = false;
                NextActionButton.IsEnabled = true;
            }
        }

        private void NextActionButton_Click(object sender, RoutedEventArgs e)
        {
            
            if (table.isTableBalanced() == true)
            {
                if (onlyStartBool == false)
                {
                    NextStageButton.IsEnabled = true;
                    NextActionButton.IsEnabled = false;
                }
                else onlyStart();
            }
            else table.nextPlayerAction();
        }

        private void timer_tick(object sender, EventArgs e)
        {
            if (table.stage == GameStage.None)
            {

                table.stage++;
                table.nextStage();
                return;
            }
            if (table.isTableBalanced() == true)
            {
                table.stage++;
                table.nextStage();
            }
            else table.nextPlayerAction();
        }

        public void resetForCircle()
        {
            StartButton.IsEnabled = false;
            NextStageButton.IsEnabled = true;
            NextActionButton.IsEnabled = false;
        }

        public void showFLopCard(Card card)
        {
            if (card != null)
            {
                cardTableFlop_img.Source = card.img;
                cardTableFlop_img.Visibility = Visibility.Visible;
            }
            else cardTableFlop_img.Visibility = Visibility.Hidden;
        }
        public void showTernCard(Card card)
        {
            if (card != null)
            {
                cardTableTern_img.Source = card.img;
                cardTableTern_img.Visibility = Visibility.Visible;
            }
            else cardTableTern_img.Visibility = Visibility.Hidden;
        }
        public void showRiverCard(Card card)
        {
            if (card != null)
            {
                cardTableRiver_img.Source = card.img;
                cardTableRiver_img.Visibility = Visibility.Visible;
            }
            else cardTableRiver_img.Visibility = Visibility.Hidden;
        }
        public void onlyStart()
        {
            onlyStartBool = true;
            StartButton.IsEnabled = true;
            NextStageButton.IsEnabled = false;
            NextActionButton.IsEnabled = false;
            SetTimerButton.IsEnabled = false;
            KillTimerButton.IsEnabled = false;
        }
        public void activateCards(int playerIndex)
        {
            CardsLeft[playerIndex].Visibility = Visibility.Visible;
            CardsLeft[playerIndex].IsEnabled = true;
            CardsRight[playerIndex].Visibility = Visibility.Visible;
            CardsRight[playerIndex].IsEnabled = true;
        }

        private void SetTimerButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Start();
            SetTimerButton.IsEnabled = false;
            KillTimerButton.IsEnabled = true;
            NextStageButton.IsEnabled = false;
            NextActionButton.IsEnabled = false;
        }

        private void KillTimerButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            SetTimerButton.IsEnabled = true;
            KillTimerButton.IsEnabled = false;
            if (table.stage == GameStage.None)
            {
                NextStageButton.IsEnabled = true;
                NextActionButton.IsEnabled = false;
            }
            else
            {
                NextStageButton.IsEnabled = false;
                NextActionButton.IsEnabled = true;
            }
        }
    }
}
