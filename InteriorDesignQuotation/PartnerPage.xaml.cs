using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace InteriorDesignQuotation
{
    /// <summary>
    /// PartnerPage.xaml 的互動邏輯
    /// </summary>
    public partial class PartnerPage : Page
    {
        public PartnerPage()
        {
          InitializeComponent();

            //check file
            DirectoryInfo dir  = new DirectoryInfo (@".\DOCS\Partner.csv");
            if (!File.Exists(dir.FullName))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(dir.FullName));
                File.Create(dir.FullName);
            }
        }




    }
}
