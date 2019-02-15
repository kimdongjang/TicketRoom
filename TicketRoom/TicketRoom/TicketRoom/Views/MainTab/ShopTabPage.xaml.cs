using Newtonsoft.Json;
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

            // 쇼핑 탭 주소 엔트리 초기화
            EntryAdress.Text = Global.adress.ROADADDR;
            if(Global.adress.ROADADDR == null)
            {
                EntryAdress.Text = "지번 또는 도로명 주소를 입력해주십시오.";
            }

            #region 검색 결과 찾을 수 없을 경우
            if (mclist != null)
            {
                GridUpdate();
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
                image.Source = ImageSource.FromUri(new Uri("http://221.141.58.49:8088/img/default/shophome.jpg"));
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
        
        private void GridUpdate()
        {
            int row = -1;
            int column = 2;
            Grid RowGrid = new Grid();

            for (int i = 0; i < mclist.Count; i++)
            {
                if (column >= 2)
                {
                    MainGrid.RowDefinitions.Add(new RowDefinition { Height = 200 });
                    row++;

                    RowGrid = new Grid
                    {
                        ColumnDefinitions =
                        {
                            new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                            new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                        },
                        RowSpacing = 0,
                        ColumnSpacing = 0,
                        BackgroundColor = Color.White,
                    };
                    column = 0;
                    MainGrid.Children.Add(RowGrid, 0, row);
                }
                // 구분선
                //BoxView gridBox = new BoxView{BackgroundColor = Color.Gray};
                Grid inGrid = new Grid
                {
                    RowDefinitions = {
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                        new RowDefinition { Height = 30 },
                    },
                    BackgroundColor = Color.White,
                    Margin = 0.5,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                };
                //RowGrid.Children.Add(gridBox, column, row);
                RowGrid.Children.Add(inGrid, column, row);
                column++;

                ClickList.Add(inGrid);

                StackLayout SLayout = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                };
                Image image = new Image();
                CustomLabel label = new CustomLabel();

                #region 의류 카테고리 이미지 생성
                if (mclist[i].SH_MAINCATE_NAME == "남성의류")
                {
                    image = new Image
                    {
                        Source = "uniform_icon",
                        WidthRequest = 100,
                        HeightRequest = 100,
                    };
                }
                else if (mclist[i].SH_MAINCATE_NAME == "여성의류")
                {
                    image = new Image
                    {
                        Source = ImageSource.FromUri(new Uri("http://221.141.58.49:8088/img/default/women_icon.png")),
                        WidthRequest = 100,
                        HeightRequest = 100,
                    };
                }
                else
                {
                    image = new Image
                    {
                        Source = ImageSource.FromUri(new Uri("http://221.141.58.49:8088/img/default/ready.png")),
                        WidthRequest = 100,
                        HeightRequest = 100,
                    };
                }
                #endregion

                label = new CustomLabel
                {
                    Size = 18,
                    Text = mclist[i].SH_MAINCATE_NAME,
                    TextColor = Color.Black,
                    HorizontalOptions = LayoutOptions.Center,
                    BindingContext = i,
                };

                inGrid.Children.Add(SLayout, 0, 0);
                inGrid.Children.Add(image, 0, 0);
                inGrid.Children.Add(label, 0, 1);
            }

            #region 그리드 탭 이벤트
            for (int k = 0; k < ClickList.Count; k++)
            {
                Grid tempGrid = ClickList[k];
                CustomLabel tempLabel = (CustomLabel)tempGrid.Children.ElementAt(2);

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

        private void EntryAdress_Focused(object sender, FocusEventArgs e)
        {
            EntryAdress.Unfocus();
            Navigation.PushModalAsync(new InputAdress(this));

        }
    }
}