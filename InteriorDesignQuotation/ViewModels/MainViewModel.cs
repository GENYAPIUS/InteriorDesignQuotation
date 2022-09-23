using System;
using System.Collections.ObjectModel;
using System.DirectoryServices;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using InteriorDesignQuotation.Extensions;
using InteriorDesignQuotation.Services.Interfaces;
using WPFLibrary;

namespace InteriorDesignQuotation.ViewModels;

public class MainViewModel : NotifyPropertyBase
{
    private readonly IWorkItemService _workItemService;

    private ICommand? _addWorkItemCommand;

    private ICommand? _removeWorkItemCommand;

    private decimal _quantity;

    private WorkItemViewModel? _selectedWorkItem;
    private decimal _itemUnitPrice;

    private ObservableCollection<WorkItemViewModel> _workItems = new();

    private string _categoriesSearchText = string.Empty;
    private string _areasSearchText = string.Empty;
    private string _unitSearchText = string.Empty;
    private int _installmentPlanNumber;

    public MainViewModel()
    {
        _workItemService = new WorkItemService();
        WorkItems = _workItemService.GetWorkItem();
        Categories = WorkItems.Select(x => x.Category).Distinct().ToObservableCollection();
        Areas = WorkItems.Select(x => x.Area).Distinct().ToObservableCollection();
        Units = WorkItems.Select(x => x.Unit).Distinct().ToObservableCollection();
    }

    public ObservableCollection<string> Categories { get; set; }

    public ObservableCollection<string> Areas { get; set; }

    public ObservableCollection<string> CategoriesSearchResult { get; set; } = new();

    public ObservableCollection<string> AreasSearchResult { get; set; } = new();

    public ObservableCollection<string> UnitsSearchResult { get; set; } = new();

    public string CategoriesSearchContent
    {
        get => _categoriesSearchText;
        set => SetProperty(ref _categoriesSearchText, value);
    }

    public string AreasSearchContent
    {
        get => _areasSearchText;
        set => SetProperty(ref _areasSearchText, value);
    }

    public string UnitSearchContent
    {
        get => _unitSearchText;
        set => SetProperty(ref _unitSearchText, value);
    }

    public string Name { get; set; } = string.Empty;

    public decimal Quantity
    {
        get => _quantity;
        set => SetProperty(ref _quantity, value, nameof(ItemTotalPrice));
    }

    public ObservableCollection<string> Units { get; set; }

    public decimal ItemUnitPrice
    {
        get => _itemUnitPrice;
        set => SetProperty(ref _itemUnitPrice, value, nameof(ItemTotalPrice));
    }

    public decimal ItemTotalPrice => Quantity * ItemUnitPrice;

    public decimal TotalPrice => WorkItems.Sum(x => x.TotalPrice);

    public string Note { get; set; }

    public string SelectedCategory { get; set; } = string.Empty;

    public string SelectedArea { get; set; } = string.Empty;

    public string SelectedUnit { get; set; } = string.Empty;

    public int InstallmentPlanNumber { get => _installmentPlanNumber;
        set
        {
            SetProperty(ref _installmentPlanNumber, value);
            if(_installmentPlanNumber <= 0) return;
            InstallmentInfos.Clear();
            for (var i = 1; i <= _installmentPlanNumber; i++)
            {
                InstallmentInfos.Add(new InstallmentPlanInfo
                {
                    Name = $"第 {i} 期", Price = TotalPrice / _installmentPlanNumber
                });
            }
        }
    }

    public ObservableCollection<InstallmentPlanInfo> InstallmentInfos { get; set; } = new();

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
        _addWorkItemCommand ??= new RelayCommand(_ => WorkItems?.Add(GetWorkItemFromView()));

    public ICommand RemoveWorkItemCommand =>  _removeWorkItemCommand ??= new RelayCommand(_ =>
    {
        if ( SelectedWorkItem == null) return;
        WorkItems.Remove(SelectedWorkItem);
        var a = TotalPrice;
    });

    private WorkItemViewModel GetWorkItemFromView()
    {
        var result = new WorkItemViewModel
        {
            Category = SelectedCategory.Trim(),
            Area = SelectedArea.Trim(),
            Name = Name.Trim(),
            Quantity = Quantity,
            Unit = SelectedUnit.Trim(),
            UnitPrice = ItemUnitPrice,
            TotalPrice = ItemTotalPrice,
            Note = Note.Trim()
        };
        return result;
    }

    public void OnWorkItemsUpdate(object? sender, EventArgs args)
    {
        return;
    }
}