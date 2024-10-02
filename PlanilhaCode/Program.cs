

public class Program
{
	public static void Main()
	{

		var demanda = new List<double?> { 876, 1205, 700, 893, 1363, 725, 958, 1262, 695, 885, 1340, 747, 877, 1428, 869, 1021, 1325, 853, 947, 1400, 876 };
		var forrecasts = new List<double?> { 946, 934, 1011, 938, 932, 1050, 974, 976, 1057, 971, 953, 1059, 985, 962, 1087, 1039, 1041, 1122, 1061, 1037, 1138, 1078 };


		//var forrecastsNaive = Forecaster.CalculateNaiveForecast(demanda);
		//var forrecastsCum = Forecaster.CalculateCumulativeMean(demanda);
		//var forrecastsMove2 = Forecaster.CalculateMovingAverage(demanda,2);
		//var forrecastsMove4 = Forecaster.CalculateMovingAverage(demanda,4);
		var forrecasts2 = Forecaster.ExponentialSmoothing(demanda, 0.333);




		//var index = 1;
		//
		//foreach (var forecast in forrecasts2)
		//{
		//	if (index < demanda.Count())
		//		Console.WriteLine($"{index}-{demanda[index]}-{forecast}");
		//	else
		//		Console.WriteLine($"{index}- null -{forecast}");
		//
		//	index++;
		//}

		//var forrecasts = Forecaster.CalculateMovingAverage(demanda, 4, 1);


		var forrecastsNaiveMAPE = Statistics.CalculateMAPE(demanda, forrecasts);
		//var forrecastsCumMAPE = Statistics.CalculateMAPE(demanda, forrecastsCum);
		//var forrecastsMove2MAPE = Statistics.CalculateMAPE(demanda, forrecastsMove2);
		//var forrecastsMove4MAPE = Statistics.CalculateMAPE(demanda, forrecastsMove4);
		//var forecastRMSE = Statistics.CalculateRMSE(demanda, forrecasts);
		//
		Console.WriteLine($"O MAPE da lista é: {forrecastsNaiveMAPE.GetValue()}");
		//Console.WriteLine($"O MAPE da lista é: {forrecastsCumMAPE.GetValue()}");
		//Console.WriteLine($"O MAPE da lista é: {forrecastsMove2MAPE.GetValue()}");
		//Console.WriteLine($"O MAPE da lista é: {forrecastsMove4MAPE.GetValue()}");
		//Console.WriteLine($"O RMSE da lista é: {forecastRMSE.FormatNumberSignificantDigits(4)}");
		//Console.WriteLine(Statistics.CalculateCoefficientOfVariation(demanda));



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
	static void CallForecasters()
	{
		// Exemplo de dados de entrada
		var identificadoresTempo = new List<string?> {"Jan 2012","Feb 2012","Mar 2012","Apr 2012","May 2012","Jun 2012","Jul 2012","Aug 2012","Sep 2012","Oct 2012","Nov 2012",
			"Dec 2012","Jan 2013","Feb 2013","Mar 2013","Apr 2013","May 2013","Jun 2013","Jul 2013","Aug 2013","Sep 2013","Oct 2013","Nov 2013","Dec 2013","Jan 2014","Feb 2014","Mar 2014","Apr 2014","May 2014",
			"Jun 2014","Jul 2014","Aug 2014","Sep 2014","Oct 2014","Nov 2014","Dec 2014"};

		var demandas = new List<double?> { 584, 552, 544, 576, 585, 784, 1026, 1098, 666, 504, 576, 1224, 720, 729, 747, 621, 819, 1260, 1368, 1530, 873, 976, 624, 1416, 855, 528, 729, 800, 744, 928, 1560, 1746, 909, 1773, 2000, 1512 };

		// Chama o método estático da classe Previsao
		Previsao.CalcularPrevisoes(identificadoresTempo, demandas);
	}
}



