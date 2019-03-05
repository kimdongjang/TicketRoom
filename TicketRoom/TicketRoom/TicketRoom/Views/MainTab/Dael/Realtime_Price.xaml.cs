using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using TicketRoom.Models.Custom;
using TicketRoom.Models.Gift;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.Dael
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Realtime_Price : ContentPage
    {
        public Realtime_Price()
        {
            InitializeComponent();
            SelectAllCategory();

            #region IOS의 경우 초기화
            NavigationPage.SetHasNavigationBar(this, false); // Navigation Bar 지우는 코드 생성자에 입력
            if (Xamarin.Forms.Device.OS == TargetPlatform.iOS)
            {
                MainGrid.RowDefinitions[0].Height = 50;
            }
            #endregion
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Global.isbackbutton_clicked = true;
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
                    AddCategoryCombo(test);

                    CategoryCombo.SelectedIndex = 0;
                }
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

        private void ShowPrice(List<G_ProductInfo> Pricelist)
        {
            Price_Grid.RowDefinitions.Clear();
            Price_Grid.Children.Clear();
            Price_Grid.RowDefinitions.Add(new RowDefinition { Height = 40 });//new GridLength(1, GridUnitType.Star)
            CustomLabel titlename = new CustomLabel
            {
                Text = "상품명",
                Size = 14,
                TextColor = Color.Black,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.Start,
                XAlign = TextAlignment.Center,
                YAlign = TextAlignment.Center
            };

            CustomLabel titleb = new CustomLabel
            {
                Text = "고객판매가\n(할인율)",
                Size = 14,
                TextColor = Color.Black,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.Center,
                XAlign = TextAlignment.Center,
                YAlign = TextAlignment.Center
            };

            CustomLabel titler = new CustomLabel
            {
                Text = "고객구매가\n(할인율)",
                Size = 14,
                TextColor = Color.Black,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.Center,
                XAlign = TextAlignment.Center,
                YAlign = TextAlignment.Center
            };

            Price_Grid.Children.Add(titlename, 0, 0);
            Price_Grid.Children.Add(titleb, 1, 0);
            Price_Grid.Children.Add(titler, 2, 0);

            int row = 1;

            for (int i = 0; i < Pricelist.Count; i++)
            {
                Price_Grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });//new GridLength(1, GridUnitType.Star)
                Price_Grid.RowDefinitions.Add(new RowDefinition { Height = 1 });//new GridLength(1, GridUnitType.Star)
                CustomLabel n = new CustomLabel
                {
                    Text = Pricelist[i].PRODUCTTYPE + "\n" + Pricelist[i].PRODUCTVALUE,
                    Size = 14,
                    TextColor = Color.Black,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.Start,
                    XAlign = TextAlignment.Start,
                    YAlign = TextAlignment.Center
                };

                CustomLabel b = new CustomLabel
                {
                    Text = Pricelist[i].SALEDISCOUNTPRICE + "\n(" + Pricelist[i].SALEDISCOUNTRATE + "%)",
                    Size = 14,
                    TextColor = Color.Blue,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.Center,
                    XAlign = TextAlignment.Center,
                    YAlign = TextAlignment.Center
                };

                CustomLabel r = new CustomLabel
                {
                    Text = Pricelist[i].PURCHASEDISCOUNTPRICE + "\n(" + Pricelist[i].PURCHASEDISCOUNTRATE + "%)",
                    Size = 14,
                    TextColor = Color.Red,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.Center,
                    XAlign = TextAlignment.Center,
                    YAlign = TextAlignment.Center
                };

                Price_Grid.Children.Add(n, 0, row);
                Price_Grid.Children.Add(b, 1, row);
                Price_Grid.Children.Add(r, 2, row);

                row++;

                BoxView gridline = new BoxView
                {
                    BackgroundColor = Color.FromHex("#f4f2f2"),
                    HeightRequest = 1,
                    VerticalOptions = LayoutOptions.End,
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };
                Price_Grid.Children.Add(gridline, 0, row);
                Grid.SetColumnSpan(gridline, 3);
                row++;
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
                SelectDetailCategory(selectedIndex.ToString());
                DetailCategoryCombo.SelectedIndex = 0;
            }
        }

        private void DetailCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectAllProduct(CategoryCombo.SelectedIndex.ToString(), DetailCategoryCombo.SelectedIndex.ToString());
        }

        private void SelectDetailCategory(string categorynum)
        {
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
        }

        private void SelectAllProduct(string categorynum, string detailcategorynum)
        {
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
                    ShowPrice(test);
                }
            }
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

        private void SearchBtn_Clicked(object sender, EventArgs e)
        {
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
                        ShowPrice(test);
                    }
                }
            }
            else
            {
                DisplayAlert("알림", "키워드를 입력하세요", "OK");
            }
        }
    }
}