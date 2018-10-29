using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructure.ServerSimulation
{
    public class LoadBalancer
    {
        private List<Server> _servers = new List<Server>();

        public bool DeliverTask(Task task)
        {
            _servers.Sort();
            bool canExecute = false;
            foreach (var server in _servers)
            {
                if (server.AddTask(task))
                {
                    canExecute = true;
                    break;
                }
            }
            return canExecute;
        }

        public void AddServer(Server server)
        {
            _servers.Add(server);
        }
    }
}
