using System;
using System.Collections.Generic;

public class Forecaster
{
	// Método para calcular o forecast naive
	public static List<double?> CalculateNaiveForecast(List<double?> demand, int monthsInFuture = 0)
	{
		if (demand == null || demand.Count == 0)
			throw new ArgumentException("A lista de demanda está vazia ou nula.");

		// Cria uma lista de forecast com espaço adicional para os meses futuros
		List<double?> forecast = new List<double?>(new double?[demand.Count + monthsInFuture]);

		// O primeiro mês não pode ter uma previsão, então começa com null
		forecast[0] = null;

		// Usar o valor do mês anterior como a previsão para o mês atual
		for (int i = 1; i < demand.Count; i++)
		{
			forecast[i] = demand[i - 1];
		}

		// Preenche os meses futuros com o valor do último mês disponível da demanda, se existir
		if (monthsInFuture > 0 && demand.Count > 0)
		{
			double? lastKnownValue = demand.Last(d => d.HasValue); // Obtém o último valor conhecido que não é nulo
			for (int i = demand.Count; i < demand.Count + monthsInFuture; i++)
			{
				forecast[i] = lastKnownValue;
			}
		}

		return forecast;
	}

	public static List<double?> CalculateCumulativeMean(List<double?> demand, int monthsInFuture = 0)
	{
		if (demand == null || demand.Count == 0)
			throw new ArgumentException("A lista de demanda está vazia ou nula.");

		// Cria uma lista de cumulative mean com espaço adicional para os meses futuros
		List<double?> cumulativeMean = new List<double?>(new double?[demand.Count + monthsInFuture + 1]);
		double sum = 0;
		int count = 0;

		// O primeiro mês não pode ter uma média cumulativa baseada em meses anteriores
		if (demand[1].HasValue)
		{
			sum += demand[0].Value;
			count++;
			cumulativeMean[1] = demand[0];  // Repete o valor do primeiro mês
		}
		else
		{
			cumulativeMean[0] = null;
		}

		// Calcula a média cumulativa para os meses atuais
		for (int i = 2; i < demand.Count + 1; i++)
		{
			if (demand[i-1].HasValue)
			{
				sum += demand[i-1].Value;  // Adiciona o valor do mês anterior
				count++;
				cumulativeMean[i] = sum / count;  // Calcula a média até o mês anterior
			}
			//else
			//{
			//	// Se o mês anterior é null, não atualiza a soma ou o contador
			//	cumulativeMean[i] = null;
			//}
		}

		// Preenche os meses futuros com a última média calculada
		if (monthsInFuture -1 > 0 && count > 0)
		{
			double? lastCalculatedMean = cumulativeMean.Last(c => c.HasValue);  // Obtém a última média calculada
			for (int i = demand.Count; i < demand.Count + monthsInFuture; i++)
			{
				cumulativeMean[i] = lastCalculatedMean;
			}
		}

		return cumulativeMean;
	}

	public static List<double?> CalculateMovingAverage(List<double?> demand, int window)
	{
		if (demand == null || demand.Count == 0)
			throw new ArgumentException("A lista de demanda está vazia ou nula.");

		if (window < 1)
			throw new ArgumentException("A janela para média móvel deve ser ao menos 1.");

		List<double?> movingAverages = new List<double?>(new double?[demand.Count + 1]);

		// Calcula a média móvel para cada mês
		for (int i = window; i < demand.Count +1; i++)
		{
			// Seleciona os dados dos últimos `window` meses antes do mês atual
			var windowData = demand.Skip(i - window).Take(window).Where(x => x.HasValue).Select(x => x.Value).ToList();

			if (windowData.Count > 0)
			{
				movingAverages[i] = windowData.Average();  // Calcula a média dos dados na janela
			}
			else
			{
				movingAverages[i] = null;  // Se não houver dados suficientes ou todos forem null, define como null
			}
		}

		return movingAverages;
	}
}
