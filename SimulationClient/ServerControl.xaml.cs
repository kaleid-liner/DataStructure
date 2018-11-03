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
    /// ServerControl.xaml 的交互逻辑
    /// </summary>
    public partial class ServerControl : UserControl
    {
        public ServerControl()
        {
            InitializeComponent();
        }

        private void CpuButton_Click(object sender, RoutedEventArgs e)
        {
            InputDialog cpuDialog = new InputDialog("你想升级多少CPU？（请输入一个整数/浮点数。）");
            cpuDialog.ShowDialog();
            if (cpuDialog.DialogResult == true)
            {
                if (Double.TryParse(cpuDialog.Result, out double result))
                    (DataContext as Server).Upgrade(0, result);
                else MessageBox.Show("请输入合法的值！");
            }
        }

        private void MemoryButton_Click(object sender, RoutedEventArgs e)
        {
            InputDialog memoryDialog = new InputDialog("你想升级多少内存？（请输入一个整数/浮点数，单位：GB）");
            memoryDialog.ShowDialog();
            if (memoryDialog.DialogResult == true)
            {
                if (Double.TryParse(memoryDialog.Result, out double result))
                    (DataContext as Server).Upgrade(result, 0);
                else MessageBox.Show("请输入合法的值！");
            }
        }
    }
}
