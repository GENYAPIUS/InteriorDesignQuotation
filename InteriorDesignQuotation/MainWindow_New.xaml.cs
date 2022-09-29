using System.Windows;

namespace InteriorDesignQuotation;

/// <summary>
///     MainPage.xaml 的互動邏輯
/// </summary>
public partial class MainPage : Window
{
    private PartnerPage partner;

    public MainPage()
    {
        InitializeComponent();
    }

    // private void Button_PartnerPage_Click(object sender, RoutedEventArgs e)
    // {
    //     if (partner == null)
    //     {
    //         partner = new PartnerPage();
    //     }
    //     this.PageFrame.Navigate(partner);
    //    
    // }
    //
    // private void Button_Contract_Click(object sender, RoutedEventArgs e)
    // {
    //
    // }
}