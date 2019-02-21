using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketRoom.Models.Custom;
using TicketRoom.Views.MainTab.Dael.Purchase;
using TicketRoom.Views.MainTab.Shop;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopupDelivery
    {
        ShopOrderPage sop;
        public string input_string = "";
        public bool is_input = false;
        public PopupDelivery(ShopOrderPage sop)
        {
            InitializeComponent();
            this.sop = sop;
            Init();
        }
        private string returnStringChangeColor(int n)
        {
            string temp = "";
            for(int i = 0; i<DeliveryGrid.Children.Count-1; i++)
            {
                if(i == n)
                {
                    CustomLabel tempLabel = (CustomLabel)DeliveryGrid.Children.ElementAt(i);
                    tempLabel.TextColor = Color.CornflowerBlue;
                    temp = tempLabel.Text;
                }
                else
                {
                    CustomLabel tempLabel = (CustomLabel)DeliveryGrid.Children.ElementAt(i);
                    tempLabel.TextColor = Color.Gray;
                }
            }
            return temp;
        }
        private void Init()
        {
            DeliveryGrid.Children[0].GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    input_string = returnStringChangeColor(0);
                    MyDeliveryEntry.IsVisible = false;
                    is_input = false;
                })
            });

            DeliveryGrid.Children[1].GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    input_string = returnStringChangeColor(1);
                    MyDeliveryEntry.IsVisible = false;
                    is_input = false;
                })
            });

            DeliveryGrid.Children[2].GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    input_string = returnStringChangeColor(2);
                    MyDeliveryEntry.IsVisible = false;
                    is_input = false;
                })
            });

            DeliveryGrid.Children[3].GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    input_string = returnStringChangeColor(3);
                    MyDeliveryEntry.IsVisible = true;
                    input_string = MyDeliveryEntry.Text;
                    is_input = true;
                })
            });
        }

        private async void ConfirmBtn_Clicked(object sender, EventArgs e)
        {
            if(is_input == true)
            {
                input_string = MyDeliveryEntry.Text;
            }
            if(await DisplayAlert("배송 요청사항 : ", input_string , "확인", "취소"))
            {
                sop.MyDeliveryLabel.Text = input_string;
                PopupNavigation.Instance.RemovePageAsync(this);
            }
        }
    }
}