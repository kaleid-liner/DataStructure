using System;
using System.Collections.Generic;
using Priority_Queue;

namespace DataStructure.VisualDijkstra
{
    static public class Dijkstra
    {
        /// <summary>
        /// find shortest path from src to dst in graph using priority queue for optimization
        /// </summary>
        /// <param name="graph">input graph</param>
        /// <param name="src">initial node</param>
        /// <param name="dst">destination node</param>
        /// <returns>tuple of the shortest path found and total length</returns>
        public static Tuple<int[], int> Solve(List<Edge>[] graph, int src, int dst)
        {
            int length = graph.Length;
            var queue = new SimplePriorityQueue<ValueTuple<int, int>, int>();
            int[] dist = new int[length];
            int[] prev = new int[length];
            for (int i = 0; i < dist.Length; i++)
                dist[i] = int.MaxValue;
            dist[src] = 0;
            prev[src] = src;
            queue.Enqueue((src, 0), 0);
            while (queue.Count != 0)
            {
                var tuple = queue.Dequeue();
                int node = tuple.Item1;
                int distance = tuple.Item2;
                if (dist[node] < distance) continue;
                dist[node] = distance;
                foreach (var edge in graph[node])
                {
                    if (edge.Cost + dist[node] < dist[edge.To])
                    {
                        dist[edge.To] = edge.Cost + dist[node];
                        queue.Enqueue((edge.To, dist[edge.To]), dist[edge.To]);
                        prev[edge.To] = node;
                    }
                }
            }
            int[] path = new int[length];
            int cnt = 0, cur = dst;
            while (cur != src)
            {
                path[cnt++] = cur;
                cur = prev[cur];
            }
            path[cnt++] = src;
            int[] ret = new int[cnt];
            Array.Copy(path, ret, cnt);
            Array.Reverse(ret);
            return new Tuple<int[], int>(ret, dist[dst]);
        }
    }

    public class Edge
    {
        public int To { get; set; }
        public int Cost { get; set; }
    }
}
