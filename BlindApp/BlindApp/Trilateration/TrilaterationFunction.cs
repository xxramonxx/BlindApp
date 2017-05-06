using MathNet.Numerics.LinearAlgebra;
using System.Collections.Generic;
using System;
/**
* Models the Trilateration problem. This is a formulation for a nonlinear least
* squares optimizer.
*
* @author scott
*
*/
namespace BlindApp
{
    public class TrilaterationFunction //: MultivariateJacobianFunction
        {

	    private const double epsilon = 1E-7;

	    /**
	     * Known positions of static nodes
	     */
	    protected double[][] positions;

	    /**
	     * Euclidean distances from static nodes to mobile node
	     */
	    protected  double[] distances;

	    public TrilaterationFunction(double[][] positions, double[] distances) {

		    if(positions.Length < 2) {
				throw new Exception("Need at least two positions.");
		    }

		    if(positions.Length != distances.Length) {
			    throw new Exception("The number of positions you provided, " +
                    positions.Length + ", does not match the number of distances, " + distances.Length + ".");
		    }

		    // bound distances to strictly positive domain
		    for (int i = 0; i < distances.Length; i++) {
			    distances[i] = Math.Max(distances[i], epsilon);
		    }

		    int positionDimension = positions[0].Length;
		    for (int i = 1; i < positions.Length; i++) {
			    if(positionDimension != positions[i].Length) {
				    throw new Exception("The dimension of all positions should be the same.");
			    }
		    }

		    this.positions = positions;
		    this.distances = distances;
	    }

	    public double[] getDistances() {
		    return distances;
	    }

	    public double[][] getPositions() {
		    return positions;
	    }

	    /**
	     * Calculate and return Jacobian function Actually return initialized function
	     *
	     * Jacobian matrix, [i][j] at
	     * J[i][0] = delta_[(x0-xi)^2 + (y0-yi)^2 - ri^2]/delta_[x0] at
	     * J[i][1] = delta_[(x0-xi)^2 + (y0-yi)^2 - ri^2]/delta_[y0] partial derivative with respect to the parameters passed to value() method
	     *
	     * @param point for which to calculate the slope
	     * @return Jacobian matrix for point
	     */
	    public Matrix<double> jacobian(Vector<double> point)
        {
		    double[] pointArray = point.ToArray();

		    double[][] jacobian = new double[distances.Length][];
		    for (int i = 0; i < jacobian.Length; i++) {
			    for (int j = 0; j < pointArray.Length; j++) {
				    jacobian[i][j] = 2 * pointArray[j] - 2 * positions[i][j];
			    }
		    }

            return Matrix<double>.Build.DenseOfColumnArrays(jacobian);  
	    }


	    public KeyValuePair< Vector<double>, Matrix<double>> value (Vector<double> point) 
            {

		    // input
		    double[] pointArray = point.ToArray();

		    // output
		    double[] resultPoint = new double[this.distances.Length];

		    // compute least squares
		    for (int i = 0; i < resultPoint.Length; i++) {
			    resultPoint[i] = 0.0;
			    // calculate sum, add to overall
			    for (int j = 0; j < pointArray.Length; j++) {
				    resultPoint[i] += (pointArray[j] - this.getPositions()[i][j]) * (pointArray[j] - this.getPositions()[i][j]);
			    }
			    resultPoint[i] -= (this.getDistances()[i]) * (this.getDistances()[i]);
		    }

            Matrix<double> jacobian = this.jacobian(point);

            return new KeyValuePair<Vector<double>, Matrix<double>>(Vector<double>.Build.DenseOfArray(resultPoint), jacobian);

        }
    }
}