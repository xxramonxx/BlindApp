using System;
using Xamarin.Forms;

namespace BlindApp
{
    public class MapPoint
    {
        //        public Array Properties { get; set; }

        public double XCoordinate { get; set; }
        public double YCoordinate { get; set; }
        public double ZCoordinate { get; set; }
        public string Floor { get; set; }
        public Point Location {
     
              get
            {
                return new Point(XCoordinate, YCoordinate);
            }
        }
    }
}
