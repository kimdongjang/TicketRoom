using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            #endregion

            plg = new PurchaseListGift(this);
            Init(plg);
        }

        public void Init(ContentView cv)
        {
            PurchaseListContentView.Content = cv;

            TapColorChangeAsync(cv);

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

                    pls = new PurchaseListShop(this);
                    PurchaseListContentView.Content = pls;
                    TapColorChangeAsync(pls);
                })
            });
            #endregion

            #region 전체 목록 보기 클릭 이벤트
            list_all_image.GestureRecognizers.Add(new TapGestureRecognizer()
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
                            plg.PostSearchPurchaseListToID(Global.ID, -99, 0, 0);
                            plg.Init();
                        }
                        else
                        {
                            plg.PostSearchPurchaseListToID(Global.ID, -99, 0, 0);
                            plg.Init();
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
                    ((Image)ImageGrid.Children[0]).Source = "list_all_h.png";
                    ((Image)ImageGrid.Children[1]).Source = "list_week_non.png";
                    ((Image)ImageGrid.Children[2]).Source = "list_month_non.png";
                    ((Image)ImageGrid.Children[3]).Source = "list_year_non.png";

                })
            });
            #endregion

            #region 일주일 목록 보기 클릭 이벤트
            list_week_image.GestureRecognizers.Add(new TapGestureRecognizer()
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
                            plg.PostSearchPurchaseListToID(Global.ID, 0, 0, -7);
                            plg.Init();
                        }
                        else
                        {
                            plg.PostSearchPurchaseListToID(Global.ID, 0, 0, -7);
                            plg.Init();
                        }
                    }
                    else // 쇼핑몰 일주일 단위 목록
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
                    ((Image)ImageGrid.Children[0]).Source = "list_all_non.png";
                    ((Image)ImageGrid.Children[1]).Source = "list_week_h.png";
                    ((Image)ImageGrid.Children[2]).Source = "list_month_non.png";
                    ((Image)ImageGrid.Children[3]).Source = "list_year_non.png";
                })
            });
            #endregion

            #region 달 목록 보기 클릭 이벤트
            list_month_image.GestureRecognizers.Add(new TapGestureRecognizer()
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
                            plg.PostSearchPurchaseListToID(Global.ID, 0, -1, 0);
                            plg.Init();
                        }
                        else
                        {
                            plg.PostSearchPurchaseListToID(Global.ID, 0, -1, 0);
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
                    ((Image)ImageGrid.Children[0]).Source = "list_all_non.png";
                    ((Image)ImageGrid.Children[1]).Source = "list_week_non.png";
                    ((Image)ImageGrid.Children[2]).Source = "list_month_h.png";
                    ((Image)ImageGrid.Children[3]).Source = "list_year_non.png";
                })
            });
            #endregion

            #region 년 목록 보기 클릭 이벤트
            list_year_image.GestureRecognizers.Add(new TapGestureRecognizer()
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
                            plg.PostSearchPurchaseListToID(Global.ID, -1, 0, 0);
                            plg.Init();
                        }
                        else
                        {
                            plg.PostSearchPurchaseListToID(Global.ID, -1, 0, 0);
                            plg.Init();
                        }
                    }
                    else // 쇼핑몰 1년 단위 목록
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
                    ((Image)ImageGrid.Children[0]).Source = "list_all_non.png";
                    ((Image)ImageGrid.Children[1]).Source = "list_week_non.png";
                    ((Image)ImageGrid.Children[2]).Source = "list_month_non.png";
                    ((Image)ImageGrid.Children[3]).Source = "list_year_h.png";
                })
            });
            #endregion
        }

        private async Task TapColorChangeAsync(ContentView cv)
        {
            await Global.LoadingStartAsync();

            if (cv == plg) // 상품권이 선택되었을 경우
            {
                TapShopingGridLabel.TextColor = Color.White;
                TapShopingGrid.BackgroundColor = Color.CornflowerBlue;

                TapGiftGridLabel.TextColor = Color.CornflowerBlue;
                TapGiftGrid.BackgroundColor = Color.White;
                TabListColorChange(0);

                if (Global.b_user_login)
                {
                    plg.PostSearchPurchaseListToID(Global.ID, -99, 0, 0);
                    await plg.Init();
                }
                else
                {
                    plg.PostSearchPurchaseListToID(Global.non_user_id, -99, 0, 0);
                    await plg.Init();
                }
            }
            else // 쇼핑몰이 선택 되었을 경우
            {

                TapShopingGridLabel.TextColor = Color.CornflowerBlue;
                TapShopingGrid.BackgroundColor = Color.White;

                TapGiftGridLabel.TextColor = Color.White;
                TapGiftGrid.BackgroundColor = Color.CornflowerBlue;
                TabListColorChange(0);

                if (Global.b_user_login) // 로그인 상태인 경우
                {
                    pls.purchaseList = SH_DB.PostSearchPurchaseListToID(Global.ID, -99, 0, 0); // 사용자 아이디로 구매 목록 가져옴
                    await pls.Init();
                }
                else
                {
                    pls.purchaseList = SH_DB.PostSearchPurchaseListToID(Global.non_user_id, -99, 0, 0); // 사용자 아이디로 구매 목록 가져옴
                    await pls.Init();
                }
            }
            ((Image)ImageGrid.Children[0]).Source = "list_all_h.png";
            ((Image)ImageGrid.Children[1]).Source = "list_week_non.png";
            ((Image)ImageGrid.Children[2]).Source = "list_month_non.png";
            ((Image)ImageGrid.Children[3]).Source = "list_year_non.png";

        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
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
                    ImageGrid.Children[i].BackgroundColor = Color.CornflowerBlue;
                }
            }
        }
    }
}