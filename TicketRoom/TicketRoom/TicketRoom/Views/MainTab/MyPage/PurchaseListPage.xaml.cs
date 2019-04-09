using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketRoom.Models.Custom;
using TicketRoom.Models.ShopData;
using TicketRoom.Views.MainTab.MyPage.PurchaseList;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.MyPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PurchaseListPage : ContentPage
    {
        public static bool isOpenPage = false;
        ShopDBFunc SH_DB = ShopDBFunc.Instance();
        PurchaseListGift plg;
        PurchaseListShop pls;

        public PurchaseListPage()
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
                MainGrid.RowDefinitions[5].Height = 30;
            }
            #endregion

            Init();
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

        public void Init()
        {
            plg = new PurchaseListGift(this);
            PurchaseListContentView.Content = plg;

            TapColorChangeAsync(plg);

            #region 상품권 탭 클릭 이벤트
            // 상품권 탭을 선택할 경우 상품권 컨텐츠 뷰를 보여줌
            TapGiftGrid.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    plg = new PurchaseListGift(this);
                    PurchaseListContentView.Content = plg;

                    TapColorChangeAsync(plg);

                })
            });
            // 쇼팡몰 탭을 선택할 경우 쇼팡몰 컨텐츠 뷰를 보여줌
            TapShopingGrid.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    DisplayAlert("알림", "준비 중입니다!", "확인");
                    return;

                    //

                    pls = new PurchaseListShop(this);
                    PurchaseListContentView.Content = pls;
                    TapColorChangeAsync(pls);
                })
            });
            #endregion

            #region 전체 목록 보기 클릭 이벤트
            ListAllGrid.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(async () =>
                {
                    // 로딩 시작
                    await Global.LoadingStartAsync();

                    TabListColorChange(0);

                    if (PurchaseListContentView.Content == plg)
                    {
                        if (Global.b_user_login)
                        {
                            plg.PostSearchPurchaseListToIDAsync(Global.ID, -99, 0, 0);
                            await plg.Init();
                        }
                        else
                        {
                            plg.PostSearchPurchaseListToIDAsync(Global.ID, -99, 0, 0);
                            await plg.Init();
                        }
                    }
                    else // 쇼핑몰 전체 목록
                    {
                        if (Global.b_user_login) // 로그인 상태인 경우
                        {
                            pls.purchaseList = SH_DB.PostSearchPurchaseListToID(Global.ID, -99, 0, 0); // 사용자 아이디로 구매 목록 가져옴
                            pls.Init();
                        }
                        else
                        {
                            pls.purchaseList = SH_DB.PostSearchPurchaseListToID(Global.non_user_id, -99, 0, 0); // 사용자 아이디로 구매 목록 가져옴
                            pls.Init();
                        }
                    }
                    ((CustomLabel)ListAllGrid.Children[0]).TextColor = Color.CornflowerBlue;
                    ((BoxView)ListAllGrid.Children[1]).BackgroundColor = Color.CornflowerBlue;
                    ((CustomLabel)ListYearGrid.Children[0]).TextColor = Color.Black;
                    ((BoxView)ListYearGrid.Children[1]).BackgroundColor = Color.White;
                    ((CustomLabel)ListMonthGrid.Children[0]).TextColor = Color.Black;
                    ((BoxView)ListMonthGrid.Children[1]).BackgroundColor = Color.White;
                    ((CustomLabel)ListDayGrid.Children[0]).TextColor = Color.Black;
                    ((BoxView)ListDayGrid.Children[1]).BackgroundColor = Color.White;
                    // 로딩 시작
                    await Global.LoadingEndAsync();
                })
            });
            #endregion

            #region 일주일 목록 보기 클릭 이벤트
            ListYearGrid.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(async () =>
                {
                    // 로딩 시작
                    await Global.LoadingStartAsync();

                    TabListColorChange(1);
                    if (PurchaseListContentView.Content == plg)
                    {
                        if (Global.b_user_login)
                        {
                            plg.PostSearchPurchaseListToIDAsync(Global.ID, -1, 0, 0);
                            await plg.Init();
                        }
                        else
                        {
                            plg.PostSearchPurchaseListToIDAsync(Global.ID, -1, 0, 0);
                            await plg.Init();
                        }
                    }
                    else // 쇼핑몰 일주일 단위 목록
                    {
                        if (Global.b_user_login) // 로그인 상태인 경우
                        {
                            pls.purchaseList = SH_DB.PostSearchPurchaseListToID(Global.ID, -1, 0, 0); // 사용자 아이디로 구매 목록 가져옴
                            pls.Init();
                        }
                        else
                        {
                            pls.purchaseList = SH_DB.PostSearchPurchaseListToID(Global.non_user_id, -1, 0, 0); // 사용자 아이디로 구매 목록 가져옴
                            pls.Init();
                        }
                    }
                    ((CustomLabel)ListAllGrid.Children[0]).TextColor = Color.Black;
                    ((BoxView)ListAllGrid.Children[1]).BackgroundColor = Color.White;
                    ((CustomLabel)ListYearGrid.Children[0]).TextColor = Color.CornflowerBlue;
                    ((BoxView)ListYearGrid.Children[1]).BackgroundColor = Color.CornflowerBlue;
                    ((CustomLabel)ListMonthGrid.Children[0]).TextColor = Color.Black;
                    ((BoxView)ListMonthGrid.Children[1]).BackgroundColor = Color.White;
                    ((CustomLabel)ListDayGrid.Children[0]).TextColor = Color.Black;
                    ((BoxView)ListDayGrid.Children[1]).BackgroundColor = Color.White;


                    // 로딩 시작
                    await Global.LoadingEndAsync();
                })
            });
            #endregion

            #region 달 목록 보기 클릭 이벤트
            ListMonthGrid.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(async () =>
                {
                    // 로딩 시작
                    await Global.LoadingStartAsync();

                    TabListColorChange(2);
                    if (PurchaseListContentView.Content == plg)
                    {
                        if (Global.b_user_login)
                        {
                            plg.PostSearchPurchaseListToIDAsync(Global.ID, 0, -1, 0);
                            plg.Init();
                        }
                        else
                        {
                            plg.PostSearchPurchaseListToIDAsync(Global.ID, 0, -1, 0);
                            plg.Init();
                        }
                    }
                    else // 쇼핑몰 월 단위 목록
                    {
                        if (Global.b_user_login) // 로그인 상태인 경우
                        {
                            pls.purchaseList = SH_DB.PostSearchPurchaseListToID(Global.ID, 0, -1, 0); // 사용자 아이디로 구매 목록 가져옴
                            pls.Init();
                        }
                        else
                        {
                            pls.purchaseList = SH_DB.PostSearchPurchaseListToID(Global.non_user_id, 0, -1, 0); // 사용자 아이디로 구매 목록 가져옴
                            pls.Init();
                        }
                    }
                    ((CustomLabel)ListAllGrid.Children[0]).TextColor = Color.Black;
                    ((BoxView)ListAllGrid.Children[1]).BackgroundColor = Color.White;
                    ((CustomLabel)ListYearGrid.Children[0]).TextColor = Color.Black;
                    ((BoxView)ListYearGrid.Children[1]).BackgroundColor = Color.White;
                    ((CustomLabel)ListMonthGrid.Children[0]).TextColor = Color.CornflowerBlue;
                    ((BoxView)ListMonthGrid.Children[1]).BackgroundColor = Color.CornflowerBlue;
                    ((CustomLabel)ListDayGrid.Children[0]).TextColor = Color.Black;
                    ((BoxView)ListDayGrid.Children[1]).BackgroundColor = Color.White;


                    // 로딩 시작
                    await Global.LoadingEndAsync();
                })
            });
            #endregion

            #region 년 목록 보기 클릭 이벤트
            ListDayGrid.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(async () =>
                {
                    // 로딩 시작
                    await Global.LoadingStartAsync();


                    TabListColorChange(3);
                    if (PurchaseListContentView.Content == plg)
                    {
                        if (Global.b_user_login)
                        {
                            plg.PostSearchPurchaseListToIDAsync(Global.ID, 0, 0, -7);
                            await plg.Init();
                        }
                        else
                        {
                            plg.PostSearchPurchaseListToIDAsync(Global.ID, 0, 0, -7);
                            await plg.Init();
                        }
                    }
                    else // 쇼핑몰 1년 단위 목록
                    {
                        if (Global.b_user_login) // 로그인 상태인 경우
                        {
                            pls.purchaseList = SH_DB.PostSearchPurchaseListToID(Global.ID, 0, 0, -7); // 사용자 아이디로 구매 목록 가져옴
                            pls.Init();
                        }
                        else
                        {
                            pls.purchaseList = SH_DB.PostSearchPurchaseListToID(Global.non_user_id, 0, 0, -7); // 사용자 아이디로 구매 목록 가져옴
                            pls.Init();
                        }
                    }
                    ((CustomLabel)ListAllGrid.Children[0]).TextColor = Color.Black;
                    ((BoxView)ListAllGrid.Children[1]).BackgroundColor = Color.White;
                    ((CustomLabel)ListYearGrid.Children[0]).TextColor = Color.Black;
                    ((BoxView)ListYearGrid.Children[1]).BackgroundColor = Color.White;
                    ((CustomLabel)ListMonthGrid.Children[0]).TextColor = Color.Black;
                    ((BoxView)ListMonthGrid.Children[1]).BackgroundColor = Color.White;
                    ((CustomLabel)ListDayGrid.Children[0]).TextColor = Color.CornflowerBlue;
                    ((BoxView)ListDayGrid.Children[1]).BackgroundColor = Color.CornflowerBlue;


                    // 로딩 시작
                    await Global.LoadingEndAsync();
                })
            });
            #endregion
        }

        private async Task TapColorChangeAsync(ContentView cv)
        {
            await Global.LoadingStartAsync();

            if (cv == plg) // 상품권이 선택되었을 경우
            {
                ShopSelectImage.Source = "main_shop_non.png";
                TapShopingGridLabel.TextColor = Color.Black;
                GiftSelectImage.Source = "main_gift_h.png";
                TapGiftGridLabel.TextColor = Color.CornflowerBlue;

                TabListColorChange(0);

                if (Global.b_user_login)
                {
                    plg.PostSearchPurchaseListToIDAsync(Global.ID, -99, 0, 0);
                    await plg.Init();
                }
                else
                {
                    plg.PostSearchPurchaseListToIDAsync(Global.non_user_id, -99, 0, 0);
                    await plg.Init();
                }
            }
            else // 쇼핑몰이 선택 되었을 경우
            {
                ShopSelectImage.Source = "main_shop_h.png";
                TapShopingGridLabel.TextColor = Color.CornflowerBlue;
                GiftSelectImage.Source = "main_gift_non.png";
                TapGiftGridLabel.TextColor = Color.Black;

                TabListColorChange(0);

                if (Global.b_user_login) // 로그인 상태인 경우
                {
                    pls.purchaseList = SH_DB.PostSearchPurchaseListToID(Global.ID, -99, 0, 0); // 사용자 아이디로 구매 목록 가져옴
                    pls.Init();
                }
                else
                {
                    pls.purchaseList = SH_DB.PostSearchPurchaseListToID(Global.non_user_id, -99, 0, 0); // 사용자 아이디로 구매 목록 가져옴
                    pls.Init();
                }
            }
            ((CustomLabel)ListAllGrid.Children[0]).TextColor = Color.CornflowerBlue;
            ((BoxView)ListAllGrid.Children[1]).BackgroundColor = Color.CornflowerBlue;
            ((CustomLabel)ListYearGrid.Children[0]).TextColor = Color.Black;
            ((BoxView)ListYearGrid.Children[1]).BackgroundColor = Color.White;
            ((CustomLabel)ListMonthGrid.Children[0]).TextColor = Color.Black;
            ((BoxView)ListMonthGrid.Children[1]).BackgroundColor = Color.White;
            ((CustomLabel)ListDayGrid.Children[0]).TextColor = Color.Black;
            ((BoxView)ListDayGrid.Children[1]).BackgroundColor = Color.White;

            // 로딩완료
            await Global.LoadingEndAsync();
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            Global.ismypagebtns_clicked = true;
            Navigation.PopAsync();
        }

        // 이미지 클릭시 색상 변경
        private void TabListColorChange(int n)
        {
            for(int i = 0; i<ImageGrid.Children.Count; i++)
            {
                if(i == n)
                {
                    ImageGrid.Children[i].BackgroundColor = Color.White;
                }
                else
                {
                    ImageGrid.Children[i].BackgroundColor = Color.White;
                }
            }
        }

        protected override bool OnBackButtonPressed()
        {
            Global.ismypagebtns_clicked = true;
            return base.OnBackButtonPressed();
        }
    }
}