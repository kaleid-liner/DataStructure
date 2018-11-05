# Server Simulation

A cluster simulator developed with .NET FrameWork 4.7.1 and WPF

GUI Styled with [MaterialDesign](https://github.com/MaterialDesignInXAML)



## Situation

1. There is a **[cluster](https://en.wikipedia.org/wiki/Computer_cluster)** consisted of serveral SuperComputers, or **server**. 
2. This cluster received large amounts of task request every unit of time.
3. Each task has **Memory cost**, **Cpu cost** and **Time cost**. Each of this cost has a linear random distribution from a minimum to a maximum.
4. Each server of the cluster has certain cpu ability and memory.
5. - If a server's available resources exceed a task's cost, then the task can be added into the server's ThreadPool and be handled immediately. 

   - If a task's cost exceed server's maximum resources, then the task fails.

   - Otherwise, the task will hang in the server's Queue and wait to be executed.
6. There is a load balancer, which applies a greedy algorithm, to deliver tasks among the servers.
7. You can configure the task's cost and the number of servers.