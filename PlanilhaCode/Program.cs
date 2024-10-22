

using MathNet.Numerics;
using PlanilhaCode;

public class Program
{
	public static void Main()
	{

	}




	private static void CallHoltWinters()
	{
		// Assuming alpha, beta, and gamma (usually these should be optimized)
		double alpha = 0.2;
		double beta = 0.1;
		double gamma = 0.15;

		// Seasonality index initialization (based on provided distributions)
		double[] seasonInitial = { 0.6, 0.8, 1.4, 1.2 }; // Starting at Q1 through Q4

		// Initialize model
		HoltWintersForecast model = new HoltWintersForecast(alpha, beta, gamma, 4, seasonInitial);

		// Historical data (you will need actual demand values here for past quarters)
		double[] historicalDemand = { 1047 / 1.4 }; // Reverse calculated base level for Q3

		// Forecast the next quarter (Q4)
		double forecastQ4 = model.ForecastNext(historicalDemand);

		Console.WriteLine($"Forecast for 2014Q4 is: {forecastQ4} units");
	}

	private static void CallEOQ()
	{
		var Demand = 2000;
		var orderCost = 50; //cost/unit
		var holdingCost = 12.5;//Ce, cost of Unit/time
		var fixOrderingCost = 50;
		InventoryManagement inventory = new InventoryManagement(2000, orderCost, 10, 0.1, 17.5, 0);

		// Calculando o EOQ - Economic Order Quantity
		double eoq = inventory.CalculateEconomicOrderQuantity_EOQ();
		Console.WriteLine($"EOQ (Quantidade Econômica de Pedido): {eoq:N2} unidades");

		Console.WriteLine($"EOQ (Quantidade Econômica de Pedido): {eoq * 100:N2} doletas");

		// Calculando o TRC - Total Relevant Cost
		double trc = inventory.CalculateTotalRelevantCost_TRC();
		Console.WriteLine($"TRC (Custo Total Relevante Ótimo): ${trc:N2}");

		// Calculando o TC - Total Cost
		double tc = inventory.CalculateTotalCost();
		Console.WriteLine($"TC (Custo Total Anual): ${tc:N2}");
	}

	private static void Discounts()
	{
		{
			// Parâmetros do problema
			int demand = 1000; // Demanda anual em unidades
			double unitCost = 10; // Custo de compra por unidade
			double holdingRate = 0.1; // Taxa de manutenção
			double fixOrderingCost = 100; // Custo fixo de fazer um pedido
			int discountThreshold = 1000; // Limiar para desconto incremental
			double discountRate = 0.005; // Taxa de desconto de 0.5%

			// Criar instância do gerenciador de inventário com os valores corretos
			InventoryManagement inventory = new InventoryManagement(unitCost: unitCost, fixOrderingCost: fixOrderingCost, demand: demand, holdingRate: holdingRate);

			// Calculando o EOQ - Economic Order Quantity
			double eoq = inventory.CalculateEconomicOrderQuantity_EOQ();
			Console.WriteLine($"EOQ (Quantidade Econômica de Pedido): {eoq:N0} unidades");

			// Usando DiscountCalculator para calcular o custo total com desconto incremental
			double totalCost = DiscountCalculator.CalculateIncrementalDiscountCost(unitCost, (int)Math.Ceiling(eoq), discountThreshold, discountRate);
			Console.WriteLine($"Custo total com desconto para {Math.Ceiling(eoq):N0} unidades: ${totalCost:N2}");

			// Calculando o TRC - Total Relevant Cost
			double trc = inventory.CalculateTotalRelevantCost_TRC();
			Console.WriteLine($"TRC (Custo Total Relevante Ótimo): ${trc:N2}");

			// Calculando o TC - Total Cost
			double tc = inventory.CalculateTotalCost();
			Console.WriteLine($"TC (Custo Total Anual): ${tc:N2}");
		}
	}

