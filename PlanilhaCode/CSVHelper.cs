using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

public static class CSVHelper
{
	private static string fieldSeparator = ";";
	private static string decimalSeparator = ",";

	// Método para gerar tabela CSV com dicionário de colunas de doubles
	public static string GerarTabelaCSV(Dictionary<string, List<double?>> colunas)
	{
		StringBuilder sb = new StringBuilder();

		// Cabeçalho
		sb.AppendLine(string.Join(fieldSeparator, colunas.Keys));

		// Número de linhas (assumindo que todas as colunas têm o mesmo número de linhas)
		int totalLinhas = colunas.First().Value.Count;

		// Linhas de dados
		for (int i = 0; i < totalLinhas; i++)
		{
			foreach (var coluna in colunas)
			{
				string valorFormatado = coluna.Value[i]?.ToString($"F2").Replace(".", decimalSeparator) ?? "null";
				sb.Append(valorFormatado);
				if (!coluna.Equals(colunas.Last()))
				{
					sb.Append(fieldSeparator);
				}
			}
			sb.AppendLine();
		}

		return sb.ToString();
	}

	// Sobrecarga que aceita várias listas como parâmetros
	public static string GerarTabelaCSV(params (string nomeColuna, IList<double?> dados)[] colunas)
	{
		var colunasDict = new Dictionary<string, List<double?>>();
		foreach (var coluna in colunas)
		{
			colunasDict.Add(coluna.nomeColuna, coluna.dados.ToList());
		}

		return GerarTabelaCSV(colunasDict);
	}

	public static string GerarTabelaCSV(params Expression<Func<IList<double?>>>[] colunaExpressions)
	{
		var colunasDict = new Dictionary<string, List<double?>>();

		foreach (var expr in colunaExpressions)
		{
			var member = expr.Body as MemberExpression;
			if (member == null)
			{
				throw new ArgumentException("Argument must be a member expression.");
			}

			var nomeColuna = member.Member.Name;
			var compiled = expr.Compile();
			var dados = compiled.Invoke().ToList();

			colunasDict.Add(nomeColuna, dados);
		}

		return GerarTabelaCSV(colunasDict);
	}

	public static void ConfigurarSeparadores(string novoSeparadorDeCampo, string novoSeparadorDecimal)
	{
		fieldSeparator = novoSeparadorDeCampo;
		decimalSeparator = novoSeparadorDecimal;
	}
}
