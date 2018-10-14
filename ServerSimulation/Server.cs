using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = DataStructure.ServerSimulation.Task;

namespace DataStructure.ServerSimulation
{
    public class Server
    {
        #region constructor
        public Server()
        {
            ServerNum++;
            ServerID = ServerNum;
        }
        #endregion constructor

        #region field
        private List<Task> _taskPool;
        private Queue<Task> _taskQueue;

        #endregion field

        #region attribute
        public int ServerID { get;}
        static public int ServerNum { get; private set; }

        public double Memory { get; private set; }

        public double Cpu { get; private set; }

        public double MemoryUsage { get; private set; }

        public double CpuUsage { get; private set; }

        public double MemoryLest => Memory - MemoryUsage;

        public double CpuLest => Cpu - CpuUsage;

        public double MemoryUsageRate => MemoryUsage / Memory;

        public double CpuUsageRate => CpuUsage / Cpu;

        public int TasksInPool => _taskPool.Count;

        public int TasksInQueue => _taskQueue.Count;
        #endregion attribute

        #region method
        private bool AddTask(Task task)
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

        public bool DeliverTask(Task task)
        {
            if (AddTask(task))
            {
                return true;
            }
            else if (Memory > task.MemoryCost
                || Cpu > task.CpuCost)
            {
                return false;
            }
            else
            {
                _taskQueue.Enqueue(task);
                return true;
            }
        }

        private void TaskDone(Task task)
        {
            MemoryUsage -= task.MemoryCost;
            CpuUsage -= task.MemoryCost;
        }

        //do nothing if memoryInc or cpuInc < 0
        public void Upgrage(double memoryInc, double cpuInc)
        {
            if (memoryInc < 0 || cpuInc < 0)
                return;
            Memory += memoryInc;
            Cpu += cpuInc;
        }

        //called per units of time
        public void Refresh()
        {
            foreach (var task in _taskPool)
            {
                task.TimeLeft--;
                if (task.TimeLeft == 0)
                {
                    TaskDone(task);
                }
            }
            _taskPool.RemoveAll(t => t.TimeLeft == 0);
            while (_taskQueue.Any())
            {
                var task = _taskQueue.Peek();
                if (AddTask(task))
                {
                    _taskQueue.Dequeue();
                }
            }
        }

        #endregion method


    }
}
