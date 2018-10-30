using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStructure.ServerSimulation;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Threading;

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
        public Simulator Simulator { get; set; }
        #endregion property

        #region constructor
        public SimulatorViewModel()
        {
            Cluster = new ObservableCollection<Server>();
            generator = new TaskGenerator();
            timer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 1)
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
            timer.Start();
        }

        public void StopSimulation()
        {
            timer.Stop();
        }
        #endregion method
    }
}
