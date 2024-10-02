using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics;
using MathNet.Numerics.LinearRegression;
using MathNet.Numerics.LinearAlgebra;

public class RegressionModel
{
	public static Tuple<double, double[], double, double, double, double[]> MultipleLinearRegression(List<int> indices, List<double?> dependentVariable, params List<double?>[] independentVariables)
	{
		int n = dependentVariable.Count;
		if (independentVariables.Any(iv => iv.Count != n))
			throw new ArgumentException("Todas as listas devem ter o mesmo número de elementos.");

		var validIndices = indices.Where(i => dependentVariable[i] != null && independentVariables.All(iv => iv[i] != null)).ToList();
		double[] y = validIndices.Select(i => dependentVariable[i].Value).ToArray();
		double[][] x = validIndices.Select(i => independentVariables.Select(iv => iv[i].Value).ToArray()).ToArray();
		double[][] xWithIntercept = x.Select(row => new[] { 1.0 }.Concat(row).ToArray()).ToArray();

		var p = MultipleRegression.NormalEquations(xWithIntercept, y);

		double rss = 0, tss = 0;
		double meanY = y.Average();
		double[] residuals = new double[y.Length];
		double[] fitted = new double[y.Length];

		for (int i = 0; i < y.Length; i++)
		{
			double fittedValue = p[0]; // Intercept
			for (int j = 0; j < xWithIntercept[i].Length - 1; j++)
			{
				fittedValue += xWithIntercept[i][j + 1] * p[j + 1];
			}
			fitted[i] = fittedValue;
			residuals[i] = y[i] - fittedValue;
			rss += residuals[i] * residuals[i];
			tss += (y[i] - meanY) * (y[i] - meanY);
		}

		double rSquared = 1 - rss / tss;
		int k = p.Length - 1; // Number of predictors
		double adjustedRSquared = 1 - ((1 - rSquared) * (y.Length - 1)) / (y.Length - k - 1);
		double mse = rss / (y.Length - k - 1);
		double[] standardErrors = new double[p.Length];
		var xMatrix = Matrix<double>.Build.DenseOfRowArrays(xWithIntercept);
		var xTxInverse = (xMatrix.TransposeThisAndMultiply(xMatrix)).Inverse();

		for (int i = 0; i < p.Length; i++)
		{
			standardErrors[i] = Math.Sqrt(mse * xTxInverse[i, i]);
		}

		return Tuple.Create(p[0], p.Skip(1).ToArray(), rSquared, adjustedRSquared, Math.Sqrt(mse), standardErrors);
	}

	public static void PrintResults(Tuple<double, double[], double, double, double, double[]> results)
	{
		Console.WriteLine($"Intercept: {results.Item1} (SE: {results.Item6[0]})");
		Console.WriteLine("Coefficients:");
		for (int i = 0; i < results.Item2.Length; i++)
		{
			Console.WriteLine($"B{i + 1}: {results.Item2[i]} (SE: {results.Item6[i + 1]})");
		}
		Console.WriteLine($"R-Squared: {results.Item3}");
		Console.WriteLine($"Adjusted R-Squared: {results.Item4}");
		Console.WriteLine($"Standard Error of Estimate: {results.Item5}");
	}

}
