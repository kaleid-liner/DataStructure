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
    class SimulatorViewModel : INotifyPropertyChanged
    {
        #region field
        private TaskGenerator generator;
        private LoadBalancer balancer;
        private DispatcherTimer timer;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion field

        #region property
        public ObservableCollection<Server> Cluster { get; set; }
        public Simulator Simulator { get; private set; }

        public RelayCommand<Server> UpgradeMemoryCommand { get; private set; }
        public RelayCommand<Server> UpgradeCpuCommand { get; private set; }
        public RelayCommand AddServerCommand { get; private set; }
        public RelayCommand StartCommand { get; private set; }
        public RelayCommand StopCommand { get; private set; }

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
            StartCommand = new RelayCommand(StartSimulation);
            StopCommand = new RelayCommand(StopSimulation);
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
            AddServer(new Server());
            timer.Start();
        }

        public void StopSimulation()
        {
            timer.Stop();
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


        #endregion method
    }
}
