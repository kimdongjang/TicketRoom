using System;
using System.IO;
using TicketRoom.Models.PointData;
using TicketRoom.Views.MainTab.MyPage;
using TicketRoom.Views.MainTab.MyPage.Point;
using TicketRoom.Views.Users.Login;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyPageTabPage : ContentView
    {
        MainPage mp;
        public MyPageTabPage(MainPage mp)
        {
            InitializeComponent();
            Global.ismypagebtns_clicked = true; // 내정보 페이지 더블 클릭 제한 해제
            this.mp = mp;

            #region IOS의 경우 초기화
            if (Device.OS == TargetPlatform.iOS)
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
            Init();
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





        private void Init()
        {
            #region 네트워크 상태 확인
            var current_network = Connectivity.NetworkAccess; // 현재 네트워크 상태
            if (current_network == NetworkAccess.Internet) // 네트워크 연결 가능
            {
                if (Global.b_user_login == true)
                {   
                    UserIDLabel.Text = Global.ID;
                    UserPhoneLabel.Text = Global.user.PHONENUM;
                    UserPointLabel.Text = PointDBFunc.Instance().PostSearchPointListToID(Global.ID).PT_POINT_HAVEPOINT.ToString("N0");
                                        
                    #endregion
                    IsLoginBtn.Text = "로그아웃";
                }
                else if (Global.b_user_login == false)
                {
                    UserIDLabel.Text = "티켓룸아이디#" + Global.non_user_id;
                    UserPhoneLabel.Text = "";
                    UserPointLabel.Text = "";
                    IsLoginBtn.Text = "로그인";
                }
            }
            #region 네트워크 연결 불가
            else
            {
                Global.b_user_login = false;
                UserPointLabel.Text = "";
                UserPhoneLabel.Text = "";
                IsLoginBtn.Text = "로그인";
            }
            #endregion
            MyInfoGrid.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(async () =>
                {
                    if (Global.ismypagebtns_clicked)
                    {
                        Global.ismypagebtns_clicked = false;

                        if (Global.b_user_login == false)
                        {
                            await App.Current.MainPage.DisplayAlert("알림", "로그인 후에 이용이 가능합니다.", "확인");
                            Global.ismypagebtns_clicked = true;
                            return;
                        }
                        await Navigation.PushAsync(new MyInfoUpdatePage());
                    }
                })
            });

            SaleListGrid.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(async () =>
                {
                    if (Global.ismypagebtns_clicked)
                    {
                        Global.ismypagebtns_clicked = false;
                        if (Global.b_user_login == false)
                        {
                            await App.Current.MainPage.DisplayAlert("알림", "로그인 후에 이용이 가능합니다.", "확인");
                            Global.ismypagebtns_clicked = true;
                            return;
                        }
                        await Navigation.PushAsync(new SaleListPage());
                    }
                })
            });
            PurchaseListGrid.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    if (Global.ismypagebtns_clicked)
                    {
                        Global.ismypagebtns_clicked = false;
                        Navigation.PushAsync(new PurchaseListPage());
                    }
                })
            });

            PointGrid.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(async () =>
                {
                    if (Global.ismypagebtns_clicked)
                    {
                        Global.ismypagebtns_clicked = false;
                        if (Global.b_user_login == false)
                        {
                            await App.Current.MainPage.DisplayAlert("알림", "로그인 후에 이용이 가능합니다.", "확인");
                            Global.ismypagebtns_clicked = true;
                            return;
                        }
                        await Navigation.PushAsync(new PointCheckPage());
                    }
                })
            });
        }


        private async void IsLoginBtn_ClickedAsync(object sender, EventArgs e)
        {
            if (Global.ismypagebtns_clicked)
            {
                Global.ismypagebtns_clicked = false;
                if (Global.b_user_login == true) // 로그아웃 버튼
                {
                    if (await App.Current.MainPage.DisplayAlert("알림", "로그아웃 하시겠습니까?", "확인", "취소") == true)
                    {
                        Global.b_user_login = false;
                        Global.b_auto_login = false;
                        Global.ID = "";

                        // config파일 재설정
                        File.WriteAllText(Global.localPath + "app.config",
                            "NonUserID=" + Global.non_user_id + "\n" +
                            "IsLogin=" + Global.b_user_login.ToString() + "\n" + // 회원 로그인 false
                            "AutoLogin=" + Global.b_auto_login.ToString() + "\n" + // 자동 로그인 false
                            "UserID=" + Global.ID + "\n");

                        await App.Current.MainPage.DisplayAlert("알림", "성공적으로 로그아웃 되었습니다.", "확인");
                        Init();
                    }
                    Global.ismypagebtns_clicked = true;
                }
                else if (Global.b_user_login == false)
                {
                    Navigation.PushAsync(new LoginPage()); // 로그인 페이지로 이동
                }
            }
        }

        private void IsLoginBtn_Clicked(object sender, EventArgs e)
        {

        }
    }
}