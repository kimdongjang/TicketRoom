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
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.Shop
{
    [ContentProperty("StaticResourceKey")]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShopSaleView : ContentView
    {
        List<SH_Product> productList = new List<SH_Product>();
        SH_Home home;

        string myShopName = "";
        ShopDataFunc dataclass = new ShopDataFunc();

        public ShopSaleView(string titleName, SH_Home home)
        {
            InitializeComponent();
            this.home = home;
            PostSearchProductToHome(home.SH_HOME_INDEX);

            myShopName = titleName;
        }


        // DB에서 홈 인덱스로 상품 목록을 가져오기
        private void PostSearchProductToHome(int homeIndex)
        {
            productList = new List<SH_Product>();
            string str = @"{";
            str += "homeIndex : " + homeIndex;
            str += "}";

            //// JSON 문자열을 파싱하여 JObject를 리턴
            JObject jo = JObject.Parse(str);

            UTF8Encoding encoder = new UTF8Encoding();
            byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "SH_SearchProductToHome") as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = data.Length;

            request.GetRequestStream().Write(data, 0, data.Length);


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
                        productList = JsonConvert.DeserializeObject<List<SH_Product>>(readdata);
                    }
                }
                Init();
            }
            catch
            {
                Label label = new Label
                {
                    Text = "검색 결과를 찾을 수 없습니다!",
                    FontSize = 18,
                    TextColor = Color.Black,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalTextAlignment = TextAlignment.Center,
                };
                //MainGrid.Children.Add(label, 0, 1);
            }
        }


        private void Init()
        {
            #region 쇼핑 메인 설명
            CustomLabel editor = new CustomLabel
            {
                Text = home.SH_HOME_DETAIL,
                Size = 18,
                TextColor = Color.Black,
                HeightRequest = 300,
                Margin = 10,
                IsEnabled = false,
            };
            #endregion

            #region 쇼핑 메인 홈 중 베스트 리스트
            Grid bestGrid = new Grid();

            for (int i = 0; i < productList.Count; i++)
            {
                // 베스트 이미지로 분류가 되었을 경우
                if (productList[i].SH_PRODUCT_ISBEST == "TRUE")
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
                        Source = ImageSource.FromUri(new Uri(productList[i].SH_PRODUCT_MAINIMAGE)),
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
                        Text = "상품 이름 : " + productList[i].SH_PRODUCT_NAME,
                        Size = 14,
                        TextColor = Color.Black,
                        MaxLines = 1,

                    };
                    CustomLabel bestValue = new CustomLabel
                    {
                        Text = productList[i].SH_PRODUCT_PRICE.ToString("N0") + "원",
                        Size = 18,
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
                            string tempString = bestHome.Text.Replace("상품 이름 : ", "");
                            for(int j = 0; j < productList.Count; j++)
                            {
                                if(tempString == productList[j].SH_PRODUCT_NAME)
                                {
                                    Navigation.PushModalAsync(new ShopDetailPage(productList[j].SH_PRODUCT_NAME, productList[j].SH_PRODUCT_INDEX));
                                }
                            }
                        })
                    });
                    #endregion

                }
                // xaml 메인 그리드 1행 --> 베스트 쇼핑몰 리스트 그리드 첨부
                BestMainGrid.Children.Add(bestGrid, 0, 1);
            }
            #endregion

            #region 쇼핑 메인 홈 중 일반 리스트
            Grid naturalGrid = new Grid();

            for (int i = 0; i < productList.Count; i++)
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
                    Source = ImageSource.FromUri(new Uri(productList[i].SH_PRODUCT_MAINIMAGE)),
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
                    Text = "상품이름 " + productList[i].SH_PRODUCT_NAME,
                    Size = 14,
                    TextColor = Color.Black,
                    MaxLines = 1,

                };
                CustomLabel naturalValue = new CustomLabel
                {
                    Text = productList[i].SH_PRODUCT_PRICE.ToString("N0") + "원",
                    Size = 18,
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
                        string tempString = naturalHome.Text.Replace("상품 이름 : ", "");
                        for (int j = 0; j < productList.Count; j++)
                        {
                            if (tempString == productList[j].SH_PRODUCT_NAME)
                            {
                                Navigation.PushModalAsync(new ShopDetailPage(productList[j].SH_PRODUCT_NAME, productList[j].SH_PRODUCT_INDEX));
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