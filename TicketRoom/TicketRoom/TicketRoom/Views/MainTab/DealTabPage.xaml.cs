using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using TicketRoom.Models.Gift;
using TicketRoom.Views.MainTab.Dael;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DealTabPage : ContentView
    {
        List<string> deallist = new List<string> { "박*우 백화점상품권 30만원 구매 [10:35]", "박*우 백화점상품권 30만원 구매 [10:35]",
                                                   "이*현 백화점상품권 30만원 구매 [10:35]", "이*현 백화점상품권 30만원 구매 [10:35]",
                                                   "백*우 백화점상품권 30만원 구매 [10:35]", "백*우 백화점상품권 30만원 구매 [10:35]",
                                                   "최*영 백화점상품권 30만원 구매 [10:35]", "테스트 백화점상품권 30만원 구매 [10:35]"};
        public DealTabPage()
        {
            InitializeComponent();
            Showdeal();
            ShowPoint();
            SelectAllCategory();
        }

        private void ShowPoint()
        {
            if (Global.ISLOGIN)
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

        private void Showdeal()
        {
            for (int i = 0; i < 7; i++)
            {
                deallist_Grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                Label label = new Label
                {
                    Text = deallist[i],
                    FontSize = 10,
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
                Image imgae = new Image
                {
                    Source = categories[i].Image,
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
                    Image clickedimage = (Image)s;
                    Navigation.PushModalAsync(new DealDeatailPage(clickedimage.BindingContext.ToString()));
                };
                imgae.GestureRecognizers.Add(tapGestureRecognizer);
            }
            Imagelist_Grid.RowDefinitions.Add(new RowDefinition { Height = 5 });
        }

        private void Arrow_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new Realtime_Price());
        }
    }
}