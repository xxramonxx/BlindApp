using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlindApp.Model
{
    public class Edge<T>
    {
        public Node<T> To { get; set; }
        public float Price { get; set; }

    }
}
