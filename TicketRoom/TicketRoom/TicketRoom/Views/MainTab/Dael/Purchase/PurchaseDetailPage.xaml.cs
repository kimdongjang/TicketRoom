using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TicketRoom.Models.Custom;
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
        int UsedPoint = 0;
        int OldPoint = 0;
        int tempdeliveryprice = 0;
        int price = 0;
        int deliveryprice = 3000;

        public PurchaseDetailPage(List<G_PurchasedetailInfo> g_PurchasedetailInfos)
        {
            InitializeComponent();
            this.g_PurchasedetailInfos = g_PurchasedetailInfos;
            DeliveryPrice_label.Text = (deliveryprice).ToString("N0");
            ShowPoint();
            ShowPrice();
            SelectAllAccount();
            Radio1_Clicked(prepaymentradio, null);  //선불 착불 기본값인 선불 선택해놈
            PurchaseListInit();
        }

        private void PurchaseListInit() // 구매할 목록 초기화
        {
            int row = 0;
            for (int i = 0; i < g_PurchasedetailInfos.Count; i++)
            {
                PurchaseListGrid.RowDefinitions.Add(new RowDefinition { Height = 100 });
                
                Grid inGrid = new Grid
                {
                    ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = 100 },
                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                        new ColumnDefinition { Width = 30 }
                    },
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Margin = new Thickness(20, 5, 20, 5),
                    RowSpacing = 0,
                    ColumnSpacing = 0,
                };

                #region 장바구니 상품 이미지
                Image product_image = new Image
                {
                    Source = g_PurchasedetailInfos[i].PRODUCT_IMAGE,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    Aspect = Aspect.AspectFill,
                };
                #endregion

                #region 상품 설명 Labellist 그리드
                Grid product_label_grid = new Grid
                {
                    Margin = new Thickness(10, 0, 0, 0),
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    RowSpacing = 0,
                    ColumnSpacing = 0,
                    RowDefinitions =
                    {
                        new RowDefinition { Height = GridLength.Auto },
                        new RowDefinition { Height = GridLength.Auto },
                        new RowDefinition { Height = 10 },
                        new RowDefinition { Height = GridLength.Auto }
                    },

                };

                #region 상품 제목 Label
                CustomLabel pro_label = new CustomLabel
                {
                    Text = g_PurchasedetailInfos[i].PRODUCT_TYPE+ " "+g_PurchasedetailInfos[i].PRODUCT_VALUE,
                    Size = 18,
                    TextColor = Color.Black,
                };
                #endregion

                #region 상품 종류 Label (사이즈, 색상, 추가옵션)
                CustomLabel type_label = null;
                if (g_PurchasedetailInfos[i].PDL_PROTYPE.Equals("1"))
                {
                    type_label = new CustomLabel
                    {
                        Text = g_PurchasedetailInfos[i].PDL_PROCOUNT + "개 (지류)",
                        Size = 14,
                        TextColor = Color.DarkGray,
                    };
                }
                else
                {
                    type_label = new CustomLabel
                    {
                        Text = g_PurchasedetailInfos[i].PDL_PROCOUNT + "개 (핀번호)",
                        Size = 14,
                        TextColor = Color.DarkGray,
                    };
                }
                #endregion

                #region 가격 내용 Label 및 장바구니 담은 날짜
                CustomLabel price_label = new CustomLabel
                {
                    Text = int.Parse(g_PurchasedetailInfos[i].PDL_ALLPRICE).ToString("N0") + "원",
                    Size = 14,
                    TextColor = Color.Gray,
                };
                #endregion

                //상품 설명 라벨 그리드에 추가
                product_label_grid.Children.Add(pro_label, 0, 0);
                product_label_grid.Children.Add(type_label, 0, 1);
                product_label_grid.Children.Add(price_label, 0, 3);
                #endregion

                #region 상품권 그리드 자식 추가
                inGrid.Children.Add(product_image, 0, 0);
                inGrid.Children.Add(product_label_grid, 1, 0);
                #endregion


                //장바구니 리스트 그리드에 추가 
                PurchaseListGrid.Children.Add(inGrid, 0, row);
                row++;
                if (g_PurchasedetailInfos.Count > 1)
                {
                    PurchaseListGrid.RowDefinitions.Add(new RowDefinition { Height = 3 });
                    #region 구분선
                    BoxView gridline = new BoxView
                    {
                        BackgroundColor = Color.FromHex("#f4f2f2"),
                        VerticalOptions = LayoutOptions.End,
                        HorizontalOptions = LayoutOptions.FillAndExpand
                    };
                    //구분선 그리드에 추가 
                    PurchaseListGrid.Children.Add(gridline, 0, row);
                    row++;
                    #endregion
                }
            }
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
                        Point_label.Text = int.Parse(test).ToString("N0");
                    }
                }
            }
            else
            {
                Point_label.Text = int.Parse("0").ToString("N0");
            }
        }

        public void ShowPrice()
        {
            for (int i = 0; i < g_PurchasedetailInfos.Count; i++)
            {
                price += int.Parse(g_PurchasedetailInfos[i].PDL_ALLPRICE);
            }
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
            tempdeliveryprice = deliveryprice;
            Purchase_AllPrice_label.Text = (price + tempdeliveryprice - UsedPoint).ToString("N0");
        }

        private void Radio2_Clicked(object sender, EventArgs e)
        {
            prepaymentradio.Source = "radio_unchecked_icon.png";
            Cashondeliveryradio.Source = "radio_checked_icon.png";
            tempdeliveryprice = 0;
            if ((price + tempdeliveryprice - UsedPoint) < 0)
            {
                UsedPoint = price; 
                Point_label.Text = (int.Parse(Point_label.Text.Replace(",", "")) + (OldPoint-UsedPoint)).ToString("N0");
                Point_box.Text = UsedPoint.ToString();
                price = 0;
                Purchase_AllPrice_label.Text = price.ToString("N0");
            }
            else
            {
                Purchase_AllPrice_label.Text = (price + tempdeliveryprice - UsedPoint).ToString("N0");
            }
        }

        private void UsedPointBtn_Clicked(object sender, EventArgs e)
        {
            try
            {
                UsedPoint = int.Parse(Point_box.Text);
                OldPoint = UsedPoint;
                Purchase_AllPrice_label.Text = (price + tempdeliveryprice - UsedPoint).ToString("N0");
                Point_label.Text = (int.Parse(Point_label.Text.Replace(",", "")) + (OldPoint-UsedPoint)).ToString("N0");
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
                if (e.NewTextValue.Contains(".") || e.NewTextValue.Contains("-"))
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
                    if (int.Parse(Point_box.Text) > price)
                    {
                        Point_box.Text = (price + tempdeliveryprice).ToString();
                    }
                    else
                    {
                        if (int.Parse(Point_box.Text) > int.Parse(Point_label.Text.Replace(",", "")))
                        {
                            Point_box.Text = Point_label.Text.Replace(",", "");
                        }
                    }

                }
            }
            catch
            {

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
                            PL_USED_POINT = UsedPoint.ToString(),
                            PL_ISSUCCESS = "",
                            PL_DELIVERYPAY_TYPE = "1",
                            PL_PAYMENT_PRICE = Purchase_AllPrice_label.Text.ToString().Replace(",", ""),
                            AC_NUM = (Combo.SelectedIndex + 1).ToString(),
                            G_PD_LIST = g_PurchasedetailInfos,
                            PL_ACCUSER_NAME = Name_box.Text,
                            PL_DV_NAME = MyNameLabel.Text,
                            PL_DV_PHONE = MyPhoneLabel.Text
                        };
                    }
                    else
                    {
                        g_PurchaseInfo = new G_PurchaseInfo
                        {
                            ID = "01024313236",
                            PL_DELIVERY_ADDRESS = EntryAdress.Text,
                            PL_USED_POINT = UsedPoint.ToString(),
                            PL_ISSUCCESS = "",
                            PL_DELIVERYPAY_TYPE = "2",
                            PL_PAYMENT_PRICE = Purchase_AllPrice_label.Text.ToString().Replace(",", ""),
                            AC_NUM = (Combo.SelectedIndex + 1).ToString(),
                            G_PD_LIST = g_PurchasedetailInfos,
                            PL_ACCUSER_NAME = Name_box.Text,
                            PL_DV_NAME = MyNameLabel.Text,
                            PL_DV_PHONE = MyPhoneLabel.Text
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