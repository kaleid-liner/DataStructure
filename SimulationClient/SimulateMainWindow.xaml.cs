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
using DataStructure.ServerSimulation;

namespace SimulationClient
{
    /// <summary>
    /// SimulateMainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SimulateMainWindow : Window
    {
        public SimulateMainWindow()
        {
            InitializeComponent();
            DataContext = new SimulatorViewModel();
        }

        private void StartExecute(object sender, ExecutedRoutedEventArgs e)
        {
            (DataContext as SimulatorViewModel).StartSimulation();
        }

        private void StopExecute(object sender, ExecutedRoutedEventArgs e)
        {
            (DataContext as SimulatorViewModel).StopSimulation();
        }

        private void ResetExecute(object sender, ExecutedRoutedEventArgs e)
        {
            (DataContext as SimulatorViewModel).ResetSimulation();
        }

        private void AboutExecute(object sender, ExecutedRoutedEventArgs e)
        {
            About about = new About();
            about.Show();
        }

        private void Document_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/kaleid-liner/DataStructure");
        }
    }
}
