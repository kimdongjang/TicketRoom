using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using TicketRoom.Models.Gift.Purchase;
using TicketRoom.Views.Users.CreateUser;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.Dael.Purchase
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PurchaseDetailPage : ContentPage
    {
        List<G_PurchasedetailInfo> g_PurchasedetailInfos = null;
        InputAdress adrAPI;
        string UsedPoint = "";
        int price = 0;
        public PurchaseDetailPage(List<G_PurchasedetailInfo> g_PurchasedetailInfos)
        {
            InitializeComponent();
            this.g_PurchasedetailInfos = g_PurchasedetailInfos;
            ShowPrice();
        }

        public void ShowPrice()
        {
            for (int i = 0; i < g_PurchasedetailInfos.Count; i++)
            {
                price += int.Parse(g_PurchasedetailInfos[i].PDL_ALLPRICE);
            }
            Purchase_AllPrice_label.Text = price.ToString("N0");
        }

        public PurchaseDetailPage()
        {
            InitializeComponent();
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            this.OnBackButtonPressed();
        }

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            //var picker = (Picker)sender;
            //int selectedIndex = picker.SelectedIndex;

            //if (selectedIndex != -1)
            //{
            //    DisplayAlert("알림", picker.Items[selectedIndex], "OK");
            //}
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EntryAdress_Focused(object sender, FocusEventArgs e)
        {
            EntryAdress.Unfocus();
            Navigation.PushModalAsync(adrAPI = new InputAdress(this));
        }

        private void PurchaseBtn_Clicked(object sender, EventArgs e)
        {
            G_PurchaseInfo g_PurchaseInfo = null;
            if (prepaymentradio.Source.ToString().Contains("radio_checked_icon.png"))
            {
                g_PurchaseInfo = new G_PurchaseInfo
                {
                    ID = "01024313236",
                    PL_DELIVERY_ADDRESS = EntryAdress.Text,
                    PL_USED_POINT = UsedPoint,
                    PL_ISSUCCESS = "",
                    PL_DELIVERYPAY_TYPE = "1",
                    PL_PAYMENT_PRICE = Purchase_AllPrice_label.Text.ToString().Replace(",", ""),
                    AC_NUM = (Combo.SelectedIndex + 1).ToString(),
                    G_PD_LIST = g_PurchasedetailInfos,
                    PL_ACCUSER_NAME = Name_box.Text
                };
            }
            else
            {
                g_PurchaseInfo = new G_PurchaseInfo
                {
                    ID = "01024313236",
                    PL_DELIVERY_ADDRESS = EntryAdress.Text,
                    PL_USED_POINT = UsedPoint,
                    PL_ISSUCCESS = "",
                    PL_DELIVERYPAY_TYPE = "2",
                    PL_PAYMENT_PRICE = Purchase_AllPrice_label.Text.ToString().Replace(",", ""),
                    AC_NUM = (Combo.SelectedIndex+1).ToString(),
                    G_PD_LIST = g_PurchasedetailInfos,
                    PL_ACCUSER_NAME = Name_box.Text
                };
            }
            

            //// JSON 문자열을 파싱하여 JObject를 리턴
            //JObject jo = JObject.Parse(str);

            var dataString = JsonConvert.SerializeObject(g_PurchaseInfo);

            UTF8Encoding encoder = new UTF8Encoding();
            byte[] data = encoder.GetBytes(dataString.ToString()); // a json object, or xml, whatever...

            //request.Method = "POST";
            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "Users_Create") as HttpWebRequest;
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
                    //Stuinfo test = JsonConvert.DeserializeObject<Stuinfo>(readdata);
                    //switch (int.Parse(readdata))
                    //{
                    //    case 0:
                    //        DisplayAlert("알림", "인증번호가 틀렸습니다.", "OK");
                    //        return;
                    //    case 1:
                    //        await ShowMessage("회원가입 되었습니다.", "알림", "OK", async () =>
                    //        {
                    //            await Navigation.PushModalAsync(new LoginPage());
                    //        });
                    //        return;
                    //    default:
                    //        DisplayAlert("알림", "서버 점검중입니다.", "OK");
                    //        return;
                    //}
                }
            }
        }

        private void UsedPointBtn_Clicked(object sender, EventArgs e)
        {
            try
            {
                UsedPoint = int.Parse(Point_box.Text).ToString();
                Purchase_AllPrice_label.Text = (price - int.Parse(UsedPoint)).ToString("N0");
            }
            catch
            {
                DisplayAlert("알림", "숫자만입력해주세요", "OK");
            }
        }

        private void Point_box_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (e.NewTextValue.Contains(".")|| e.NewTextValue.Equals("-"))
                {
                    if (e.OldTextValue != null)
                    {
                        Point_box.Text = e.OldTextValue;
                    }
                    else
                    {
                        Point_box.Text = "";
                    }
                    return;
                }
                else
                {
                    if (int.Parse(Point_box.Text) > 1000)
                    {
                        Point_box.Text = 1000.ToString();
                    }
                }
            }
            catch
            {

            }
        }
    }
}