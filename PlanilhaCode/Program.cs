public class Program
{
	public static void Main()
	{
		CallForecastingModel();
	}
	static void CallForecastingModel()
	{
		var Alpha = 0.25;
		var Beta = 0.1;
		var initialLevel = 90;
		var initialTrend = 8.5;
		var valueAtInitialTime = 92;

		var howMuchInTheFuture = 4;

		// Initialize the forecasting model with alpha, beta, initial level, and initial trend
		ForecastingModel model = new ForecastingModel(Alpha, Beta, initialLevel, initialTrend);

		// Update model with the observed value at t=100
		model.UpdateModel(valueAtInitialTime);

		// Print Markdown table header
		Console.WriteLine("| Period | Forecast |");
		Console.WriteLine("|--------|----------|");


		// Print forecasts for each future period in Markdown format
		for (int i = 1; i <= howMuchInTheFuture; i++)
		{
			double forecast = model.GetForecast(i);
			Console.WriteLine($"| {100 + i}    | {forecast:F2} |");
		}

		// Output the final forecasted demand
		double finalForecast = model.GetForecast(howMuchInTheFuture);
		Console.WriteLine($"\nForecasted demand for t=105: {finalForecast:F2}");
	}
}
