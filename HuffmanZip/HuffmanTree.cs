using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Priority_Queue;

namespace DataStructure.HuffmanZip
{
    class HuffmanTree
    {
        private HuffmanNode[] tree;
        private int currentNode;

        public HuffmanTree(uint[] frequency)
        {
            tree = new HuffmanNode[frequency.Length * 2 - 1];
            for (int i = 0; i < frequency.Length; i++)
            {
                tree[i].weight = frequency[i];
                tree[i].left = tree[i].right = i;
            }
            Encoding();
            currentNode = tree.Length - 1;
        }

        private void Encoding()
        {
            var heap = new SimplePriorityQueue<int, uint>();
            for (int i = 0; i < Length; i++)
            {
                heap.Enqueue(i, tree[i].weight);
            }
            for (int i = 0; i < tree.Length; i++)
                tree[i].parent = -1;
            for (int i = Length; i < tree.Length; i++)
            {
                uint w1 = heap.GetPriority(heap.First);
                int item1 = heap.Dequeue();
                uint w2 = heap.GetPriority(heap.First);
                int item2 = heap.Dequeue();
                tree[i].weight = w1 + w2;
                tree[i].left = item1;
                tree[i].right = item2;
                tree[item1].parent = tree[item2].parent = i;
                heap.Enqueue(i, tree[i].weight);
            }
            for (int i = 0; i < Length; i++)
            {
                tree[i].code = "";
                int curr = i;
                while (tree[curr].parent != -1)
                {
                    int parent = tree[curr].parent;
                    if (tree[parent].left == curr)
                        tree[i].code = '0' + tree[i].code;
                    else tree[i].code = '1' + tree[i].code;
                    curr = parent;
                }

            }
        }

        /// <summary>
        ///  push in a bit to decode (bit resides in the least significant bit of the integer
        /// </summary>
        /// <param name="bit">the bit to decode</param>
        /// <returns>if completed, return the byte, else return -1</returns>
        public int Next(int bit)
        {
            if (bit == 0)
                currentNode = tree[currentNode].left;
            else currentNode = tree[currentNode].right;
            if (currentNode < Length)
            {
                int ret = currentNode;
                currentNode = tree.Length - 1;
                return ret;
            }
            else return -1;
        }

        public string this[int index] => tree[index].code;

        public int Length => tree.Length / 2 + 1;
    }

    struct HuffmanNode
    {
        public int parent, left, right;
        public uint weight;
        public string code;
    }
}

