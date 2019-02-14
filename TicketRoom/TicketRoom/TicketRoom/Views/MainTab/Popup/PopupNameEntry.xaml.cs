using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketRoom.Views.MainTab.Dael.Purchase;
using TicketRoom.Views.MainTab.Shop;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopupNameEntry
    {
        ShopOrderPage sop;
        PurchaseDetailPage pdp;
        int type = 0; //팝업 쓰는 위치 (1: 상품권 구매 , 2: 쇼핑몰 구매)
        public PopupNameEntry(ShopOrderPage sop)
        {
            InitializeComponent();
            this.sop = sop;
            type = 2;
        }

        public PopupNameEntry(PurchaseDetailPage pdp)
        {
            InitializeComponent();
            this.pdp = pdp;
            type = 1;
        }

        private void ConfirmBtn_Clicked(object sender, EventArgs e)
        {
            if (type == 1)
            {
                pdp.MyNameLabel.Text = MyNameEntry.Text;
                PopupNavigation.Instance.RemovePageAsync(this);
            }
            else if (type == 2)
            {
                sop.MyNameLabel.Text = MyNameEntry.Text;
                PopupNavigation.Instance.RemovePageAsync(this);
            }
        }
    }
}