using System;
using System.Collections.Generic;
using System.Linq;
using TicketRoom.Models.Custom;
using TicketRoom.Views.MainTab.Basket;
using TicketRoom.Views.MainTab.Dael.Purchase;
using TicketRoom.Views.MainTab.Shop;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BasketTabPage : ContentView
    {
        public static bool isOpenPage = false;
        BasketGiftView bgv;
        BasketShopView bsv;
        Queue<CustomLabel> SelectTap_Queue = new Queue<CustomLabel>();

        public BasketTabPage()
        {
            InitializeComponent();
            bgv = new BasketGiftView(this);
            bsv = new BasketShopView(this);

            #region IOS의 경우 초기화
            if (Device.OS == TargetPlatform.iOS)
            {
                TabGrid.RowDefinitions[0].Height = 50;
            }
            #endregion

            if(Global.isBasketDeal == true)
            {
                init(bgv);
            }
            else if(Global.isBasketShop == true)
            {
                init(bsv);
            }
            
        }
        private void TapColorChange(ContentView cv)
        {
            //상품권 장바구니 주문 버튼 더블클릭방지
            Global.isgiftbastketorderbtn_clicked = true;

            if (cv == bgv) // 상품권이 선택되었을 경우
            {
                ShopSelect.TextColor = Color.White;
                ((Grid)ShopSelect.Parent).BackgroundColor = Color.CornflowerBlue;
                GiftSelect.TextColor = Color.CornflowerBlue;
                ((Grid)GiftSelect.Parent).BackgroundColor = Color.White;
            }
            else // 쇼핑몰이 선택 되었을 경우
            {
                ShopSelect.TextColor = Color.CornflowerBlue;
                ((Grid)ShopSelect.Parent).BackgroundColor = Color.White;
                GiftSelect.TextColor = Color.White;
                ((Grid)GiftSelect.Parent).BackgroundColor = Color.CornflowerBlue;
            }
        }


        public void init(ContentView cv)
        {
            BasketContentView.Content = cv;

            TapColorChange(cv);

            // 상품권 탭을 선택할 경우 상품권 컨텐츠 뷰를 보여줌
            GiftSelectGrid.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    bgv = new BasketGiftView(this);
                    BasketContentView.Content = bgv;

                    TapColorChange(bgv);

                })
            });
            // 쇼팡몰 탭을 선택할 경우 쇼팡몰 컨텐츠 뷰를 보여줌
            ShopSelectGrid.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    bsv = new BasketShopView(this);
                    BasketContentView.Content = bsv;

                    TapColorChange(bsv);
                })
            });
        }

        private async void OrderBtn_ClickedAsync(object sender, EventArgs e)
        {
            if (BasketContentView.Content == bsv) // 쇼핑몰 컨텐츠가 활성화 되어있을때
            { /*
                string orderString = "";
                string changeStringToInt = "";
                int orderPay = 0;
                for (int i = 0; i < bsv.SH_ProductNameList.Count; i++)
                {
                    orderString += bsv.SH_ProductNameList[i] + " "; // 상품 이름
                    orderString += bsv.SH_ProductCountList[i] + "개 "; // 수량
                    orderString += bsv.SH_ProductTypeList[i] + "\n"; // 사이즈
                    // 30,000원 => 30000으로 변경
                    changeStringToInt = bsv.SH_ProductPriceList[i].Replace("원", "");
                    changeStringToInt = changeStringToInt.Replace(",", "");
                    orderPay += int.Parse(changeStringToInt); // 금액
                }

                bool check = await App.Current.MainPage.DisplayAlert("주문 내역", orderString + "\n 총 결제금액 : " + orderPay.ToString("N0") + "원", "확인", "취소");
                if (check == false)
                {
                    return;
                }
                await Navigation.PushModalAsync(new ShopOrderPage(bsv.SH_ProductNameList));*/
            }
            else // 상품권 컨텐츠일때.
            {
                Dictionary<string, string> data = new Dictionary<string, string>();
                List<Xamarin.Forms.View> container = bgv.Basketlist_Grid.Children.ToList();
                for (int i = 0; i < container.Count; i++)
                {
                    if (i % 2 == 0)
                    {
                        List<Xamarin.Forms.View> productlist = ((Grid)container[i]).Children.ToList();
                        List<Xamarin.Forms.View> labelgrid = ((Grid)productlist[1]).Children.ToList();
                        List<Xamarin.Forms.View> countgrid = ((Grid)productlist[2]).Children.ToList();

                        data.Add(((Label)labelgrid[0]).Text, ((Label)countgrid[1]).Text);
                    }
                }
                await Navigation.PushAsync(new PurchaseDetailPage());
            }
        }
    }
}