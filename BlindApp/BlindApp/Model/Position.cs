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
using MathNet.Numerics;
 

namespace BlindApp.Model
{
    [ImplementPropertyChanged]
    public class Position
    {
        public double XCoordinate { get; set; }
        public double YCoordinate { get; set; }
        public Point Location
        {
            get
            {
                return new Point(XCoordinate, YCoordinate);
            }
        }
        public double Rotation { get; set; }

        public string Floor { get; set; }
        public bool goodDirection { get; set; }

        public Position LastPosition;

        private DateTime LastDirectionUpdate;

        public Vector2 change;


        public Position() 
        {
            LastDirectionUpdate = DateTime.Now;    
        }

        // direction change : substract begin vector as 0 and new trilat vector

        // direction change / (now - last direction update) = speed 

        // vzdialenost od nasledujuce milestone: vysledok trilateracie - suradnice milestonu


        public void Localize(List<SharedBeacon> bestBeacons)
        {
            double top = 0;
            double bot = 0;
            SharedBeacon beacon1 = null;
            SharedBeacon c2 = null;
            SharedBeacon c3 = null;

            for (int i = 0; i < 3; i++)
            {
                beacon1 = bestBeacons[i];
                if (i == 0)
                {
                    c2 = bestBeacons[1];
                    c3 = bestBeacons[2];
                }
                else if (i == 1)
                {
                    c2 = bestBeacons[0];
                    c3 = bestBeacons[2];
                }
                else
                {
                    c2 = bestBeacons[0];
                    c3 = bestBeacons[1];
                }

                double d = c2.XCoordinate - c3.XCoordinate;

                double v1 = (beacon1.XCoordinate * beacon1.XCoordinate + beacon1.YCoordinate * beacon1.YCoordinate) -
                    ((beacon1.Distance * 100) * (beacon1.Distance* 100));
                top += d * v1;

                double v2 = beacon1.YCoordinate * d;
                bot += v2;

            }

            double y = top / (2 * bot);
            beacon1 = bestBeacons[0];
            c2 = bestBeacons[1];
            top = Math.Pow(c2.Distance * 100, 2)+
                  Math.Pow(beacon1.XCoordinate, 2) +
                  Math.Pow(beacon1.YCoordinate, 2) -
                  Math.Pow(beacon1.Distance* 100, 2) -
                  Math.Pow(c2.XCoordinate, 2) -
                  Math.Pow(c2.YCoordinate, 2) -
                  (2 * (beacon1.YCoordinate - c2.YCoordinate) * y);
            
            bot = beacon1.XCoordinate - c2.XCoordinate;
            double x = top / (2 * bot);

            var previousPosition = this.Clone();

            XCoordinate = x;
            YCoordinate = y;

            var distance = GetDistance(this, previousPosition);
            if (distance > 1)
            {
                LastPosition = previousPosition;
            }
            Debug.WriteLine("Your position is: " + x + "," + y);
        }

        public bool NewLocalize(List<SharedBeacon> bestBeacons)
        {
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

            var previousPosition = this.Clone();

            XCoordinate = result.X;
            YCoordinate = result.Y;

            var distance = GetDistance(this, previousPosition);
            if (distance > 1)
            {
                LastPosition = previousPosition;
            }
            Debug.WriteLine("Your position is: " + x + "," + y);

            return true;
        }

        public double GetDistance(Position From, Position To)
        {
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
