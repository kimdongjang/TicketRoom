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
	public partial class DealDeatailView : ContentView
	{
        MainPage mainpage;
        string categorynum;
        public DealDeatailView(MainPage mainpage, string categorynum)
        {
            InitializeComponent();
            
            Global.isDealTabCliecked = true;
            this.mainpage = mainpage;
            this.categorynum = categorynum;
            #region IOS의 경우 초기화
            NavigationPage.SetHasNavigationBar(this, false); // Navigation Bar 지우는 코드 생성자에 입력
            if (Xamarin.Forms.Device.OS == TargetPlatform.iOS)
            {
                TabGrid.RowDefinitions[0].Height = 50;
            }
            #endregion

            if (Global.current_Tab_P_or_S.Equals("P"))
            {
                Tab_Changed(PurchaseTab, null);
            }
            else
            {
                Tab_Changed(SaleTab, null);
            }

        }

        private async void Tab_Changed(object sender, EventArgs e)
        {
            //구매 or 판매 리스트 클릭 가능상태
            Global.isgiftlistcliecked = true;

            PurchaseTab.BackgroundColor = Color.CornflowerBlue;
            PurchaseTab.TextColor = Color.White;
            SaleTab.BackgroundColor = Color.CornflowerBlue;
            SaleTab.TextColor = Color.White;


            Button selectedtab = (Button)sender;
            selectedtab.BackgroundColor = Color.White;
            selectedtab.TextColor = Color.CornflowerBlue;

            if (selectedtab.Text.Equals("상품권 구매"))
            {
                // 로딩 시작
                await Global.LoadingStartAsync();
                Global.current_Tab_P_or_S = "P";
                // 초기화 코드 작성
                TabContent.Content = new PurchaseTabPage(mainpage, categorynum);

                // 로딩 완료
                await Global.LoadingEndAsync();
            }
            else if (selectedtab.Text.Equals("상품권 판매"))
            {
                // 로딩 시작
                await Global.LoadingStartAsync();
                Global.current_Tab_P_or_S = "S";
                // 초기화 코드 작성
                if (Global.b_user_login)
                {
                    TabContent.Content = new SaleTabPage(categorynum);
                }
                else
                {
                    if (Global.isDealTabCliecked)
                    {
                        Global.isDealTabCliecked = false;
                        await mainpage.ShowMessage("로그인상태에서 이용할수 있습니다.", "알림", "OK", async () =>
                        {
                            //App.Current.MainPage = new MainPage();
                            Navigation.PushAsync(new LoginPage());
                        });
                    }
                }

                // 로딩 완료
                await Global.LoadingEndAsync();

            }
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            if (Global.isDealTabCliecked)
            {
                Global.isDealTabCliecked = false;
                mainpage.ShowDeal();
            }
        }
    }
}