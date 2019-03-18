using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.MyPage.PurchaseList
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeliveryLookup : ContentPage
    {
        string carrier_id = "";
        string track_id = ""; // 송장번호
        string delivery_api_adress = "";

        public DeliveryLookup(string track_id)
        { 
            InitializeComponent();
            #region IOS의 경우 초기화
            NavigationPage.SetHasNavigationBar(this, false); // Navigation Bar 지우는 코드 생성자에 
            if (Xamarin.Forms.Device.OS == TargetPlatform.iOS)
            {
                TabGrid.RowDefinitions[0].Height = 50;
            }
            #endregion
            this.track_id = track_id;

            Init();
        }
        private void Init()
        {
            //delivery_api_adress = "https://tracker.delivery/#/" + carrier_id + "/" + track_id;
            //default
            delivery_api_adress = "https://service.epost.go.kr/trace.RetrieveDomRigiTraceList.comm?sid1=" + track_id + "&displayHeader=N"; // 우체국 등기
            //delivery_api_adress = "https://tracker.delivery/#/kr.logen/90179076831"; // 택배
            DeliveryWeb.Source = delivery_api_adress;
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}