using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Navigation;
using BindingLibrary;
using InteriorDesignQuotation.Extensions;
using InteriorDesignQuotation.Models;
using InteriorDesignQuotation.Services.Interfaces;
using MessageBox = System.Windows.MessageBox;
using TextBox = System.Windows.Controls.TextBox;

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
    private ICommand? _loadQuotationCommand;
    private ICommand? _saveQuotationCommand;
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
            InstallmentPlanNumber = InstallmentPlanNumber;
            OnPropertyChanged(nameof(TotalPrice));
        });

    public ICommand LoadQuotationCommand => 
        _loadQuotationCommand ??= new RelayCommand(_ =>
        {
            var dialog = new FolderBrowserDialog();
            var dialogResult = dialog.ShowDialog();
            if (dialogResult != DialogResult.OK) return;
            // Quotation Service
            var selectedPath = dialog.SelectedPath;
            if (string.IsNullOrWhiteSpace(selectedPath)) return;
            var directoryInfo = new DirectoryInfo(selectedPath);
            var subDirectories = directoryInfo.GetDirectories();
            var quotationDirectory = subDirectories.FirstOrDefault(x => string.Equals(x.Name, "quotation", StringComparison.InvariantCultureIgnoreCase));
            if (quotationDirectory == null)
            {
                var text = "不是符合的資料夾！";
                var caption = "讀取錯誤";
                MessageBox.Show(text, caption, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                return;
            }

            var quotationFiles = quotationDirectory.GetFiles();
            var quotationFileName = "quotation.json";
            var quotationFileExtension = ".json";
            var quotationFile = quotationFiles.FirstOrDefault(x => x.Name == quotationFileName && x.Extension == quotationFileExtension);
            if (quotationFile == null)
            {
                var text = "找不到估價資料！";
                var caption = "讀取錯誤";
                MessageBox.Show(text, caption, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                return;
            }

            using var quotationStream = quotationFile.OpenText();
            var quotationData = JsonSerializer.Deserialize<QuotationModel>(quotationStream.ReadToEnd());
            if (quotationData == null)
            {
                var text = "資料讀取出錯！";
                var caption = "讀取錯誤";
                MessageBox.Show(text, caption, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                return;
            }
            WorkItems = quotationData.WorkItems.ToWorkItemViewModel();
            InstallmentPlanNumber = quotationData.InstallmentPlanNumber;
            OnPropertyChanged(nameof(TotalPrice));
        });

    public ICommand SaveQuotationCommand =>
        _saveQuotationCommand ??= new RelayCommand(_ =>
        {
            var dialog = new FolderBrowserDialog()
            {
                ShowNewFolderButton = true
            };

            var dialogResult = dialog.ShowDialog();
            if (dialogResult != DialogResult.OK) return;
            var selectedPath = dialog.SelectedPath;
            var selectedDirectoryInfo = new DirectoryInfo(selectedPath);
            var subDirectoryInfo = selectedDirectoryInfo.GetDirectories();

            var quotationDirectoryName = "quotation";
            var quotationDirectoryInfo = subDirectoryInfo.FirstOrDefault(x =>
                                             string.Equals(x.Name, quotationDirectoryName,
                                                 StringComparison.InvariantCultureIgnoreCase)) ??
                                         Directory.CreateDirectory($"{selectedPath}/{quotationDirectoryName}");
            var quotationData = new QuotationModel();
            var workItemData = WorkItems.ToWorkItemModel();
            quotationData.WorkItems = workItemData;
            quotationData.InstallmentPlanNumber = InstallmentPlanNumber;
            var workItemDataJson =  JsonSerializer.Serialize(quotationData);
            var quotationFileName = "quotation.json";
            var quotationFilePath = $"{quotationDirectoryInfo.FullName}/{quotationFileName}";
            using var quotationStreamWriter = File.CreateText(quotationFilePath);
            quotationStreamWriter.Write(workItemDataJson);
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