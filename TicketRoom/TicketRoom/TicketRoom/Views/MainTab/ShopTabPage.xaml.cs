using FFImageLoading.Forms;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TicketRoom.Models.Custom;
using TicketRoom.Models.ShopData;
using TicketRoom.Models.Users;
using TicketRoom.Views.MainTab.Shop;
using TicketRoom.Views.Users.CreateUser;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShopTabPage : ContentView
    {
        ShopDBFunc SH_DB = ShopDBFunc.Instance();
        public static List<MainCate> mclist;

        List<Grid> ClickList = new List<Grid>();
        Queue<string> imageList = new Queue<string>();

        public static int imagelist_count = 2;
        public int grid_count = 2;
        int columnCount = 0;
        int rowCount = 0;

        public ADRESS myAdress = new ADRESS();

        public ShopTabPage()
        {
            InitializeComponent();

            #region IOS의 경우 초기화
            if (Device.OS == TargetPlatform.iOS)
            {
                TabGrid.RowDefinitions[0].Height = 50;
            }
            #endregion

            ImageSlideAsync();

            mclist = SH_DB.GetCategoryListAsync();

            //Navigation.PushAsync(new IMPWebView());
            //Navigation.PushAsync(new IMPHybridWebView());

            // 쇼핑 탭 주소 엔트리 초기화

            #region 검색 결과 찾을 수 없을 경우
            if (mclist != null)
            {
                GridUpdate();
                RecentViewUpdate();
            }
            else
            {
                CustomLabel label = new CustomLabel
                {
                    Text = "검색 결과를 찾을 수 없습니다!",
                    Size = 18,
                    TextColor = Color.Black,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalTextAlignment = TextAlignment.Center,
                };
                //IsInputAdress = false;
                MainGrid.Children.Add(label, 0, 0);
            }
            #endregion
        }


        [System.ComponentModel.TypeConverter(typeof(System.UriTypeConverter))]
        private async void ImageSlideAsync()
        {
            while (true)
            {
                uint transitionTime = 2000;
                double displacement = image.Width;

                await Task.WhenAll(
                    image.FadeTo(0, transitionTime, Easing.Linear),
                    image.TranslateTo(-displacement, image.Y, transitionTime, Easing.CubicInOut));

                //System.Diagnostics.Debug.WriteLine("Fade Out");

                // Changes image source.
                image.Source = "shophome.jpg";
                Uri uri = new Uri("http://naver.com/");
                image.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = new Command(() =>
                        Xamarin.Forms.Device.OpenUri(uri))
                });


                await image.TranslateTo(displacement, 0, 0);
                await Task.WhenAll(
                    image.FadeTo(1, transitionTime / 6, Easing.Linear),
                    image.TranslateTo(0, image.Y, transitionTime / 6, Easing.CubicInOut));

                System.Diagnostics.Debug.WriteLine("Fade In");
                await Task.Delay(5000);

            }
        }

        // 최근 본 상품 목록 업데이트
        private void RecentViewUpdate()
        {
            SH_RecentView RecentView;
            if (Global.b_user_login == true) // 회원인 상태로 로그인이 되어있다면
            {
                RecentView = SH_DB.PostSelectRecentViewToID(Global.ID);
            }
            else // 비회원 상태
            {
                RecentView = SH_DB.PostSelectRecentViewToID(Global.non_user_id);
            }

            List<SH_Home> HomeList = new List<SH_Home>();

            if (RecentView.SH_HOME_INDEX1 != 0)
            {
                SH_Home sh = SH_DB.PostSearchHomeToHome(RecentView.SH_HOME_INDEX1);
                HomeList.Add(sh);
            }
            if (RecentView.SH_HOME_INDEX2 != 0)
            {
                SH_Home sh = SH_DB.PostSearchHomeToHome(RecentView.SH_HOME_INDEX2);
                HomeList.Add(sh);
            }
            if (RecentView.SH_HOME_INDEX3 != 0)
            {
                SH_Home sh = SH_DB.PostSearchHomeToHome(RecentView.SH_HOME_INDEX3);
                HomeList.Add(sh);
            }

            Grid ColumnGrid = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star)  },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star)  },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star)  }
                },
                Margin = new Thickness(15, 0, 15, 0),
            };
            RecentViewGrid.Children.Add(ColumnGrid);

            if (HomeList.Count == 0) // 최근 본 상품에 이미지가 없을 경우
            {
                CustomLabel errorLabel = new CustomLabel
                {
                    Size = 18,
                    TextColor = Color.Black,
                    HorizontalOptions = LayoutOptions.Center,
                    Text = "최근 본 상품이 없습니다!",
                };
                RecentViewGrid.Children.Add(errorLabel);
                return;
            }

            for (int i = 0; i < HomeList.Count; i++)
            {
                Grid inGrid = new Grid
                {
                    RowDefinitions =
                    {
                        new RowDefinition { Height = 100  },
                        new RowDefinition { Height = 30  },
                    },
                    Margin = new Thickness(15, 0, 15, 0),
                };
                ColumnGrid.Children.Add(inGrid, i, 0);
                CachedImage recentImage = new CachedImage
                {
                    LoadingPlaceholder = Global.LoadingImagePath,
                    ErrorPlaceholder = Global.LoadingImagePath,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    Aspect = Aspect.AspectFill,

                };
                CustomLabel recentLabel = new CustomLabel
                {
                    Size = 14,
                    TextColor = Color.Black,
                    HorizontalOptions = LayoutOptions.Center,
                };
                
                recentImage.Source = HomeList[i].SH_HOME_IMAGE;
                recentLabel.Text = HomeList[i].SH_HOME_NAME;

                inGrid.Children.Add(recentImage, 0, 0);
                inGrid.Children.Add(recentLabel, 0, 1);
            }
        }
        
        private void GridUpdate()
        {
            int columnindex = 3;
            int rowindex = 0;
            Grid ColumnGrid = new Grid();

            for (int i = 0; i < mclist.Count; )
            {
                if (columnindex > 2) // 열 그리드
                {
                    columnindex = 0;
                    MainGrid.RowDefinitions.Add(new RowDefinition { Height = 100 });
                    MainGrid.RowDefinitions.Add(new RowDefinition { Height = 3 });
                    ColumnGrid = new Grid
                    {
                        ColumnDefinitions =
                        {
                            new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star)},
                            new ColumnDefinition { Width = 3},
                            new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star)}
                        }
                    };
                    MainGrid.Children.Add(ColumnGrid, 0, rowindex);

                    BoxView borderR = new BoxView
                    {
                        BackgroundColor = Color.LightGray,
                        Opacity = 0.5,
                    };
                    MainGrid.Children.Add(borderR, 0, rowindex + 1);
                    rowindex += 2;
                }

                if (columnindex == 1)
                {
                    BoxView borderC = new BoxView
                    {
                        BackgroundColor = Color.LightGray,
                        Opacity = 0.5,
                    };
                    ColumnGrid.Children.Add(borderC, 1, 0);
                }
                else
                {
                    Grid inGrid = new Grid
                    {
                        RowDefinitions = {
                            new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                            new RowDefinition { Height = 50 },
                        },
                        BackgroundColor = Color.White,
                        VerticalOptions = LayoutOptions.Center,
                    };
                    ColumnGrid.Children.Add(inGrid, columnindex, 0);
                    ClickList.Add(inGrid);


                    CustomLabel label = new CustomLabel
                    {
                        Size = 18,
                        Text = mclist[i].SH_MAINCATE_NAME,
                        TextColor = Color.Black,
                        HorizontalOptions = LayoutOptions.Start,
                        VerticalOptions = LayoutOptions.Center,
                        BindingContext = i,
                        Margin = new Thickness(10,0,0,0)
                    };

                    CachedImage image = new CachedImage
                    {
                        LoadingPlaceholder = Global.LoadingImagePath,
                        ErrorPlaceholder = Global.LoadingImagePath,
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.End,
                        Aspect = Aspect.AspectFill,
                        Margin = new Thickness(0, 0, 10, 0)
                    };


                    #region 의류 카테고리 이미지 생성
                    if (mclist[i].SH_MAINCATE_NAME == "남성의류")
                    {
                        image.Source = "men_wear_icon.png";
                    }
                    else if (mclist[i].SH_MAINCATE_NAME == "여성의류")
                    {
                        image.Source = "women_icon.png";
                    }
                    #endregion

                    inGrid.Children.Add(label, 0, 0);
                    inGrid.Children.Add(image, 0, 1);
                    i++; // 내부 그리드가 추가 되었기 때문에 index증가
                }
                columnindex++;
                // 구분선
                //BoxView gridBox = new BoxView{BackgroundColor = Color.Gray};
            }

            #region 그리드 탭 이벤트
            for (int k = 0; k < ClickList.Count; k++)
            {
                Grid tempGrid = ClickList[k];
                CustomLabel tempLabel = (CustomLabel)tempGrid.Children.ElementAt(0);

                tempGrid.GestureRecognizers.Add(new TapGestureRecognizer()
                {
                    Command = new Command(() =>
                    {
                        // 탭을 한번 클릭했다면 다시 열리지 않도록 제어
                        if (Global.isOpen_ShopListPage == true)
                        {
                            return;
                        }
                        Global.isOpen_ShopListPage = true;

                        int tempIndex = 0;
                        for (int i = 0; i < mclist.Count; i++)
                        {
                            if (tempLabel.Text == mclist[i].SH_MAINCATE_NAME)
                            {
                                tempIndex = mclist[i].SH_MAINCATE_INDEX; // 메인 카테고리 인덱스를 찾음
                                Global.OnShopListTapIndex = i;
                            }
                        }
                        Navigation.PushAsync(new ShopListPage(tempIndex)); // 메인 카테고리 인덱스 기반으로 서브 카테고리(리스트 페이지)를 오픈
                    })
                });
            }
            #endregion

        }


        private void Slider_Focused(object sender, FocusEventArgs e)
        {
        }
    }
}