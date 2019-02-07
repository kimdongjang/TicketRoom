using System;
using System.Collections.Generic;
using System.Linq;
using TicketRoom.Models.Custom;
using TicketRoom.Models.Effect;
using TicketRoom.Models.ShopData;
using TicketRoom.Views.MainTab.Shop;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.Basket
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BasketShopView : ContentView
    {
        public bool isScollUsed = false;

        BasketTabPage btp;
        ShopDBFunc SH_DB = ShopDBFunc.Instance();
        public List<SH_BasketList> basketList = new List<SH_BasketList>();
        public SH_Home home = new SH_Home(); 

        public List<Grid> BasketGridList = new List<Grid>();
        public int orderPay = 0;


        public BasketShopView(BasketTabPage btp)
        {
            InitializeComponent();
            this.btp = btp;

            ShowBasketList();


            PriceUpdate();
        }

        // 결제 금액 갱신
        private void PriceUpdate()
        {
            string changeStringToInt = "";

            for (int i = 0; i < basketList.Count; i++)
            {
                orderPay += basketList[i].SH_BASKET_PRICE; // 금액
            }
            PriceLabel.Text = "합계 : " + orderPay.ToString("N0") + "원";
        }

        private void ShowBasketList()
        {
            basketList = SH_DB.PostSearchBasketListToID("dnsrl1122"); // 사용자 아이디

            BasketGridList.Clear();
            Basketlist_Grid.Children.Clear();
            Basketlist_Grid.RowDefinitions.Clear();

            if(basketList == null)
            {
                CustomLabel alert = new CustomLabel
                {
                    Text = "장바구니에 내용이 없습니다!",
                    Size = 18,
                    HorizontalOptions = LayoutOptions.Center,
                };
                Basketlist_Grid.Children.Add(alert);
                return;
            }

            int row = 0;
            for (int i = 0; i < basketList.Count; i++)
            {
                Basketlist_Grid.RowDefinitions.Add(new RowDefinition { Height = 100 });
                Basketlist_Grid.RowDefinitions.Add(new RowDefinition { Height = 3 });
                Grid inGrid = new Grid
                {
                    ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = 100 },
                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                        new ColumnDefinition { Width = 30 }
                    },
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Margin = new Thickness(0, 5, 0, 5),
                    RowSpacing = 0,
                    ColumnSpacing = 0,
                };

                BasketGridList.Add(inGrid); // 장바구니에 추가되는 리스트를 관리하기 위한 그리드 리스트(x버튼으로 삭제 관리를 위함)

                #region 장바구니 상품 이미지
                Image product_image = new Image
                {
                    Source = basketList[i].SH_BASKET_IMAGE,
                    BackgroundColor = Color.White,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    Aspect = Aspect.AspectFill,
                };
                #endregion

                #region 상품 설명 Labellist 그리드
                Grid product_label_grid = new Grid
                {
                    Margin = new Thickness(10, 0, 0, 0),
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    RowSpacing = 0,
                    ColumnSpacing = 0,
                    RowDefinitions =
                    {
                        new RowDefinition { Height = GridLength.Auto },
                        new RowDefinition { Height = GridLength.Auto },
                        new RowDefinition { Height = 10 },
                        new RowDefinition { Height = GridLength.Auto }
                    },
                    
                };

                #region 상품 제목 Label
                CustomLabel pro_label = new CustomLabel
                {
                    Text = basketList[i].SH_BASKET_NAME,
                    Size = 18,
                    TextColor = Color.Black,
                };
                #endregion

                #region 상품 종류 Label (사이즈, 색상, 추가옵션)
                CustomLabel type_label = new CustomLabel
                {
                    Text = "색상 : " + basketList[i].SH_BASKET_COLOR + ", 사이즈 : " + basketList[i].SH_BASKET_SIZE + ", " + basketList[i].SH_BASKET_COUNT + "개",
                    Size = 14,
                    TextColor = Color.DarkGray,
                };
                #endregion

                #region 가격 내용 Label 및 장바구니 담은 날짜
                CustomLabel price_label = new CustomLabel
                {
                    Text = basketList[i].SH_BASKET_PRICE.ToString("N0") + "원 / " + basketList[i].SH_BASKET_DATE,
                    Size = 14,
                    TextColor = Color.Gray,
                };
                #endregion

                //상품 설명 라벨 그리드에 추가
                product_label_grid.Children.Add(pro_label, 0, 0);
                product_label_grid.Children.Add(type_label, 0, 1);
                product_label_grid.Children.Add(price_label, 0, 3);
                #endregion

                #region 상품권 그리드 자식 추가
                inGrid.Children.Add(product_image, 0, 0);
                inGrid.Children.Add(product_label_grid, 1, 0);
                #endregion


                // 카테고리 길게 클릭시 해당 상품 페이지로 넘어갈 수 있는 이벤트
                LongPressedEffect.SetCommand(inGrid, new Command(execute: async () =>
                {
                    if (isScollUsed == false) // 만약 스크롤이 활성화 되어있지 않다면 이벤트 실행
                    {
                        if (await Application.Current.MainPage.DisplayAlert("안내", "해당 페이지로 이동하시겠습니까?", "확인", "취소"))
                        {
                            int tempIndex = 0;
                            for (int j = 0; j < basketList.Count; j++)
                            {
                                if (basketList[j].SH_BASKET_NAME == pro_label.Text) // 리스트에 저장된 이름과 라벨에 저장된 이름을 비교해서 페이지로 이동시킴
                                {
                                    tempIndex = basketList[j].SH_HOME_INDEX;
                                }
                            }
                            await Navigation.PushModalAsync(new ShopMainPage(tempIndex));
                        }
                    }
                }));

                #region 장바구니 삭제 버튼
                Grid deleteGrid = new Grid
                {
                    RowDefinitions =
                    {
                         new RowDefinition { Height = new GridLength(2, GridUnitType.Star) },
                         new RowDefinition { Height = new GridLength(8, GridUnitType.Star) },
                    }
                };
                Image deleteImage = new Image
                {
                    Source = "x.png",
                    HeightRequest = 40,
                    WidthRequest = 40,
                };


                deleteGrid.Children.Add(deleteImage, 0, 0);
                inGrid.Children.Add(deleteGrid, 2, 0);

                // X버튼 누를시에 해당 리스트 삭제 이벤트
                deleteImage.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = new Command(async () =>
                    {
                        bool check = await App.Current.MainPage.DisplayAlert("삭제", "장바구니 항목을 삭제하시겠습니까?", "확인", "취소");
                        if (check == false)
                        {
                            return;
                        }

                        // 그리드 리스트에서 일치하는 그리드 탐색
                        int index = SearchIndexInGrid(inGrid);

                        if (SH_DB.PostDeleteBasketListToBasket(basketList[index].SH_BASKET_INDEX.ToString()) == false)
                        {
                            await App.Current.MainPage.DisplayAlert("에러", "삭제하는 도중 문제가 발생했습니다. 다시 시도해주십시오.", "확인");
                            return;
                        }

                        await App.Current.MainPage.DisplayAlert("알림", "정상적으로 삭제되었습니다.", "확인");
                        /*
                        SH_ProductPriceList.RemoveAt(index);
                        SH_ProductTypeList.RemoveAt(index);
                        SH_ProductNameList.RemoveAt(index);
                        SH_ImageSourceList.RemoveAt(index);
                        SH_ProductCountList.RemoveAt(index);*/
                        System.Diagnostics.Debug.WriteLine("갱신@@");

                        ShowBasketList();
                    })
                });
                #endregion

                //장바구니 리스트 그리드에 추가 
                Basketlist_Grid.Children.Add(inGrid, 0, row);
                row++;


                // 구분선
                BoxView gridline = new BoxView
                {
                    BackgroundColor = Color.FromHex("#f4f2f2"),
                    VerticalOptions = LayoutOptions.End,
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };
                Basketlist_Grid.Children.Add(gridline, 0, row);
                row++;
            }
        }

        // 그리드 리스트에서 그리드와 일치하는 인덱스 반환
        private int SearchIndexInGrid(Grid grid)
        {
            int index = 0;
            for (int i = 0; i < BasketGridList.Count; i++)
            {
                if (grid == BasketGridList[i])
                {
                    index = i;
                    return index;
                }
            }
            return -1;
        }

        // 주문하기 버튼
        private async void OrderBtn_ClickedAsync(object sender, EventArgs e)
        {
            if (BasketTabPage.isOpenPage == false)
            {
                await Navigation.PushModalAsync(new ShopOrderPage(basketList)); // 주문 페이지로 이동
                BasketTabPage.isOpenPage = true;
            }
            
        }
    }
}