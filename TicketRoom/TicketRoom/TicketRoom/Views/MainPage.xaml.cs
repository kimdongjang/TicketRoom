using System;
using System.Collections.Generic;
using TicketRoom.Views.MainTab;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        List<Button> tablist = new List<Button>();
        public MainPage()
        {
            InitializeComponent();
            tablist.Add(DealTab);
            tablist.Add(ShopTab);
            tablist.Add(BasketTab);
            tablist.Add(MyPageTab);
            Tab_Changed(tablist[0], null);
        }

        private void Tab_Changed(object sender, EventArgs e)
        {
            DealTab.TextColor = Color.Black;
            ShopTab.TextColor = Color.Black;
            BasketTab.TextColor = Color.Black;
            MyPageTab.TextColor = Color.Black;

            DealTab.FontSize = 14;
            ShopTab.FontSize = 14;
            BasketTab.FontSize = 14;
            MyPageTab.FontSize = 14;

            Button selectedtab = (Button)sender;
            selectedtab.FontSize = 15;
            selectedtab.TextColor = Color.Blue;
            if (selectedtab.Text.Equals("구매/판매"))
            {
                TabContent.Content = new DealTabPage();
            }
            else if (selectedtab.Text.Equals("쇼핑"))
            {
                TabContent.Content = new ShopTabPage();
            }
            else if (selectedtab.Text.Equals("장바구니"))
            {
                TabContent.Content = new BasketTabPage();
            }
            else if (selectedtab.Text.Equals("내정보"))
            {
                TabContent.Content = new MyPageTabPage();
            }
        }

        public void SetTabContent(Xamarin.Forms.View page)
        {
            TabContent.Content = page;
        }
    }
}