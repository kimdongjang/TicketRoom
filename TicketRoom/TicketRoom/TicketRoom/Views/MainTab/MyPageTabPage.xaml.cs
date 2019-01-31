using System;
using TicketRoom.Views.MainTab.MyPage;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyPageTabPage : ContentView
    {
        public MyPageTabPage()
        {
            InitializeComponent();
        }

        private void MyInfoUpdate_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new MyInfoUpdatePage());
        }

        private void SaleList_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new SaleListPage());
        }

        private void PurchaseList_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new PurchaseListPage());
        }

        private void PointCheck_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new PointCheckPage());
        }
    }
}