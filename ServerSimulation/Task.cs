using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructure.ServerSimulation
{
    public class Task
    {
        public Task(double memoryCost, double cpuCost, int timeCost)
        {
            MemoryCost = memoryCost;
            CpuCost = cpuCost;
            TimeCost = timeCost;
            TimeLeft = timeCost;
        }

        public void WorkFor(int timeSpan)
        {
            TimeLeft = Math.Max(TimeLeft - timeSpan, 0);
        }

        public double MemoryCost { get; }

        public double CpuCost { get; }

        public int TimeCost { get; }

        public int TimeLeft { get; private set; }

        public bool Done => TimeLeft == 0;
    }
}
