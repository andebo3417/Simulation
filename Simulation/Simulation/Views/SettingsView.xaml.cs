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
    /// Логика взаимодействия для SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl
    {
        public SettingsView()
        {
            InitializeComponent();
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ResizeMargins();
        }

        private void ResizeMargins()
        {
            int cellWidth = (int)ColumnDef1.ActualWidth;
            int cellHeight = (int)RowDef1.ActualHeight;
            double fontValue = 14 * (cellHeight + cellWidth) * 0.5 * 0.018;
            double halfCell = cellWidth * 0.5;

            LabelPlayers.FontSize = fontValue;
            LabelAuto.FontSize = fontValue;
            LabelRandom.FontSize = fontValue;
            LabelModifier.FontSize = fontValue;
            LabelBlindSize.FontSize = fontValue;
            LabelBlindInc.FontSize = fontValue;
            LabelStartWealth.FontSize = fontValue;

            PlayersValue.FontSize = fontValue;
            AutoValue.FontSize = fontValue;
            RandomValue.FontSize = fontValue;
            ModifierValue.FontSize = fontValue;
            BlindSizeValue.FontSize = fontValue;
            BlindIncValue.FontSize = fontValue;
            StartWealthValue.FontSize = fontValue;

            PlayersValue.Margin = new Thickness(halfCell* 0.2, 0, 0, 0);
            AutoValue.Margin = new Thickness(halfCell * 0.2, 0, 0, 0); ;
            RandomValue.Margin = new Thickness(halfCell * 0.2, 0, 0, 0); ;
            ModifierValue.Margin = new Thickness(halfCell * 0.2, 0, 0, 0);
            BlindSizeValue.Margin = new Thickness(halfCell * 0.2, 0, 0, 0);
            BlindIncValue.Margin = new Thickness(halfCell * 0.2, 0, 0, 0);
            StartWealthValue.Margin = new Thickness(halfCell * 0.2, 0, 0, 0);

            //SaveButton.FontSize = fontValue;

            SliderPlayers.Width= halfCell * 3;
            SliderAuto.Width = halfCell * 3;
            SliderRandom.Width = halfCell * 3;
            SliderModifier.Width = halfCell * 3;
            SliderBlindSize.Width = halfCell * 3;
            SliderBlindInc.Width = halfCell * 3;
            SliderStartWealth.Width = halfCell * 3;
        }

        private void SliderPlayers_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            PlayersValue.Content = SliderPlayers.Value.ToString();
        }
        private void SliderAuto_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            AutoValue.Content = SliderAuto.Value.ToString();
        }
        private void SliderRandom_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            RandomValue.Content = SliderRandom.Value.ToString();
        }
        private void SliderModifier_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ModifierValue.Content = SliderModifier.Value.ToString();
        }
        private void SliderBlindSize_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            BlindSizeValue.Content = SliderBlindSize.Value.ToString();
        }
        private void SliderBlindInc_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            BlindIncValue.Content = SliderBlindInc.Value.ToString();
        }
        private void SliderStartWealth_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            StartWealthValue.Content = SliderStartWealth.Value.ToString();
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SimView.playerAmount = (int)SliderPlayers.Value;
            SimView.startingWealth = (int)SliderStartWealth.Value * 1000;
            Player.cardWeightRandomnessModifier = SliderRandom.Value;
            Player.cardWeightModifier = SliderModifier.Value;
            Table.blindSize = (int)SliderBlindSize.Value;
            Table.blindInc = SliderBlindInc.Value;
        }
    }
}
