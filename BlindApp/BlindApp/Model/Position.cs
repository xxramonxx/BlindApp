using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Complex;
using System.Diagnostics;

namespace BlindApp.Model
{
    public class Position
    {
        public double XCoordinate { get; set; }
        public double YCoordinate { get; set; }

        public string Floor { get; set; }
        public bool goodDirection { get; set; }

        public Position LastPosition;

        private long LastDirectionUpdate;

        // direction change : substract begin vector as 0 and new trilat vector

        // direction change / (now - last direction update) = speed 

        // vzdialenost od nasledujuce milestone: vysledok trilateracie - suradnice milestonu

        public double Trilateration(List<SharedBeacon> bestBeacons)
        {
            SharedBeacon beacon1 = bestBeacons[0];
            SharedBeacon beacon2 = bestBeacons[1];
            SharedBeacon beacon3 = bestBeacons[2];
            SharedBeacon beacon4 = bestBeacons[3];

            double D1 = beacon1.Distance, D2 = beacon2.Distance, D3 = beacon3.Distance, D4 = beacon4.Distance;
            double X1 = beacon1.XCoordinate, X2 = beacon2.XCoordinate, X3 = beacon3.XCoordinate, X4 = beacon4.XCoordinate;
            double Y1 = beacon1.XCoordinate, Y2 = beacon2.XCoordinate, Y3 = beacon3.XCoordinate, Y4 = beacon4.YCoordinate;
            double Z1 = beacon1.ZCoordinate, Z2 = beacon2.ZCoordinate, Z3 = beacon3.ZCoordinate, Z4 = beacon4.ZCoordinate;

            double Alfa = (Math.Pow(D1, 2) - Math.Pow(D2, 2)) - (Math.Pow(X1, 2) - Math.Pow(X2, 2)) - (Math.Pow(Y1, 2) - Math.Pow(Y2, 2)) - (Math.Pow(Z1, 2) - Math.Pow(Z2, 2));
            double Beta = (Math.Pow(D1, 2) - Math.Pow(D3, 2)) - (Math.Pow(X1, 2) - Math.Pow(X3, 2)) - (Math.Pow(Y1, 2) - Math.Pow(Y3, 2)) - (Math.Pow(Z1, 2) - Math.Pow(Z3, 2));
            double Gama = (Math.Pow(D1, 2) - Math.Pow(D4, 2)) - (Math.Pow(X1, 2) - Math.Pow(X4, 2)) - (Math.Pow(Y1, 2) - Math.Pow(Y4, 2)) - (Math.Pow(Z1, 2) - Math.Pow(Z4, 2));

            double X21 = 2 * (X2 - X1);
            double X31 = 2 * (X3 - X1);
            double X41 = 2 * (X4 - X1);

            double Y21 = 2 * (Y2 - Y1);
            double Y31 = 2 * (Y3 - Y1);
            double Y41 = 2 * (Y4 - Y1);

            double Z21 = 2 * (Z2 - Z1);
            double Z31 = 2 * (Z3 - Z1);
            double Z41 = 2 * (Z4 - Z1);

            double[][] Xm =
            {
                new double[]{ Alfa, Y21, Z21 },
                new double[]{ Beta, Y31, Z31 },
                new double[]{ Gama, Y41, Z41 }
                };
            double[][] Ym =
            {
                new double[]{ X21, Alfa, Z21 },
                new double[]{ X31, Beta, Z31 },
                new double[]{ X41, Gama, Z41 }
            };
            double[][] Zm =
            {
                new double[] { X21, Y21, Alfa },
                new double[]{ X31, Y31, Beta },
                new double[]{ X41, Y41, Gama }
            };
            double[][] Mm =
            {
                new double[]{ X21, Y21, Z21 },
                new double[]{ X31, Y31, Z31 },
                new double[]{ X41, Y41, Z41 }
            };

            double Xd = Matrix<double>.Build.DenseOfColumnArrays(Xm).Determinant();
            double Yd = Matrix<double>.Build.DenseOfColumnArrays(Ym).Determinant();
            double Zd = Matrix<double>.Build.DenseOfColumnArrays(Zm).Determinant();
            double Md = Matrix<double>.Build.DenseOfColumnArrays(Mm).Determinant();

            double X = Xd / Md;
            double Y = Yd / Md;
            double Z = Zd / Md;

            double[] coordinates = { X, Y, Z };
            return 0;
        }

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
                            (beacon1.Distance * beacon1.Distance);
                top += d * v1;

                double v2 = beacon1.YCoordinate * d;
                bot += v2;

            }

            double y = top / (2 * bot);
            beacon1 = bestBeacons[0];
            c2 = bestBeacons[1];
            top = c2.Distance * c2.Distance +
                  beacon1.XCoordinate * beacon1.XCoordinate +
                  beacon1.YCoordinate * beacon1.YCoordinate -
                  beacon1.Distance * beacon1.Distance -
                  c2.XCoordinate * c2.XCoordinate -
                  c2.YCoordinate * c2.YCoordinate -
                  2 * (beacon1.YCoordinate - c2.YCoordinate) * y;
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

        public double GetDistance(Position From, Position To)
        {
            var distance = Math.Pow(From.XCoordinate - To.XCoordinate, 2) + Math.Pow(From.YCoordinate - To.YCoordinate, 2);
            return Math.Sqrt(distance);
        }
    }
}
