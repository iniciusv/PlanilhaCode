using System;

public class InventoryManagement
{
	public double? Demand { get; set; }
	public double UnitCost { get; set; }
	public double? HoldingRate { get; set; }
	public double? HoldingCost { get; set; }
	public double? SellingPrice { get; set; }
	public double FixOrderingCost { get; set; }
	public double? Quantity { get; set; }
	public double? LeadTimeDays { get; set; }
	double? OrderCycleTime_T;

	public InventoryManagement(){}
	public InventoryManagement(double unitCost, double fixOrderingCost, double? demand = null, double? quantity = null, double? sellingPrice = null, double? holdingRate = null, double? holdingCost = null, double? leadTimeDays = null)
	{
		Demand = demand;
		Quantity = quantity;
		UnitCost = unitCost;
		SellingPrice = sellingPrice;
		FixOrderingCost = fixOrderingCost;
		HoldingRate = holdingRate;
		HoldingCost = holdingCost ?? (holdingRate.HasValue ? holdingRate.Value * unitCost : 0);
		LeadTimeDays = leadTimeDays;
	}

	public double CalculateEconomicOrderQuantity_EOQ()
	{
		if (!HoldingCost.HasValue)
			throw new InvalidOperationException("Holding cost must be specified.");
		if (!Demand.HasValue)
			throw new InvalidOperationException("Demand must be specified to calculate EOQ.");

		Quantity = Math.Sqrt((2 * Demand.Value * FixOrderingCost) / HoldingCost.Value);
		CalculateOrderCycleTime(); // Atualizando o Order Cycle Time após o cálculo do EOQ
		return Quantity.Value;
	}
	public double CalculateReorderPoint(int daysInYear = 336)
	{
		if (!Demand.HasValue || !LeadTimeDays.HasValue)
			throw new InvalidOperationException("Both Demand and Lead Time must be specified to calculate the reorder point.");

		double leadTimeInYears = LeadTimeDays.Value / daysInYear;
		return Demand.Value * leadTimeInYears;
	}
	public double CalculateTotalRelevantCost_TRC()
	{
		if (!Quantity.HasValue || !Demand.HasValue)
			throw new InvalidOperationException("Both Quantity and Demand must be specified to calculate total relevant cost.");

		if (!OrderCycleTime_T.HasValue)
			CalculateOrderCycleTime();

		return (FixOrderingCost * (1 / OrderCycleTime_T.Value)) + (HoldingCost.Value * (Quantity.Value / 2));
	}

	public double CalculateTotalCost()
	{
		if (!Demand.HasValue)
			throw new InvalidOperationException("Demand must be specified to calculate total cost.");

		double trc = CalculateTotalRelevantCost_TRC();
		double totalPurchaseCost = Demand.Value * UnitCost;
		return trc + totalPurchaseCost;
	}

	public double CalculateReplenishmentOrderQuantity()
	{
		if (!Quantity.HasValue)
			throw new InvalidOperationException("Quantity must be specified to calculate replenishment order quantity.");

		return Quantity.Value;
	}

	public double? CalculateOrderCycleTime()
	{
		if (!Quantity.HasValue || !Demand.HasValue)
			throw new InvalidOperationException("Both Quantity and Demand must be specified to calculate order cycle time.");
		OrderCycleTime_T = Quantity.Value / Demand.Value;
		return OrderCycleTime_T;
	}

	public void SetOrderCycleTime_T(double newOrderCycleTime)
	{
		if (!Demand.HasValue || !HoldingCost.HasValue)
			throw new InvalidOperationException("Demand and Holding Cost must be specified before setting Order Cycle Time.");

		OrderCycleTime_T = newOrderCycleTime;
		CalculateQuantityFromOrderCycleTime(); // Recalculate Quantity based on the new Order Cycle Time
	}
	void CalculateQuantityFromOrderCycleTime()
	{
		if (!OrderCycleTime_T.HasValue || !Demand.HasValue)
			throw new InvalidOperationException("Order Cycle Time and Demand must be specified to calculate Quantity.");

		// Quantity is inversely proportional to OrderCycleTime_T
		Quantity = Demand.Value * OrderCycleTime_T.Value;
	}
	public double CalculateInventoryPosition()
	{
		double eoq = CalculateEconomicOrderQuantity_EOQ();
		double reorderPoint = CalculateReorderPoint();
		return Math.Round(reorderPoint + eoq);
	}
	public double CalculateTotalHoldingCost()
	{
		if (!Quantity.HasValue)
			throw new InvalidOperationException("Quantity must be specified to calculate total holding cost.");
		if (!HoldingCost.HasValue)
			throw new InvalidOperationException("Holding cost per unit must be specified to calculate total holding cost.");

		double averageInventory = Quantity.Value / 2;

		double totalHoldingCost = averageInventory * HoldingCost.Value;

		return totalHoldingCost;
	}
	public double CalculateOrderCycleTimeInWeeks(int weeksInYear = 52)
	{
		if (!Quantity.HasValue || !Demand.HasValue)
			throw new InvalidOperationException("Both Quantity and Demand must be specified to calculate order cycle time in weeks.");

		double orderCycleTimeInYears = Quantity.Value / Demand.Value;
		return orderCycleTimeInYears * weeksInYear;
	}


}
