public class SmoothingParameters
{
	public double Alpha { get; set; }
	public double Beta { get; set; }
	public double CurrentLevel { get; private set; }
	public double CurrentTrend { get; private set; }

	public SmoothingParameters(double alpha, double beta, double initialLevel, double initialTrend)
	{
		Alpha = alpha;
		Beta = beta;
		CurrentLevel = initialLevel;
		CurrentTrend = initialTrend;
	}

	public void Update(double observedValue)
	{
		double newLevel = Alpha * observedValue + (1 - Alpha) * (CurrentLevel + CurrentTrend);
		double newTrend = Beta * (newLevel - CurrentLevel) + (1 - Beta) * CurrentTrend;

		CurrentLevel = newLevel;
		CurrentTrend = newTrend;
	}

	public double Forecast(int periodsAhead)
	{
		return CurrentLevel + periodsAhead * CurrentTrend;
	}
}

public class ForecastingModel
{
	public SmoothingParameters parameters;

	public ForecastingModel(double alpha, double beta, double initialLevel, double initialTrend)
	{
		parameters = new SmoothingParameters(alpha, beta, initialLevel, initialTrend);
	}

	public void UpdateModel(double observedValue)
	{
		parameters.Update(observedValue);
	}

	public double GetForecast(int periodsAhead)
	{
		return parameters.Forecast(periodsAhead);
	}

	public double UpdateAndForecast(double observedValue)
	{
		UpdateModel(observedValue);  // Updates the model with the latest observed value
		return GetForecast(1);       // Returns forecast for one period ahead
	}
}