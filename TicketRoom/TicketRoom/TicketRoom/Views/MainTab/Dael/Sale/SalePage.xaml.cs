using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TicketRoom.Models.Custom;
using TicketRoom.Models.Gift;
using TicketRoom.Models.Gift.SaleList;
using TicketRoom.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.Dael.Sale
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SalePage : ContentPage
    {
        int PinNumCount = 0;
        G_ProductInfo productInfo = null;

        public string Sale_Price = "";
        public string DiscountSale_Price = "";
        GiftDBFunc giftDBFunc = GiftDBFunc.Instance();
        List<G_PinInfo> g_Pinlist = new List<G_PinInfo>();

        public SalePage(G_ProductInfo productInfo)
        {
            InitializeComponent();

            #region IOS의 경우 초기화
            NavigationPage.SetHasNavigationBar(this, false); // Navigation Bar 지우는 코드 생성자에 입력
            if (Xamarin.Forms.Device.OS == TargetPlatform.iOS)
            {
                MainGrid.RowDefinitions[0].Height = 50;
            }
            #endregion
            this.productInfo = productInfo;
            Pro_imgae.Source = ImageSource.FromUri(new Uri(productInfo.PRODUCTIMAGE));
            Pro_Name.Text = productInfo.PRODUCTTYPE + " " + productInfo.PRODUCTVALUE;
            Pro_price.Text = productInfo.SALEDISCOUNTPRICE + "[" + productInfo.SALEDISCOUNTRATE + "%]";
            ShowPinAddForm(productInfo);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Global.isSaleBtnclicked = true;
        }

        public void ShowPinAddForm(G_ProductInfo productInfo)
        {
            if (int.Parse(productInfo.DETAILCATEGORYNUM) == 1) //도서문화상품권
            {
                Pin4.MaxLength = 4;
                Pin4.Placeholder = "4자리";
                PinAddBtn_Grid1.IsVisible = false;
                PinAddBtn_Grid2.IsVisible = true;
                PinCertify_Entry.MaxLength = 4;
                PinCertify_Entry.Placeholder = "인증번호 4자리";
            }
            else if (int.Parse(productInfo.DETAILCATEGORYNUM) == 2) //컬쳐랜드
            {
                Pin4.MaxLength = 6;
                Pin4.Placeholder = "4~6자리";
                PinAddBtn_Grid1.IsVisible = true;
                PinAddBtn_Grid2.IsVisible = false;
            }
            else if (int.Parse(productInfo.DETAILCATEGORYNUM) == 3) //해피머니
            {
                Pin4.MaxLength = 4;
                Pin4.Placeholder = "4자리";
                PinAddBtn_Grid1.IsVisible = false;
                PinAddBtn_Grid2.IsVisible = true;
                PinCertify_Entry.MaxLength = 8;
                PinCertify_Entry.Placeholder = "발행일 or 인증번호 8자리";
            }
            else
            {
                PinGrid.IsVisible = false;
            }
        }

        public SalePage()
        {
            InitializeComponent();
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            if (Global.isSaleBtnclicked)
            {
                Global.isSaleBtnclicked = false;
                Navigation.PopAsync();
            }
        }

        private void PlusBtn_Clicked(object sender, EventArgs e)
        {
            Count_label.Text = (int.Parse(Count_label.Text) + 1).ToString();
            Sale_Price = (int.Parse(productInfo.PROPRICE) * int.Parse(Count_label.Text)).ToString("N0");
            DiscountSale_Price = (int.Parse(productInfo.SALEDISCOUNTPRICE) * int.Parse(Count_label.Text)).ToString("N0");
            Sale_DiscountPrice_span.Text = DiscountSale_Price;
            //int teqwteqw = int.Parse(Purchase_Price.Replace(",", "")); //1,000->1000
        }

        private void MinusBtn_Clicked(object sender, EventArgs e)
        {
            if (int.Parse(Count_label.Text) > 0)
            {
                Count_label.Text = (int.Parse(Count_label.Text) - 1).ToString();
                Sale_Price = (int.Parse(productInfo.PROPRICE) * int.Parse(Count_label.Text)).ToString("N0");
                DiscountSale_Price = (int.Parse(productInfo.SALEDISCOUNTPRICE) * int.Parse(Count_label.Text)).ToString("N0");
                Sale_DiscountPrice_span.Text = DiscountSale_Price;
            }
        }

        private void OnDateSelected(object sender, DateChangedEventArgs e)
        {

        }

        private void Radio1_Clicked(object sender, EventArgs e)
        {
            Paperradio.Source = "radio_checked_icon.png";
            Pinnumberradio.Source = "radio_unchecked_icon.png";
            RadioContent1.IsVisible = true;
            RadioContent2.IsVisible = false;
            ProCount_Grid.IsVisible = true;
        }

        private void Radio2_Clicked(object sender, EventArgs e)
        {
            Paperradio.Source = "radio_unchecked_icon.png";
            Pinnumberradio.Source = "radio_checked_icon.png";
            RadioContent1.IsVisible = false;
            RadioContent2.IsVisible = true;
            ProCount_Grid.IsVisible = false;
        }

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void AddPinNumBtn_Clicked(object sender, EventArgs e)
        {
            if (int.Parse(productInfo.DETAILCATEGORYNUM) == 1) //도서문화상품권
            {
                if (Pin1.Text != null && Pin1.Text != "" && Pin2.Text != null && Pin2.Text != "" && Pin3.Text != null && Pin3.Text != "" && Pin4.Text != null && Pin4.Text != "")
                {
                    if(PinCertify_Entry.Text!=null && PinCertify_Entry.Text != "")
                    {
                        addpin();
                    }
                    else
                    {
                        DisplayAlert("알림", "인증번호를 입력해주세요", "OK");
                    }
                }
                else
                {
                    DisplayAlert("알림", "핀번호를 입력해주세요", "OK");
                }
            }
            else if (int.Parse(productInfo.DETAILCATEGORYNUM) == 2) //컬쳐랜드
            {
                if (Pin1.Text != null && Pin1.Text != "" && Pin2.Text != null && Pin2.Text != "" && Pin3.Text != null && Pin3.Text != "" && Pin4.Text != null && Pin4.Text != "")
                {
                    addpin();
                }
                else
                {
                    DisplayAlert("알림", "핀번호를 입력해주세요", "OK");
                }
            }
            else if (int.Parse(productInfo.DETAILCATEGORYNUM) == 3) //해피머니
            {
                if (Pin1.Text != null && Pin1.Text != "" && Pin2.Text != null && Pin2.Text != "" && Pin3.Text != null && Pin3.Text != "" && Pin4.Text != null && Pin4.Text != "")
                {
                    if (PinCertify_Entry.Text != null && PinCertify_Entry.Text != "")
                    {
                        addpin();
                    }
                    else
                    {
                        DisplayAlert("알림", "인증번호를 입력해주세요", "OK");
                    }
                }
                else
                {
                    DisplayAlert("알림", "핀번호를 입력해주세요", "OK");
                }
            }
        }

        public void addpin()
        {
            if (PinCertify_Entry == null)
            {
                PinCertify_Entry.Text = "";
            }

            PinNumlist.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            #region 핀번호 라벨
            CustomLabel label = new CustomLabel
            {
                Text = Pin1.Text + " " + Pin2.Text + " " + Pin3.Text + " " + Pin4.Text + " " + PinCertify_Entry.Text,
                Size = 15,
                TextColor = Color.Black,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.Start
            };
            #endregion

            #region 그리드에 추가
            PinNumlist.Children.Add(label, 0, PinNumCount); //부모그리드에 핀번호 추가
            PinNumCount++;
            #endregion

            G_PinInfo g_PinInfo = new G_PinInfo
            {
                SDL_PIN1 = Pin1.Text,
                SDL_PIN2 = Pin2.Text,
                SDL_PIN3 = Pin3.Text,
                SDL_PIN4 = Pin4.Text,
                SDL_CERTIFICATIONNUM = PinCertify_Entry.Text
            };

            g_Pinlist.Add(g_PinInfo);
            
            DiscountSale_Price = (int.Parse(productInfo.SALEDISCOUNTPRICE) * g_Pinlist.Count).ToString("N0");
            Sale_DiscountPrice_span.Text = DiscountSale_Price;
        }

        public async Task ShowMessage(string message, string title, string buttonText, Action afterHideCallback)
        {
            await DisplayAlert(title, message, buttonText);

            afterHideCallback?.Invoke();
        }

        private async void SaleBtn_Cliecked(object sender, EventArgs e)
        {
            if (Global.isSaleBtnclicked)
            {
                Global.isSaleBtnclicked = false;

                G_SaleInfo g_SaleInfo = null;

                string date = startDatePicker.Date.ToString("yyyy/MM/dd HH:mm:ss");

                if (Paperradio.Source.ToString().Contains("radio_checked_icon.png"))
                {
                    if (int.Parse(Count_label.Text) > 0)
                    {
                        if (BankName_picker.SelectedItem != null)
                        {
                            if (UserName_box.Text != null && UserName_box.Text != "")
                            {
                                if (AccountNum_box.Text != null && AccountNum_box.Text != "")
                                {
                                    g_SaleInfo = new G_SaleInfo
                                    {
                                        SL_USERID = Global.ID,
                                        SL_PRONUM = productInfo.PRONUM,
                                        SL_PROCOUNT = Count_label.Text,
                                        SL_BANK_NAME = BankName_picker.SelectedItem.ToString(),
                                        SL_ACC_NAME = UserName_box.Text,
                                        SL_ACC_NUM = AccountNum_box.Text,
                                        SL_SEND_DATE = date,
                                        SL_SENDSTRING = SendString_editor.Text,
                                        SL_TOTAL_PRICE = Sale_DiscountPrice_span.Text.Replace(",", ""),
                                        SL_SALEPRO_TYPE = "1",
                                        SL_PIN_LIST = g_Pinlist
                                    };

                                    int result = giftDBFunc.UserAddSale(g_SaleInfo);
                                    if (result == 3)
                                    {
                                        await ShowMessage("판매내역에서 확인해주세요.", "알림", "OK", async () =>
                                        {
                                            Navigation.PopToRootAsync();
                                            MainPage mp = (MainPage)Application.Current.MainPage.Navigation.NavigationStack[0];
                                        });
                                    }
                                    else if (result == 2)
                                    {
                                        DisplayAlert("알림", "판매실패", "OK");
                                        Global.isSaleBtnclicked = true;
                                    }
                                    else if (result == 4)
                                    {
                                        DisplayAlert("알림", "서버점검중입니다", "OK");
                                        Global.isSaleBtnclicked = true;
                                    }
                                }
                                else
                                {
                                    DisplayAlert("알림", "입금계좌번호를 입력하세요", "OK");
                                    Global.isSaleBtnclicked = true;
                                }
                            }
                            else
                            {
                                DisplayAlert("알림", "예금주명을 입력하세요", "OK");
                                Global.isSaleBtnclicked = true;
                            }
                        }
                        else
                        {
                            DisplayAlert("알림", "은행명을 입력하세요", "OK");
                            Global.isSaleBtnclicked = true;
                        }
                    }
                    else
                    {
                        DisplayAlert("알림", "수량을 입력해주세요", "OK");
                        Global.isSaleBtnclicked = true;
                    }
                }
                else
                {
                    if (g_Pinlist.Count > 0)
                    {
                        if (BankName_picker2.SelectedItem != null)
                        {
                            if (UserName_box2.Text != null && UserName_box2.Text != "")
                            {
                                if (AccountNum_box2.Text != null && AccountNum_box.Text != "")
                                {
                                    if (OrderNum_box.Text != null && OrderNum_box.Text != "")
                                    {
                                        if (OrderNumCheck_box.Text != null && OrderNumCheck_box.Text != "")
                                        {
                                            if (OrderNum_box.Text.Equals(OrderNumCheck_box.Text))
                                            {
                                                g_SaleInfo = new G_SaleInfo
                                                {
                                                    SL_USERID = Global.ID,
                                                    SL_PRONUM = productInfo.PRONUM,
                                                    SL_PROCOUNT = g_Pinlist.Count.ToString(),
                                                    SL_BANK_NAME = BankName_picker2.SelectedItem.ToString(),
                                                    SL_ACC_NAME = UserName_box2.Text,
                                                    SL_ACC_NUM = AccountNum_box2.Text,
                                                    SL_SEND_DATE = date,
                                                    SL_SENDSTRING = "",
                                                    SL_TOTAL_PRICE = Sale_DiscountPrice_span.Text.Replace(",", ""),
                                                    SL_SALEPRO_TYPE = "2",
                                                    SL_PIN_LIST = g_Pinlist,
                                                    SL_SALE_PW = OrderNum_box.Text
                                                };

                                                int result = giftDBFunc.UserAddSale(g_SaleInfo);
                                                if (result == 3)
                                                {
                                                    await ShowMessage("판매내역에서 확인해주세요.", "알림", "OK", async () =>
                                                    {
                                                        Navigation.PopToRootAsync();
                                                        MainPage mp = (MainPage)Application.Current.MainPage.Navigation.NavigationStack[0];
                                                    });
                                                }
                                                else if (result == 2)
                                                {
                                                    DisplayAlert("알림", "판매실패", "OK");
                                                    Global.isSaleBtnclicked = true;
                                                }
                                                else if (result == 4)
                                                {
                                                    DisplayAlert("알림", "서버점검중입니다", "OK");
                                                    Global.isSaleBtnclicked = true;
                                                }
                                            }
                                            else
                                            {
                                                DisplayAlert("알림", "접수비밀번호가 다릅니다", "OK");
                                                Global.isSaleBtnclicked = true;
                                            }
                                        }
                                        else
                                        {
                                            DisplayAlert("알림", "접수비밀번호확인를 입력하세요", "OK");
                                            Global.isSaleBtnclicked = true;
                                        }
                                    }
                                    else
                                    {
                                        DisplayAlert("알림", "접수비밀번호를 입력하세요", "OK");
                                        Global.isSaleBtnclicked = true;
                                    }
                                }
                                else
                                {
                                    DisplayAlert("알림", "입금계좌번호를 입력하세요", "OK");
                                    Global.isSaleBtnclicked = true;
                                }
                            }
                            else
                            {
                                DisplayAlert("알림", "예금주명을 입력하세요", "OK");
                                Global.isSaleBtnclicked = true;
                            }
                        }
                        else
                        {
                            DisplayAlert("알림", "은행명을 입력하세요", "OK");
                            Global.isSaleBtnclicked = true;
                        }
                    }
                    else
                    {
                        DisplayAlert("알림", "수량을 입력해주세요", "OK");
                        Global.isSaleBtnclicked = true;
                    }
                }
            }
        }
    }
}