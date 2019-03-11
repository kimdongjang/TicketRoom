using FFImageLoading.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using TicketRoom.Models.Custom;
using TicketRoom.Models.Gift;
using TicketRoom.Services;
using TicketRoom.Views.MainTab.Dael;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DealTabPage : ContentView
    {
        MainPage mainPage;
        GiftDBFunc giftDBFunc = GiftDBFunc.Instance();
        List<G_DealInfo> g_DealInfolist = new List<G_DealInfo>();
        public DealTabPage(MainPage mainPage)
        {
            InitializeComponent();
            this.mainPage = mainPage;
            #region IOS의 경우 초기화
            if (Device.OS == TargetPlatform.iOS)
            {
                TabGrid.RowDefinitions[0].Height = 50;
            }
            #endregion
            
            Showdeal();
            ShowPoint();
            SelectAllCategory();
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
                        if (test != null&& test != "null")
                        {
                            MyPointLabel.Text = "보유포인트 : " + int.Parse(test).ToString("N0");
                        }
                        else
                        {
                            MyPointLabel.Text = "보유 포인트 : 0";
                        }
                        
                    }
                }
            }
            else
            {
                MyPointLabel.Text = "보유포인트 : "  + int.Parse("0").ToString("N0");
            }
        }

        private void Showdeal()
        {
            g_DealInfolist = giftDBFunc.SelectDealList();
            if (g_DealInfolist.Count == 0)
            {
                RealTimeGrid.RowDefinitions.Add(new RowDefinition{ Height = 30 });
                CustomLabel label = new CustomLabel
                {
                    Text = "거래내역이 없습니다",
                    Size = 14,
                    TextColor = Color.Black,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center
                };
                RealTimeGrid.Children.Add(label, 0, 1);         //실시간거래 그리드에 라벨추가
                return;
            }
            
            for (int i = 0; i < 3; i++)
            {
                RealTimeGrid.RowDefinitions.Add(new RowDefinition { Height = 30 });
                Grid inGrid = new Grid
                {
                    ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(7, GridUnitType.Star) },
                },
                    ColumnSpacing = 5,
                };

                // 결제 상태 레이블
                CustomLabel statusLabel = new CustomLabel
                {
                    BackgroundColor = Color.CornflowerBlue,
                    TextColor = Color.White,
                    Size = 14,
                    Text = "구매완료",
                    WidthRequest = 30,
                };

                inGrid.Children.Add(statusLabel, 0, 0);

                string name = "";
                string title = g_DealInfolist[i].TITLE;
                if(g_DealInfolist[i].NAME!=null&& g_DealInfolist[i].NAME != "")
                {
                    if (g_DealInfolist[i].NAME.Length == 2)
                    {
                        name = g_DealInfolist[i].NAME.Remove(1) + "*";
                    }
                    else if (g_DealInfolist[i].NAME.Length == 3)
                    {
                        name = g_DealInfolist[i].NAME.Remove(1) + "*" + g_DealInfolist[i].NAME.Remove(0, 2);
                    }
                    else if (g_DealInfolist[i].NAME.Length == 4)
                    {
                        name = g_DealInfolist[i].NAME.Remove(1) + "**" + g_DealInfolist[i].NAME.Remove(0, 3);
                    }
                }

                if (g_DealInfolist[i].ISCHECK.Equals("1"))
                {
                    title += " 구매";
                }
                else
                {
                    title += " 판매";
                }

                DateTime date = DateTime.ParseExact(g_DealInfolist[i].TOTALDATE, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                string time = date.TimeOfDay.ToString();
                string s4 = date.Day.ToString();
                string hour = date.Hour.ToString();
                string minute = date.Minute.ToString();

                CustomLabel label = new CustomLabel
                {
                    Text = name + " " + title + "["+ hour+":"+minute+"]",
                    Size = 10,
                    TextColor = Color.Black,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.Start
                };
                inGrid.Children.Add(statusLabel, 1, 0);
                RealTimeGrid.Children.Add(inGrid, 0, i + 1);         //실시간거래 그리드에 행 추가
            }
        }


        

        private void SelectAllCategory()
        {
            //request.Method = "POST";
            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "SelectAllCategory") as HttpWebRequest;
            request.Method = "GET";

            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                    Console.Out.WriteLine("Error fetching data. Server returned status code: {0}", response.StatusCode);
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    var readdata = reader.ReadToEnd();
                    List<G_CategoryInfo> test = JsonConvert.DeserializeObject<List<G_CategoryInfo>>(readdata);
                    if (test != null)
                    {
                        Showimge(test);
                    }
                }
            }

        }

        private void Showimge(List<G_CategoryInfo> categories)
        {
            if (categories.Count == 0)
            {
                if (categories[0].Error == null || categories[0].Error == "")
                {
                    CustomLabel label = new CustomLabel
                    {
                        Text = "상품권 목록을 불러 올 수 없습니다!",
                        Size = 10,
                        TextColor = Color.Black,
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Center
                    };
                    CategoryGrid.Children.Add(label, 0, 0);         //실시간거래 그리드에 라벨추가
                    return;
                }
            }

            int columnindex = 3;
            int rowindex = 0;
            Grid ColumnGrid = new Grid();

            for (int i = 0; i < categories.Count;)
            {          
                if (columnindex > 2) // 열 그리드
                {
                    columnindex = 0;
                    CategoryGrid.RowDefinitions.Add(new RowDefinition { Height = 100 });
                    CategoryGrid.RowDefinitions.Add(new RowDefinition { Height = 3 });
                    ColumnGrid = new Grid
                    {
                        ColumnDefinitions =
                        {
                            new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star)},
                            new ColumnDefinition { Width = 3},
                            new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star)}
                        },
                        RowSpacing = 0,
                    };
                    CategoryGrid.Children.Add(ColumnGrid, 0, rowindex);

                    BoxView borderR = new BoxView
                    {
                        BackgroundColor = Color.LightGray,
                        Opacity = 0.5,
                    };
                    CategoryGrid.Children.Add(borderR, 0, rowindex+1);
                    rowindex+=2;
                }

                if(columnindex == 1)
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
                    // 내부 그리드
                    Grid inGrid = new Grid
                    {
                        RowDefinitions =
                        {
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Auto)},
                        new RowDefinition { Height = 60 },
                        },
                        BindingContext = i+1,
                    };
                    ColumnGrid.Children.Add(inGrid, columnindex, 0);


                    CustomLabel nameLabel = new CustomLabel
                    {
                        Text = categories[i].Name,
                        Size = 14,
                        TextColor = Color.Black,
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Start,
                        Margin = new Thickness(15, 15, 0, 0),
                        FontAttributes = FontAttributes.Bold,
                    };
                    inGrid.Children.Add(nameLabel, 0, 0);

                    CachedImage image = new CachedImage
                    {
                        LoadingPlaceholder = Global.LoadingImagePath,
                        ErrorPlaceholder = Global.LoadingImagePath,
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.End,
                        Aspect = Aspect.AspectFit,
                        Source = categories[i].Image,
                    };
                    inGrid.Children.Add(image, 0, 1);


                    inGrid.GestureRecognizers.Add(new TapGestureRecognizer()
                    {
                        Command = new Command(() =>
                        {
                            mainPage.ShowDealDetail(inGrid.BindingContext.ToString());
                        })
                    });
                    i++;
                }
                columnindex++;
            }
        }

        private void Arrow_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Realtime_Price());
        }
    }
}