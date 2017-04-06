using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlindApp
{
    public class CompassChangedEventArgs : EventArgs
    {
        public CompassChangedEventArgs(double values)
        {
            Values = values;
        }
        public double Values { get; }
    }
}
