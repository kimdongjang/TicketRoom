using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TicketRoom.Views.MainTab.Dael.Purchase;
using TicketRoom.Views.MainTab.MyPage;
using TicketRoom.Views.MainTab.MyPage.SaleList;
using TicketRoom.Views.MainTab.Shop;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppExit_Pop
    {
        MainPage mainpage;
        public AppExit_Pop(MainPage mainpage)
        {
            InitializeComponent();
            this.mainpage = mainpage;
        }
        
        private void ConfirmBtn_Clicked(object sender, EventArgs e)
        {
            mainpage.isexit_check_result = false;
            PopupNavigation.Instance.RemovePageAsync(this);
        }

        private void CancelBtn_Clicked(object sender, EventArgs e)
        {
            mainpage.isexit_check_result = true;
            PopupNavigation.Instance.RemovePageAsync(this);
        }
    }
}