using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TicketRoom.Views.MainTab.Shop;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopupPhoneEntry
    {
        ShopOrderPage sop;
        public PopupPhoneEntry(ShopOrderPage sop)
        {
            InitializeComponent();
            this.sop = sop;
        }

        private void ConfirmBtn_Clicked(object sender, EventArgs e)
        {
            sop.MyPhoneLabel.Text = MyPhoneEntry.Text;
            PopupNavigation.Instance.RemovePageAsync(this);
        }

        private void MyPhoneEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                MyPhoneEntry.Text = Regex.Replace(MyPhoneEntry.Text, @"\D", "");
                if (e.NewTextValue.Contains(".") || e.NewTextValue.Equals("-"))
                {
                    if (e.OldTextValue != null)
                    {
                        MyPhoneEntry.Text = e.OldTextValue;
                    }
                    else
                    {
                        MyPhoneEntry.Text = "";
                    }
                    return;
                }
            }
            catch
            {

            }
        }
    }
}