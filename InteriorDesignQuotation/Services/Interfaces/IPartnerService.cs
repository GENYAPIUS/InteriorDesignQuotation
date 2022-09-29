using System.Collections.ObjectModel;
using InteriorDesignQuotation.ViewModels;

namespace InteriorDesignQuotation.Services.Interfaces;

public interface IPartnerService
{
    ObservableCollection<PartnerWorkItem> GetPartnerWorkItems();
}

internal class PartnerService : IPartnerService
{
    public ObservableCollection<PartnerWorkItem> GetPartnerWorkItems() =>
        new ObservableCollection<PartnerWorkItem>
        {
            new ()
            {
                ManufacturerName = "A 廠商", WorkItem = "拆除", UnitPrice = 5000, Unit = "坪"
            },
            new ()
            {
                ManufacturerName = "A 廠商", WorkItem = "修補", UnitPrice = 3000, Unit = "坪"
            },
            new ()
            {
                ManufacturerName = "B 廠商", WorkItem = "拆除", UnitPrice = 5000, Unit = "坪"
            },
            new ()
            {
                ManufacturerName = "B 廠商", WorkItem = "泥作", UnitPrice = 7000, Unit = "坪"
            },
            new ()
            {
                ManufacturerName = "C 廠商", WorkItem = "泥作", UnitPrice = 1000, Unit = "坪"
            },
            new ()
            {
                ManufacturerName = "C 廠商", WorkItem = "修補", UnitPrice = 8000, Unit = "坪"
            }
        };
}