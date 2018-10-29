using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructure.ServerSimulation
{
    public static class Simulator
    {
        public static int FailedTasks { get; private set; } = 0;

        private static Random random = new Random();

        private static readonly object randLock = new object();

        private static double possibility = 0.5;

        //possibility of generating any task in one unit of time
        public static double Possibility
        {
            get => possibility;
            set
            {
                possibility = Math.Max(0, value);
                possibility = Math.Min(1, possibility);
            }
            
        }

        public static void Simulate(LoadBalancer balancer, 
            TaskGenerator generator, int time)
        {
            while (time-- != 0)
            {
                bool anyTask;
                lock(randLock)
                {
                    anyTask = random.NextDouble() > possibility;
                }
                if (anyTask)
                {
                    bool canExecute = balancer.DeliverTask(generator.NextTask);
                    if (!canExecute)
                        FailedTasks++;
                }
            }
        }

    }
}