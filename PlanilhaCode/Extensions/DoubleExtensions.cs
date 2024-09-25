using System;
using System.Globalization;

public static class DoubleExtensions
{
	// Método de extensão para formatação com casas decimais
	public static string FormatNumberDecimalPlaces(this double? number, int decimalPlaces)
	{
		if (!number.HasValue)
			return "null";

		return number.Value.ToString($"F{decimalPlaces}", CultureInfo.InvariantCulture);
	}

	// Método de extensão para formatação com dígitos significativos
	public static string FormatNumberSignificantDigits(this double? number, int significantDigits)
	{
		if (!number.HasValue)
			return "null";

		if (number.Value == 0)
			return "0";

		double scale = Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(number.Value))) + 1);
		return (Math.Round(number.Value / scale, significantDigits) * scale).ToString("G", CultureInfo.InvariantCulture);
	}
}
