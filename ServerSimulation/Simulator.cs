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

        private int _frequency = 1;
        //单位时间内产生任务次数
        public int Frequency
        {
            get => _frequency;
            set
            {
                _frequency = Math.Max(1, value);
                OnPropertyChanged(nameof(Frequency));
            }
        }

        private Random random = new Random();

        private static readonly object randLock = new object();

        private double possibility = 0.5;

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        //possibility of generating any task
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
            for (int i = 0; i < _frequency; i++)
            {
                bool anyTask;
                lock(randLock)
                {
                    anyTask = random.NextDouble() <= possibility;
                }
                if (anyTask)
                {
                    Tasks++;
                    bool canExecute = balancer.DeliverTask(generator.NextTask);
                    if (!canExecute)
                        FailedTasks++;
                }
            }
            WaitTime += balancer.Refresh();
        }

        public void Reset()
        {
            FailedTasks = 0;
            Tasks = 0;
            WaitTime = 0;
            Possibility = 0.5;
            Frequency = 1;
        }

    }
}