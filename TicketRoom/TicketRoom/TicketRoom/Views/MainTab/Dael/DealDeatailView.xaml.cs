using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketRoom.Views.MainTab.MyPage;
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
            this.categorynum = Global.deal_select_category_num;
            #region IOS의 경우 초기화
            NavigationPage.SetHasNavigationBar(this, false); // Navigation Bar 지우는 코드 생성자에 입력
            if (Xamarin.Forms.Device.OS == TargetPlatform.iOS)
            {
                TabGrid.RowDefinitions[0].Height = 50;
            }
            #endregion

            LoadingInitAsync();

        }
        private async void LoadingInitAsync()
        {
            // 로딩 시작
            await Global.LoadingStartAsync();


            if (Global.deal_select_category_value == "구매")
            {
                PurchaseTab.TextColor = Color.CornflowerBlue;
                PurchaseLine.BackgroundColor = Color.CornflowerBlue;
                SaleTab.TextColor = Color.Black;
                SaleLine.BackgroundColor = Color.White;
                TabContent.Content = new PurchaseTabPage(mainpage, categorynum);
            }
            else if (Global.deal_select_category_value == "판매")
            {
                PurchaseTab.TextColor = Color.Black;
                PurchaseLine.BackgroundColor = Color.White;
                SaleTab.TextColor = Color.CornflowerBlue;
                SaleLine.BackgroundColor = Color.CornflowerBlue;
                TabContent.Content = new SaleTabPage(categorynum);
            }

            MainTabClick();
            NavigationInit();

            // 로딩 완료
            await Global.LoadingEndAsync();
        }

        private void NavigationInit()
        {
            NavigationButton.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(async () =>
                {
                    // 로딩 시작
                    await Global.LoadingStartAsync();

                    await Navigation.PushAsync(new NavagationPage());

                    // 로딩 완료
                    await Global.LoadingEndAsync();
                })
            });
        }
        // 상위 탭 클릭
        private void MainTabClick()
        {
            PurchaseTab.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(async () =>
                {
                    //구매 or 판매 리스트 클릭 가능상태
                    Global.isgiftlistcliecked = true;

                    // 구매 탭 선택
                    Global.deal_select_category_value = "구매";

                    PurchaseTab.TextColor = Color.CornflowerBlue;
                    PurchaseLine.BackgroundColor= Color.CornflowerBlue;
                    SaleTab.TextColor = Color.Black;
                    SaleLine.BackgroundColor = Color.White;
                    // 로딩 시작
                    await Global.LoadingStartAsync();

                    TabContent.Content = new PurchaseTabPage(mainpage, Global.deal_select_category_num);

                    // 로딩 완료
                    await Global.LoadingEndAsync();
                })
            });
            SaleTab.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(async () =>
                {
                    //구매 or 판매 리스트 클릭 가능상태
                    Global.isgiftlistcliecked = true;

                    // 판매 탭 선택
                    Global.deal_select_category_value = "판매";

                    PurchaseTab.TextColor = Color.Black;
                    PurchaseLine.BackgroundColor = Color.White;
                    SaleTab.TextColor = Color.CornflowerBlue;
                    SaleLine.BackgroundColor = Color.CornflowerBlue;

                    // 로딩 시작
                    await Global.LoadingStartAsync();

                    // 초기화 코드 작성
                    if (Global.b_user_login)
                    {
                        TabContent.Content = new SaleTabPage(Global.deal_select_category_num);
                    }
                    else
                    {
                        if (Global.isDealTabCliecked)
                        {
                            Global.isDealTabCliecked = false;
                            await mainpage.ShowMessage("로그인상태에서 이용할수 있습니다.", "알림", "OK", async () =>
                            {
                                //App.Current.MainPage = new MainPage();
                                await Navigation.PushAsync(new LoginPage());
                            });
                        }
                    }

                    // 로딩 완료
                    await Global.LoadingEndAsync();
                })
            });
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