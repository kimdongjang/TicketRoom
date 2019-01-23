using System;
using System.Collections.Generic;
using System.Linq;
using TicketRoom.Models.Custom;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.Basket
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BasketShopView : ContentView
    {
        BasketTabPage btp;

        public List<Grid> BasketList = new List<Grid>();
        public List<string> SH_ImageSourceList = new List<string>();
        public List<string> SH_ProductNameList = new List<string>();
        public List<string> SH_ProductTypeList = new List<string>();
        public List<string> SH_ProductPriceList = new List<string>();
        public List<string> SH_ProductCountList = new List<string>();
        public int BasketCount = 3;
        public int orderPay = 0;

        // 수량 체크를 위한 리스트
        List<Image> plusbtnlist = new List<Image>();
        List<Image> minusbtnlist = new List<Image>();
        List<Label> countlabelist = new List<Label>();

        public BasketShopView(BasketTabPage btp)
        {
            InitializeComponent();

            SH_ImageSourceList.Add("shop_clothes1.jpg");
            SH_ProductNameList.Add("와이셔츠");
            SH_ProductTypeList.Add("사이즈(L)+검정색"); // << 사이즈와 색상, 옵션은 데이터를 받아올 때 "사이즈" + "+" + "검정색"으로 초기화하기 바람
            SH_ProductPriceList.Add("30,000원");
            SH_ProductCountList.Add("1");

            SH_ImageSourceList.Add("shop_clothes1.jpg");
            SH_ProductNameList.Add("항공점퍼");
            SH_ProductTypeList.Add("사이즈(S)+보라색"); // << 사이즈와 색상, 옵션은 데이터를 받아올 때 "사이즈" + "+" + "검정색"으로 초기화하기 바람
            SH_ProductPriceList.Add("30,000원");
            SH_ProductCountList.Add("1");

            SH_ImageSourceList.Add("shop_clothes1.jpg");
            SH_ProductNameList.Add("넥타이");
            SH_ProductTypeList.Add("사이즈(S)+빨간색"); // << 사이즈와 색상, 옵션은 데이터를 받아올 때 "사이즈" + "+" + "검정색"으로 초기화하기 바람
            SH_ProductPriceList.Add("20,000원");
            SH_ProductCountList.Add("1");

            ShowBasketList();
            this.btp = btp;
            PriceUpdate();
        }

        // 결제 금액 갱신
        private void PriceUpdate()
        {
            string changeStringToInt = "";

            for (int i = 0; i < SH_ProductPriceList.Count; i++)
            {
                changeStringToInt = SH_ProductPriceList[i].Replace("원", "");
                changeStringToInt = changeStringToInt.Replace(",", "");
                orderPay += int.Parse(changeStringToInt); // 금액
            }
            //btp.PriceLabel.Text = "합계 : " + orderPay.ToString("N0") + "원";
        }

        private void ShowBasketList()
        {
            BasketList.Clear();
            Basketlist_Grid.Children.Clear();
            Basketlist_Grid.RowDefinitions.Clear();

            int row = 0;
            for (int i = 0; i < BasketCount; i++)
            {
                System.Diagnostics.Debug.WriteLine("디버그 하기 위한 인덱스 : " + i);
                Basketlist_Grid.RowDefinitions.Add(new RowDefinition { Height = 100 });
                Basketlist_Grid.RowDefinitions.Add(new RowDefinition { Height = 1 });
                Grid inGrid = new Grid
                {
                    ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = 100 },
                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                        new ColumnDefinition { Width = 40 },
                        new ColumnDefinition { Width = 20 }
                    },
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Margin = new Thickness(0, 10, 0, 10),
                    RowSpacing = 0,
                    ColumnSpacing = 0,
                };

                BasketList.Add(inGrid); // 장바구니에 추가되는 리스트를 관리하기 위한 그리드 리스트(x버튼으로 삭제 관리를 위함)

                #region 장바구니 상품 이미지
                Image product_image = new Image
                {
                    Source = SH_ImageSourceList[i],
                    BackgroundColor = Color.White,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    Aspect = Aspect.AspectFit,
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
                    }
                };

                #region 상품 제목 Label
                CustomLabel pro_label = new CustomLabel
                {
                    Text = SH_ProductNameList[i],
                    Size = 16,
                    TextColor = Color.Black,
                };
                #endregion

                #region 상품 종류 Label (사이즈, 색상, 추가옵션)
                CustomLabel type_label = new CustomLabel
                {
                    Text = SH_ProductTypeList[i],
                    Size = 14,
                    TextColor = Color.Black,
                };
                #endregion

                #region 가격 내용 Label
                CustomLabel price_label = new CustomLabel
                {
                    Text = SH_ProductPriceList[i],
                    Size = 14,
                    TextColor = Color.Gray,
                };
                #endregion

                //상품 설명 라벨 그리드에 추가
                product_label_grid.Children.Add(pro_label, 0, 0);
                product_label_grid.Children.Add(type_label, 0, 1);
                product_label_grid.Children.Add(price_label, 0, 4);
                #endregion

                #region 상품 수량 그리드
                Grid product_count_grid = new Grid
                {
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    RowSpacing = 0,
                    ColumnSpacing = 0,
                    //BindingContext = i,
                    RowDefinitions =
                    {
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
                    }
                };

                #region 플러스 버튼 
                Image Plus_btn = new Image
                {
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Source = "plus.png",
                    Aspect = Aspect.AspectFit
                };
                #endregion

                #region 상품 수량 label
                CustomLabel Count_label = new CustomLabel
                {
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    Text = SH_ProductCountList[i],
                    Size = 14,
                    TextColor = Color.Black,
                };
                #endregion

                #region 마이너스 버튼
                Image minus_btn = new Image
                {
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Source = "minus.png",
                    Aspect = Aspect.AspectFit
                };
                #endregion


                //상품 수량 그리드에 추가
                product_count_grid.Children.Add(Plus_btn, 0, 0);
                product_count_grid.Children.Add(Count_label, 0, 1);
                product_count_grid.Children.Add(minus_btn, 0, 2);
                #endregion

                #region 상품권 그리드 자식 추가
                inGrid.Children.Add(product_image, 0, 0);
                inGrid.Children.Add(product_label_grid, 1, 0);
                inGrid.Children.Add(product_count_grid, 2, 0);
                #endregion

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
                inGrid.Children.Add(deleteGrid, 3, 0);

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

                        BasketCount--;
                        // 그리드 리스트에서 일치하는 그리드 탐색
                        int index = SearchIndexInGrid(inGrid);

                        Basketlist_Grid.Children.Remove(inGrid); // 해당 그리드 삭제

                        BasketList.RemoveAt(index);

                        SH_ProductPriceList.RemoveAt(index);
                        SH_ProductTypeList.RemoveAt(index);
                        SH_ProductNameList.RemoveAt(index);
                        SH_ImageSourceList.RemoveAt(index);
                        SH_ProductCountList.RemoveAt(index);
                        System.Diagnostics.Debug.WriteLine("갱신@@");

                        ShowBasketList();
                    })
                });
                #endregion

                //장바구니 리스트 그리드에 추가 
                Basketlist_Grid.Children.Add(inGrid, 0, row);
                row++;


                #region 플러스 이미지 클릭 이벤트
                // Your label tap event
                var plus_tap = new TapGestureRecognizer();
                plus_tap.Tapped += (s, e) =>
                {
                    plusBtn_Clicked(s, e);
                };
                #endregion

                #region 마이너스 이미지 클릭 이벤트
                // Your label tap event
                var minus_tap = new TapGestureRecognizer();
                minus_tap.Tapped += (s, e) =>
                {
                    minusBtn_Clicked(s, e);
                };
                #endregion

                Plus_btn.GestureRecognizers.Add(plus_tap);
                minus_btn.GestureRecognizers.Add(minus_tap);


                // 구분선
                BoxView gridline = new BoxView
                {
                    BackgroundColor = Color.FromHex("#f4f2f2"),
                    HeightRequest = 1,
                    VerticalOptions = LayoutOptions.End,
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };
                Basketlist_Grid.Children.Add(gridline, 0, row);
                row++;
            }
        }


        private void plusBtn_Clicked(object s, EventArgs e)
        {
            Image image = (Image)s;
            Grid grid = (Grid)image.Parent;
            Grid ParentGrid = (Grid)grid.Parent;

            // 그리드 리스트에서 일치하는 그리드 탐색
            int index = SearchIndexInGrid(ParentGrid);

            SH_ProductCountList[index] = (int.Parse(SH_ProductCountList[index]) + 1).ToString(); // + 1로 갱신

            List<Xamarin.Forms.View> tempList = grid.Children.ToList();
            CustomLabel count = (CustomLabel)tempList[1]; // 그리드 리스트 중 1번에 해당하는 레이블 => 수량 선택
            count.Text = SH_ProductCountList[index];


        }

        private void minusBtn_Clicked(object s, EventArgs e)
        {
            Image image = (Image)s;
            Grid grid = (Grid)image.Parent;
            Grid ParentGrid = (Grid)grid.Parent;

            // 그리드 리스트에서 일치하는 그리드 탐색
            int index = SearchIndexInGrid(ParentGrid);

            SH_ProductCountList[index] = (int.Parse(SH_ProductCountList[index]) - 1).ToString(); // - 1로 갱신

            List<Xamarin.Forms.View> tempList = grid.Children.ToList();
            CustomLabel count = (CustomLabel)tempList[1]; // 행 그리드 내부에 수량 그리드 중 1번에 해당하는 레이블 => 수량 선택
            count.Text = SH_ProductCountList[index];

        }


        // 그리드 리스트에서 그리드와 일치하는 인덱스 반환
        private int SearchIndexInGrid(Grid grid)
        {
            int index = 0;
            for (int i = 0; i < BasketList.Count; i++)
            {
                if (grid == BasketList[i])
                {
                    index = i;
                    return index;
                }
            }
            return -1;
        }
    }
}