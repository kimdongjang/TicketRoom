using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            TabColorChanged(0);

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
            AddImage.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(async () =>
                {
                    // 로딩 시작
                    await Global.LoadingStartAsync();

                    pal = new PointAddList(this, pp);
                    PointContentView.Content = pal;
                    TabColorChanged(0);
                    AddImage.Source = "point_addlist_h.png";
                    UsedImage.Source = "point_uselist_non.png";
                    ChargeImage.Source = "point_charge_non.png";
                    WidhdrawImage.Source = "point_withdraw_non.png";

                    // 로딩 완료
                    await Global.LoadingEndAsync();
                })
            });
            // 사용 내역 이벤트
            UsedImage.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(async () =>
                {
                    // 로딩 시작
                    await Global.LoadingStartAsync();

                    pul = new PointUsedList(this, pp);
                    PointContentView.Content = pul;
                    TabColorChanged(1);
                    AddImage.Source = "point_addlist_non.png";
                    UsedImage.Source = "point_uselist_h.png";
                    ChargeImage.Source = "point_charge_non.png";
                    WidhdrawImage.Source = "point_withdraw_non.png";

                    // 로딩 완료
                    await Global.LoadingEndAsync();

                })
            });
            // 포인트 충전 이벤트
            ChargeImage.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    pcv = new PointChargeView(this, pp);
                    PointContentView.Content = pcv;
                    TabColorChanged(2);
                    AddImage.Source = "point_addlist_non.png";
                    UsedImage.Source = "point_uselist_non.png";
                    ChargeImage.Source = "point_charge_h.png";
                    WidhdrawImage.Source = "point_withdraw_non.png";

                })
            });
            // 포인트 출금 이벤트
            WidhdrawImage.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    pwv = new PointWidhdrawView(this, pp);
                    PointContentView.Content = pwv;
                    TabColorChanged(3);
                    AddImage.Source = "point_addlist_non.png";
                    UsedImage.Source = "point_uselist_non.png";
                    ChargeImage.Source = "point_charge_non.png";
                    WidhdrawImage.Source = "point_withdraw_h.png";

                })
            });
        }

        private void TabColorChanged(int n)
        {
            for (int i = 0; i < ImageGrid.Children.Count; i++)
            {
                if (i == n)
                {
                    ImageGrid.Children[i].BackgroundColor = Color.White;
                }
                else
                {
                    ImageGrid.Children[i].BackgroundColor = Color.CornflowerBlue;
                }
            }
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