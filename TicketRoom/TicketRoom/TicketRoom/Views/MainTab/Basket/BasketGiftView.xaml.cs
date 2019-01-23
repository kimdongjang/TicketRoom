using System;
using System.Collections.Generic;
using System.Linq;
using TicketRoom.Views.MainTab.Dael.Purchase;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.Basket
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BasketGiftView : ContentView
    {
        BasketTabPage btp;

        public BasketGiftView(BasketTabPage btp)
        {
            InitializeComponent();
            ShowBasketlist();
            this.btp = btp;
        }

        private void ShowBasketlist()
        {
            Basketlist_Grid.Children.Clear();
            Basketlist_Grid.RowDefinitions.Clear();

            int row = 0;
            int result_price = 0;

            #region 상품이 준비중
            if (Global.BasketList.Count == 0)
            {
                Basketlist_Grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

                Label nullproduct = new Label
                {
                    Text = "장바구니에 상품이 없습니다",
                    FontSize = 25,
                    TextColor = Color.Black,
                    VerticalOptions = LayoutOptions.Center,
                    YAlign = TextAlignment.Center,
                    HorizontalOptions = LayoutOptions.Center
                };
                Basketlist_Grid.Children.Add(nullproduct, 0, 0);
                Bottom_Grid.IsVisible = false;
                return;
            }
            #endregion

            for (int i = 0; i < Global.BasketList.Count; i++)
            {
                Basketlist_Grid.RowDefinitions.Add(new RowDefinition { Height = 100 });
                Basketlist_Grid.RowDefinitions.Add(new RowDefinition { Height = 1 });

                #region 상품 그리드
                Grid product_grid = new Grid
                {
                    Margin = new Thickness(0, 10, 0, 10),
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    RowSpacing = 0,
                    ColumnSpacing = 0,
                    ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = 100 },
                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                        new ColumnDefinition { Width = 40 },
                        new ColumnDefinition { Width = 20 }
                    }
                };

                #region 장바구니 상품 이미지
                Image product_image = new Image
                {
                    Source = Global.BasketList[i].BK_PRODUCT_IMAGE,
                    BackgroundColor = Color.White,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    Aspect = Aspect.AspectFit
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
                Label pro_label = null;
                if (Global.BasketList[i].BK_TYPE.Equals("1"))
                {
                    pro_label = new Label
                    {
                        Text = Global.BasketList[i].BK_PRODUCT_TYPE + " (지류)",
                        FontSize = 16,
                        TextColor = Color.Black,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        XAlign = TextAlignment.Start,
                        YAlign = TextAlignment.Center,
                        HorizontalOptions = LayoutOptions.StartAndExpand
                    };
                }
                else
                {
                    pro_label = new Label
                    {
                        Text = Global.BasketList[i].BK_PRODUCT_TYPE + " (핀번호)",
                        FontSize = 16,
                        TextColor = Color.Black,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        XAlign = TextAlignment.Start,
                        YAlign = TextAlignment.Center,
                        HorizontalOptions = LayoutOptions.StartAndExpand
                    };
                }
                #endregion

                #region 상품 종류 Label
                Label type_label = new Label
                {
                    Text = Global.BasketList[i].BK_PRODUCT_VALUE,
                    FontSize = 14,
                    TextColor = Color.Black,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    XAlign = TextAlignment.Start,
                    YAlign = TextAlignment.Center,
                    HorizontalOptions = LayoutOptions.StartAndExpand
                };
                #endregion

                #region 가격 내용 Label
                Label price_label = new Label
                {
                    Text = Global.BasketList[i].BK_PRODUCT_PURCHASE_DISCOUNTPRICE + "원",
                    FontSize = 14,
                    TextColor = Color.Gray,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    XAlign = TextAlignment.Start,
                    YAlign = TextAlignment.Center,
                    HorizontalOptions = LayoutOptions.StartAndExpand
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
                Label Count_label = new Label
                {
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Text = Global.BasketList[i].BK_PROCOUNT,
                    FontSize = 14,
                    TextColor = Color.Black,
                    XAlign = TextAlignment.Center,
                    YAlign = TextAlignment.Center
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
                product_grid.Children.Add(product_image, 0, 0);
                product_grid.Children.Add(product_label_grid, 1, 0);
                product_grid.Children.Add(product_count_grid, 2, 0);
                #endregion
                #endregion

                //장바구니 리스트 그리드에 추가 
                Basketlist_Grid.Children.Add(product_grid, 0, row);
                row++;

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
                    BindingContext = i,
                    Source = "x.png",
                    HeightRequest = 40,
                    WidthRequest = 40,
                };


                deleteGrid.Children.Add(deleteImage, 0, 0);
                product_grid.Children.Add(deleteGrid, 3, 0);

                // X버튼 누를시에 해당 리스트 삭제 이벤트
                #region 장바구니 삭제 이벤트
                // Your label tap event
                var deletebtn_Clicked = new TapGestureRecognizer();
                deletebtn_Clicked.Tapped += async (s, e) =>
                {
                    bool check = await App.Current.MainPage.DisplayAlert("삭제", "장바구니 항목을 삭제하시겠습니까?", "확인", "취소");
                    if (check == false)
                    {
                        return;
                    }
                    Image deletegrid = (Image)s;
                    Global.BasketList.RemoveAt(int.Parse(deletegrid.BindingContext.ToString()));
                    ShowBasketlist();
                };
                #endregion
                deleteImage.GestureRecognizers.Add(deletebtn_Clicked);
                #endregion


                #region 이미지 클릭 이벤트
                // Your label tap event
                var plus_tap = new TapGestureRecognizer();
                plus_tap.Tapped += (s, e) =>
                {
                    plusBtn_Clicked(s, e);
                };
                #endregion

                #region 이미지 클릭 이벤트
                // Your label tap event
                var minus_tap = new TapGestureRecognizer();
                minus_tap.Tapped += (s, e) =>
                {
                    minusBtn_Clicked(s, e);
                };
                #endregion

                Plus_btn.GestureRecognizers.Add(plus_tap);
                minus_btn.GestureRecognizers.Add(minus_tap);

                BoxView gridline = new BoxView
                {
                    BackgroundColor = Color.FromHex("#f4f2f2"),
                    HeightRequest = 1,
                    VerticalOptions = LayoutOptions.End,
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };
                Basketlist_Grid.Children.Add(gridline, 0, row);
                row++;

                result_price += int.Parse(Global.BasketList[i].BK_PROCOUNT) * int.Parse(Global.BasketList[i].BK_PRODUCT_PURCHASE_DISCOUNTPRICE);
            }
            ResultPrice_label.Text = result_price.ToString("N0");
        }

        private void plusBtn_Clicked(object s, EventArgs e)
        {
            Image button = (Image)s;
            //countlabelist[int.Parse(button.Parent.BindingContext.ToString())].Text = (int.Parse(countlabelist[int.Parse(button.Parent.BindingContext.ToString())].Text)+1).ToString();
            Grid g = (Grid)button.Parent;
            List<Xamarin.Forms.View> b = g.Children.ToList();
            Label count = (Label)b[1];
            count.Text = (int.Parse(count.Text) + 1).ToString();

            Grid g2 = (Grid)g.Parent;
            List<Xamarin.Forms.View> b2 = g2.Children.ToList();
            Grid g3 = (Grid)b2[1];
            List<Xamarin.Forms.View> b3 = g3.Children.ToList();
            Label price = (Label)b3[2];
            ResultPrice_label.Text = (int.Parse(ResultPrice_label.Text.Replace(",", "")) + int.Parse(price.Text.Replace("원", ""))).ToString("N0");
        }

        private void minusBtn_Clicked(object s, EventArgs e)
        {
            Image button = (Image)s;
            //countlabelist[int.Parse(button.Parent.BindingContext.ToString())].Text = (int.Parse(countlabelist[int.Parse(button.Parent.BindingContext.ToString())].Text) - 1).ToString();

            Grid g = (Grid)button.Parent;
            List<Xamarin.Forms.View> b = g.Children.ToList();
            Label count = (Label)b[1];

            if (int.Parse(count.Text) > 0)
            {
                count.Text = (int.Parse(count.Text) - 1).ToString();
                Grid g2 = (Grid)g.Parent;
                List<Xamarin.Forms.View> b2 = g2.Children.ToList();
                Grid g3 = (Grid)b2[1];
                List<Xamarin.Forms.View> b3 = g3.Children.ToList();
                Label price = (Label)b3[2];
                ResultPrice_label.Text = (int.Parse(ResultPrice_label.Text.Replace(",", "")) - int.Parse(price.Text.Replace("원", ""))).ToString("N0");
            }
        }

        private void OrderBtn_Clicked(object sender, EventArgs e)
        {
            //Dictionary<string, string> data = new Dictionary<string, string>();
            //List<Xamarin.Forms.View> container = Basketlist_Grid.Children.ToList();
            //for (int i = 0; i < container.Count; i++)
            //{
            //    if (i % 2 == 0)
            //    {
            //        List<Xamarin.Forms.View> productlist = ((Grid)container[i]).Children.ToList();
            //        List<Xamarin.Forms.View> labelgrid = ((Grid)productlist[1]).Children.ToList();
            //        List<Xamarin.Forms.View> countgrid = ((Grid)productlist[2]).Children.ToList();

            //        data.Add(((Label)labelgrid[0]).Text, ((Label)countgrid[1]).Text);
            //    }
            //}

            Navigation.PushModalAsync(new PurchaseDetailPage());
        }
    }
}