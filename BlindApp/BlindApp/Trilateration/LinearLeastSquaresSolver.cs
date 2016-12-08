using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Factorization;
using System;
using System.Diagnostics;
/*
package com.lemmingapex.trilateration;

import org.apache.commons.math3.linear.*;*/

/**
 *
 * For testing only. A linear approach to solve the Trilateration problem.
 * see http://inside.mines.edu/~whereman/talks/TurgutOzal-11-Trilateration.pdf
 *
 * @author scott
 */
namespace BlindApp
{
    public class LinearLeastSquaresSolver {

        protected TrilaterationFunction function;

        public LinearLeastSquaresSolver(TrilaterationFunction function) {
            this.function = function;
        }

        public Vector<double> solve(bool debugInfo) {
            int numberOfPositions = function.getPositions().Length;
            int positionDimension = function.getPositions()[0].Length;

            double[][] Ad = new double[numberOfPositions - 1][];

            // TODO: which reference position should be used?  currently using postion and distance in index 0.

            for (int i = 1; i < numberOfPositions; i++) {
                double[] Adi = new double[positionDimension];
                for (int j = 0; j < positionDimension; j++) {
                    Adi[j] = function.getPositions()[i][j] - function.getPositions()[0][j];
                }
                Ad[i - 1] = Adi;
            }
            if (debugInfo) {
                Debug.WriteLine(Matrix<double>.Build.DenseOfColumnArrays(Ad));
            }

            // reference point is function.getPositions()[0], with distance function.getDistances()[0]
            double referenceDistance = function.getDistances()[0];
            double r0squared = referenceDistance * referenceDistance;
            double[] bd = new double[numberOfPositions - 1];
            for (int i = 1; i < numberOfPositions; i++) {
                double ri = function.getDistances()[i];
                double risquared = ri * ri;

                // Localize distance between ri and r0
                double di0squared = 0;
                for (int j = 0; j < positionDimension; j++) {
                    double dij0j = function.getPositions()[i][j] - function.getPositions()[0][j];
                    di0squared += dij0j * dij0j;
                }
                bd[i - 1] = 0.5 * (r0squared - risquared + di0squared);
            }
            if (debugInfo) {
                Debug.WriteLine(Vector<double>.Build.DenseOfArray(bd));
            }

            Matrix<double> A = Matrix<double>.Build.DenseOfColumnArrays(Ad);
            Vector<double> b = Vector<double>.Build.DenseOfArray(bd);

            var solver = A.QR();
            // new QR<double>().Determinant();// QR<Vector<double>>()

            Vector<double> x;

            if (solver.Determinant == 0) {
                // bummer...
                x = Vector<double>.Build.DenseOfArray(new double[positionDimension]);
            } else {
                x = solver.Solve(b);
            }

            return x.Add(Vector<double>.Build.DenseOfArray(function.getPositions()[0]));
        }

        public Vector<double> solve() {
            return solve(false);
        }
    }
}