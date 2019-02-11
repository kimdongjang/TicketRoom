using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketRoom.Views.MainTab.Shop;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopupNameEntry
    {
        ShopOrderPage sop;
        public PopupNameEntry(ShopOrderPage sop)
        {
            InitializeComponent();
            this.sop = sop;
        }

        private void ConfirmBtn_Clicked(object sender, EventArgs e)
        {
            sop.MyNameLabel.Text = MyNameEntry.Text;
            PopupNavigation.Instance.RemovePageAsync(this);
        }
    }
}