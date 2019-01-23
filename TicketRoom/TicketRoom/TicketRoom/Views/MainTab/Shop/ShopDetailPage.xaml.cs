using System;
using System.Collections.Generic;
using TicketRoom.Views.MainTab.Shop.GridImage;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.Shop
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShopDetailPage : ContentPage
    {
        string myShopName = "";
        int clothes_count = 0;


        public ShopDetailPage(string titleName)
        {
            InitializeComponent();
            myShopName = titleName;
            Init();

        }

        private async void BasketBtn_ClickedAsync(object sender, EventArgs e)
        {
            string size = "";
            string color = "";
            int selectedIndex = ClothesSelectSize.SelectedIndex;
            if (selectedIndex != -1)
            {
                size = ClothesSelectSize.Items[selectedIndex];
            }
            selectedIndex = ClothesSelectColor.SelectedIndex;
            if (selectedIndex != -1)
            {
                color = ClothesSelectColor.Items[selectedIndex];
            }

            if (size != "")
            {
                if (color != "")
                {
                    if (ClothesCountLabel.Text != "0")
                    {
                        //장바구니로 이동
                        var answer = await DisplayAlert("사이즈 : " + size + " , 색상 : " + color + ", 수량 : " + ClothesCountLabel.Text, "주문 정보가 맞습니까?", "확인", "취소");
                        if (answer)
                        {
                            var basket_answer = await DisplayAlert("주문 완료", "장바구니로 이동하시겠습니까?", "확인", "취소");
                            if (basket_answer)
                            {
                                //Navigation.PushModalAsync();
                            }
                        }
                    }
                }
            }
        }

        private void Tab_Changed(object sender, EventArgs e)
        {

        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {

        }

        private void Init()
        {
            TitleName.Text = myShopName;
            CountEvent();
            SelectColor();
            SelectSize();


            #region 다른 고객이 함께 본 상품 목록
            Grid other_grid = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star)  },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star)  },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star)  }
                },
                Margin = new Thickness(15, 0, 15, 0),
            };
            for (int i = 0; i < 3; i++)
            {
                Image image = new Image
                {
                    Source = "shop_clothes1.jpg",
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                };
                other_grid.Children.Add(image, i, 0);
                // 이미지 클릭시 해당 페이지로 이동(아직 미구현)
                image.GestureRecognizers.Add(new TapGestureRecognizer()
                {
                    Command = new Command(() =>
                    {

                    })
                });
            }
            OtherProduct.Children.Add(other_grid, 0, 0);
            #endregion

            #region 이미지 리스트 그리드 클릭 이벤트
            ImageListGrid.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    Navigation.PushModalAsync(new PictureList());
                })
            });
            #endregion

        }

        private void SelectColor()
        {
            List<string> color_list = new List<string>();

            color_list.Add("파랑");
            color_list.Add("연두");
            color_list.Add("보라");
            color_list.Add("빨강");
            // 선택할 색상은 동적으로 할 예정

            foreach (string colorName in color_list)
            {
                ClothesSelectColor.Items.Add(colorName);
            }
        }

        private void SelectSize()
        {
            List<string> size_list = new List<string>();

            size_list.Add("S(90)");
            size_list.Add("M(95)");
            size_list.Add("L(100)");
            size_list.Add("XL(110)");
            // 선택할 색상은 동적으로 할 예정

            foreach (string colorName in size_list)
            {
                ClothesSelectSize.Items.Add(colorName);
            }
        }

        private void CountEvent()
        {
            #region +,- 수량 체크 이벤트
            plusCount.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    clothes_count = int.Parse(ClothesCountLabel.Text);
                    clothes_count += 1;
                    ClothesCountLabel.Text = clothes_count.ToString();
                })
            });

            minusCount.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    clothes_count = int.Parse(ClothesCountLabel.Text);
                    if (clothes_count != 0)
                    {
                        clothes_count -= 1;
                    }
                    ClothesCountLabel.Text = clothes_count.ToString();
                })
            });
            #endregion

        }

        private void BasketBtn_Clicked(object sender, EventArgs e)
        {

        }
    }
}