using System;
using System.Collections.Generic;
using System.Text;

public static class Previsao
{
	public static void CalcularPrevisoes(List<string> identificadoresTempo, IList<double?> demandas)
	{
		int totalPeriodos = identificadoresTempo.Count;
		int periodosComDemanda = demandas.Count;

		// Calcula Naive Forecast
		var naiveForecast = CalcularNaiveForecast(demandas, totalPeriodos);

		// Calcula Média Cumulativa
		var mediaCumulativa = CalcularMediaCumulativa(demandas, totalPeriodos);

		// Calcula Média Móvel (defina o período conforme necessário)
		int periodoMediaMovel = 3;
		var mediaMovel = CalcularMediaMovel(demandas, totalPeriodos, periodoMediaMovel);

		// Calcula as métricas de erro para cada método
		Console.WriteLine("\nMétricas de Erro:");

		// Naive Forecast
		var (errosNaive, errosAbsolutosNaive, errosQuadradosNaive, errosPercentuaisAbsolutosNaive) = CalcularErrosIndividuais(demandas, naiveForecast);
		var metricasNaive = CalcularMetricasErro(errosNaive, errosAbsolutosNaive, errosQuadradosNaive, errosPercentuaisAbsolutosNaive);
		Console.WriteLine("### Naive Forecast:");
		ExibirMetricasErro(metricasNaive);


		// Média Cumulativa
		var (errosCumulativa, errosAbsolutosCumulativa, errosQuadradosCumulativa, errosPercentuaisAbsolutosCumulativa) = CalcularErrosIndividuais(demandas, mediaCumulativa);
		var metricasMediaCumulativa = CalcularMetricasErro(errosCumulativa, errosAbsolutosCumulativa, errosQuadradosCumulativa, errosPercentuaisAbsolutosCumulativa);

		Console.WriteLine("\n### Média Cumulativa:");
		ExibirMetricasErro(metricasMediaCumulativa);

		// Média Móvel
		var (errosMediaMovel, errosAbsolutosMediaMovel, errosQuadradosMediaMovel, errosPercentuaisAbsolutosMediaMovel) = CalcularErrosIndividuais(demandas, mediaMovel);
		var metricasMediaMovel = CalcularMetricasErro(errosMediaMovel, errosAbsolutosMediaMovel, errosQuadradosMediaMovel, errosPercentuaisAbsolutosMediaMovel);
		Console.WriteLine("\n### Média Móvel:");
		ExibirMetricasErro(metricasMediaMovel);


		// Gera e exibe a tabela em Markdown usando o método da classe MarkdownHelper
		//string tabelaMarkdown = MarkdownHelper.GerarTabelaMarkdown(colPeriodo, colDemanda, colNaive, colMediaCumulativa, colMediaMovel );
		string tabelaMarkdown = MarkdownHelper.GerarTabelaMarkdown(
			() => demandas,
			() => naiveForecast,
			() => mediaCumulativa,
			() => errosCumulativa,
			() => errosAbsolutosCumulativa,
			() => errosQuadradosCumulativa,
			() => errosPercentuaisAbsolutosCumulativa
		);

		Console.WriteLine(tabelaMarkdown);

		Console.WriteLine(tabelaMarkdown);
	}

	private static IList<double?> CalcularNaiveForecast(IList<double?> demandas, int totalPeriodos)
	{
		var forecast = new List<double?>();
		forecast.Add(null);

		for (int i = 1; i < totalPeriodos; i++)
		{
			if (i < demandas.Count)
			{
				// Previsão ingênua é igual à demanda do período anterior
				forecast.Add(demandas[i - 1]);
			}
			else
			{
				// Para períodos futuros, a previsão é igual à última demanda conhecida
				forecast.Add(demandas[demandas.Count - 1]);
			}
		}

		return forecast;
	}

	private static IList<double?> CalcularMediaCumulativa(IList<double?> demandas, int totalPeriodos)
	{
		var medias = new List<double?>();
		double soma = 0;
		int contagemValida = 0;  // Contador para elementos não nulos

		for (int i = 0; i < totalPeriodos; i++)
		{
			if (i < demandas.Count && demandas[i].HasValue)
			{
				soma += demandas[i].Value;
				contagemValida++;  // Incrementa somente se o valor não for nulo
			}

			if (contagemValida > 0)
			{
				double media = soma / contagemValida;  // Calcula média com base no número de elementos não nulos
				medias.Add(media);
			}
			else
			{
				medias.Add(null);  // Adiciona null se nenhum valor válido foi adicionado até agora
			}
		}

		// Preenche o resto das médias, se necessário, com a última média válida ou null
		while (medias.Count < totalPeriodos)
		{
			medias.Add(medias.LastOrDefault());
		}

		return medias;
	}

