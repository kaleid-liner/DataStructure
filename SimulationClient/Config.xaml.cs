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
        public Config()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MinCpu = Double.Parse(minCpuBox.Text);
                MaxCpu = Double.Parse(maxCpuBox.Text);
                MinMemory = Double.Parse(minMemoryBox.Text);
                MaxMemory = Double.Parse(maxMemoryBox.Text);
                MinTime = Int32.Parse(minTimeBox.Text);
                MaxTime = Int32.Parse(maxTimeBox.Text);
                Frequency = Int32.Parse(frequencyBox.Text);
                Possibility = Double.Parse(possibilityBox.Text);
                DialogResult = true;
                Close();
            }
            catch (System.FormatException)
            {
                MessageBox.Show("请输入合法格式的数字！");
            }
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

        private void RadioButton_Checked_50(object sender, RoutedEventArgs e)
        {
            RefreshTime = 50;
        }

        private void RadioButton_Checked_100(object sender, RoutedEventArgs e)
        {
            RefreshTime = 100;
        }

        private void RadioButton_Checked_500(object sender, RoutedEventArgs e)
        {
            RefreshTime = 500;
        }

        private void RadioButton_Checked_1000(object sender, RoutedEventArgs e)
        {
            RefreshTime = 1000;
        }
    }
}
