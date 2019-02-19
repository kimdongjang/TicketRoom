using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketRoom.Views.MainTab.MyPage.PurchaseList;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.MyPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PurchaseListPage : ContentPage
    {
        public static bool isOpenPage = false;
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
            pls = new PurchaseListShop(this);
            Init(plg);
        }

        private void TapColorChange(ContentView cv)
        {
            if (cv == plg) // 상품권이 선택되었을 경우
            {
                TapShopingGridLabel.TextColor = Color.Black;
                TapShopingGrid.BackgroundColor = Color.White;

                TapGiftGridLabel.TextColor = Color.White;
                TapGiftGrid.BackgroundColor = Color.Black;
            }
            else // 쇼핑몰이 선택 되었을 경우
            {
                TapShopingGridLabel.TextColor = Color.White;
                TapShopingGrid.BackgroundColor = Color.Black;

                TapGiftGridLabel.TextColor = Color.Black;
                TapGiftGrid.BackgroundColor = Color.White;
            }
        }

        public void Init(ContentView cv)
        {
            PurchaseListContentView.Content = cv;

            TapColorChange(cv);

            // 상품권 탭을 선택할 경우 상품권 컨텐츠 뷰를 보여줌
            TapGiftGrid.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    plg = new PurchaseListGift(this);
                    PurchaseListContentView.Content = plg;

                    TapColorChange(plg);

                })
            });
            // 쇼팡몰 탭을 선택할 경우 쇼팡몰 컨텐츠 뷰를 보여줌
            TapShopingGrid.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    pls = new PurchaseListShop(this);
                    PurchaseListContentView.Content = pls;

                    TapColorChange(pls);
                })
            });
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            this.OnBackButtonPressed();
        }

        private void allbtn_clicked(object sender, EventArgs e)
        {
            if(PurchaseListContentView.Content== plg)
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
        }

        private void yearbtn_clicked(object sender, EventArgs e)
        {
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
        }

        private void monthbtn_clicked(object sender, EventArgs e)
        {
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
        }

        private void weekbtn_clicked(object sender, EventArgs e)
        {
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
        }
    }
}