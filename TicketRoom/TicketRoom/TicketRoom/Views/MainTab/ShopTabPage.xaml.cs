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
using TicketRoom.Views.MainTab.Shop;
using TicketRoom.Views.Users.CreateUser;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShopTabPage : ContentView
    {
        List<Grid> ClickList = new List<Grid>();
        List<MainCate> mcl;
        Queue<string> imageList = new Queue<string>();

        public static int imagelist_count = 2;
        public int grid_count = 2;
        int columnCount = 0;
        int rowCount = 0;


        public ShopTabPage()
        {
            InitializeComponent();
            ImageSlideAsync();
            GetCategoryListAsync();
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
                image.Source = ImageSource.FromFile("shophome.jpg");
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
        private void Init()
        {
            #region 그리드 탭 이벤트

            for (int i = 0; i < ClickList.Count; i++)
            {
                Grid tempGrid = ClickList[i];
                var temp = (CustomLabel)tempGrid.Children.ElementAt(1);
                CustomLabel cl = (CustomLabel)temp;

                tempGrid.GestureRecognizers.Add(new TapGestureRecognizer()
                {
                    Command = new Command(() =>
                    {
                        Navigation.PushModalAsync(new ShopListPage(cl.Text));
                    })
                });
            }

            #endregion

        }

        #region 서버에서 GET메소드/메인 카테고리 리스트 요청
        private async void GetCategoryListAsync()
        {

            // loading 
            Loading loadingScreen = new Loading(true);
            await Navigation.PushModalAsync(loadingScreen);
            mcl = new List<MainCate>();

            /*
            string str = @"{";
            str += "word : ' " + word;
            str += " '}";

            //// JSON 문자열을 파싱하여 JObject를 리턴
            JObject jo = JObject.Parse(str);*/

            UTF8Encoding encoder = new UTF8Encoding();
            //byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "SH_SearchMainCate") as HttpWebRequest;
            request.Method = "GET";
            request.ContentType = "application/json";
            //request.ContentLength = data.Length;

            //request.GetRequestStream().Write(data, 0, data.Length);


            try
            {
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {

                    if (response.StatusCode != HttpStatusCode.OK)
                        Console.Out.WriteLine("Error fetching data. Server returned status code: {0}", response.StatusCode);
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {

                        // readdata
                        var readdata = reader.ReadToEnd();
                        mcl = JsonConvert.DeserializeObject<List<MainCate>>(readdata);

                    }
                    GridUpdate();

                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
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

            // loading end
            await Navigation.PopModalAsync();
        }
        #endregion

        private void GridUpdate()
        {
            int three_row = 0;
            int three_col = 0;
            Grid RowGrid = new Grid();

            for (int i = 0; i < mcl.Count; i++)
            {
                if (i % 3 == 0)
                {
                    MainGrid.RowDefinitions.Add(new RowDefinition { Height = 130 });

                    RowGrid = new Grid
                    {
                        ColumnDefinitions =
                        {
                            new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                            new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                            new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                        },
                        RowSpacing = 0,
                        ColumnSpacing = 0,
                    };
                    MainGrid.Children.Add(RowGrid, 0, three_row); // 열 그리드 추가
                    three_row++;
                }
                if (three_col >= 3)
                {
                    three_col = 0;
                }
                // 열 추가를 위한 그리드 생성


                BoxView gridBox = new BoxView // 구분선
                {
                    BackgroundColor = Color.Gray
                };
                Grid inGrid = new Grid
                {
                    RowDefinitions = {
                        new RowDefinition { Height = 100 },
                        new RowDefinition { Height = 30 },
                    },
                    BackgroundColor = Color.White,
                    Margin = 0.5,
                };
                RowGrid.Children.Add(gridBox, three_col, 0);
                RowGrid.Children.Add(inGrid, three_col, 0);
                three_col++;


                StackLayout SLayout = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                };
                Image image = new Image();
                CustomLabel label = new CustomLabel();

                #region 의류 카테고리 이미지 생성
                if (mcl[i].SH_MAINCATE_NAME == "단체복")
                {
                    image = new Image
                    {
                        Source = "uniform_icon.png",
                        WidthRequest = 100,
                        HeightRequest = 100,
                    };
                }
                else if (mcl[i].SH_MAINCATE_NAME == "여성의류")
                {
                    image = new Image
                    {
                        Source = "women_icon.png",
                        WidthRequest = 100,
                        HeightRequest = 100,
                    };
                }
                else if (mcl[i].SH_MAINCATE_NAME == "남성의류")
                {
                    image = new Image
                    {
                        Source = "men_wear_icon.png",
                        WidthRequest = 100,
                        HeightRequest = 100,
                    };
                }
                else if (mcl[i].SH_MAINCATE_NAME == "기프티콘")
                {
                    image = new Image
                    {
                        Source = "gift_icon.png",
                        WidthRequest = 100,
                        HeightRequest = 100,
                    };
                }
                else
                {
                    image = new Image
                    {
                        Source = "ready.png",
                        WidthRequest = 100,
                        HeightRequest = 100,
                    };
                }
                #endregion

                label = new CustomLabel
                {
                    Size = 18,
                    Text = mcl[i].SH_MAINCATE_NAME,
                    TextColor = Color.Black,
                    HorizontalOptions = LayoutOptions.Center,
                    BindingContext = i,
                };

                inGrid.Children.Add(SLayout, 0, 0);
                inGrid.Children.Add(image, 0, 0);
                inGrid.Children.Add(label, 0, 1);

                #region 그리드 탭 이벤트

                Grid tempGrid = inGrid;

                inGrid.GestureRecognizers.Add(new TapGestureRecognizer()
                {
                    Command = new Command(() =>
                    {
                        Navigation.PushModalAsync(new ShopListPage(label.Text));
                    })
                });
                /*
                ClickList.Add(inGrid);

                for (int k = 0; k < ClickList.Count; k++)
                {
                }*/
                #endregion
            }
        }


        private void Slider_Focused(object sender, FocusEventArgs e)
        {
        }

        private void EntryAdress_Focused(object sender, FocusEventArgs e)
        {
            EntryAdress.Text = "";
            Navigation.PushModalAsync(new InputAdress(this));

        }
    }
}