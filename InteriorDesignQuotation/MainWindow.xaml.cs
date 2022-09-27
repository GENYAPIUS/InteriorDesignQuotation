using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace InteriorDesignQuotation
{
    /// <summary>
    /// MainPage.xaml 的互動邏輯
    /// </summary>
    public partial class MainPage : Window
    {

        PartnerPage partner;

        public MainPage()
        {
            InitializeComponent();
        }



        private void Button_PartnerPage_Click(object sender, RoutedEventArgs e)
        {
            if (partner == null)
            {
                partner = new PartnerPage();
            }
            this.PageFrame.Navigate(partner);
           
        }

        private void Button_Contract_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
