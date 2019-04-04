using FFImageLoading.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using TicketRoom.Models.Custom;
using TicketRoom.Models.ShopData;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.Shop
{
    [ContentProperty("StaticResourceKey")]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShopSaleView : ContentView
    {
        ShopDBFunc SH_DB = ShopDBFunc.Instance();
        public static bool isOpenPage = false;

        List<SH_Product> productList = new List<SH_Product>();
        SH_Home home;

        string myShopName = "";

        public ShopSaleView(string titleName, SH_Home home)
        {
            InitializeComponent();
            this.home = home;
            #region 네트워크 상태 확인
            var current_network = Connectivity.NetworkAccess; // 현재 네트워크 상태
            if (current_network != NetworkAccess.Internet) // 네트워크 연결 불가
            {
                productList = null;
            }
            #endregion
            #region 네트워크 연결 가능
            else
            {
                productList = SH_DB.PostSearchProductToHome(home.SH_HOME_INDEX);

            }
            #endregion
            Init();

            myShopName = titleName;
        }

        private void Init()
        {
            #region 쇼핑 메인 설명
            Device.BeginInvokeOnMainThread(async () =>
            {
                CustomLabel editor = new CustomLabel
                {
                    Text = home.SH_HOME_DETAIL,
                    Size = 14,
                    TextColor = Color.Gray,
                    Margin = 10,
                    IsEnabled = false,
                };
                DetailStack.Children.Add(editor);
                ///Code to update view here
            });
            ///Code to update view here
            ///
            #endregion

            Grid bestGrid = new Grid();

            #region 쇼핑 메인 홈 중 베스트 리스트
            #region 네트워크 연결 불가
            if (productList == null)
            {
                bestGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                CustomLabel best_error = new CustomLabel
                {
                    Text = "네트워크에 연결 할 수 없습니다!",
                    Size = 18,
                    TextColor = Color.Black,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                };
                bestGrid.Children.Add(best_error, 0, 0);
                return;
            }
            #endregion
            if (productList.Count == 0) // 등록된 상품이 없을 경우
            {
                bestGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                CustomLabel best_error = new CustomLabel
                {
                    Text = "등록된 상품이 없습니다.",
                    Size = 18,
                    TextColor = Color.Black,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                };
                bestGrid.Children.Add(best_error, 0, 0);
                return;
            }
            for (int i = 0; i < productList.Count; i++)
            {
                // 베스트 이미지로 분류가 되었을 경우
                if (productList[i].SH_PRODUCT_ISBEST == "TRUE")
                {
                    bestGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

                    Grid best_rowGrid = new Grid
                    {
                        ColumnDefinitions = {
                        new ColumnDefinition { Width = 20 },
                        new ColumnDefinition { Width = new GridLength(4, GridUnitType.Star) },
                        new ColumnDefinition { Width = new GridLength(6, GridUnitType.Star)  }
                    },
                    };

                    // 쇼핑몰 이미지
                    CachedImage bestimage = new CachedImage
                    {
                        LoadingPlaceholder = Global.LoadingImagePath,
                        ErrorPlaceholder = Global.NotFoundImagePath,
                        Source = ImageSource.FromUri(new Uri(productList[i].SH_PRODUCT_MAINIMAGE)),
                        Aspect = Aspect.AspectFit,
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Center,
                        Margin = 5,
                        HeightRequest = 100,
                    };

                    // 쇼핑 페이지 타이틀, 평점, 세부내용 출력을 위한 내부 그리드
                    Grid best_columnGrid = new Grid
                    {
                        RowDefinitions =
                    {
                        new RowDefinition {Height = new GridLength(1, GridUnitType.Auto) },
                        new RowDefinition {Height = new GridLength(1, GridUnitType.Auto) },
                        new RowDefinition {Height = new GridLength(1, GridUnitType.Auto) },
                        new RowDefinition {Height = new GridLength(1, GridUnitType.Auto) },
                        new RowDefinition {Height = new GridLength(1, GridUnitType.Auto) },
                    },
                        Margin = new Thickness(10, 0, 10, 0),
                    };

                    #region 그리드 내부 타이틀, 평점, 세부내용
                    // 상품 이름

                    CustomLabel bestHome = new CustomLabel
                    {
                        Text = "상품 이름 : " + productList[i].SH_PRODUCT_NAME,
                        Size = 18,
                        TextColor = Color.Black,
                        MaxLines = 1,

                    };
                    CustomLabel bestValue = new CustomLabel
                    {
                        Text = productList[i].SH_PRODUCT_PRICE.ToString("N0") + "원",
                        Size = 14,
                        TextColor = Color.Black,
                    };
                    CustomLabel bestAddDetail = new CustomLabel
                    {
                        Text = productList[i].SH_PRODUCT_CONTENT,
                        Size = 14,
                        TextColor = Color.Black,
                        MaxLines = 2,
                    };
                    #endregion

                    #region 그리드 자식으로 추가
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
                            // 탭을 한번 클릭했다면 다시 열리지 않도록 제어
                            if (Global.isOpen_ShopDetailPage == true)
                            {
                                return;
                            }

                            string tempString = bestHome.Text.Replace("상품 이름 : ", "");
                            for(int j = 0; j < productList.Count; j++)
                            {
                                if(tempString == productList[j].SH_PRODUCT_NAME)
                                {
                                    Navigation.PushAsync(new ShopDetailPage(productList[j].SH_PRODUCT_NAME, productList[j].SH_PRODUCT_INDEX, home));
                                    Global.isOpen_ShopDetailPage = true;
                                }
                            }
                        })
                    });
                    #endregion

                }
            }
            // xaml 메인 그리드 1행 --> 베스트 쇼핑몰 리스트 그리드 첨부
            BestMainGrid.Children.Add(bestGrid, 0, 1);
            #endregion

            #region 쇼핑 메인 홈 중 일반 리스트
            Grid naturalGrid = new Grid();
            #region 네트워크 연결 불가
            if (productList == null)
            {
                naturalGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                CustomLabel natural_error = new CustomLabel
                {
                    Text = "네트워크에 연결 할 수 없습니다!",
                    Size = 18,
                    TextColor = Color.Black,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                };
                naturalGrid.Children.Add(natural_error, 0, 0);
                return;
            }
            #endregion
            if (productList.Count == 0) // 등록된 상품이 없을 경우
            {
                naturalGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                CustomLabel natural_error = new CustomLabel
                {
                    Text = "등록된 상품이 없습니다.",
                    Size = 18,
                    TextColor = Color.Black,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                };
                naturalGrid.Children.Add(natural_error, 0, 0);
                return;
            }
            for (int i = 0; i < productList.Count; i++)
            {
                naturalGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

                Grid natural_rowGrid = new Grid
                {
                    ColumnDefinitions = {
                        new ColumnDefinition { Width = 20 },
                        new ColumnDefinition { Width = new GridLength(4, GridUnitType.Star) },
                        new ColumnDefinition { Width = new GridLength(6, GridUnitType.Star)  }
                    },
                };

                // 쇼핑몰 이미지
                CachedImage natrueimage = new CachedImage
                {
                    LoadingPlaceholder = Global.LoadingImagePath,
                    ErrorPlaceholder = Global.NotFoundImagePath,
                    Source = ImageSource.FromUri(new Uri(productList[i].SH_PRODUCT_MAINIMAGE)),
                    Aspect = Aspect.AspectFit,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    Margin = 5,
                    HeightRequest = 100,
                };

                // 쇼핑 페이지 타이틀, 평점, 세부내용 출력을 위한 내부 그리드
                Grid natural_columnGrid = new Grid
                {
                    RowDefinitions =
                    {
                        new RowDefinition {Height = new GridLength(1, GridUnitType.Auto) },
                        new RowDefinition {Height = new GridLength(1, GridUnitType.Auto) },
                        new RowDefinition {Height = new GridLength(1, GridUnitType.Auto) },
                        new RowDefinition {Height = new GridLength(1, GridUnitType.Auto) },
                        new RowDefinition {Height = new GridLength(1, GridUnitType.Auto) },
                    },
                    Margin = new Thickness(10, 0, 10, 0),
                };

                #region 그리드 내부 타이틀, 평점, 세부내용
                // 상품 이름
                CustomLabel naturalHome = new CustomLabel
                {
                    Text = "상품 이름 : " + productList[i].SH_PRODUCT_NAME,
                    Size = 18,
                    TextColor = Color.Black,
                    MaxLines = 1,

                };
                CustomLabel naturalValue = new CustomLabel
                {
                    Text = productList[i].SH_PRODUCT_PRICE.ToString("N0") + "원",
                    Size = 14,
                    TextColor = Color.Black,
                };
                CustomLabel naturalAddDetail = new CustomLabel
                {
                    Text = productList[i].SH_PRODUCT_CONTENT,
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
                        // 탭을 한번 클릭했다면 다시 열리지 않도록 제어
                        if (Global.isOpen_ShopDetailPage == true)
                        {
                            return;
                        }

                        string tempString = naturalHome.Text.Replace("상품 이름 : ", "");
                        for (int j = 0; j < productList.Count; j++)
                        {
                            if (tempString == productList[j].SH_PRODUCT_NAME)
                            {
                                Navigation.PushAsync(new ShopDetailPage(productList[j].SH_PRODUCT_NAME, productList[j].SH_PRODUCT_INDEX, home));
                                Global.isOpen_ShopDetailPage = true;
                            }
                        }
                    })
                });
                #endregion
            }
            // xaml 메인 그리드 1행 --> 베스트 쇼핑몰 리스트 그리드 첨부
            NatureMainGrid.Children.Add(naturalGrid, 0, 1);
            #endregion
        }
    }
}