using System;

public class HoltWintersForecast
{
	private double alpha;
	private double beta;
	private double gamma;
	private int seasonLength;
	private double[] seasonInitial;

	public HoltWintersForecast(double alpha, double beta, double gamma, int seasonLength, double[] seasonInitial)
	{
		this.alpha = alpha;
		this.beta = beta;
		this.gamma = gamma;
		this.seasonLength = seasonLength;
		this.seasonInitial = seasonInitial;
	}

	public double ForecastNext(double[] series)
	{
		int length = series.Length;
		double[] level = new double[length];
		double[] trend = new double[length];
		double[] season = new double[length + seasonLength]; // Adjust for full season length

		// Initialization might need adjustment depending on available data
		level[0] = series[0] / seasonInitial[0];
		trend[0] = series.Length > 1 ? series[1] / series[0] - 1 : 0; // Avoid out-of-range if only one data point

		// Ensure seasonInitial is copied correctly and handle cases where series < seasonLength
		for (int i = 0; i < seasonLength; i++)
		{
			season[i] = (i < series.Length) ? seasonInitial[i] : seasonInitial[i % seasonLength];
		}

		for (int i = 1; i < length; i++)
		{
			level[i] = alpha * (series[i] / season[i % seasonLength]) + (1 - alpha) * (level[i - 1] + trend[i - 1]);
			trend[i] = beta * (level[i] - level[i - 1]) + (1 - beta) * trend[i - 1];
			season[i + seasonLength] = gamma * (series[i] / level[i]) + (1 - gamma) * season[i % seasonLength];
		}

		// Forecasting one extra period beyond the current data
		return (level[length - 1] + trend[length - 1]) * season[length % seasonLength];
	}

}
