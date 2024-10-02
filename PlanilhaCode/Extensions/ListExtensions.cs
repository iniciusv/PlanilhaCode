using System;
using System.Collections.Generic;
using System.Globalization;

public static class ListExtensions
{
	// Método de extensão para converter List<double?> em List<string>
	public static List<string> ToFormattedStringList(this List<double?> source, string format = "G", CultureInfo culture = null)
	{
		if (culture == null)
		{
			culture = CultureInfo.CurrentCulture;
		}

		List<string> result = new List<string>();
		foreach (double? number in source)
		{
			// Verifica se o número é nulo antes de tentar convertê-lo
			if (number.HasValue)
			{
				result.Add(number.Value.ToString(format, culture));
			}
			else
			{
				result.Add("null"); // Ou outro placeholder que prefira para valores nulos
			}
		}
		return result;
	}
	public static List<double?> ToDoubleListNull(this List<int> source)
	{
		var result = new List<double?>();
		foreach (int number in source)
		{
			result.Add((double?)number);
		}
		return result;
	}
}
