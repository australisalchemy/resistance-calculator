using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ResistorCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private dynamic CalculateResistance(double supplyVoltage, double loadVoltage, double loadCurrent, int LEDCount = 1)
        {
            //return (supplyVoltage - (supplyVoltage - loadVoltage)) / (loadCurrent / 1000) * LEDCount;
            // parallel = same voltage, different current
            // series = voltage drop, same current
            if (btnSeries.IsChecked == true)
            {
                return (supplyVoltage - loadVoltage) / (loadCurrent / 1000) * LEDCount;
            }
            else
            {
                double resistance = 0;
                for(int i = 0; i < LEDCount; i++)
                {
                    resistance += 1 / (supplyVoltage / (loadCurrent / 1000));
                }

                return 1 / resistance;
            }
        }

        private dynamic CalculateVoltageDrop()
        {
            return (double.Parse(this.txtSupplyVoltage.Text) - (double.Parse(this.txtSupplyVoltage.Text) - double.Parse(this.txtLoadVoltage.Text))) * double.Parse(this.txtLEDs.Text);
        }

        private void btnCalculate_Click(object sender, RoutedEventArgs e)
        {
                // resistance is usually a double, but can be an int depending on input
              dynamic resistance = CalculateResistance(double.Parse(txtSupplyVoltage.Text), double.Parse(txtLoadVoltage.Text), double.Parse(txtLoadCurrent.Text), int.Parse(txtLEDs.Text));

            dynamic voltageDrop = CalculateVoltageDrop();

            // colors for the ohm output
            if (resistance < 1000)
            {
                lblResistance.Content = string.Format("{0} Ω", Math.Round(resistance, 1));
                lblResistance.Foreground = Brushes.Black;
            }
            else if (resistance >= 1000 && resistance <= 10000) // hundred/kilo
            {
                lblResistance.Content = string.Format("{0}k Ω", Math.Round(resistance / 1000, 1));
                lblResistance.Foreground = Brushes.Blue;
            }
            else if (resistance > 10000 && resistance < 1000000) // kilo
            {
                lblResistance.Content = string.Format("{0}k Ω", Math.Round(resistance / 10000, 1));
                lblResistance.Foreground = Brushes.Purple;
            }
            else if (resistance >= 1000000) // mega
            {
                lblResistance.Content = string.Format("{0} MΩ", Math.Round(resistance / 100000, 1));
                lblResistance.Foreground = Brushes.Orange;
            }

            // power rating
            double current = double.Parse(txtLoadCurrent.Text);
            double powerRating = Math.Pow(current / 1000, 2) * resistance;

            if (powerRating < 0.5)
            {
                lblPowerRating.Content = "1/4 W Resistor";
            }
            else if (powerRating > 0.5)
            {
                lblPowerRating.Content = "1/2 W Resistor";
            }
            else if (powerRating >= 1.0)
            {
                lblPowerRating.Content = "1 W Resistor";
            }

            // power rating label
            if (powerRating > 1000)
            {
                lblPower.Content = Math.Round(powerRating / 1000, 3) + " W";
            }
            else
            {
                lblPower.Content = Math.Round(powerRating, 3) + " mW";
            }

            // voltage drop colours
            if (voltageDrop > 50 && voltageDrop <= 120)
            {
                lblVoltageDrop.Foreground = Brushes.Navy;
            }
            else if (voltageDrop > 120 && voltageDrop < 1500)
            {
                lblVoltageDrop.Foreground = Brushes.Orange;
            }
            else if (voltageDrop >= 1500)
            {
                lblVoltageDrop.Foreground = Brushes.SteelBlue;
            }
            else if (voltageDrop > double.Parse(txtSupplyVoltage.Text))
            {
                lblVoltageDrop.Foreground = Brushes.Red;
            }

            lblVoltageDrop.Content = string.Format("Vd = {0}V", voltageDrop);

        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtLoadCurrent.Text = "0";
            txtLoadVoltage.Text = "0";
            txtSupplyVoltage.Text = "0";
            lblPower.Content = "";
            lblResistance.Content = "";
            lblPowerRating.Content = "";
            txtLEDs.Text = "1";
            lblVoltageDrop.Content = "";
        }
    }
}
