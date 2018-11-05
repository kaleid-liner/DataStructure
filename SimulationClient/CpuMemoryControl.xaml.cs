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
using DataStructure.ServerSimulation;

namespace SimulationClient
{
    /// <summary>
    /// CpuMemoryControl.xaml 的交互逻辑
    /// </summary>
    public partial class CpuMemoryControl : UserControl
    {
        public CpuMemoryControl()
        {
            InitializeComponent();
        }

        private void CpuDialogClosing(object sender, MaterialDesignThemes.Wpf.DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == true)
            {
                if (Double.TryParse(cpuBox.Text, out double cpuInc))
                {
                    var server = DataContext as Server;
                    server.Upgrade(0, cpuInc);
                }
            }
        }

        private void MemoryDialogClosing(object sender, MaterialDesignThemes.Wpf.DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == true)
            {
                if (Double.TryParse(memoryBox.Text, out double memoryInc))
                {
                    var server = DataContext as Server;
                    server.Upgrade(memoryInc, 0);
                }
            }
        }
    }
}
