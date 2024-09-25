public class Statistics
{
	public static Result CalculateRMSE(List<double?> observed, List<double?> forecasted)
	{
		var validPairs = FilterValidPairs(observed, forecasted);
		double rmse = CalculateRootMeanSquareError(validPairs);
		return new Result(rmse);
	}

	private static IEnumerable<(double, double)> FilterValidPairs(List<double?> observed, List<double?> forecasted)
	{
		return observed.Zip(forecasted, (obs, fore) => (obs, fore))
					   .Where(pair => pair.obs.HasValue && pair.fore.HasValue)
					   .Select(pair => (pair.obs.Value, pair.fore.Value));
	}

	private static double CalculateRootMeanSquareError(IEnumerable<(double, double)> validPairs)
	{
		double sumSquareError = validPairs.Sum(pair => Math.Pow(pair.Item1 - pair.Item2, 2));
		int validCount = validPairs.Count();
		if (validCount == 0)
			throw new InvalidOperationException("Não há dados suficientes para calcular o RMSE.");

		return Math.Sqrt(sumSquareError / validCount);
	}

	public static Result CalculateMAE(List<double?> observed, List<double?> forecasted)
	{
		var validPairs = FilterValidPairs(observed, forecasted);
		double mae = CalculateMeanAbsoluteError(validPairs);
		return new Result(mae);
	}
	private static double CalculateMeanAbsoluteError(IEnumerable<(double, double)> validPairs)
	{
		double sumAbsoluteError = validPairs.Sum(pair => Math.Abs(pair.Item1 - pair.Item2));
		int validCount = validPairs.Count();
		if (validCount == 0)
			throw new InvalidOperationException("Não há dados suficientes para calcular o MAE.");

		return sumAbsoluteError / validCount;
	}
	public static Result CalculateMAPE(List<double?> observed, List<double?> forecasted)
	{
		var validPairs = FilterValidPairsForMAPE(observed, forecasted);
		double mape = CalculateMeanAbsolutePercentageError(validPairs);
		return new Result(mape);
	}

	private static IEnumerable<(double, double)> FilterValidPairsForMAPE(List<double?> observed, List<double?> forecasted)
	{
		return observed.Zip(forecasted, (obs, fore) => (obs, fore))
					   .Where(pair => pair.obs.HasValue && pair.fore.HasValue && pair.obs.Value != 0)
					   .Select(pair => (pair.obs.Value, pair.fore.Value));
	}
	private static double CalculateMeanAbsolutePercentageError(IEnumerable<(double, double)> validPairs)
	{
		double sumPercentageError = validPairs.Sum(pair => Math.Abs((pair.Item1 - pair.Item2) / pair.Item1) * 100);
		int validCount = validPairs.Count();
		if (validCount == 0)
			throw new InvalidOperationException("Não há dados suficientes para calcular o MAPE.");

		return sumPercentageError / validCount;
	}
	public static double CalculateCoefficientOfVariation(List<double?> demand)
	{
		if (demand == null || demand.Count == 0)
			throw new ArgumentException("A lista de demanda está vazia ou nula.");

		// Filtra valores nulos e converte para List<double>
		List<double> validDemands = demand.Where(d => d.HasValue).Select(d => d.Value).ToList();

		if (validDemands.Count == 0)
			throw new InvalidOperationException("Não há dados suficientes para calcular a variabilidade.");

		// Calcula a média dos valores
		double mean = validDemands.Average();

		if (mean == 0)
			throw new InvalidOperationException("A média dos dados é zero, o coeficiente de variação não é definido.");

		// Calcula o desvio padrão
		double sumOfSquaresOfDifferences = validDemands.Select(val => (val - mean) * (val - mean)).Sum();
		double standardDeviation = Math.Sqrt(sumOfSquaresOfDifferences / validDemands.Count);

		// Calcula o coeficiente de variação em percentual
		double coefficientOfVariation = (standardDeviation / mean) * 100;

		return coefficientOfVariation;
	}
}
public class Result
{
	private double value;

	public Result(double value)
	{
		this.value = value;
	}

	// Retorna o valor RMSE como double
	public double GetValue()
	{
		return value;
	}

	// Método para formatar o número com dígitos significativos
	public string FormatNumberSignificantDigits(int significantDigits)
	{
		if (value == 0) return "0";
		double scale = Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(value))) + 1);
		return (Math.Round(value / scale, significantDigits) * scale).ToString("G");
	}

	// Método opcional para formatar com casas decimais
	public string FormatNumberDecimalPlaces(int decimalPlaces)
	{
		return value.ToString($"F{decimalPlaces}");
	}
}
