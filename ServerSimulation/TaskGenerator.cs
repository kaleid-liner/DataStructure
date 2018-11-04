using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructure.ServerSimulation
{
    public class TaskGenerator
    {
        #region field
        private double _minMemory;
        private double _maxMemory;
        private double _minCpu;
        private double _maxCpu;
        private int _minTime;
        private int _maxTime;
        private Random random = new Random();
        #endregion field

        public TaskGenerator()
        {
            _minMemory = 1;
            _maxMemory = 10;
            _minCpu = 0.01;
            _maxCpu = 1;
            _minTime = 1;
            _maxTime = 60;
        }

        public TaskGenerator(double minMemory, double maxMemory,
            double minCpu, double maxCpu, int minTime, int maxTime)
        {
            Config(minMemory, maxMemory, minCpu, maxCpu, minTime, maxTime);
        }

        public void Config(double minMemory, double maxMemory,
            double minCpu, double maxCpu, int minTime, int maxTime)
        {
            _minMemory = Math.Max(0, minMemory);
            _maxMemory = Math.Max(_minMemory, maxMemory);
            _minCpu = Math.Max(0, minCpu);
            _maxCpu = Math.Max(_minCpu, maxCpu);
            _minTime = Math.Max(0, minTime);
            _maxTime = Math.Max(_minTime, maxTime);
        }

        public Task NextTask
        {
            get
            {
                double memory = random.NextDouble(_minMemory, _maxMemory);
                double cpu = random.NextDouble(_minCpu, _maxCpu);
                int time = random.Next(_minTime, _maxTime);
                return new Task(memory, cpu, time);
            }
        }

        public void Reset()
        {
            _minMemory = 1;
            _maxMemory = 10;
            _minCpu = 0.01;
            _maxCpu = 1;
            _minTime = 1;
            _maxTime = 60;
        }
    }

    static class RandomExtensions
    {
        public static double NextDouble(this Random random, double minVal, double maxVal)
        {
            return random.NextDouble() * (maxVal - minVal) + minVal;
        }
    }
}