	public static void CalculateOptimalOrderQuantity()
	{
		// Initialize the InventoryManagement class with necessary parameters
		InventoryManagement inventoryManagement = new InventoryManagement(
			unitCost: 20,                   // Cost per unit
			fixOrderingCost: 90,            // Ordering cost
			demand: 7432,                   // Annual demand
			holdingRate: 0.03               // Holding rate (3% of the unit cost)
		);

		// Calculate EOQ
		double eoq = inventoryManagement.CalculateEconomicOrderQuantity_EOQ();
		Console.WriteLine($"The Economic Order Quantity (EOQ) is: {Math.Ceiling(eoq)} units.");
		double orderCycleTimeInWeeks = inventoryManagement.CalculateOrderCycleTimeInWeeks();


		Console.WriteLine($"The expected time between orders is: {Math.Round(orderCycleTimeInWeeks, 2)} weeks.");
		Console.WriteLine($"Quantas vezes no ano: {52/orderCycleTimeInWeeks} .");

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

	static void Week4Gradded1_Part1()
	{ var inventory = new InventoryManagement(
        unitCost: 78,  // Unit cost per bottle
        fixOrderingCost: 513,  // Cost of placing an order
        demand: 34954,  // Annual demand for the wine
        holdingRate: 0.14
		,  // Holding rate
        leadTimeDays: 2 * (365.0 / 48)  // Lead time converted to days, considering 48 weeks/year
	);



		double eoq = inventory.CalculateEconomicOrderQuantity_EOQ();
		Console.WriteLine($"EOQ ${eoq}");

		double orderCycleTimeInWeeks = inventory.CalculateOrderCycleTimeInWeeks();

    // Annual Purchasing Cost
    double annualPurchasingCost = inventory.Demand.Value * inventory.UnitCost;

    // Annual Ordering Cost
    double numberOfOrdersPerYear = inventory.Demand.Value / eoq;
    double annualOrderingCost = numberOfOrdersPerYear * inventory.FixOrderingCost;

    // Annual Average Cycle Stock Cost
    double averageCycleStock = eoq / 2;
    double annualAverageCycleStockCost = averageCycleStock * inventory.HoldingCost.Value;

    // Annual Pipeline Inventory Cost
    double annualPipelineInventoryCost = inventory.CalculateReorderPoint() * inventory.UnitCost;

    Console.WriteLine($"Annual Purchasing Cost: ${annualPurchasingCost}");
    Console.WriteLine($"Annual Ordering Cost: ${annualOrderingCost}");
    Console.WriteLine($"Annual Average Cycle Stock Cost: ${annualAverageCycleStockCost}");
    Console.WriteLine($"Annual Pipeline Inventory Cost: ${annualPipelineInventoryCost}");
}

	static void Week4Gradded1part2()
	{
		double annualDemand = 34954;
		double purchaseCostPerUnit = 78;
		double orderingCostPerOrder = 513;
		double holdingCostRate = 0.14;
		double weeksPerYear = 48;
		double orderLeadTimeWeeks = 2;
		double orderCycleTimeWeeks = 2.48967214052755;
		double optimalOrderQuantity = 1813; // From previous part

		double totalPurchaseCost = purchaseCostPerUnit * annualDemand;

		double numOrdersPerYear = weeksPerYear / orderCycleTimeWeeks;
		double totalOrderingCost = numOrdersPerYear * orderingCostPerOrder;

		double weeklyDemand = annualDemand / weeksPerYear;
		double averagePipelineInventory = weeklyDemand * orderLeadTimeWeeks;
		double annualPipelineInventoryCost = holdingCostRate * averagePipelineInventory * purchaseCostPerUnit;

		Console.WriteLine($"Total Purchase Cost: ${Math.Round(totalPurchaseCost)}");
		Console.WriteLine($"Total Ordering Cost: ${Math.Round(totalOrderingCost)}");
		Console.WriteLine($"Annual Pipeline Inventory Cost: ${Math.Round(annualPipelineInventoryCost)}");

		double totalAnnualCost = totalPurchaseCost + totalOrderingCost + annualPipelineInventoryCost;
		Console.WriteLine($"Estimated Total Annual Costs: ${Math.Round(totalAnnualCost)}");

		// Calculate average cycle stock
		double averageCycleStock = optimalOrderQuantity / 2;

		// Calculate annual average cycle stock cost
		double annualAverageCycleStockCost = averageCycleStock * purchaseCostPerUnit * holdingCostRate;

		// Output the annual average cycle stock cost
		Console.WriteLine($"Annual Average Cycle Stock Cost: ${Math.Round(annualAverageCycleStockCost)}");
		Console.WriteLine($"---------------------------------------------------------");

		InventoryManagement inventory = new InventoryManagement(unitCost: purchaseCostPerUnit, fixOrderingCost: orderingCostPerOrder, demand: annualDemand, holdingRate: holdingCostRate/*, leadTimeDays: leadTime*/);
		// Calculando o EOQ - Economic Order Quantity
		double eoq = inventory.CalculateEconomicOrderQuantity_EOQ();
		inventory.UnitCost = 78+ 0.34;
		var TC = inventory.CalculateTotalCost();

		// Arredonda para cima e imprime o resultado
		int optimalOrderQuantity1 = (int)Math.Ceiling(eoq);
		Console.WriteLine($"The optimal order quantity for MelissaWhite is: {optimalOrderQuantity} bottles");
		Console.WriteLine($"The TC for MelissaWhite is: {TC} $$$");
		inventory.SetOrderCycleTime_T(409);
		double eoq2 = inventory.CalculateEconomicOrderQuantity_EOQ();
		int optimalOrderQuantity2 = (int)Math.Ceiling(eoq);
		Console.WriteLine($"The optimal order quantity for MelissaWhite is: {optimalOrderQuantity2} bottles");


		//inventory.UnitCost = purchaseCostPerUnit ()

	}
	static void Week4Gradded2part1()
	{
		double annualDemand = 3960;
		double unitCost = 135;
		double fixOrderingCost = 181;
		double holdingRate = 0.2;

		// Criar instância do gerenciador de inventário com os valores corretos
		InventoryManagement inventory = new InventoryManagement(unitCost: unitCost, fixOrderingCost: fixOrderingCost, demand: annualDemand, holdingRate: holdingRate);

		// Calculando o EOQ - Economic Order Quantity
		double eoq = inventory.CalculateEconomicOrderQuantity_EOQ();
		Console.WriteLine($"EOQ (Quantidade Econômica de Pedido): {eoq:N0} unidades");

		double T = inventory.CalculateOrderCycleTime().Value;

		Console.WriteLine($"T: {T} ");


		var TRC = inventory.CalculateTotalRelevantCost_TRC();
		Console.WriteLine($"TRC {TRC} $$");
		Console.WriteLine($"---------------------------------------------------------");


		var totalHoldingCost = inventory.CalculateTotalHoldingCost();
		Console.WriteLine($"totalHoldingCost {totalHoldingCost} $$");
		inventory.Quantity = 300;
		var totalHoldingCost2 = inventory.CalculateTotalHoldingCost();
		Console.WriteLine($"totalHoldingCost {totalHoldingCost2} $$");
		Console.WriteLine($"diference {totalHoldingCost- totalHoldingCost2} $$");

	}
}






