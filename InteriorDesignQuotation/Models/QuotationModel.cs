using System.Collections.Generic;

namespace InteriorDesignQuotation.Models;

public class QuotationModel
{
    public IList<WorkItemModel> WorkItems { get; set; } = new List<WorkItemModel>();

    public int InstallmentPlanNumber { get; set; }
}

public class WorkItemModel
{
    public string Category { get; set; } = string.Empty;

    public string Area { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public decimal Quantity { get; set; }

    public string Unit { get; set; } = string.Empty;

    public decimal UnitPrice { get; set; }

    public decimal TotalPrice { get; set; }

    public string Note { get; set; } = string.Empty;

}