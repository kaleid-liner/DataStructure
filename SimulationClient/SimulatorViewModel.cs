using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStructure.ServerSimulation;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Threading;
using System.Windows;
using System.Windows.Input;

namespace SimulationClient
{
    public class SimulatorViewModel : INotifyPropertyChanged
    {
        #region field
        private TaskGenerator generator;
        private LoadBalancer balancer;
        private DispatcherTimer timer;
        private Config config = new Config()
        {
            Possibility = 0.5
        };

        public event PropertyChangedEventHandler PropertyChanged;

        public static RoutedCommand StartCommand = new RoutedCommand("Start", typeof(SimulatorViewModel),
            new InputGestureCollection() { new KeyGesture(Key.N, ModifierKeys.Control) });

        public static RoutedCommand StopCommand = new RoutedCommand("Stop", typeof(SimulatorViewModel),
            new InputGestureCollection() { new KeyGesture(Key.W, ModifierKeys.Control) });

        public static RoutedCommand ResetCommand = new RoutedCommand("Reset", typeof(SimulatorViewModel),
            new InputGestureCollection() { new KeyGesture(Key.R, ModifierKeys.Control) });

        public static RoutedCommand AboutCommand = new RoutedCommand("About", typeof(SimulatorViewModel),
            new InputGestureCollection() { new KeyGesture(Key.A, ModifierKeys.Control) });

        #endregion field

        #region property
        public ObservableCollection<Server> Cluster { get; set; }
        public Simulator Simulator { get; private set; }

        public RelayCommand<Server> UpgradeMemoryCommand { get; private set; }
        public RelayCommand<Server> UpgradeCpuCommand { get; private set; }
        public RelayCommand AddServerCommand { get; private set; }
        public RelayCommand ConfigCommand { get; private set; }

        #endregion property

        #region constructor
        public SimulatorViewModel()
        {
            Cluster = new ObservableCollection<Server>();
            generator = new TaskGenerator();
            balancer = new LoadBalancer();
            Simulator = new Simulator();
            UpgradeMemoryCommand = new RelayCommand<Server>(UpgradeMemoryExecute);
            UpgradeCpuCommand = new RelayCommand<Server>(UpgradeCpuExecute);
            AddServerCommand = new RelayCommand(AddServerExecute);
            ConfigCommand = new RelayCommand(ConfigExecute);
            timer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 0, 0, 100)
            };
            timer.Tick += Refresh;
        }

        #endregion constructor

        #region method
        public void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void Refresh(object sender, EventArgs e)
        {
            Simulator.Simulate(balancer, generator);
        }
        
        public void AddServer(Server server)
        {
            Cluster.Add(server);
            balancer.AddServer(server);
        }

        public void StartSimulation()
        {
            if (Cluster.Count == 0)
                AddServer(new Server());
            timer.Start();
        }

        public void StopSimulation()
        {
            timer.Stop();
        }

        public void ResetSimulation()
        {
            timer.Stop();
            Cluster.Clear();
            Server.Reset();
            Simulator.Reset();
            balancer.Reset();
            generator.Reset();
        }

        private void UpgradeCpuExecute(Server server)
        {
            InputDialog cpuDialog = new InputDialog("你想升级多少CPU？（请输入一个整数/浮点数。）");
            cpuDialog.ShowDialog();
            if (cpuDialog.DialogResult == true)
            {
                if (Double.TryParse(cpuDialog.Result, out double result))
                    server.Upgrade(0, result);
                else MessageBox.Show("请输入合法的值！");
            }
        }

        private void UpgradeMemoryExecute(Server server)
        {
            InputDialog memoryDialog = new InputDialog("你想升级多少内存？（请输入一个整数/浮点数，单位：GB）");
            memoryDialog.ShowDialog();
            if (memoryDialog.DialogResult == true)
            {
                if (Double.TryParse(memoryDialog.Result, out double result))
                    server.Upgrade(result, 0);
                else MessageBox.Show("请输入合法的值！");
            }
        }

        private void AddServerExecute()
        {
            AddServer(new Server());
        }

        private void ConfigExecute()
        {
            timer.Stop();
            if (config.ShowDialog() == true)
            {
                generator.Config(config.MinMemory, config.MaxMemory,
                    config.MinCpu, config.MaxCpu,
                    config.MinTime, config.MaxTime);
                Simulator.Frequency = config.Frequency;
                Simulator.Possibility = config.Possibility;
                timer.Interval = new TimeSpan(0, 0, 0, 0, config.RefreshTime);
            }
            timer.Start();
        }

        #endregion method
    }
}
