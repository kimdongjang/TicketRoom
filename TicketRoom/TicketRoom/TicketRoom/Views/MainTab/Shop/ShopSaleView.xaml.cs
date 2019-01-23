using TicketRoom.Models.Custom;
using TicketRoom.Models.ShopData;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.Shop
{
    [ContentProperty("StaticResourceKey")]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShopSaleView : ContentView
    {
        string myShopName = "";
        ShopDataFunc dataclass = new ShopDataFunc();

        public ShopSaleView(string titleName)
        {
            InitializeComponent();
            myShopName = titleName;
            Init();
        }

        private void Init()
        {
            #region 쇼핑 메인 설명
            CustomLabel editor = new CustomLabel
            {
                Text = dataclass.GetShopDetailData(myShopName),
                Size = 18,
                TextColor = Color.Black,
                HeightRequest = 300,
                Margin = 10,
                IsEnabled = false,
            };
            #endregion

            #region 쇼핑 메인 홈 중 베스트 리스트
            Grid bestGrid = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Auto  },
                },
            };

            for (int i = 1; i <= dataclass.GetShopHomeBestCnt(myShopName); i++)
            {
                bestGrid.RowDefinitions.Add(new RowDefinition { Height = 100 });

                Grid best_rowGrid = new Grid
                {
                    ColumnDefinitions = {
                        new ColumnDefinition { Width = 30 },
                        new ColumnDefinition { Width = new GridLength(4, GridUnitType.Star) },
                        new ColumnDefinition { Width = new GridLength(6, GridUnitType.Star)  }
                    },
                };

                // 쇼핑몰 이미지
                Image bestimage = new Image
                {
                    Source = "shop_clothes1.jpg",
                    Aspect = Aspect.Fill,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    Margin = 5
                };

                // 쇼핑 페이지 타이틀, 평점, 세부내용 출력을 위한 내부 그리드
                Grid best_columnGrid = new Grid
                {
                    RowDefinitions =
                    {
                        new RowDefinition {Height = 10 },
                        new RowDefinition {Height = new GridLength(1, GridUnitType.Star) },
                        new RowDefinition {Height = new GridLength(1, GridUnitType.Star) },
                        new RowDefinition {Height = 28 },
                        new RowDefinition {Height = 10 },
                    },
                    Margin = new Thickness(10, 0, 10, 0),
                };

                #region 그리드 내부 타이틀, 평점, 세부내용
                // 상품 이름

                CustomLabel bestHome = new CustomLabel
                {
                    Text = "상품 이름" + i,
                    Size = 14,
                    TextColor = Color.Black,
                    MaxLines = 1,

                };
                CustomLabel bestValue = new CustomLabel
                {
                    Text = "Value" + i + "원",
                    Size = 18,
                    TextColor = Color.Black,
                };
                CustomLabel bestAddDetail = new CustomLabel
                {
                    Text = "추가 정보" + i + "으나으마ㅇ루허ㅏㄷㄱ하ㅓ",
                    Size = 14,
                    TextColor = Color.Black,
                    MaxLines = 2,
                };
                #endregion

                #region 그리드 자식으로 추가
                DetailStack.Children.Add(editor);

                best_columnGrid.Children.Add(bestHome, 0, 1);
                best_columnGrid.Children.Add(bestValue, 0, 2);
                best_columnGrid.Children.Add(bestAddDetail, 0, 3);
                best_rowGrid.Children.Add(bestimage, 1, 0);
                best_rowGrid.Children.Add(best_columnGrid, 2, 0);
                #endregion

                bestGrid.Children.Add(best_rowGrid, 0, i);

                #region 탭 클릭시 쇼핑 디테일 페이지로 이동하는 이벤트
                best_rowGrid.GestureRecognizers.Add(new TapGestureRecognizer()
                {
                    Command = new Command(() =>
                    {
                        Navigation.PushModalAsync(new ShopDetailPage(bestHome.Text));
                    })
                });
                #endregion

                // xaml 메인 그리드 1행 --> 베스트 쇼핑몰 리스트 그리드 첨부
                BestMainGrid.Children.Add(bestGrid, 0, 1);
            }
            #endregion

            #region 쇼핑 메인 홈 중 일반 리스트
            Grid naturalGrid = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Auto  },
                },
            };

            for (int i = 1; i <= dataclass.GetShopHomeNatureCnt(myShopName); i++)
            {
                naturalGrid.RowDefinitions.Add(new RowDefinition { Height = 100 });

                Grid natural_rowGrid = new Grid
                {
                    ColumnDefinitions = {
                        new ColumnDefinition { Width = 30 },
                        new ColumnDefinition { Width = new GridLength(4, GridUnitType.Star) },
                        new ColumnDefinition { Width = new GridLength(6, GridUnitType.Star)  }
                    },
                };

                // 쇼핑몰 이미지
                Image natrueimage = new Image
                {
                    Source = "shop_clothes1.jpg",
                    Aspect = Aspect.Fill,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    Margin = 5
                };

                // 쇼핑 페이지 타이틀, 평점, 세부내용 출력을 위한 내부 그리드
                Grid natural_columnGrid = new Grid
                {
                    RowDefinitions =
                    {
                        new RowDefinition {Height = 10 },
                        new RowDefinition {Height = new GridLength(1, GridUnitType.Star) },
                        new RowDefinition {Height = new GridLength(1, GridUnitType.Star) },
                        new RowDefinition {Height = 28 },
                        new RowDefinition {Height = 10 },
                    },
                    Margin = new Thickness(10, 0, 10, 0),
                };

                #region 그리드 내부 타이틀, 평점, 세부내용
                // 상품 이름
                CustomLabel naturalHome = new CustomLabel
                {
                    Text = "상품이름 " + i + "사이즈도 필요하지 안흥ㄹ까?",
                    Size = 14,
                    TextColor = Color.Black,
                    MaxLines = 1,

                };
                CustomLabel naturalValue = new CustomLabel
                {
                    Text = "Value" + i + "원",
                    Size = 18,
                    TextColor = Color.Black,
                };
                CustomLabel naturalAddDetail = new CustomLabel
                {
                    Text = "추가 정보" + i + "으나으마니으마닝ㅂ더ㅜㅎㄴ어ㅏㅗㅠ워ㅘㅠㅌ류ㅡ하ㅓㅇㄹ하ㅓㅇ루허ㅏㄷㄱ하ㅓ",
                    Size = 14,
                    TextColor = Color.Black,
                    MaxLines = 2,
                };
                #endregion

                #region 그리드 자식으로 추가
                natural_columnGrid.Children.Add(naturalHome, 0, 1);
                natural_columnGrid.Children.Add(naturalValue, 0, 2);
                natural_columnGrid.Children.Add(naturalAddDetail, 0, 3);
                natural_rowGrid.Children.Add(natrueimage, 1, 0);
                natural_rowGrid.Children.Add(natural_columnGrid, 2, 0);
                #endregion

                naturalGrid.Children.Add(natural_rowGrid, 0, i);

                #region 탭 클릭시 쇼핑 디테일 페이지로 이동하는 이벤트
                natural_rowGrid.GestureRecognizers.Add(new TapGestureRecognizer()
                {
                    Command = new Command(() =>
                    {
                        Navigation.PushModalAsync(new ShopDetailPage(naturalHome.Text));
                    })
                });
                #endregion


                // xaml 메인 그리드 1행 --> 베스트 쇼핑몰 리스트 그리드 첨부
                NatureMainGrid.Children.Add(naturalGrid, 0, 1);
            }
            #endregion
        }
    }
}