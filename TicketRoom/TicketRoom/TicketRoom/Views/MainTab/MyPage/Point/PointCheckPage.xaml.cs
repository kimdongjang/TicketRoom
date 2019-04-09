using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketRoom.Models.Custom;
using TicketRoom.Models.PointData;
using TicketRoom.Views.MainTab.MyPage.Point;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.MyPage.Point
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PointCheckPage : ContentPage
    {
        PointDBFunc PT_DB = PointDBFunc.Instance();

        public static bool isOpenPage = false;
        PointAddList pal;
        PointUsedList pul;
        PointChargeView pcv;
        PointWidhdrawView pwv;
        PT_Point pp;

        public PointCheckPage()
        {
            InitializeComponent();

            #region IOS의 경우 초기화
            NavigationPage.SetHasNavigationBar(this, false); // Navigation Bar 지우는 코드 생성자에 입력
            if (Xamarin.Forms.Device.OS == TargetPlatform.iOS)
            {
                MainGrid.RowDefinitions[0].Height = 50;
            }
            if (Global.ios_x_model == true) // ios X 이상의 모델일 경우
            {
                MainGrid.RowDefinitions[4].Height = 30;
            }
            #endregion

            LoadingInitAsync();
            NavigationInit();
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


        private async void LoadingInitAsync()
        {

            // 로딩 시작
            await Global.LoadingStartAsync();
            #region 네트워크 상태 확인
            var current_network = Connectivity.NetworkAccess; // 현재 네트워크 상태
            if (current_network != NetworkAccess.Internet) // 네트워크 연결 불가
            {
                await DisplayAlert("알림", "네트워크에 연결할 수 없습니다. 다시 한번 시도해주세요.", "확인");
                pp = null;
                // 로딩 완료
                await Global.LoadingEndAsync();
                return;
            }
            #endregion
            #region 네트워크 연결 가능
            else
            {
                pp = PT_DB.PostSearchPointListToID(Global.ID); // 사용자 아이디로 아이디에 해당하는 포인트 테이블 가져옴
            }
            #endregion

            pal = new PointAddList(this, pp);
            pul = new PointUsedList(this, pp);
            pcv = new PointChargeView(this, pp);
            pwv = new PointWidhdrawView(this, pp);

            init(pal);

            // 로딩 완료
            await Global.LoadingEndAsync();
        }
        

        public void init(ContentView cv)
        {
            PointContentView.Content = cv;

            // 적립 내역 이벤트
            AddGrid.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(async () =>
                {
                    // 로딩 시작
                    await Global.LoadingStartAsync();

                    pal = new PointAddList(this, pp);
                    PointContentView.Content = pal;

                    ((CustomLabel)AddGrid.Children[0]).TextColor = Color.CornflowerBlue;
                    ((BoxView)AddGrid.Children[1]).BackgroundColor = Color.CornflowerBlue;
                    ((CustomLabel)UsedGrid.Children[0]).TextColor = Color.Black;
                    ((BoxView)UsedGrid.Children[1]).BackgroundColor = Color.White;
                    ((CustomLabel)ChargeGrid.Children[0]).TextColor = Color.Black;
                    ((BoxView)ChargeGrid.Children[1]).BackgroundColor = Color.White;
                    ((CustomLabel)WidhdrawGrid.Children[0]).TextColor = Color.Black;
                    ((BoxView)WidhdrawGrid.Children[1]).BackgroundColor = Color.White;

                    // 로딩 완료
                    await Global.LoadingEndAsync();
                })
            });
            // 사용 내역 이벤트
            UsedGrid.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(async () =>
                {
                    // 로딩 시작
                    await Global.LoadingStartAsync();

                    pul = new PointUsedList(this, pp);
                    PointContentView.Content = pul;

                    ((CustomLabel)AddGrid.Children[0]).TextColor = Color.Black;
                    ((BoxView)AddGrid.Children[1]).BackgroundColor = Color.White;
                    ((CustomLabel)UsedGrid.Children[0]).TextColor = Color.CornflowerBlue;
                    ((BoxView)UsedGrid.Children[1]).BackgroundColor = Color.CornflowerBlue;
                    ((CustomLabel)ChargeGrid.Children[0]).TextColor = Color.Black;
                    ((BoxView)ChargeGrid.Children[1]).BackgroundColor = Color.White;
                    ((CustomLabel)WidhdrawGrid.Children[0]).TextColor = Color.Black;
                    ((BoxView)WidhdrawGrid.Children[1]).BackgroundColor = Color.White;

                    // 로딩 완료
                    await Global.LoadingEndAsync();

                })
            });
            // 포인트 충전 이벤트
            ChargeGrid.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    pcv = new PointChargeView(this, pp);
                    PointContentView.Content = pcv;

                    ((CustomLabel)AddGrid.Children[0]).TextColor = Color.Black;
                    ((BoxView)AddGrid.Children[1]).BackgroundColor = Color.White;
                    ((CustomLabel)UsedGrid.Children[0]).TextColor = Color.Black;
                    ((BoxView)UsedGrid.Children[1]).BackgroundColor = Color.White;
                    ((CustomLabel)ChargeGrid.Children[0]).TextColor = Color.CornflowerBlue;
                    ((BoxView)ChargeGrid.Children[1]).BackgroundColor = Color.CornflowerBlue;
                    ((CustomLabel)WidhdrawGrid.Children[0]).TextColor = Color.Black;
                    ((BoxView)WidhdrawGrid.Children[1]).BackgroundColor = Color.White;

                })
            });
            // 포인트 출금 이벤트
            WidhdrawGrid.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    pwv = new PointWidhdrawView(this, pp);
                    PointContentView.Content = pwv;
                    ((CustomLabel)AddGrid.Children[0]).TextColor = Color.Black;
                    ((BoxView)AddGrid.Children[1]).BackgroundColor = Color.White;
                    ((CustomLabel)UsedGrid.Children[0]).TextColor = Color.Black;
                    ((BoxView)UsedGrid.Children[1]).BackgroundColor = Color.White;
                    ((CustomLabel)ChargeGrid.Children[0]).TextColor = Color.Black;
                    ((BoxView)ChargeGrid.Children[1]).BackgroundColor = Color.White;
                    ((CustomLabel)WidhdrawGrid.Children[0]).TextColor = Color.CornflowerBlue;
                    ((BoxView)WidhdrawGrid.Children[1]).BackgroundColor = Color.CornflowerBlue;

                })
            });
        }


        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            Global.ismypagebtns_clicked = true;
            PointCheckPage.isOpenPage = false;
            Navigation.PopAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            Global.ismypagebtns_clicked = true;
            PointCheckPage.isOpenPage = false;
            return base.OnBackButtonPressed();

        }
    }
}