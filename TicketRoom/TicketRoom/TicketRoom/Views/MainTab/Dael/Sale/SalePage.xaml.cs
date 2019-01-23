using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.Dael.Sale
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SalePage : ContentPage
    {
        int PinNumCount = 0;
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

        }

        private void MinusBtn_Clicked(object sender, EventArgs e)
        {

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
                #region 약관 내용 Label
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