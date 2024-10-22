using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanilhaCode;

public class DiscountCalculator
{
	// Método para calcular o custo com desconto incremental
	public static double CalculateIncrementalDiscountCost(double unitCost, int unitsOrdered, int threshold, double discountRate)
	{
		if (unitsOrdered <= threshold)
		{
			// Sem desconto se a quantidade pedida é igual ou inferior ao limiar
			return unitsOrdered * unitCost;
		}
		else
		{
			// Calcula o custo para as primeiras 'threshold' unidades sem desconto
			double costWithoutDiscount = threshold * unitCost;

			// Calcula o custo para as unidades além do limiar com desconto
			double discountedUnitCost = unitCost * (1 - discountRate);
			double costWithDiscount = (unitsOrdered - threshold) * discountedUnitCost;

			// Retorna o custo total combinado
			return costWithoutDiscount + costWithDiscount;
		}
	}
}
