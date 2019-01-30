using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TicketRoom.Models.Gift;
using TicketRoom.Models.Gift.Purchase;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.Dael.Purchase
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PurchasePage : ContentPage
    {
        G_ProductInfo productInfo = null;
        public string Purchase_Price = "";
        public string DiscountPurchase_Price = "";
        public PurchasePage(G_ProductInfo productInfo)
        {
            InitializeComponent();
            this.productInfo = productInfo;
            Pro_imgae.Source = productInfo.PRODUCTIMAGE;
            Pro_Name.Text = productInfo.PRODUCTTYPE + " " + productInfo.PRODUCTVALUE;
            Pro_price.Text = productInfo.PURCHASEDISCOUNTPRICE + "[" + productInfo.PURCHASEDISCOUNTRATE + "%]";
            Purchase_Price = (int.Parse(productInfo.PROPRICE) * int.Parse(Count_label.Text)).ToString("N0");
            DiscountPurchase_Price = (int.Parse(productInfo.PURCHASEDISCOUNTPRICE) * int.Parse(Count_label.Text)).ToString("N0");
            Purchase_Price_span.Text = Purchase_Price;
            Purchase_DiscountPrice_span.Text = DiscountPurchase_Price;
            DisCountRate_label.Text = productInfo.PURCHASEDISCOUNTRATE + "%";
            G_ProductCount test = null;
            string str = @"{";
            str += "ProNum:'" + productInfo.PRONUM;  //아이디찾기에선 Name으로 
            str += "'}";

            //// JSON 문자열을 파싱하여 JObject를 리턴
            JObject jo = JObject.Parse(str);

            UTF8Encoding encoder = new UTF8Encoding();
            byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

            //request.Method = "POST";
            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "Get_Product_Ccount") as HttpWebRequest;
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
                    test = JsonConvert.DeserializeObject<G_ProductCount>(readdata);
                    Purchase_Count_span.Text = test.PAPER_GC_COUNT + " 개";
                    Pin_Count_span.Text = test.PIN_GC_COUNT + " 개";
                }
            }
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            this.OnBackButtonPressed();
        }

        private void PlusBtn_Clicked(object sender, EventArgs e)
        {
            Count_label.Text = (int.Parse(Count_label.Text) + 1).ToString();
            Purchase_Price = (int.Parse(productInfo.PROPRICE) * int.Parse(Count_label.Text)).ToString("N0");
            DiscountPurchase_Price = (int.Parse(productInfo.PURCHASEDISCOUNTPRICE) * int.Parse(Count_label.Text)).ToString("N0");
            Purchase_Price_span.Text = Purchase_Price;
            Purchase_DiscountPrice_span.Text = DiscountPurchase_Price;
            //int teqwteqw = int.Parse(Purchase_Price.Replace(",", "")); //1,000->1000
        }

        private void MinusBtn_Clicked(object sender, EventArgs e)
        {
            if (int.Parse(Count_label.Text) > 0)
            {
                Count_label.Text = (int.Parse(Count_label.Text) - 1).ToString();
                Purchase_Price = (int.Parse(productInfo.PROPRICE) * int.Parse(Count_label.Text)).ToString("N0");
                DiscountPurchase_Price = (int.Parse(productInfo.PURCHASEDISCOUNTPRICE) * int.Parse(Count_label.Text)).ToString("N0");
                Purchase_Price_span.Text = Purchase_Price;
                Purchase_DiscountPrice_span.Text = DiscountPurchase_Price;
            }
        }

        private void DoPurchase_Clicked(object sender, EventArgs e)
        {
            if (int.Parse(Count_label.Text) == 0)
            {
                DisplayAlert("알림", "수량을 입력해주세요", "OK");
                return;
            }

            List<G_PurchasedetailInfo> g_PurchasedetailInfos = new List<G_PurchasedetailInfo>();
            if (prepaymentradio.Source.ToString().Contains("radio_checked_icon.png"))
            {
                G_PurchasedetailInfo g_PurchasedetailInfo = new G_PurchasedetailInfo
                {
                    PDL_PRONUM = productInfo.PRONUM,
                    PDL_PROCOUNT = Count_label.Text,
                    PDL_PROTYPE = "1",
                    PDL_ALLPRICE = (int.Parse(productInfo.PURCHASEDISCOUNTPRICE)*int.Parse(Count_label.Text)).ToString()
                };
                g_PurchasedetailInfos.Add(g_PurchasedetailInfo);
            }
            else
            {
                G_PurchasedetailInfo g_PurchasedetailInfo = new G_PurchasedetailInfo
                {
                    PDL_PRONUM = productInfo.PRONUM,
                    PDL_PROCOUNT = Count_label.Text,
                    PDL_PROTYPE = "2",
                    PDL_ALLPRICE = (int.Parse(productInfo.PURCHASEDISCOUNTPRICE) * int.Parse(Count_label.Text)).ToString()
                };
                g_PurchasedetailInfos.Add(g_PurchasedetailInfo);
            }

            Navigation.PushModalAsync(new PurchaseDetailPage(g_PurchasedetailInfos));
        }

        private async void AddBasketBtn_Clicked(object sender, EventArgs e)
        {
            if (int.Parse(Count_label.Text) == 0)
            {
                DisplayAlert("알림", "수량을 정해주세요", "OK");
                return;
            }
            G_BasketInfo basketInfo = new G_BasketInfo();
            basketInfo.BK_PRONUM = productInfo.PRONUM;
            basketInfo.BK_PROCOUNT = Count_label.Text;
            basketInfo.BK_PRODUCT_IMAGE = productInfo.PRODUCTIMAGE;
            basketInfo.BK_PRODUCT_PURCHASE_DISCOUNTPRICE = productInfo.PURCHASEDISCOUNTPRICE;
            basketInfo.BK_PRODUCT_TYPE = productInfo.PRODUCTTYPE;
            basketInfo.BK_PRODUCT_VALUE = productInfo.PRODUCTVALUE;

            if (prepaymentradio.Source.ToString().Contains("radio_checked_icon.png"))
            {
                basketInfo.BK_TYPE = "1";
            }
            else
            {
                basketInfo.BK_TYPE = "2";
            }
            Global.BasketList.Add(basketInfo);
            await ShowMessage("장바구니에 추가되었습니다.", "알림", "OK", async () =>
            {
                this.OnBackButtonPressed();
            });
        }

        private void Radio1_Clicked(object sender, EventArgs e)
        {
            prepaymentradio.Source = "radio_checked_icon.png";
            Cashondeliveryradio.Source = "radio_unchecked_icon.png";
        }

        private void Radio2_Clicked(object sender, EventArgs e)
        {
            prepaymentradio.Source = "radio_unchecked_icon.png";
            Cashondeliveryradio.Source = "radio_checked_icon.png";
        }

        public async Task ShowMessage(string message, string title, string buttonText, Action afterHideCallback)
        {
            await DisplayAlert(title, message, buttonText);

            afterHideCallback?.Invoke();
        }
    }
}