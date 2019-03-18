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
            Showimge(giftDBFunc.SelectAllCategory());
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
                            MyPointLabel.Text = int.Parse(test).ToString("N0") + " Point";
                        }
                        else
                        {
                            MyPointLabel.Text = "0 Point";
                        }
                        
                    }
                }
            }
            else
            {
                MyPointLabel.Text = int.Parse("0").ToString("N0") + " Point";
            }
        }


        // 실시간 거래
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
            // 실시간 거래 행은 3행으로 고정
            int row_count = 0;
            if(g_DealInfolist.Count >= 3)
            {
                row_count = 3;
            }
            else
            {
                row_count = g_DealInfolist.Count;
            }

            for (int i = 0; i < row_count; i++)
            {
                RealTimeGrid.RowDefinitions.Add(new RowDefinition { Height = 30 });
                Grid inGrid = new Grid
                {
                    ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(7, GridUnitType.Star) },
                },
                };
                StackLayout sl = new StackLayout
                {
                    BackgroundColor = Color.CornflowerBlue,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                };
                // 결제 상태 레이블
                CustomLabel statusLabel = new CustomLabel
                {
                    TextColor = Color.White,
                    Size = 14,
                    Text = "구매완료",
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    WidthRequest = 80,
                    HorizontalTextAlignment = TextAlignment.Center,
                    
                };
                sl.Children.Add(statusLabel);
                inGrid.Children.Add(sl, 0, 0);

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

                CustomLabel dateLabel = new CustomLabel
                {
                    Text = name + " " + title + "["+ hour+":"+minute+"]",
                    Size = 14,
                    TextColor = Color.Black,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Start
                };
                inGrid.Children.Add(dateLabel, 1, 0);
                RealTimeGrid.Children.Add(inGrid, 0, i);         //실시간거래 그리드에 행 추가
            }
        }


        

        

        // 상품권 목록
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

            int columnindex = 2;
            int rowindex = 0;
            Grid ColumnGrid = new Grid();

            for (int i = 0; i < categories.Count;)
            {          
                if (columnindex > 1) // 열 그리드
                {
                    columnindex = 0;
                    CategoryGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                    ColumnGrid = new Grid
                    {
                        ColumnDefinitions =
                        {
                            new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star)},
                            new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star)}
                        },
                        RowSpacing = 0,
                        Margin = new Thickness(10, 0, 10, 0),
                    };
                    CategoryGrid.Children.Add(ColumnGrid, 0, rowindex);
                    rowindex+=1;
                }

                BoxView borderLine = new BoxView
                {
                    BackgroundColor = Color.FromHex("#ebecf9"),
                };
                // 내부 그리드
                Grid inGrid = new Grid
                {
                    RowDefinitions =
                        {
                        new RowDefinition {  Height = new GridLength(1, GridUnitType.Auto)},
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Auto)},
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Auto)},
                        },
                    BindingContext = i + 1,
                    BackgroundColor = Color.White,
                    Margin = 1,
                };
                ColumnGrid.Children.Add(borderLine, columnindex, 0);
                ColumnGrid.Children.Add(inGrid, columnindex, 0);



                CachedImage image = new CachedImage
                {
                    LoadingPlaceholder = Global.LoadingImagePath,
                    ErrorPlaceholder = Global.LoadingImagePath,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    Aspect = Aspect.AspectFit,
                    Source = categories[i].Image,
                };
                inGrid.Children.Add(image, 0, 0);

                CustomLabel nameLabel = new CustomLabel
                {
                    Text = categories[i].Name,
                    Size = 14,
                    TextColor = Color.Black,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Start,
                    FontAttributes = FontAttributes.Bold,
                    Margin = new Thickness(15,0,0,0),
                };
                inGrid.Children.Add(nameLabel, 0, 1);

                CustomLabel detailLabel = new CustomLabel
                {
                    Text = "설명ㅇㅇㅇㅇㅇㅇㅇㅇㅇㅇㅇㅇ2줄ㅇㅇㅇㅇ",
                    Size = 14,
                    TextColor = Color.Gray,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Start,
                    FontAttributes = FontAttributes.Bold,
                    Margin = new Thickness(15, 0, 0, 0),
                };
                inGrid.Children.Add(detailLabel, 0, 2);

                inGrid.GestureRecognizers.Add(new TapGestureRecognizer()
                {
                    Command = new Command(() =>
                    {
                        mainPage.ShowDealDetail(inGrid.BindingContext.ToString());
                    })
                });
                i++;
                columnindex++;
            }
        }

        private void Arrow_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Realtime_Price());
        }
    }
}