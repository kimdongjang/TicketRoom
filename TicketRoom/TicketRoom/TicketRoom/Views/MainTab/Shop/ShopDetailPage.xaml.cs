using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using TicketRoom.Models.ShopData;
using TicketRoom.Views.MainTab.Shop.GridImage;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.Shop
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShopDetailPage : ContentPage
    {
        List<SH_ImageList> imageList;
        List<SH_OtherView> otherList;
        List<SH_Pro_Option> optionList;

        int option_selectColor = 0;
        int option_selectSize = 0;

        string myShopName = "";
        int clothes_count = 0;
        int productIndex = 0;

        public ShopDetailPage(string titleName, int productIndex)
        {
            InitializeComponent();
            myShopName = titleName;
            this.productIndex = productIndex;

            PostSearchImageListToProductAsync(productIndex);
            PostSearchOtherViewToProductAsync(productIndex);
            PostSearchProOptionToProductAsync(productIndex);

            Init();

        }


        // DB에서 상품 인덱스로 이미지 목록을 가져오기
        private async void PostSearchImageListToProductAsync(int productIndex)
        {
            imageList = new List<SH_ImageList>();
            string str = @"{";
            str += "productIndex : " + productIndex;
            str += "}";

            //// JSON 문자열을 파싱하여 JObject를 리턴
            JObject jo = JObject.Parse(str);

            UTF8Encoding encoder = new UTF8Encoding();
            byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "SH_SearchImageListToProduct") as HttpWebRequest;
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
                        imageList = JsonConvert.DeserializeObject<List<SH_ImageList>>(readdata);
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("에러", ex.ToString(), "확인");
            }
        }

        // DB에서 홈 상품 인덱스로 다른 고객이 본 상품을 가져오기
        private async void PostSearchOtherViewToProductAsync(int productIndex)
        {
            otherList = new List<SH_OtherView>();
            string str = @"{";
            str += "productIndex : " + productIndex;
            str += "}";

            //// JSON 문자열을 파싱하여 JObject를 리턴
            JObject jo = JObject.Parse(str);

            UTF8Encoding encoder = new UTF8Encoding();
            byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "SH_SearchOtherViewToProduct") as HttpWebRequest;
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
                        otherList = JsonConvert.DeserializeObject<List<SH_OtherView>>(readdata);
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("에러", ex.ToString(), "확인");
            }
        }

        // DB에서 홈 상품 인덱스로 다른 고객이 본 상품을 가져오기
        private async void PostSearchProOptionToProductAsync(int productIndex)
        {
            optionList = new List<SH_Pro_Option>();
            string str = @"{";
            str += "productIndex : " + productIndex;
            str += "}";

            //// JSON 문자열을 파싱하여 JObject를 리턴
            JObject jo = JObject.Parse(str);

            UTF8Encoding encoder = new UTF8Encoding();
            byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "SH_SearchProOptionToProduct") as HttpWebRequest;
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
                        optionList = JsonConvert.DeserializeObject<List<SH_Pro_Option>>(readdata);
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("에러", ex.ToString(), "확인");
            }
        }

        private async void BasketBtn_ClickedAsync(object sender, EventArgs e)
        {
            string size = "";
            string color = "";
            int selectedIndex = ClothesSelectSize.SelectedIndex;
            if (selectedIndex != -1)
            {
                size = ClothesSelectSize.Items[selectedIndex];
            }
            selectedIndex = ClothesSelectColor.SelectedIndex;
            if (selectedIndex != -1)
            {
                color = ClothesSelectColor.Items[selectedIndex];
            }

            if (size != "")
            {
                if (color != "")
                {
                    if (ClothesCountLabel.Text != "0")
                    {
                        //장바구니로 이동
                        var answer = await DisplayAlert("사이즈 : " + size + " , 색상 : " + color + ", 수량 : " + ClothesCountLabel.Text, "주문 정보가 맞습니까?", "확인", "취소");
                        if (answer)
                        {
                            var basket_answer = await DisplayAlert("주문 완료", "장바구니로 이동하시겠습니까?", "확인", "취소");
                            if (basket_answer)
                            {
                                //Navigation.PushModalAsync();
                            }
                        }
                    }
                }
            }
        }

        private void Tab_Changed(object sender, EventArgs e)
        {

        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {

        }

        private void Init()
        {
            TitleName.Text = myShopName;
            CountEvent();
            SelectColor();
            SelectSize();


            #region 다른 고객이 함께 본 상품 목록
            Grid other_grid = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star)  },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star)  },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star)  }
                },
                Margin = new Thickness(15, 0, 15, 0),
            };
            for (int i = 0; i < 3; i++)
            {
                Image image = new Image
                {
                    Source = "shop_clothes1.jpg",
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                };
                other_grid.Children.Add(image, i, 0);
                // 이미지 클릭시 해당 페이지로 이동(아직 미구현)
                image.GestureRecognizers.Add(new TapGestureRecognizer()
                {
                    Command = new Command(() =>
                    {

                    })
                });
            }
            OtherProduct.Children.Add(other_grid, 0, 0);
            #endregion

            #region 이미지 리스트 그리드 클릭 이벤트
            ImageListGrid.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    Navigation.PushModalAsync(new PictureList());
                })
            });
            #endregion

        }

        private void SelectColor()
        {
            List<string> color_list = new List<string>();

            color_list.Add("파랑");
            color_list.Add("연두");
            color_list.Add("보라");
            color_list.Add("빨강");
            // 선택할 색상은 동적으로 할 예정

            foreach (string colorName in color_list)
            {
                ClothesSelectColor.Items.Add(colorName);
            }
        }

        private void SelectSize()
        {
            List<string> size_list = new List<string>();

            size_list.Add("S(90)");
            size_list.Add("M(95)");
            size_list.Add("L(100)");
            size_list.Add("XL(110)");
            // 선택할 색상은 동적으로 할 예정

            foreach (string colorName in size_list)
            {
                ClothesSelectSize.Items.Add(colorName);
            }
        }

        private void CountEvent()
        {
            #region +,- 수량 체크 이벤트
            plusCount.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(async () =>
                {
                    for (int i = 0; i < optionList.Count; i++)
                    {
                        if (optionList[i].SH_PRO_OPTION_COUNT < int.Parse(ClothesCountLabel.Text))
                        {
                            var basket_answer = await DisplayAlert("주문 오류", "주문 가능한 수량을 초과했습니다!", "확인", "취소");
                            return;
                        }
                    }
                    clothes_count = int.Parse(ClothesCountLabel.Text);
                    clothes_count += 1;
                    ClothesCountLabel.Text = clothes_count.ToString();
                })
            });

            minusCount.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    clothes_count = int.Parse(ClothesCountLabel.Text);
                    if (clothes_count != 0)
                    {
                        clothes_count -= 1;
                    }
                    ClothesCountLabel.Text = clothes_count.ToString();
                })
            });
            #endregion

        }

        private void BasketBtn_Clicked(object sender, EventArgs e)
        {

        }
    }
}