using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
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
        int OldPoint = 0;
        int price = 0;
        int deliveryprice = 3000;
        bool radiostate = true; // true : 선불 , false : 착불

        public PurchaseDetailPage(List<G_PurchasedetailInfo> g_PurchasedetailInfos)
        {
            InitializeComponent();
            this.g_PurchasedetailInfos = g_PurchasedetailInfos;
            DeliveryPrice_label.Text = (deliveryprice).ToString("N0");
            price += deliveryprice;
            ShowPrice();
            SelectAllAccount();
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
            if (!radiostate)
            {
                prepaymentradio.Source = "radio_checked_icon.png";
                Cashondeliveryradio.Source = "radio_unchecked_icon.png";
                price += deliveryprice;
                radiostate = true;
                Purchase_AllPrice_label.Text = price.ToString("N0");
            }
        }

        private void Radio2_Clicked(object sender, EventArgs e)
        {
            if (radiostate)
            {
                prepaymentradio.Source = "radio_unchecked_icon.png";
                Cashondeliveryradio.Source = "radio_checked_icon.png";
                price -= deliveryprice;
                radiostate = false;
                Purchase_AllPrice_label.Text = price.ToString("N0");
            }
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

        private async void PurchaseBtn_Clicked(object sender, EventArgs e)
        {
            if (EntryAdress.Text != "" && EntryAdress.Text != null)
            {
                if (Name_box.Text != "" && Name_box.Text != null)
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
                            AC_NUM = (Combo.SelectedIndex + 1).ToString(),
                            G_PD_LIST = g_PurchasedetailInfos,
                            PL_ACCUSER_NAME = Name_box.Text
                        };
                    }


                    //// JSON 문자열을 파싱하여 JObject를 리턴
                    //JObject jo = JObject.Parse(str);

                    var dataString = JsonConvert.SerializeObject(g_PurchaseInfo);

                    JObject jo = JObject.Parse(dataString);

                    UTF8Encoding encoder = new UTF8Encoding();

                    string str = @"{";
                    str += "g_PurchaseInfo:" + jo.ToString();  //아이디찾기에선 Name으로 
                    str += "}";

                    JObject jo2 = JObject.Parse(str);

                    byte[] data = encoder.GetBytes(jo2.ToString()); // a json object, or xml, whatever...

                    //request.Method = "POST";
                    HttpWebRequest request = WebRequest.Create(Global.WCFURL + "UserAddPurchase") as HttpWebRequest;
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
                            string[] test2 = test.Split('*');
                            if (int.Parse(test2[0].ToString()) == 3)
                            {
                                await ShowMessage("구매내역에서 확인해주세요.", "알림", "OK", async () =>
                                {
                                    App.Current.MainPage = new MainPage();
                                });
                            }
                            else if (int.Parse(test2[0].ToString()) == 2)
                            {
                                if (test2[1] != null && test2[1] != "")
                                {
                                    string[] proinfos = test2[1].Split('@');
                                    string[] procnts = test2[2].Split('@');
                                    string errormessage = "";
                                    for (int i = 0; i < proinfos.Length - 1; i++)
                                    {
                                        errormessage += proinfos[i] + "가 " + procnts[i] + "개 있습니다";
                                    }
                                    DisplayAlert("알림", errormessage, "OK");
                                }
                            }
                            else if (int.Parse(test2[0].ToString()) == 4)
                            {
                                DisplayAlert("알림", "서버점검중입니다", "OK");
                            }
                        }
                    }
                }
                else
                {
                    DisplayAlert("알림", "입금예정인을 입력해주세요", "OK");
                }
            }
            else
            {
                DisplayAlert("알림", "주소를 입력해주세요", "OK");
            }
        }

        public async Task ShowMessage(string message, string title, string buttonText, Action afterHideCallback)
        {
            await DisplayAlert(title, message, buttonText);

            afterHideCallback?.Invoke();
        }

        private void UsedPointBtn_Clicked(object sender, EventArgs e)
        {
            try
            {
                price += OldPoint;
                UsedPoint = int.Parse(Point_box.Text).ToString();
                OldPoint = int.Parse(UsedPoint);
                price -= int.Parse(UsedPoint);
                Purchase_AllPrice_label.Text = price.ToString("N0");
                DisplayAlert("알림", "포인트가 적용되었습니다.", "OK");
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

        private void Point_box_Focused(object sender, FocusEventArgs e)
        {
            Point_box.Text = "";
        }

        private void SelectAllAccount()
        {
            //request.Method = "POST";
            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "SelectAllAccount") as HttpWebRequest;
            request.Method = "GET";

            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                    Console.Out.WriteLine("Error fetching data. Server returned status code: {0}", response.StatusCode);
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    var readdata = reader.ReadToEnd();
                    List<AccountInfo> test = JsonConvert.DeserializeObject<List<AccountInfo>>(readdata);
                    if (test != null)
                    {
                        ShowAccount(test);
                    }
                }
            }

        }

        private void ShowAccount(List<AccountInfo> accountlist)
        {
            if (accountlist.Count == 0)
            {
                if (accountlist[0].Error == null || accountlist[0].Error == "")
                {
                    this.OnBackButtonPressed();
                    return;
                }
            }

            for (int i = 0; i < accountlist.Count; i++)
            {
                Combo.Items.Clear();
                Combo.Items.Add(accountlist[i].AC_BANKNAME + ": " + accountlist[i].AC_ACCOUNTNUM + " " + accountlist[i].AC_NAME);
            }
        }
    }
}