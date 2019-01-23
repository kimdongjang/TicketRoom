using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.MyPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyInfoUpdatePage : ContentPage
    {
        public MyInfoUpdatePage()
        {
            InitializeComponent();
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            this.OnBackButtonPressed();
        }

        private void CheckNumSendBtn_Clicked(object sender, EventArgs e)
        {
            CheckNumGrid.IsVisible = true;
        }

        private void CheckNumCheckBtn_Clicked(object sender, EventArgs e)
        {

        }
    }
}