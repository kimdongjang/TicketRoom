using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using TicketRoom.Models.Custom;
using TicketRoom.Models.Gift;
using TicketRoom.Views.MainTab.MyPage;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.Dael
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Realtime_Price : ContentPage
    {
        List<G_CategoryInfo> categoryInfoList = new List<G_CategoryInfo>();
        public Realtime_Price()
        {
            InitializeComponent();

            #region IOS의 경우 초기화
            NavigationPage.SetHasNavigationBar(this, false); // Navigation Bar 지우는 코드 생성자에 입력
            if (Xamarin.Forms.Device.OS == TargetPlatform.iOS)
            {
                MainGrid.RowDefinitions[0].Height = Global.title_size_value;
                if (Global.ios_x_model == true) // ios X 이상의 모델일 경우
                {
                    MainGrid.RowDefinitions[5].Height = 30;
                }
            }
            #endregion

            LoadingInitAsync();
        }

        private void LoadingInitAsync()
        {
            //// 로딩 시작
            //await Global.LoadingStartAsync();



            #region 네트워크 상태 확인
            var current_network = Connectivity.NetworkAccess; // 현재 네트워크 상태
            if (current_network == NetworkAccess.Internet) // 네트워크 연결 가능
            {
                categoryInfoList = SelectAllCategory(); // 카테고리 피커 리스트 검색

            }
            else
            {
                categoryInfoList = null;
            }
            #endregion

            #region 네트워크 검색 불가
            if (categoryInfoList == null) // 검색 불가일 경우
            {
                ContentView.Content = new Realtime_PriceView(null);
                return;
                // 카테고리 피커 초기화 하지 않음
            }
            else
            {
                AddCategoryCombo(categoryInfoList);
                CategoryCombo.SelectedIndex = 0;
            }
            #endregion

            NavigationInit();

            //// 로딩 완료
            //await Global.LoadingEndAsync();
        }

        private void NavigationInit()
        {
            NavigationButton.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(async () =>
                {
                    // 로딩 시작
                    await Global.LoadingStartAsync();

                    await Navigation.PushAsync(new NavagationPage());

                    // 로딩 완료
                    await Global.LoadingEndAsync();
                })
            });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Global.isbackbutton_clicked = true;
        }

        private List<G_CategoryInfo> SelectAllCategory()
        {
            try
            {
                //request.Method = "GET";
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
                        return test;
                    }
                }
            }
            catch
            {
                return null;
            }
        }



        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            if (Global.isbackbutton_clicked)
            {
                Global.isbackbutton_clicked = false;
                Navigation.PopAsync();
            }
        }

        private void Category_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;

            if (selectedIndex == 0)
            {
                DetailCategoryCombo.Items.Clear();
                DetailCategoryCombo.Items.Add("전체");
                DetailCategoryCombo.SelectedIndex = 0;
            }
            else
            {
                SelectDetailCategoryAsync(selectedIndex.ToString());
                DetailCategoryCombo.SelectedIndex = 0;
            }
        }

        private void DetailCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;
            if (selectedIndex != -1)
            {
                SelectAllProductAsync(CategoryCombo.SelectedIndex.ToString(), DetailCategoryCombo.SelectedIndex.ToString());
            }
        }

        private void SelectDetailCategoryAsync(string categorynum)
        {
            //// 로딩 시작
            //await Global.LoadingStartAsync();

            string str = @"{";
            str += "CategoryNum:'" + categorynum;  //아이디찾기에선 Name으로 
            str += "'}";

            //// JSON 문자열을 파싱하여 JObject를 리턴
            JObject jo = JObject.Parse(str);

            UTF8Encoding encoder = new UTF8Encoding();
            byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

            //request.Method = "POST";
            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "SelectDetailCategory") as HttpWebRequest;
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
                    List<G_DetailCategoryInfo> test = JsonConvert.DeserializeObject<List<G_DetailCategoryInfo>>(readdata);
                    AddDeatilCategoryCombo(test);
                }
            }

            //// 로딩 완료
            //await Global.LoadingEndAsync();
        }

        private async void SelectAllProductAsync(string categorynum, string detailcategorynum)
        {
            // 로딩 시작
            await Global.LoadingStartAsync();

            string str = @"{";
            str += "CategoryNum:'" + categorynum;  //아이디찾기에선 Name으로 
            str += "',DetailcategoryNum:'" + detailcategorynum;
            str += "'}";

            //// JSON 문자열을 파싱하여 JObject를 리턴
            JObject jo = JObject.Parse(str);

            UTF8Encoding encoder = new UTF8Encoding();
            byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

            //request.Method = "POST";
            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "SelectAllProduct") as HttpWebRequest;
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
                    ContentView.Content = new Realtime_PriceView(test);
                    //ShowPrice(test);
                }
            }
            // 로딩 완료
            await Global.LoadingEndAsync();
        }

        private void AddCategoryCombo(List<G_CategoryInfo> categorylist)
        {
            CategoryCombo.Items.Clear();
            CategoryCombo.Items.Add("전체");
            for (int i = 0; i < categorylist.Count; i++)
            {
                CategoryCombo.Items.Add(categorylist[i].Name);
            }
        }

        private void AddDeatilCategoryCombo(List<G_DetailCategoryInfo> detailcategorylist)
        {
            DetailCategoryCombo.Items.Clear();
            DetailCategoryCombo.Items.Add("전체");
            for (int i = 0; i < detailcategorylist.Count; i++)
            {
                DetailCategoryCombo.Items.Add(detailcategorylist[i].PRODUCTTYPE);
            }
        }

        private async void SearchBtn_ClickedAsync(object sender, EventArgs e)
        {
            // 로딩 시작
            await Global.LoadingStartAsync();

            if (Search_box.Text != "" && Search_box.Text != null)
            {
                Search_box.Unfocus();
                string str = @"{";
                str += "Keyword:'" + Search_box.Text;  //검색 키워드
                str += "'}";

                //// JSON 문자열을 파싱하여 JObject를 리턴
                JObject jo = JObject.Parse(str);

                UTF8Encoding encoder = new UTF8Encoding();
                byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

                //request.Method = "POST";
                HttpWebRequest request = WebRequest.Create(Global.WCFURL + "SearchProduct") as HttpWebRequest;
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
                        ContentView.Content = new Realtime_PriceView(test);
                    }
                }
            }
            else
            {
                DisplayAlert("알림", "키워드를 입력하세요", "OK");
            }


            // 로딩 완료
            await Global.LoadingEndAsync();
        }
    }
}