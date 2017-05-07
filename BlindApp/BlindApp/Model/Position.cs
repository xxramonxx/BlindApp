using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Complex;
using System.Diagnostics;
using Plugin.Compass;
using Xamarin.Forms;
using BlindApp.Interfaces;
using PropertyChanged;

using System.Numerics;
 

namespace BlindApp.Model
{
    [ImplementPropertyChanged]
    public class Position
    {
        public string Floor { get; set; }
        public double XCoordinate { get; set; }
        public double YCoordinate { get; set; }
        public Point Location
        {
            get
            {
                return new Point(XCoordinate, YCoordinate);
            }
        }

        public bool goodDirection { get; set; }

        public Position LastPosition;
        private DateTime LastPositionUpdate;

        public Position() 
        {
            LastPositionUpdate = DateTime.Now;
        }

        // direction change : substract begin vector as 0 and new trilat vector

        // direction change / (now - last direction update) = speed 

        // vzdialenost od nasledujuce milestone: vysledok trilateracie - suradnice milestonu

        public bool NewLocalize(List<SharedBeacon> Beacons)
        {
			var floor = Beacons.GroupBy(item => item.Major).Max().FirstOrDefault().Major;
			var bestBeacons = Beacons.Where(bimbas => bimbas.Major == floor).OrderBy(item => item.Distance).ToList();
            var P1 = new Vector3(
                (float)bestBeacons[0].XCoordinate,
                (float)bestBeacons[0].YCoordinate,
                0f);
            var P2 = new Vector3(
                (float)bestBeacons[1].XCoordinate,
                (float)bestBeacons[1].YCoordinate,
                0f);
            var P3 = new Vector3(
                (float)bestBeacons[2].XCoordinate,
                (float)bestBeacons[2].YCoordinate,
                0f);

            var DistA = bestBeacons[0].Distance * 100;
            var DistB = bestBeacons[1].Distance * 100;
            var DistC = bestBeacons[2].Distance * 100;

            var ex = (P2 - P1) / Norm(P2 - P1);
            var i = Vector3.Dot(ex, P3 - P1);
            var ey = (P3 - P1 - i * ex) / (Norm(P3 - P1 - i * ex));
            var d = Norm(P2 - P1);
            var j = Vector3.Dot(ey, (P3 - P1));

            float x = (float)(Math.Pow(DistA, 2) - Math.Pow(DistB, 2) + Math.Pow(d, 2)) / (2 * d);
            float y = (float)((Math.Pow(DistA, 2) - Math.Pow(DistC, 2) + Math.Pow(i, 2) + Math.Pow(j, 2)) / (2 * j)) - ((i / j) * x);
            var result = P1 + (x * ex) + (y * ey);

            XCoordinate = result.X;
            YCoordinate = result.Y;
			Floor = floor;

            var distance = GetDistance(this, LastPosition);
            if (distance > 100)
            {
                LastPosition = this.Clone();
            }

            Debug.WriteLine("Your position is: " + x + "," + y);

            return true;
        }

        public double GetDistance(Position From, Position To)
        {
            if (To == null)
                return Double.PositiveInfinity;
            var distance = Math.Pow(From.XCoordinate - To.XCoordinate, 2) + Math.Pow(From.YCoordinate - To.YCoordinate, 2);
            return Math.Sqrt(distance);
        }

        private static float Norm(Vector3 v)
        {
            return (float)Math.Sqrt(
                Math.Pow(v.X, 2) +
                Math.Pow(v.Y, 2) +
                Math.Pow(v.Z, 2));
        }
    }
}
