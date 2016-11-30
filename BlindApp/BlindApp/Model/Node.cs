using BlindApp.Model;
using System;
using System.Collections.Generic;

namespace BlindApp.Model
{
    public class Node<T> : IComparable<Node<T>>
    {
        public T Data { get; set; }
        public LinkedList<Edge<T>> Neighbours { get; set; }

        public bool Visited { get; set; }
        public float PathPrice { get; set; }
        public Node<T> PreviousNode { get; set; }

        public Node()
        {
            Neighbours = new LinkedList<Edge<T>>();
            PathPrice = int.MaxValue;
        }

        public int CompareTo(Node<T> other)
        {
            if (this.PathPrice < other.PathPrice) return -1;
            if (this.PathPrice == other.PathPrice) return 0;
            return 1;
        }
    }
}
