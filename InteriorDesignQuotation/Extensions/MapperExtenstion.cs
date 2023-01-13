using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using InteriorDesignQuotation.Models;
using InteriorDesignQuotation.Services.Interfaces;

namespace InteriorDesignQuotation.Extensions;

public static class MapperExtenstion
{
    public static ObservableCollection<WorkItemViewModel> ToWorkItemViewModel(this IList<WorkItemModel> quotation) =>
        quotation.Select(x => new WorkItemViewModel
        {
            Category = x.Category,
            Area = x.Area,
            Name = x.Name,
            Quantity = x.Quantity,
            Unit = x.Unit,
            UnitPrice = x.UnitPrice,
            TotalPrice = x.TotalPrice,
            Note = x.Note
        }).ToObservableCollection();

    public static IList<WorkItemModel> ToWorkItemModel(this ObservableCollection<WorkItemViewModel> workItemView) => workItemView.Select(x => new WorkItemModel
    {
        Category = x.Category,
        Area = x.Area,
        Name = x.Name,
        Quantity = x.Quantity,
        Unit = x.Unit,
        UnitPrice = x.UnitPrice,
        TotalPrice = x.TotalPrice,
        Note = x.Note
    }).ToList();
}