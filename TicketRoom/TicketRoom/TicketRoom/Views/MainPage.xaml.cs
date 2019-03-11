﻿using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TicketRoom.Models.Users;
using TicketRoom.Views.MainTab;
using TicketRoom.Views.MainTab.MyPage;
using TicketRoom.Views.MainTab.Popup;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading;
using TicketRoom.Models.Custom;
using TicketRoom.Views.MainTab.Dael;
using TicketRoom.Models.ShopData;

namespace TicketRoom.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        List<Button> tablist = new List<Button>();
        UserDBFunc USER_DB = UserDBFunc.Instance();
        ShopDBFunc SH_DB = ShopDBFunc.Instance();
        string categorynum = "";

        public MainPage()
        {
            InitializeComponent();

            #region IOS의 경우 초기화
            NavigationPage.SetHasNavigationBar(this, false); // Navigation Bar 지우는 코드 생성자에 입력
            if (Xamarin.Forms.Device.OS == TargetPlatform.iOS)
            {
                MainGrid.RowDefinitions[0].Height = 50;
            }
            #endregion

            TabContent.Content = new DealTabPage(this);
        }

        protected override void OnAppearing() // PopAsync 호출 또는 페이지 초기화때 시동
        {
            Init();
            TabInit();
            ChangeTabInitAsync();





            base.OnAppearing();
        }

        private async void ChangeTabInitAsync()
        {
            // 로딩 시작
            await Global.LoadingStartAsync();

            DealTabPage dtp;
            ShopTabPage stp;
            BasketTabPage btp;
            MyPageTabPage mtp;

            #region OnAppearing을 사용해 사용중인 탭으로 되돌리기
            if (Global.isMainDeal == true)
            {
                //Tab_Changed(tablist[0], null);
                dtp = new DealTabPage(this);
                TabColorChanged("deal");
                Global.InitOnAppearingBool("deal");
                TabContent.Content = dtp;
            }
            else if (Global.isMainShop == true)
            {
                //Tab_Changed(tablist[1], null);
                stp = new ShopTabPage();
                TabColorChanged("shop");
                Global.InitOnAppearingBool("shop");
                TabContent.Content = stp;
            }
            else if (Global.isMainBasket == true)
            {
                //Tab_Changed(tablist[2], null);
                btp = new BasketTabPage();
                TabColorChanged("basket");
                Global.InitOnAppearingBool("basket");
                TabContent.Content = btp;
            }
            else if (Global.isMainMyinfo == true)
            {
                //Tab_Changed(tablist[3], null);
                mtp = new MyPageTabPage(this);
                TabColorChanged("myinfo");
                Global.InitOnAppearingBool("myinfo");
                TabContent.Content = mtp;
            }
            else if (Global.isMainDealDeatil == true)
            {
                ShowDealDetail(this.categorynum);
            }
            #endregion

            if (Global.b_user_login == true) // 회원인 상태로 로그인이 되어있다면
            {
                SH_DB.PostInsertRecentViewToID(Global.ID); // 최근 본 상품 로우 생성
            }
            else // 비회원 상태
            {
                SH_DB.PostInsertRecentViewToID(Global.non_user_id);
            }

            // 로딩 완료
            await Global.LoadingEndAsync();
        }


        private void Init()
        {
            try
            {
                if (File.Exists(Global.localPath + "app.config") == false) // 앱 설정 파일이 없다면 생성
                {
                    Global.non_user_id = USER_DB.PostInsertNonUsersID();
                    // db에서 비회원 아이디를 가져옴. 중복 검사 필요.
                    File.WriteAllText(Global.localPath + "app.config",
                        "NonUserID=" + Global.non_user_id + "\n" +
                        "IsLogin=" + Global.b_user_login.ToString() + "\n" + // 회원 로그인 false
                        "AutoLogin=" + Global.b_auto_login.ToString() + "\n" + // 자동 로그인 false
                        "UserID=" + Global.ID + "\n"); // 회원 아이디(지금은 잠시 아이디로 대체함)
                }
                else
                {
                    // config 파일 정보를 읽어옴
                    string text = File.ReadAllText(Global.localPath + "app.config");
                    string[] temp = text.Split('\n');
                    int textPoint = temp[0].IndexOf("NonUserID="); // 0행 째의 라인
                    Global.non_user_id = temp[0].Substring(textPoint).Replace("NonUserID=", "");
                    textPoint = temp[1].IndexOf("IsLogin="); // 1행 째의 라인
                    Global.b_user_login = IsBoolCheckFunc(temp[1].Substring(textPoint).Replace("IsLogin=", ""));
                    textPoint = temp[2].IndexOf("AutoLogin="); // 2행 째의 라인
                    Global.b_auto_login = IsBoolCheckFunc(temp[2].Substring(textPoint).Replace("AutoLogin=", ""));
                    if (Global.b_auto_login == true) // 자동 로그인이 되어있다면
                    {
                        textPoint = temp[3].IndexOf("UserID="); // 3행 째의 라인
                        Global.ID = temp[3].Substring(textPoint).Replace("UserID=", "");
                    }
                    else
                    {
                        Global.ID = "";
                    }
                }
            }
            catch
            {

            }
            Global.user = USER_DB.PostSelectUserToID(Global.ID);
            Global.adress = USER_DB.PostSelectAdressToID(Global.ID);

        }

        private bool IsBoolCheckFunc(string s)
        {
            if(s == "True") return true;
            else return false;
        }

        public static void ConfigUpdateIsLogin()
        {
            File.WriteAllText(Global.localPath + "app.config",
                "NonUserID=" + Global.non_user_id + "\n" +
                "IsLogin=" + Global.b_user_login.ToString() + "\n" + // 회원 로그인 false
                "AutoLogin=" + Global.b_auto_login.ToString() + "\n" + // 자동 로그인 false
                "UserID=" + Global.ID + "\n"); // 회원 아이디(지금은 잠시 아이디로 대체함)
        }

        private void TabColorChanged(string s)
        {
            if (s == "deal")
            {
                ((Image)GiftTab.Children[0]).Source = "main_gift_h.png";
                ((CustomLabel)GiftTab.Children[1]).TextColor = Color.CornflowerBlue;

                ((Image)ShopTab.Children[0]).Source = "main_shop_non.png";
                ((CustomLabel)ShopTab.Children[1]).TextColor = Color.Black;

                ((Image)BasketTab.Children[0]).Source = "main_basket_non.png";
                ((CustomLabel)BasketTab.Children[1]).TextColor = Color.Black;

                ((Image)UserTab.Children[0]).Source = "main_user_non.png";
                ((CustomLabel)UserTab.Children[1]).TextColor = Color.Black;
            }
            else if(s == "shop")
            {

                ((Image)GiftTab.Children[0]).Source = "main_gift_non.png";
                ((CustomLabel)GiftTab.Children[1]).TextColor = Color.Black;

                ((Image)ShopTab.Children[0]).Source = "main_shop_h.png";
                ((CustomLabel)ShopTab.Children[1]).TextColor = Color.CornflowerBlue;

                ((Image)BasketTab.Children[0]).Source = "main_basket_non.png";
                ((CustomLabel)BasketTab.Children[1]).TextColor = Color.Black;

                ((Image)UserTab.Children[0]).Source = "main_user_non.png";
                ((CustomLabel)UserTab.Children[1]).TextColor = Color.Black;
            }
            else if (s == "basket")
            {

                ((Image)GiftTab.Children[0]).Source = "main_gift_non.png";
                ((CustomLabel)GiftTab.Children[1]).TextColor = Color.Black;

                ((Image)ShopTab.Children[0]).Source = "main_shop_non.png";
                ((CustomLabel)ShopTab.Children[1]).TextColor = Color.Black;

                ((Image)BasketTab.Children[0]).Source = "main_basket_h.png";
                ((CustomLabel)BasketTab.Children[1]).TextColor = Color.CornflowerBlue;

                ((Image)UserTab.Children[0]).Source = "main_user_non.png";
                ((CustomLabel)UserTab.Children[1]).TextColor = Color.Black;
            }
            else if (s == "myinfo")
            {

                ((Image)GiftTab.Children[0]).Source = "main_gift_non.png";
                ((CustomLabel)GiftTab.Children[1]).TextColor = Color.Black;

                ((Image)ShopTab.Children[0]).Source = "main_shop_non.png";
                ((CustomLabel)ShopTab.Children[1]).TextColor = Color.Black;

                ((Image)BasketTab.Children[0]).Source = "main_basket_non.png";
                ((CustomLabel)BasketTab.Children[1]).TextColor = Color.Black;

                ((Image)UserTab.Children[0]).Source = "main_user_h.png";
                ((CustomLabel)UserTab.Children[1]).TextColor = Color.CornflowerBlue;
            }
        }

        private void TabInit()
        {

            GiftTab.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(async () =>
                {
                    // 로딩 시작
                    await Global.LoadingStartAsync();

                    TabContent.Content = new DealTabPage(this);
                    Global.InitOnAppearingBool("deal");

                    TabColorChanged("deal");

                    // 로딩 완료
                    await Global.LoadingEndAsync();
                })
            });

            ShopTab.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(async () =>
                {
                    // 로딩 시작
                    await Global.LoadingStartAsync();

                    TabContent.Content = new ShopTabPage();
                    Global.InitOnAppearingBool("shop");

                    TabColorChanged("shop");

                    // 로딩 완료
                    await Global.LoadingEndAsync();
                })
            });

            BasketTab.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(async () =>
                {
                    // 로딩 시작
                    await Global.LoadingStartAsync();

                    TabContent.Content = new BasketTabPage();
                    Global.InitOnAppearingBool("basket");

                    TabColorChanged("basket");

                    // 로딩 완료
                    await Global.LoadingEndAsync();

                })
            });


            UserTab.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(async () =>
                {
                    // 로딩 시작
                    await Global.LoadingStartAsync();

                    TabContent.Content = new MyPageTabPage(this);
                    Global.InitOnAppearingBool("myinfo");

                    TabColorChanged("myinfo");

                    // 로딩 완료
                    await Global.LoadingEndAsync();
                })
            });
        }


        public void ShowDealDetail(string categorynum)
        {
            this.categorynum = categorynum;
            TabContent.Content = new DealDeatailView(this, categorynum);
            Global.InitOnAppearingBool("dealdetail");
        }

        public void ShowDeal()
        {
            TabContent.Content = new DealTabPage(this);
            Global.InitOnAppearingBool("deal");
        }
        public void SetTabContent(Xamarin.Forms.View page)
        {
            TabContent.Content = page;
        }

        protected override bool OnBackButtonPressed()
        {
            if (Global.isMainDealDeatil == true)
            {
                ShowDeal();
                return true;
            }
            Device.BeginInvokeOnMainThread(async () =>
            {
                var result = await this.DisplayAlert("알림", "정말로 앱을 종료하시겠습니까?", "확인", "취소");
                if (result)
                {
                    var closer = DependencyService.Get<ICloseApplication>();
                    if (closer != null)
                        closer.closeApplication();
                }
            });
            return true;
        }

        public async Task ShowMessage(string message, string title, string buttonText, Action afterHideCallback)
        {
            await DisplayAlert(title, message, buttonText);

            afterHideCallback?.Invoke();
        }
    }
}