using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using TicketRoom.Models.Custom;
using TicketRoom.Models.ShopData;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.Shop
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShopMainPage : ContentPage
    {
        SH_Home home;

        Uri instaUri;
        Uri webUri;

        string myShopName = "";
        ShopDataFunc dataclass = new ShopDataFunc();
        List<Button> tablist = new List<Button>();
        Queue<CustomButton> SelectTap_Queue = new Queue<CustomButton>();

        public ShopMainPage(int sub_index, string sub_name)
        {
            InitializeComponent();
            myShopName = sub_name;
            PostSearchHomeAsync(sub_index);
        }

        private async void PostSearchHomeAsync(int sub_index)
        {
            // loading start
            Loading loadingScreen = new Loading(true);
            await Navigation.PushModalAsync(loadingScreen);

            home = new SH_Home();
            string str = @"{";
            str += "subCateIndex : " + sub_index;
            str += "}";

            //// JSON 문자열을 파싱하여 JObject를 리턴
            JObject jo = JObject.Parse(str);

            UTF8Encoding encoder = new UTF8Encoding();
            byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "SH_SearchHome") as HttpWebRequest;
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
                        home = JsonConvert.DeserializeObject<SH_Home>(readdata);
                    }
                    Init();
                }
            }
            catch (Exception ex)
            {

                System.Diagnostics.Debug.WriteLine(ex);
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

            // loading end
            await Navigation.PopModalAsync();
        }


        // DB에서 가져온 홈 페이지 정보로 초기화 진행
        private void Init()
        {
            // 타이틀 탭 초기화
            TitleName.Text = myShopName;

            // 리스트에 버튼 추가
            tablist.Add(Content_Sale);
            tablist.Add(Content_Info);
            tablist.Add(Content_Review);

            MainShopImage.Source = ImageSource.FromUri(new Uri(home.SH_HOME_IMAGE));
            MainShopGrade.Text = home.SH_HOME_GRADE.ToString();
            MainShopDelivery.Text = "무료 배송 금액 : " + home.SH_HOME_FREEDELEVERY.ToString("N0") + "원";
            MainShopPay.Text = "결제 방법 : " + home.SH_HOME_PAYWAY;
            instaUri = new Uri(home.SH_HOME_INSTA);
            webUri = new Uri(home.SH_HOME_WEB);


            Content_Changed(tablist[0], null); // Default로 보여질 콘텐츠 뷰
        }



        /// <summary>
        /// 콘텐츠 뷰 변경 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Content_Changed(object sender, EventArgs e)
        {
            CustomButton selectedtab = (CustomButton)sender;

            if (SelectTap_Queue.Count < 2)
            {
                if (SelectTap_Queue.Count != 0)
                {
                    CustomButton temp = SelectTap_Queue.Dequeue();
                    temp.TextColor = Color.Black;
                    temp.BackgroundColor = Color.White;
                }
                selectedtab.TextColor = Color.White;
                selectedtab.BackgroundColor = Color.Black;
                SelectTap_Queue.Enqueue(selectedtab);
            }

            if (selectedtab.Text.Equals("판매품"))
            {
                ShopContentView.Content = new ShopSaleView(myShopName, home);
            }
            else if (selectedtab.Text.Equals("정보"))
            {
                ShopContentView.Content = new ShopInfoView(myShopName);
            }
            else if (selectedtab.Text.Equals("리뷰"))
            {
                ShopContentView.Content = new ShopReviewView(myShopName);
            }

        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {

        }

        private void Tab_Changed(object sender, EventArgs e)
        {

        }

        private void CallBtn_Clicked(object sender, EventArgs e)
        {

        }

        private void BasketBtn_Clicked(object sender, EventArgs e)
        {

        }

        private void Insta_btn_Clicked(object sender, EventArgs e)
        {
            Xamarin.Forms.Device.OpenUri(instaUri);
        }

        private void MoveShop_btn_Clicked(object sender, EventArgs e)
        {
            Xamarin.Forms.Device.OpenUri(webUri);
        }
    }
}