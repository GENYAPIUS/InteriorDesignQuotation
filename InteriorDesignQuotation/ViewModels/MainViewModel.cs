using BindingLibrary;

namespace InteriorDesignQuotation.ViewModels;

public class MainViewModel : NotifyPropertyBase
{
    private Partner? _partner;
    private Quotation? _quotation;

    public Quotation Quotation
    {
        get => _quotation ??= new Quotation();
        set => SetProperty(ref _quotation, value);
    }

    public Partner Partner
    {
        get => _partner ??= new Partner();
        set => SetProperty(ref _partner, value);
    }
}