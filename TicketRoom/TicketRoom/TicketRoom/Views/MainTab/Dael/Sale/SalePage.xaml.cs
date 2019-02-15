using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketRoom.Models.Gift;
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

        public SalePage(G_ProductInfo productInfo)
        {
            InitializeComponent();
            this.productInfo = productInfo;
            Pro_imgae.Source = productInfo.PRODUCTIMAGE;
            Pro_Name.Text = productInfo.PRODUCTTYPE + " " + productInfo.PRODUCTVALUE;
            Pro_price.Text = productInfo.SALEDISCOUNTPRICE + "[" + productInfo.SALEDISCOUNTRATE + "%]";
        }

        public SalePage()
        {
            InitializeComponent();
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            this.OnBackButtonPressed();
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
        }

        private void Radio2_Clicked(object sender, EventArgs e)
        {
            Paperradio.Source = "radio_unchecked_icon.png";
            Pinnumberradio.Source = "radio_checked_icon.png";
            RadioContent1.IsVisible = false;
            RadioContent2.IsVisible = true;
        }

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void AddPinNumBtn_Clicked(object sender, EventArgs e)
        {
            if (Pin1.Text != null && Pin1.Text != "" && Pin2.Text != null && Pin2.Text != "" && Pin3.Text != null && Pin3.Text != "" && Pin4.Text != null && Pin4.Text != "")
            {
                PinNumlist.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                #region 핀번호 라벨
                Label label = new Label
                {
                    Text = Pin1.Text + " " + Pin2.Text + " " + Pin3.Text + " " + Pin4.Text,
                    FontSize = 15,
                    TextColor = Color.Black,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.Start
                };
                #endregion

                #region 그리드에 추가
                PinNumlist.Children.Add(label, 0, PinNumCount); //부모그리드에 핀번호 추가
                PinNumCount++;
                #endregion
            }
            else
            {
                DisplayAlert("알림", "핀번호를 입력해주세요", "OK");
            }

        }
    }
}