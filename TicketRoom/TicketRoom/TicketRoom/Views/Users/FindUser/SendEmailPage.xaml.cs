using System;
using TicketRoom.Views.Users.Login;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.Users.FindUser
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SendEmailPage : ContentPage
    {

        public SendEmailPage()
        {
            InitializeComponent();
        }

        private void SendEmailBtn_Clicked(object sender, EventArgs e)
        {
            SendEmailBtn.Text = "재전송";
            SendResult_Grid.IsVisible = true;
        }

        private void FindIDPWBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FindPWPage());
        }

        private void SendEmailOKbtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new LoginPage());
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}