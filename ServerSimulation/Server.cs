using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = DataStructure.ServerSimulation.Task;
using System.ComponentModel;

namespace DataStructure.ServerSimulation
{
    public class Server : IComparable<Server>, INotifyPropertyChanged
    {
        #region constructor
        public Server()
        {
            ServerNum++;
            ServerID = ServerNum;
            _taskPool = new List<Task>();
            _taskQueue = new Queue<Task>();
        }
        #endregion constructor

        #region field
        private List<Task> _taskPool;
        private Queue<Task> _taskQueue;
        private double _memory = 128;
        private double _cpu = 8;
        private double _memoryUsage = 0;
        private double _cpuUsage = 0;
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion field

        #region property
        public int ServerID { get;}
        static public int ServerNum { get; private set; } = 0;

        //unit: GB
        public double Memory
        {
            get => _memory;
            private set
            {
                _memory = value;
                OnPropertyChanged(nameof(Memory));
            }
        }

        //in general, this represents the core num of cpu
        public double Cpu
        {
            get => _cpu;
            private set
            {
                _cpu = value;
                OnPropertyChanged(nameof(Cpu));
            }
        }

        public double MemoryUsage
        {
            get => _memoryUsage;
            private set
            {
                _memoryUsage = value;
                OnPropertyChanged(nameof(MemoryUsage));
            }
        }

        public double CpuUsage
        {
            get => _cpuUsage;
            private set
            {
                _cpuUsage = value;
                OnPropertyChanged(nameof(CpuUsage));
            }
        }

        public double MemoryLeft => Memory - MemoryUsage;

        public double CpuLeft => Cpu - CpuUsage;

        public double MemoryUsageRate => MemoryUsage / Memory;

        public double CpuUsageRate => CpuUsage / Cpu;

        public int TasksInPool => _taskPool.Count;

        public int TasksInQueue => _taskQueue.Count;
        #endregion attribute

        #region method
        public bool AddTask(Task task)
        {
            if (MemoryUsage + task.MemoryCost < Memory
                && CpuUsage + task.CpuCost < Cpu)
            {
                MemoryUsage += task.MemoryCost;
                CpuUsage += task.CpuCost;
                _taskPool.Add(task);
                return true;
            }
            return false;
        }

        //use this method if you accept the task may be delayed
        public bool DeliverTask(Task task)
        {
            if (AddTask(task))
            {
                return true;
            }
            else if (!CanExecuteTask(task))
            {
                return false;
            }
            else
            {
                _taskQueue.Enqueue(task);
                return true;
            }
        }

        //in any case, push the task to taskQueue
        public bool LazyTask(Task task)
        {
            if (!CanExecuteTask(task))
                return false;
            _taskQueue.Enqueue(task);
            return true;
        }

        public bool CanExecuteTask(Task task)
        {
            return Memory > task.MemoryCost && Cpu > task.CpuCost;
        }

        private void TaskDone(Task task)
        {
            MemoryUsage -= task.MemoryCost;
            CpuUsage -= task.CpuCost;
        }

        //do nothing if memoryInc or cpuInc < 0
        public void Upgrade(double memoryInc, double cpuInc)
        {
            if (memoryInc < 0 || cpuInc < 0)
                return;
            Memory += memoryInc;
            Cpu += cpuInc;
        }

        //called per units of time
        public void Refresh()
        {
            _taskPool.ForEach(t =>
            {
                t.WorkFor(1);
                if (t.Done)
                {
                    TaskDone(t);
                }
            });
            _taskPool.RemoveAll(t => t.Done);
            while (_taskQueue.Any())
            {
                var task = _taskQueue.Peek();
                if (AddTask(task))
                {
                    _taskQueue.Dequeue();
                }
                else break;
            }
        }

        public int CompareTo(Server other)
        {
            return (MemoryLeft / Memory + CpuLeft / Cpu).CompareTo(
                other.MemoryLeft / other.Memory + other.CpuLeft / other.Cpu);
        }
        
        public void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion method


    }
}
