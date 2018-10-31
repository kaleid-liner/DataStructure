using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace DataStructure.ServerSimulation
{
    public class Simulator : INotifyPropertyChanged
    {
        private int _failedTasks = 0;
        public int FailedTasks
        {
            get => _failedTasks;
            private set
            {
                _failedTasks = value;
                OnPropertyChanged(nameof(FailedTasks));
            }
        }

        private int _tasks = 0;
        public int Tasks
        {
            get => _tasks;
            private set
            {
                _tasks = value;
                OnPropertyChanged(nameof(Tasks));
            }
        }

        private int _waitTime = 0;
        public int WaitTime
        {
            get => _waitTime;
            private set
            {
                _waitTime = value;
                OnPropertyChanged(nameof(WaitTime));
            }
        }

        private int _interval = 1;
        public int Interval
        {
            get => _interval;
            set
            {
                _interval = Math.Min(1, value);
                OnPropertyChanged(nameof(Interval));
            }
        }

        private Random random = new Random();

        private static readonly object randLock = new object();

        private double possibility = 0.5;

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        //possibility of generating any task in one unit of time
        public double Possibility
        {
            get => possibility;
            set
            {
                possibility = Math.Max(0, value);
                possibility = Math.Min(1, possibility);
                OnPropertyChanged(nameof(Possibility));
            }
        }

        public void Simulate(LoadBalancer balancer, 
            TaskGenerator generator)
        {
            for (int i = 0; i < _interval; i++)
            {
                bool anyTask;
                lock(randLock)
                {
                    anyTask = random.NextDouble() > possibility;
                }
                if (anyTask)
                {
                    Tasks++;
                    bool canExecute = balancer.DeliverTask(generator.NextTask);
                    if (!canExecute)
                        FailedTasks++;
                }
                _waitTime += balancer.Refresh();
            }
        }

    }
}