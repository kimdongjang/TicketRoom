using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketRoom.Models.PointData;
using TicketRoom.Views.MainTab.MyPage.Point;
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
            #endregion
            pp = PT_DB.PostSearchPointListToID(Global.ID); // 사용자 아이디로 아이디에 해당하는 포인트 테이블 가져옴

            pal = new PointAddList(this, pp);
            pul = new PointUsedList(this, pp);
            pcv = new PointChargeView(this, pp);
            pwv = new PointWidhdrawView(this, pp);

            init(pal);
        }

        public void init(ContentView cv)
        {
            PointContentView.Content = cv;

            // 적립 내역 이벤트
            AddImage.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    pal = new PointAddList(this, pp);
                    PointContentView.Content = pal;

                })
            });
            // 사용 내역 이벤트
            UsedImage.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    pul = new PointUsedList(this, pp);
                    PointContentView.Content = pul;

                })
            });
            // 포인트 충전 이벤트
            ChargeImage.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    pcv = new PointChargeView(this, pp);
                    PointContentView.Content = pcv;

                })
            });
            // 포인트 출금 이벤트
            WidhdrawImage.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    pwv = new PointWidhdrawView(this, pp);
                    PointContentView.Content = pwv;

                })
            });
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            PointCheckPage.isOpenPage = false;
            Navigation.PopAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            PointCheckPage.isOpenPage = false;
            return base.OnBackButtonPressed();

        }
    }
}