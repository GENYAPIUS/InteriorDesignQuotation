using System.Collections.ObjectModel;
using BindingLibrary;

namespace InteriorDesignQuotation.Services.Interfaces;

public interface IWorkItemService
{
    ObservableCollection<WorkItemViewModel> GetWorkItem();
}

public class WorkItemViewModel : NotifyPropertyBase
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

internal class WorkItemService : IWorkItemService
{
    public ObservableCollection<WorkItemViewModel> GetWorkItem() =>
        new()
        {
            new WorkItemViewModel
            {
                Category = "拆除工程",
                Area = "前陽台",
                Name = "地磚",
                Quantity = 5,
                Unit = "坪",
                UnitPrice = 1600,
                TotalPrice = 8000,
                Note = "牆面壁磚破損疑慮，泥作回補，有補洞痕跡，地面孔洞待確認"
            },
            new WorkItemViewModel
            {
                Category = "拆除工程",
                Area = "前陽台",
                Name = "落地門窗",
                Quantity = 1,
                Unit = "式",
                UnitPrice = 3500,
                TotalPrice = 3500,
                Note = string.Empty
            },
            new WorkItemViewModel
            {
                Category = "拆除工程",
                Area = "大浴室",
                Name = "地磚",
                Quantity = 1.5m,
                Unit = "坪",
                UnitPrice = 1600,
                TotalPrice = 2400,
                Note = string.Empty
            },
            new WorkItemViewModel
            {
                Category = "泥作工程",
                Area = "大浴室",
                Name = "牆面防水",
                Quantity = 8,
                Unit = "坪",
                UnitPrice = 1500,
                TotalPrice = 12000,
                Note = string.Empty
            }
        };
}