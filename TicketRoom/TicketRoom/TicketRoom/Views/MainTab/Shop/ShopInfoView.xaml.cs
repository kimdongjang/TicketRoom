using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketRoom.Models.Custom;
using TicketRoom.Models.ShopData;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.Shop
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShopInfoView : ContentView
    {
        string myShopName = "";
        ShopDataFunc dataclass = new ShopDataFunc();

        public ShopInfoView(string titleName)
        {
            InitializeComponent();
            myShopName = titleName;
            Init();
        }
        private void Init()
        {
            #region 쇼핑 메인 정보
            CustomLabel editor = new CustomLabel
            {
                Text = dataclass.GetShopInfoData(myShopName),
                Size = 18,
                TextColor = Color.Black,
                IsEnabled = false,
                Margin = 10,
                HeightRequest = 300,
            };
            InfoStack.Children.Add(editor);
            #endregion
        }
    }
}