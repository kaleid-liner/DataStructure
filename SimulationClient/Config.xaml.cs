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
using System.Windows.Shapes;

namespace SimulationClient
{
    /// <summary>
    /// Config.xaml 的交互逻辑
    /// </summary>
    public partial class Config : Window
    {
        public Config(int interval)
        {
            InitializeComponent();
            DataContext = this;
            RefreshTime = interval;
            switch (RefreshTime)
            {
                case 50: radio50.IsChecked = true; break;
                case 100: radio100.IsChecked = true; break;
                case 500: radio500.IsChecked = true; break;
                case 1000: radio1000.IsChecked = true; break;
                default: radio100.IsChecked = true; break;
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            if (radio50.IsChecked == true)
                RefreshTime = 50;
            else if (radio100.IsChecked == true)
                RefreshTime = 100;
            else if (radio500.IsChecked == true)
                RefreshTime = 500;
            else if (radio1000.IsChecked == true)
                RefreshTime = 1000;
            else RefreshTime = 100;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
                DialogResult = false;
                Close();
        }

        public double MinCpu { get; set; }

        public double MaxCpu { get; set; }

        public double MinMemory { get; set; }

        public double MaxMemory { get; set; }

        public int MinTime { get; set; }

        public int MaxTime { get; set; }

        public int Frequency { get; set; }

        public int RefreshTime { get; set; }

        public double Possibility { get; set; }
    }
}
