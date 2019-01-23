using System;
using System.Collections.Generic;
using TicketRoom.Views.Users.CreateUser;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.Dael.Purchase
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PurchaseDetailPage : ContentPage
    {
        Dictionary<string, string> data = null;
        InputAdress adrAPI;
        public PurchaseDetailPage(Dictionary<string, string> data)
        {
            InitializeComponent();
            this.data = data;
            string s = "";
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
    }
}