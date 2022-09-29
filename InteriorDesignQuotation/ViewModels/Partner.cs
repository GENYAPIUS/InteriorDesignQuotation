using System.Collections.ObjectModel;
using System.Linq;
using BindingLibrary;
using InteriorDesignQuotation.Extensions;
using InteriorDesignQuotation.Services.Interfaces;

namespace InteriorDesignQuotation.ViewModels;

public class Partner : NotifyPropertyBase
{
    private readonly IPartnerService _partnerService = new PartnerService();
    private ObservableCollection<string> _manufacturerNames;
    private decimal _unitPrice;
    private ObservableCollection<string> _units;
    private string _workItemName = string.Empty;
    private ObservableCollection<PartnerWorkItem> _workItems;

    public Partner()
    {
        _workItems = _partnerService.GetPartnerWorkItems();
        _manufacturerNames = WorkItems.Select(x => x.ManufacturerName).Distinct().ToObservableCollection();
        _units = WorkItems.Select(x => x.Unit).Distinct().ToObservableCollection();
    }

    public ObservableCollection<PartnerWorkItem> WorkItems
    {
        get => _workItems;
        set => SetProperty(ref _workItems, value);
    }

    public ObservableCollection<string> ManufacturerNames
    {
        get => _manufacturerNames;
        set => SetProperty(ref _manufacturerNames, value);
    }

    public ObservableCollection<string> Units
    {
        get => _units;
        set => SetProperty(ref _units, value);
    }

    public string WorkItemName
    {
        get => _workItemName;
        set => SetProperty(ref _workItemName, value);
    }

    public decimal UnitPrice
    {
        get => _unitPrice;
        set => SetProperty(ref _unitPrice, value);
    }
}

public class PartnerWorkItem
{
    public string ManufacturerName { get; set; } = string.Empty;

    public string WorkItem { get; set; } = string.Empty;

    public decimal UnitPrice { get; set; }

    public string Unit { get; set; } = string.Empty;
}