	private static List<double?> CalcularMediaMovel(IList<double?> demandas, int totalPeriodos, int periodo)
	{
		var medias = new List<double?>();

		for (int i = 0; i < totalPeriodos; i++)
		{
			if (i >= periodo - 1)
			{
				double soma = 0;
				int contador = 0;

				for (int j = i - (periodo - 1); j <= i; j++)
				{
					if (j < demandas.Count && demandas[j].HasValue)
					{
						soma += demandas[j].Value; // Somente adiciona se não for null
						contador++; // Conta apenas valores válidos
					}
					else if (j >= demandas.Count && medias.Count > 0 && medias[medias.Count - 1].HasValue)
					{
						soma += medias[medias.Count - 1].Value; // Aproveita a última média móvel conhecida, se não for null
					}
				}

				if (contador > 0)
				{
					double media = soma / contador; // Calcula a média móvel apenas se houver valores válidos
					medias.Add(media);
				}
				else
				{
					medias.Add(null); // Adiciona null se todos valores do período foram null ou se estava fora do alcance
				}
			}
			else
			{
				medias.Add(null); // Não há média móvel para os primeiros períodos insuficientes
			}
		}

		return medias;
	}


	private static (List<double?> Erros, List<double?> ErrosAbsolutos, List<double?> ErrosQuadrados, List<double?> ErrosPercentuaisAbsolutos) 
		CalcularErrosIndividuais(IList<double?> demandas, IList<double?> previsoes)
	{
		List<double?> erros = new List<double?>();
		List<double?> errosAbsolutos = new List<double?>();
		List<double?> errosQuadrados = new List<double?>();
		List<double?> errosPercentuaisAbsolutos = new List<double?>();

		int n = Math.Min(demandas.Count, previsoes.Count); // Considera o menor tamanho para evitar index out of range

		for (int i = 0; i < n; i++)
		{
			// Verifica se ambos os valores na posição i não são nulos e se não são NaN
			if (demandas[i].HasValue && previsoes[i].HasValue && !double.IsNaN(demandas[i].Value) && !double.IsNaN(previsoes[i].Value))
			{
				double erro = demandas[i].Value - previsoes[i].Value;
				double erroAbsoluto = Math.Abs(erro);
				double erroQuadrado = erro * erro;
				double erroPercentualAbsoluto = (demandas[i].Value != 0) ? erroAbsoluto / demandas[i].Value : double.NaN; // Evita divisão por zero

				erros.Add(erro);
				errosAbsolutos.Add(erroAbsoluto);
				errosQuadrados.Add(erroQuadrado);
				errosPercentuaisAbsolutos.Add(erroPercentualAbsoluto);
			}
			else
			{
				// Adiciona nulos nas listas se um dos valores for nulo ou NaN
				erros.Add(null);
				errosAbsolutos.Add(null);
				errosQuadrados.Add(null);
				errosPercentuaisAbsolutos.Add(null);
			}
		}

		// Preenche as listas até o tamanho da lista mais longa, se necessário
		int maxLen = Math.Max(demandas.Count, previsoes.Count);
		while (erros.Count < maxLen)
		{
			erros.Add(null);
			errosAbsolutos.Add(null);
			errosQuadrados.Add(null);
			errosPercentuaisAbsolutos.Add(null);
		}

		return (erros, errosAbsolutos, errosQuadrados, errosPercentuaisAbsolutos);
	}

	private static Dictionary<string, double> CalcularMetricasErro(List<double?> erros, List<double?> errosAbsolutos, List<double?> errosQuadrados, List<double?> errosPercentuaisAbsolutos)
	{
		int n = erros.Count;
		int validCount = 0; // Contador para números válidos usados no cálculo

		double somaErro = 0;
		double somaErroAbsoluto = 0;
		double somaErroQuadrado = 0;
		double somaErroPercentualAbsoluto = 0;

		for (int i = 0; i < n; i++)
		{
			if (erros[i].HasValue && errosAbsolutos[i].HasValue && errosQuadrados[i].HasValue && errosPercentuaisAbsolutos[i].HasValue)
			{
				somaErro += erros[i].Value;
				somaErroAbsoluto += errosAbsolutos[i].Value;
				somaErroQuadrado += errosQuadrados[i].Value;
				somaErroPercentualAbsoluto += errosPercentuaisAbsolutos[i].Value;
				validCount++;
			}
		}

		// Cálculo das métricas, garantindo que não há divisão por zero
		double me = validCount > 0 ? somaErro / validCount : 0;
		double mae = validCount > 0 ? somaErroAbsoluto / validCount : 0;
		double mse = validCount > 0 ? somaErroQuadrado / validCount : 0;
		double rmse = Math.Sqrt(mse);
		double mape = validCount > 0 ? (somaErroPercentualAbsoluto / validCount) * 100 : 0;

		var metricas = new Dictionary<string, double>
	{
		{ "ME", me },
		{ "MAE", mae },
		{ "MSE", mse },
		{ "RMSE", rmse },
		{ "MAPE", mape }
	};

		return metricas;
	}

	private static void ExibirMetricasErro(Dictionary<string, double> metricas)
	{
		Console.WriteLine($"ME: {metricas["ME"]:F2}");
		Console.WriteLine($"MAE: {metricas["MAE"]:F2}");
		Console.WriteLine($"MSE: {metricas["MSE"]:F2}");
		Console.WriteLine($"RMSE: {metricas["RMSE"]:F2}");
		Console.WriteLine($"MAPE: {metricas["MAPE"]:F2}%");
	}
}