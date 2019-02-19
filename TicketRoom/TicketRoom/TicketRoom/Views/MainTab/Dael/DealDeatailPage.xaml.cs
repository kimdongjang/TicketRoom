using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketRoom.Views.Users.Login;
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
            #region IOS의 경우 초기화
            NavigationPage.SetHasNavigationBar(this, false); // Navigation Bar 지우는 코드 생성자에 입력
            if (Xamarin.Forms.Device.OS == TargetPlatform.iOS)
            {
                MainGrid.RowDefinitions[0].Height = 50;
            }
            #endregion
            Tab_Changed(PurchaseTab, null);
        }

        private async void Tab_Changed(object sender, EventArgs e)
        {
            PurchaseTab.TextColor = Color.Black;
            SaleTab.TextColor = Color.Black;

            PurchaseTab.FontSize = 14;
            SaleTab.FontSize = 14;

            Button selectedtab = (Button)sender;
            selectedtab.FontSize = 15;
            selectedtab.TextColor = Color.Blue;
            if (selectedtab.Text.Equals("상품권 구매"))
            {
                TabContent.Content = new PurchaseTabPage(categorynum);
            }
            else if (selectedtab.Text.Equals("상품권 판매"))
            {
                if (Global.b_user_login)
                {
                    TabContent.Content = new SaleTabPage(categorynum);
                }
                else
                {
                    await ShowMessage("로그인상태에서 이용할수 있습니다.", "알림", "OK", async () =>
                    {
                        //App.Current.MainPage = new MainPage();
                        Navigation.PushAsync(new LoginPage());
                    });
                }
            }
        }

        public async Task ShowMessage(string message, string title, string buttonText, Action afterHideCallback)
        {
            await DisplayAlert(title, message, buttonText);

            afterHideCallback?.Invoke();
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            this.OnBackButtonPressed();
        }
    }
}