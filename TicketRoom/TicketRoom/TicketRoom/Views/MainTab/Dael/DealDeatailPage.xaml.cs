using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.Dael
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DealDeatailPage : ContentPage
    {
        string categorynum;

        public DealDeatailPage(string categorynum)
        {
            InitializeComponent();
            this.categorynum = categorynum;
            Tab_Changed(PurchaseTab, null);
        }

        private void Tab_Changed(object sender, EventArgs e)
        {
            PurchaseTab.TextColor = Color.Black;
            SaleTab.TextColor = Color.Black;

            PurchaseTab.FontSize = 14;
            SaleTab.FontSize = 14;

            Button selectedtab = (Button)sender;
            selectedtab.FontSize = 15;
            selectedtab.TextColor = Color.Blue;
            if (selectedtab.Text.Equals("구매"))
            {
                TabContent.Content = new PurchaseTabPage(categorynum);
            }
            else if (selectedtab.Text.Equals("판매"))
            {
                TabContent.Content = new SaleTabPage(categorynum);
            }
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            this.OnBackButtonPressed();
        }
    }
}