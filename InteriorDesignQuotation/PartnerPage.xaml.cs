using System.IO;
using System.Windows.Controls;

namespace InteriorDesignQuotation;

/// <summary>
///     PartnerPage.xaml 的互動邏輯
/// </summary>
public partial class PartnerPage : Page
{
    public PartnerPage()
    {
        InitializeComponent();

        //check file
        // var dir  = new DirectoryInfo (@".\DOCS\Partner.csv");
        // if (!File.Exists(dir.FullName))
        // {
        //     Directory.CreateDirectory(Path.GetDirectoryName(dir.FullName));
        //     File.Create(dir.FullName);
        // }
    }
}