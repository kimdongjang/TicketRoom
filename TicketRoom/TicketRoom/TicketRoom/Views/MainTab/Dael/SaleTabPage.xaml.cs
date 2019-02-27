using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using TicketRoom.Models.Gift;
using TicketRoom.Views.MainTab.Dael.Sale;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.Dael
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SaleTabPage : ContentView
    {
        string categorynum = "";

        public SaleTabPage(string categorynum)
        {
            InitializeComponent();
            this.categorynum = categorynum;
            SelectSaleCategory(categorynum);
            ShowPoint();
        }

        private void ShowPoint()
        {
            if (Global.b_user_login)
            {
                string str = @"{";
                str += "USER_ID:'" + Global.ID;  //아이디찾기에선 Name으로 
                str += "'}";

                //// JSON 문자열을 파싱하여 JObject를 리턴
                JObject jo = JObject.Parse(str);

                UTF8Encoding encoder = new UTF8Encoding();
                byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

                //request.Method = "POST";
                HttpWebRequest request = WebRequest.Create(Global.WCFURL + "SelectUserPoint") as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = data.Length;

                //request.Expect = "application/json";

                request.GetRequestStream().Write(data, 0, data.Length);

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        Console.Out.WriteLine("Error fetching data. Server returned status code: {0}", response.StatusCode);
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        var readdata = reader.ReadToEnd();
                        string test = JsonConvert.DeserializeObject<string>(readdata);
                        Point_label.Text = int.Parse(test).ToString("N0");
                    }
                }
            }
            else
            {
                Point_label.Text = int.Parse("0").ToString("N0");
            }
        }

        private void SelectSaleCategory(string categorynum)
        {
            string str = @"{";
            str += "CategoryNum:'" + categorynum;  //아이디찾기에선 Name으로 
            str += "'}";

            //// JSON 문자열을 파싱하여 JObject를 리턴
            JObject jo = JObject.Parse(str);

            UTF8Encoding encoder = new UTF8Encoding();
            byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

            //request.Method = "POST";
            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "SelectSaleProduct") as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = data.Length;

            //request.Expect = "application/json";

            request.GetRequestStream().Write(data, 0, data.Length);

            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                    Console.Out.WriteLine("Error fetching data. Server returned status code: {0}", response.StatusCode);
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    var readdata = reader.ReadToEnd();
                    List<G_ProductInfo> test = JsonConvert.DeserializeObject<List<G_ProductInfo>>(readdata);
                    Showlist(test);
                }
            }
        }

        private void Showlist(List<G_ProductInfo> salelist)
        {
            int row = 1;

            var label_tap = new TapGestureRecognizer();
            label_tap.Tapped += (s, e) =>
            {
                if (Global.isgiftlistcliecked)
                {
                    Global.isgiftlistcliecked = false;
                    
                    Grid g = (Grid)s;
                    Navigation.PushAsync(new SalePage(salelist[int.Parse(g.BindingContext.ToString())]));
                }
            };

            if (salelist.Count == 0)
            {
                Salelist_Grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                #region 상품이 준비중
                Label nullproduct = new Label
                {
                    Text = "상품 준비중입니다.",
                    FontSize = 25,
                    TextColor = Color.Black,
                    VerticalOptions = LayoutOptions.Center,
                    YAlign = TextAlignment.Center,
                    HorizontalOptions = LayoutOptions.Center
                };
                #endregion
                Salelist_Grid.Children.Add(nullproduct, 0, 1);
                return;
            }

            for (int i = 0; i < salelist.Count; i++)
            {
                Salelist_Grid.RowDefinitions.Add(new RowDefinition { Height = 100 });
                Salelist_Grid.RowDefinitions.Add(new RowDefinition { Height = 1 });
                #region list 그리드
                Grid listgrid = new Grid
                {
                    Margin = new Thickness(15, 5, 0, 5),
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    RowSpacing = 0,
                    ColumnSpacing = 0,
                    BindingContext = i,
                    ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = 100 },
                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
                    }
                };
                #endregion

                #region 이미지
                Image image = new Image
                {
                    Source = salelist[i].PRODUCTIMAGE,
                    BackgroundColor = Color.White,
                    VerticalOptions = LayoutOptions.Center
                };
                #endregion

                #region label 그리드
                Grid labelgrid = new Grid
                {
                    Margin = new Thickness(10, 0, 0, 0),
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    RowSpacing = 0,
                    ColumnSpacing = 0,
                    RowDefinitions =
                    {
                        new RowDefinition { Height = 25 },
                        new RowDefinition { Height = 30 },
                        new RowDefinition { Height = 20 }
                    }
                };
                #endregion

                #region 상풍권 이름 Label
                Label Name_label = new Label
                {
                    Text = salelist[i].PRODUCTTYPE + " " + salelist[i].PRODUCTVALUE,
                    FontSize = 18,
                    TextColor = Color.Black,
                    VerticalOptions = LayoutOptions.Start,
                    YAlign = TextAlignment.Center,
                    HorizontalOptions = LayoutOptions.Start
                };
                #endregion

                #region 할인율 Label
                var formattedString = new FormattedString();
                formattedString.Spans.Add(new Span
                {
                    Text = "고객구매가(할인율) : ",
                    LineHeight = 1.8,
                    TextColor = Color.Black
                });

                formattedString.Spans.Add(new Span
                {
                    Text = salelist[i].SALEDISCOUNTPRICE + " [" + salelist[i].SALEDISCOUNTRATE + "%]",
                    LineHeight = 1.8,
                    TextColor = Color.Blue
                });

                Label discountrate_label = new Label
                {
                    FormattedText = formattedString,
                    FontSize = 12,
                    TextColor = Color.Black,
                    VerticalOptions = LayoutOptions.Start,
                    YAlign = TextAlignment.Center,
                    HorizontalOptions = LayoutOptions.Start
                };
                #endregion

                #region label 그리드에 추가
                labelgrid.Children.Add(Name_label, 0, 1);         //약관 그리드에 라벨추가
                labelgrid.Children.Add(discountrate_label, 0, 2);         //약관 그리드에 Radio이미지 추가
                #endregion

                #region label 그리드에 추가
                listgrid.Children.Add(image, 0, 0); //부모그리드에 약관 그리드 추가
                listgrid.Children.Add(labelgrid, 1, 0);         //약관 그리드에 라벨추가
                #endregion

                #region Purchaselist 그리드에 추가
                Salelist_Grid.Children.Add(listgrid, 0, row); //부모그리드에 약관 그리드 추가
                row++;
                #endregion

                #region list 그리드 클릭이벤트
                listgrid.GestureRecognizers.Add(label_tap); //라벨 클릭 이벤트 등록
                #endregion


                BoxView gridline = new BoxView
                {
                    BackgroundColor = Color.FromHex("#f4f2f2"),
                    HeightRequest = 1,
                    VerticalOptions = LayoutOptions.End,
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };
                Salelist_Grid.Children.Add(gridline, 0, row);
                row++;
            }
        }
    }
}