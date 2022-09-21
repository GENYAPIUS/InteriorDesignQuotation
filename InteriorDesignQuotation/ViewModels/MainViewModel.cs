using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using InteriorDesignQuotation.Extensions;
using InteriorDesignQuotation.Services.Interfaces;
using WPFLibrary;

namespace InteriorDesignQuotation.ViewModels;

public class MainViewModel : NotifyPropertyBase
{
    private readonly IWorkItemService _workItemService;

    private ICommand? _addWorkItemCommand;

    private decimal _quantity;

    private WorkItemViewModel? _selectedWorkItem;
    private decimal _unitPrice;

    private ObservableCollection<WorkItemViewModel> _workItems = new();

    public MainViewModel()
    {
        _workItemService = new WorkItemService();
        WorkItems = _workItemService.GetWorkItem();
        Categories = WorkItems.Select(x => x.Category.Trim()).Distinct().ToObservableCollection();
        Areas = WorkItems.Select(x => x.Area.Trim()).Distinct().ToObservableCollection();
        Units = WorkItems.Select(x => x.Unit.Trim()).Distinct().ToObservableCollection();
    }

    public ObservableCollection<string> Categories { get; set; }

    public ObservableCollection<string> Areas { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal Quantity
    {
        get => _quantity;
        set => SetProperty(ref _quantity, value, nameof(TotalPrice));
    }

    public ObservableCollection<string> Units { get; set; }

    public decimal UnitPrice
    {
        get => _unitPrice;
        set => SetProperty(ref _unitPrice, value, nameof(TotalPrice));
    }

    public decimal TotalPrice => Quantity * UnitPrice;

    public string Note { get; set; }

    public string SelectedCategory { get; set; } = string.Empty;

    public string SelectedArea { get; set; } = string.Empty;

    public string SelectedUnit { get; set; } = string.Empty;

    public ObservableCollection<WorkItemViewModel> WorkItems
    {
        get => _workItems;
        set => SetProperty(ref _workItems, value);
    }

    public WorkItemViewModel? SelectedWorkItem
    {
        get => _selectedWorkItem;
        set => SetProperty(ref _selectedWorkItem, value);
    }

    public ICommand AddWorkItemCommand =>
        _addWorkItemCommand ??= new RelayCommand(_ => WorkItems.Add(GetWorkItemFromView()));

    private WorkItemViewModel GetWorkItemFromView()
    {
        var result = new WorkItemViewModel
        {
            Category = SelectedCategory,
            Area = SelectedArea,
            Name = Name,
            Quantity = Quantity,
            Unit = SelectedUnit,
            UnitPrice = UnitPrice,
            TotalPrice = TotalPrice,
            Note = Note
        };
        return result;
    }
}