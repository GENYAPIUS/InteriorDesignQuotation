using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using BindingLibrary;
using InteriorDesignQuotation.Extensions;
using InteriorDesignQuotation.Services.Interfaces;

namespace InteriorDesignQuotation.ViewModels;

public class Quotation : NotifyPropertyBase
{
    private readonly IWorkItemService _workItemService;
    private ICommand? _addWorkItemCommand;
    private ICommand? _cellUpdateCommand;
    private int _installmentPlanNumber;
    private decimal? _itemUnitPrice;
    private decimal? _quantity;
    private ICommand? _removeWorkItemCommand;
    private object _selectedValue;
    private WorkItemViewModel? _selectedWorkItem;
    private ObservableCollection<WorkItemViewModel> _workItems = new();

    public Quotation()
    {
        _workItemService = new WorkItemService();
        WorkItems = _workItemService.GetWorkItem();
        Categories = WorkItems.Select(x => x.Category).Distinct().ToObservableCollection();
        Areas = WorkItems.Select(x => x.Area).Distinct().ToObservableCollection();
        Units = WorkItems.Select(x => x.Unit).Distinct().ToObservableCollection();
    }

    public ObservableCollection<string> Categories { get; set; }

    public ObservableCollection<string> Areas { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal? Quantity
    {
        get => _quantity;
        set
        {
            SetProperty(ref _quantity, value);
            OnPropertyChanged(nameof(ItemTotalPrice));
        }
    }

    public ObservableCollection<string> Units { get; set; }

    public decimal? ItemUnitPrice
    {
        get => _itemUnitPrice;
        set
        {
            SetProperty(ref _itemUnitPrice, value);
            OnPropertyChanged(nameof(ItemTotalPrice));
        }
    }

    public decimal ItemTotalPrice => (Quantity ?? 0) * (ItemUnitPrice ?? 0);

    public decimal TotalPrice => WorkItems.Sum(x => x.TotalPrice);

    public string Note { get; set; } = string.Empty;

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
        set
        {
            SetProperty(ref _selectedWorkItem, value);
            OnPropertyChanged(nameof(TotalPrice));
        }
    }

    public int InstallmentPlanNumber
    {
        get => _installmentPlanNumber;
        set
        {
            if (value <= 0) return;
            InstallmentInfos.Clear();
            for (var i = 1; i <= value; i++)
                InstallmentInfos.Add(new InstallmentPlanInfo
                {
                    Name = $"第 {i} 期",
                    Price = TotalPrice / value
                });
            SetProperty(ref _installmentPlanNumber, value);
        }
    }

    public ObservableCollection<InstallmentPlanInfo> InstallmentInfos { get; set; } = new();

    public ICommand AddWorkItemCommand =>
        _addWorkItemCommand ??= new RelayCommand(_ =>
        {
            WorkItems.Add(GetWorkItemFromView());
            OnPropertyChanged(nameof(TotalPrice));
        });

    public ICommand RemoveWorkItemCommand =>
        _removeWorkItemCommand ??= new RelayCommand(_ =>
        {
            if (SelectedWorkItem == null) return;
            WorkItems.Remove(SelectedWorkItem);
        });

    public ICommand CellUpdateCommand =>
        _cellUpdateCommand ??= new RelayCommand(sender =>
        {
            var source = sender as DataGridCellEditEndingEventArgs;
            if (source!.Column.DisplayIndex != 6) return;
            if (!decimal.TryParse((source.EditingElement as TextBox)?.Text, out var totalPrice)) return;
            if (SelectedWorkItem == null) return;
            SelectedWorkItem.TotalPrice = totalPrice;
            OnPropertyChanged(nameof(TotalPrice));
        });

    private WorkItemViewModel GetWorkItemFromView()
    {
        var result = new WorkItemViewModel
        {
            Category = SelectedCategory.Trim(),
            Area = SelectedArea.Trim(),
            Name = Name.Trim(),
            Quantity = Quantity ?? 0,
            Unit = SelectedUnit.Trim(),
            UnitPrice = ItemUnitPrice ?? 0,
            TotalPrice = ItemTotalPrice,
            Note = Note.Trim()
        };
        return result;
    }
}