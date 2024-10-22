using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PlanilhaCode.Testes;

public class InventoryManagementTests
{
	[Fact]
	public void Lesson2_TestSensitivityAnalysisII()
	{
		int demand = 25000;
		double unitCost = 25.5;
		double holdingRate = 0.15;
		var fixOrderingCost = 750;
		InventoryManagement inventory = new InventoryManagement(demand: demand, unitCost: unitCost, fixOrderingCost: fixOrderingCost, holdingRate: holdingRate);

		// Calculando o EOQ - Economic Order Quantity
		double eoq = inventory.CalculateEconomicOrderQuantity_EOQ();
		double trc = inventory.CalculateTotalRelevantCost_TRC();
		double T = inventory.CalculateOrderCycleTime().Value;

		// Asserções para verificar os resultados calculados
		Assert.Equal(3131.12, eoq, 2);
		Assert.Equal(0.125, T, 3);
		Assert.Equal(11976, trc, 0, MidpointRounding.ToZero);
	}
	[Fact]
	public void Lesson2_QQ3()
	{
		var holdingCost = 12.5;
		var unitCost = 50;
		int quantity = 400;
		int demand = 2000;
		int fixOrderingCost = 500;
		InventoryManagement inventory = new InventoryManagement() { Quantity = quantity, Demand = demand, HoldingCost = holdingCost, UnitCost = unitCost, FixOrderingCost = fixOrderingCost };

		// Calculando o EOQ - Economic Order Quantity
		double trc = inventory.CalculateTotalRelevantCost_TRC();

		Assert.Equal(5000, trc, 0);

	}
	[Fact]
	public void Lesson2_QQ4()
	{
		int demand = 27500;
		double unitCost = 100;
		double holdingRate = 0.24;
		var fixOrderingCost = 300;
		InventoryManagement inventory = new InventoryManagement(demand: demand, unitCost: unitCost, fixOrderingCost: fixOrderingCost, holdingRate: holdingRate);

		// Calculando o EOQ - Economic Order Quantity
		double eoq = inventory.CalculateEconomicOrderQuantity_EOQ();
		double trc = inventory.CalculateTotalRelevantCost_TRC();
		double T = inventory.CalculateOrderCycleTime().Value;
		double tc = inventory.CalculateTotalCost();


		Assert.Equal(829.16, eoq, 2);
		Assert.Equal(0.03015113445777636, T, 8);
		Assert.Equal(19899.75, trc, 2);
		Assert.Equal(2769899.75, tc, 2);

		// Ajustando o OrderCycleTime_T e recalculando o TRC
		double newT = 0.0192307692;
		inventory.SetOrderCycleTime_T(newT);
		double newTrc = inventory.CalculateTotalRelevantCost_TRC();

		// Validar o novo TRC após ajuste
		Assert.Equal(21946.15, newTrc, 2);
	}
	[Fact]
	public void Lesson2_QQ5Maybe()
	{
		// Separação das variáveis de input
		double unitCost = 50;
		double fixOrderingCost = 100;
		int demand = 12000;
		double holdingRate = 0.20;
		int leadTimeDays = 30;
		int daysInYear = 360;

		// Setup: Criar uma instância de InventoryManagement com os parâmetros especificados
		var inventoryManager = new InventoryManagement(unitCost: unitCost, fixOrderingCost: fixOrderingCost, demand: demand, holdingRate: holdingRate, leadTimeDays: leadTimeDays);

		// Action: Calcular EOQ, Reorder Point, e Inventory Position
		double eoq = inventoryManager.CalculateEconomicOrderQuantity_EOQ();
		double reorderPoint = inventoryManager.CalculateReorderPoint(360);
		double inventoryPosition = inventoryManager.CalculateInventoryPosition();

		Assert.Equal(490, Math.Round(eoq)); // Verifica se o EOQ arredondado é 490
		Assert.Equal(1000, Math.Round(reorderPoint)); // Verifica se o Reorder Point arredondado é 1000
		Assert.Equal(1490, Math.Round(inventoryPosition)); // Verifica se a Inventory Position arredondada é 1490

		inventoryManager.LeadTimeDays = 15; // Reduzindo o lead time para metade do mês
		double newReorderPoint = inventoryManager.CalculateReorderPoint();
		Assert.Equal(500, Math.Round(newReorderPoint)); // Verifica se o novo Reorder Point arredondado é 500 após a redução do lead time
	}
	[Fact]
	public void TestOptimalOrderQuantityAndCycleTime()
	{
		// Setup
		var inventory = new InventoryManagement(
			unitCost: 78,  // Unit cost per bottle
			fixOrderingCost: 513,  // Cost of placing an order
			demand: 34954,  // Annual demand for the wine
			holdingRate: 0.14  // Holding rate
		);
		double eoq = inventory.CalculateEconomicOrderQuantity_EOQ();
		double weeksBetweenOrders = inventory.CalculateOrderCycleTimeInWeeks();

		Assert.Equal(1812.22, eoq, 2);
		Assert.Equal(2.489, weeksBetweenOrders, 3);
	}

}