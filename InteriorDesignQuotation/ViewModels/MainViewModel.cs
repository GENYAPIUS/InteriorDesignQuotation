using BindingLibrary;

namespace InteriorDesignQuotation.ViewModels;

public class MainViewModel : NotifyPropertyBase
{
    private Quotation? _quotation;

    public Quotation Quotation
    {
        get => _quotation ??= new Quotation();
        set => SetProperty(ref _quotation, value);
    }
}