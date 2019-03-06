using FFImageLoading.Forms;
using FFImageLoading.Svg.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TicketRoom.Models.Custom;
using TicketRoom.Models.ShopData;
using TicketRoom.Views.MainTab.Basket;
using TicketRoom.Views.MainTab.Shop.GridImage;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.Shop
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShopDetailPage : ContentPage
    {
        ShopDBFunc SH_DB = ShopDBFunc.Instance();

        SH_Home home;
        SH_Product product;
        List<SH_ImageList> imageList;
        List<SH_OtherView> otherList;
        List<SH_Pro_Option> optionList;
        List<SH_Home> otherHomeList = new List<SH_Home>();

        string selectColor = "";
        string selectSize = "";

        string myShopName = "";
        int clothes_count = 0;
        int productIndex = 0;

        string shopDetailPage_ID = "";

        public ShopDetailPage(string titleName, int productIndex, SH_Home home)
        {
            InitializeComponent();

            #region IOS의 경우 초기화
            NavigationPage.SetHasNavigationBar(this, false); // Navigation Bar 지우는 코드 생성자에 입력
            if (Device.OS == TargetPlatform.iOS)
            {
                MainGrid.RowDefinitions[0].Height = 50;
            }
            #endregion
            myShopName = titleName;
            this.productIndex = productIndex;
            this.home = home;

            LoadingInit();


        }


        private async Task LoadingInit()
        {
            // 로딩 시작
            await Global.LoadingStartAsync();

            imageList = SH_DB.PostSearchImageListToProductAsync(productIndex);
            otherList = SH_DB.PostSearchOtherViewToHome(home.SH_HOME_INDEX);
            optionList = SH_DB.PostSearchProOptionToProductAsync(productIndex);
            product = SH_DB.PostSearchProductToProduct(productIndex);

            if (otherList.Count != 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (otherList.Count <= i)
                    {
                        break;
                    }
                    otherHomeList.Add(SH_DB.PostSearchHomeToHome(otherList[i].SH_OTHERHOME_INDEX)); // 다른 고객이 본 상품 목록을 리스트에 추가
                }
            }

            // 아이디 초기화
            if (Global.b_user_login == false)
            {
                shopDetailPage_ID = Global.non_user_id;
            }
            else if (Global.b_user_login == true)
            {
                shopDetailPage_ID = Global.ID;
            }

            Init();

            // 로딩 완료
            await Global.LoadingEndAsync();
        }


        // 장바구니에 담기 버튼 클릭 이벤트
        private async void BasketBtn_ClickedAsync(object sender, EventArgs e)
        {
            string color = "";
            int selectedIndex = ClothesSelectOption.SelectedIndex;
            if (selectedIndex != -1)
            {
                if (ClothesCountLabel.Text != "0")
                {
                    //장바구니로 이동
                    var answer = await DisplayAlert("사이즈 : " + optionList[selectedIndex].SH_PRO_OPTION_SIZE + 
                        " , 색상 : " + optionList[selectedIndex].SH_PRO_OPTION_COLOR +
                        " , 수량 : " + ClothesCountLabel.Text +
                        " , 가격 : " + ClothesPriceLabel.Text + "",
                        "주문 정보가 맞습니까?", "확인", "취소");
                    if (answer)
                    {
                        // DB 장바구니 테이블에 데이터 삽입
                        if (SH_DB.PostInsertBasketListToHome(
                            home.SH_HOME_INDEX.ToString(), // 홈 페이지 인덱스
                            ClothesPriceLabel.Text.Replace("원", "").Replace(",", ""), // 가격
                            ClothesCountLabel.Text, // 수량
                            optionList[selectedIndex].SH_PRO_OPTION_COLOR, // 색상
                            optionList[selectedIndex].SH_PRO_OPTION_SIZE, // 사이즈
                            shopDetailPage_ID, // 아이디
                            product.SH_PRODUCT_NAME, // 상품이름
                            System.DateTime.Now.ToString(),
                            product.SH_PRODUCT_MAINIMAGE,
                            product.SH_PRODUCT_INDEX.ToString()) == true)
                        {
                            var basket_answer = await DisplayAlert("주문 완료", "장바구니로 이동하시겠습니까?", "확인", "취소");
                            if (basket_answer)
                            {
                                Navigation.PopToRootAsync();
                                Global.InitOnAppearingBool("basket");
                                Global.InitBasketOnAppearingBool("shop");
                                MainPage mp = (MainPage)Application.Current.MainPage.Navigation.NavigationStack[0];
                            }
                        }
                        else
                        {
                            await DisplayAlert("주문 실패", "다시 한 번 시도해주십시오.", "확인");
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
            ColorSizeInit();

            MainImage.LoadingPlaceholder = Global.LoadingImagePath;
            MainImage.ErrorPlaceholder = Global.LoadingImagePath;
            MainImage.Source = ImageSource.FromUri(new Uri(product.SH_PRODUCT_MAINIMAGE)); // 이미지 리스트의 첫번째 사진 노출

            ImageListInit();

            // 상품설명
            Device.BeginInvokeOnMainThread(async () =>
            {
                DetailEditor.Text = product.SH_PRODUCT_DETAIL;
            });


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
            // 다른 고객이 함께본 상품이 없다면 리턴.
            for (int i = 0; i < 3; i++)
            {
                Grid inGrid = new Grid
                {
                    RowDefinitions =
                    {
                        new RowDefinition { Height = 100  },
                        new RowDefinition { Height = 20  },
                    },
                    Margin = new Thickness(10, 0, 10, 0),
                };
                CachedImage image = new CachedImage
                {
                    LoadingPlaceholder = Global.LoadingImagePath,
                    ErrorPlaceholder = Global.LoadingImagePath,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    Aspect = Aspect.AspectFill,
                };

                if (otherList.Count <= i) // 다른 고객이 함께본 상품에 이미지가 없을 경우
                {
                    image.Source = "no_image.png";
                }
                else
                {
                    image.Source = ImageSource.FromUri(new Uri(otherList[i].SH_OTHERVIEW_IMAGE));
                }


                CustomLabel label = new CustomLabel
                {
                    Size = 14,
                    TextColor = Color.Black,
                    HorizontalOptions = LayoutOptions.Center,
                };

                if (otherHomeList.Count <= i) // 다른 고객이 함께본 상품에 이미지가 없을 경우
                {
                    label.Text = "No data";
                }
                else
                {
                    label.Text = otherHomeList[i].SH_HOME_NAME; // 홈 페이지의 타이틀
                }

                inGrid.Children.Add(image, 0, 0);
                inGrid.Children.Add(label, 0, 1);
                other_grid.Children.Add(inGrid, i, 0);

                inGrid.GestureRecognizers.Add(new TapGestureRecognizer()
                {
                    Command = new Command(() =>
                    {
                        // 다른 고객이 본 상품 -> 홈 메인 페이지로 이동하는데 더블 클릭 제어
                        if(Global.isOpen_ShopOtherPage == true)
                        {
                            return;
                        }
                        Global.isOpen_ShopOtherPage = true;

                        for (int j = 0; j < otherHomeList.Count; j++)
                        {
                            if(label.Text == otherHomeList[j].SH_HOME_NAME)
                            {
                                Global.OtherIndexUpdate(otherHomeList[j].SH_HOME_INDEX); // 다른 고객이 함께본 상품 초기화를 위한 처리
                                SH_DB.PostUpdateViewsOtherViewToIndex(Global.g_main_index, Global.g_other_index); // main인덱스와 other인덱스 서버로 전달
                                SH_DB.PostUpdateRecentViewToID(Global.ID, otherHomeList[j].SH_HOME_INDEX.ToString()); // 최근 본 상품 목록 갱신

                                var nav = Navigation.NavigationStack;
                                this.Navigation.RemovePage(nav[nav.Count-1]);
                                this.Navigation.RemovePage(nav[nav.Count-2]);
                                Navigation.PushAsync(new ShopMainPage(otherHomeList[j].SH_HOME_INDEX)); 
                            }
                        }
                        if(otherHomeList.Count == 0)
                        {
                            Global.isOpen_ShopOtherPage = false;
                        }
                    })
                });
            }
            OtherProduct.Children.Add(other_grid, 0, 0);
            #endregion

            

        }

        private void ImageListInit()
        {
            try
            {
                int column = 0;
                // 3장까지 혹은 3장 이하의 이미지일 경우 3개의 콜럼에 이미지를 채우도록 한다.
                for (int i = 0; i < 3; i++)
                {
                    column = i;
                    CachedImage image = new CachedImage
                    {
                        LoadingPlaceholder = Global.LoadingImagePath,
                        ErrorPlaceholder = Global.LoadingImagePath,
                        Aspect = Aspect.AspectFill,
                    };
                    if (imageList.Count <= i)
                    {
                        image.Source = ImageSource.FromUri(new Uri("http://221.141.58.49:8088/img/default/no_image.png"));
                    }
                    else
                    {
                        image.Source = ImageSource.FromUri(new Uri(imageList[i].SH_IMAGELIST_SOURCE));
                    }
                    ImageListGrid.Children.Add(image, i, 0);
                }
                // 3장 이상의 사진이 있을 경우
                if (imageList.Count > 3)
                {
                    Grid Dubogi = new Grid
                    {
                        RowDefinitions =
                {
                     new RowDefinition { Height = new GridLength(5, GridUnitType.Star) },
                     new RowDefinition { Height = new GridLength(5, GridUnitType.Star) },
                }
                    };
                    BoxView borderLine = new BoxView { BackgroundColor = Color.Black, Opacity = 0.5 };
                    CustomLabel Label = new CustomLabel
                    {
                        Size = 18,
                        TextColor = Color.White,
                        Text = "+" + (imageList.Count - 3).ToString() + "장", // 3장을 뺀 더 볼 수 있는 이미지
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center
                    };
                    Dubogi.Children.Add(borderLine, 0, 1);
                    Dubogi.Children.Add(Label, 0, 1);
                    ImageListGrid.Children.Add(Dubogi, column, 0);
                }
                #region 이미지 리스트 그리드 클릭 이벤트
                ImageListGrid.GestureRecognizers.Add(new TapGestureRecognizer()
                {
                    Command = new Command(() =>
                    {
                        if(Global.isOpen_PictureList == true)
                        {
                            return;
                        }
                        Global.isOpen_PictureList = true;
                        Navigation.PushAsync(new PictureList(imageList));
                    })
                });
                #endregion
            }
            catch (Exception ex)// 이미지 리스트가 초기화가 안되어있을 경우
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }

        // 선택 가능한 색상 및 사이즈 초기화
        private void ColorSizeInit()
        {
            for(int i = 0; i<optionList.Count; i++)
            {
                ClothesSelectOption.Items.Add(optionList[i].SH_PRO_OPTION_COLOR+"("+ optionList[i].SH_PRO_OPTION_SIZE+")"); // 색상+사이즈
            }
        }

        private void CountEvent()
        {
            #region +,- 수량 체크 이벤트
            plusCount.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(async () =>
                {
                    // 옵션이 선택되지 않을 경우 리턴
                    if(ClothesSelectOption.SelectedIndex == -1)
                    {
                        return;
                    }

                    int max_count = 0;
                    for (int i = 0; i < optionList.Count; i++)
                    { 
                        if(optionList[i].SH_PRO_OPTION_COLOR == selectColor && optionList[i].SH_PRO_OPTION_SIZE == selectSize)
                        {
                            max_count = optionList[i].SH_PRO_OPTION_COUNT;
                            if (max_count <= int.Parse(ClothesCountLabel.Text))
                            {
                                var basket_answer = await DisplayAlert("주문 오류", "주문 가능한 수량을 초과했습니다!", "확인", "취소");
                                clothes_count -= 1;
                                return;
                            }
                        }
                    }
                    clothes_count = int.Parse(ClothesCountLabel.Text);
                    clothes_count += 1;
                    ClothesCountLabel.Text = clothes_count.ToString();
                    ClothesPriceLabel.Text = (optionList[ClothesSelectOption.SelectedIndex].SH_PRO_OPTION_PRICE * clothes_count).ToString("N0") + "원";
                })
            });

            minusCount.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    // 옵션이 선택되지 않을 경우 리턴
                    if (ClothesSelectOption.SelectedIndex == -1)
                    {
                        return;
                    }
                    clothes_count = int.Parse(ClothesCountLabel.Text);
                    if (clothes_count != 0)
                    {
                        clothes_count -= 1;
                    }
                    ClothesCountLabel.Text = clothes_count.ToString();
                    ClothesPriceLabel.Text = (optionList[ClothesSelectOption.SelectedIndex].SH_PRO_OPTION_PRICE * clothes_count).ToString("N0") + "원";
                })
            });
            #endregion

        }

        private void BasketBtn_Clicked(object sender, EventArgs e)
        {

        }

        // 피커에서 색상 및 디자인을 선택하는 이벤트
        private void ClothesSelectOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectColor = optionList[ClothesSelectOption.SelectedIndex].SH_PRO_OPTION_COLOR;
            selectSize = optionList[ClothesSelectOption.SelectedIndex].SH_PRO_OPTION_SIZE;
            ClothesPriceLabel.Text = (optionList[ClothesSelectOption.SelectedIndex].SH_PRO_OPTION_PRICE * clothes_count).ToString("N0") + "원";
        }

        private void BackButton_Clicked(object sender, EventArgs e)
        {
            Global.isOpen_ShopDetailPage = false;
            Navigation.PopAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            Global.isOpen_ShopDetailPage = false;
            return base.OnBackButtonPressed();

        }
    }
}