using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

public static class MarkdownHelper
{
	// Método para gerar tabela Markdown com dicionário de colunas de doubles
	public static string GerarTabelaMarkdown(Dictionary<string, List<double?>> colunas)
	{
		StringBuilder sb = new StringBuilder();

		// Obter os títulos das colunas a partir das chaves do dicionário
		var titulos = new List<string>(colunas.Keys);

		// Determinar o comprimento máximo para cada coluna
		Dictionary<string, int> maxComprimentos = titulos.ToDictionary(titulo => titulo, titulo =>
		{
			int maxTitulo = titulo.Length;
			int maxValor = colunas[titulo].Max(val => val?.ToString("F2").Length ?? 4); // "null" tem comprimento 4
			return Math.Max(maxTitulo, maxValor);
		});

		// Linha de cabeçalho
		foreach (var titulo in titulos)
		{
			sb.Append($"| {titulo.PadRight(maxComprimentos[titulo])} ");
		}
		sb.Append("|\n");

		// Linha separadora
		foreach (var comprimento in maxComprimentos)
		{
			sb.Append($"|{new string('-', comprimento.Value + 2)}");
		}
		sb.Append("|\n");

		// Obter o número de linhas (assumindo que todas as colunas têm o mesmo número de linhas)
		int totalLinhas = colunas[titulos[0]].Count;

		// Linhas de dados
		for (int i = 0; i < totalLinhas; i++)
		{
			foreach (var titulo in titulos)
			{
				string valorFormatado = colunas[titulo][i]?.ToString("F2") ?? "null";
				sb.Append($"| {valorFormatado.PadRight(maxComprimentos[titulo])} ");
			}
			sb.Append("|\n");
		}

		return sb.ToString();
	}

	// Sobrecarga que aceita várias listas como parâmetros
	public static string GerarTabelaMarkdown(params (string nomeColuna, IList<double?> dados)[] colunas)
	{
		var colunasDict = new Dictionary<string, List<double?>>();
		foreach (var coluna in colunas)
		{
			colunasDict.Add(coluna.nomeColuna, coluna.dados.ToList());
		}

		return GerarTabelaMarkdown(colunasDict);
	}

	public static string GerarTabelaMarkdown(params Expression<Func<IList<double?>>>[] colunaExpressions)
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

		return GerarTabelaMarkdown(colunasDict);
	}
}
