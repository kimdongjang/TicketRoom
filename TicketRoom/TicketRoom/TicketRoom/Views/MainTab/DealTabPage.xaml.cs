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
                            Point_label.Text = int.Parse(test).ToString("N0");
                        }
                        else
                        {
                            Point_label.Text = "0";
                        }
                        
                    }
                }
            }
            else
            {
                Point_label.Text = int.Parse("0").ToString("N0");
            }
        }

        private void Showdeal()
        {
            g_DealInfolist = giftDBFunc.SelectDealList();
            if (g_DealInfolist.Count == 0)
            {
                CustomLabel label = new CustomLabel
                {
                    Text = "거래내역이 없습니다",
                    Size = 10,
                    TextColor = Color.Black,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center
                };
                deallist_Grid.Children.Add(label, 0, 1);         //실시간거래 그리드에 라벨추가
            }

            for (int i = 0; i < g_DealInfolist.Count; i++)
            {
                //deallist_Grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
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
                deallist_Grid.Children.Add(label, 0, i + 1);         //실시간거래 그리드에 라벨추가
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
                    NowPoint_Label.Text = categories[0].Error;
                    return;
                }
            }

            for (int i = 0; i < categories.Count; i++)
            {
                CachedImage imgae = new CachedImage
                {
                    LoadingPlaceholder = Global.LoadingImagePath,
                    ErrorPlaceholder = Global.LoadingImagePath,
                    Source = ImageSource.FromUri(new Uri(categories[i].Image)),
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Aspect = Aspect.AspectFit,
                    BindingContext = categories[i].CategoryNum
                };
                if (i == 0)
                {
                    Imagelist_Grid.RowDefinitions.Add(new RowDefinition { Height = 150 });
                    Imagelist_Grid.Children.Add(imgae, 0, 0);
                }
                else
                {
                    if ((i % 2) == 0)
                    {
                        Imagelist_Grid.RowDefinitions.Add(new RowDefinition { Height = 150 });
                    }
                    Imagelist_Grid.Children.Add(imgae, (i % 2), (i / 2));         //실시간거래 그리드에 라벨추가
                }

                TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += (s, e) =>
                {
                    CachedImage clickedimage = (CachedImage)s;
                    mainPage.ShowDealDetail(clickedimage.BindingContext.ToString());
                    //Navigation.PushAsync(new DealDeatailPage(clickedimage.BindingContext.ToString()));
                };
                imgae.GestureRecognizers.Add(tapGestureRecognizer);
            }
            Imagelist_Grid.RowDefinitions.Add(new RowDefinition { Height = 5 });
        }

        private void Arrow_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Realtime_Price());
        }
    }
}