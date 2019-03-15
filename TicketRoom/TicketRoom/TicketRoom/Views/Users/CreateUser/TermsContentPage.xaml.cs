using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.Users.CreateUser
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TermsContentPage : ContentPage
    {
        public string PageTitle { get; set; }
        public string SubTitle { get; set; }
        public string TermsContent { get; set; }

        public TermsContentPage(string subtitle,string content)
        {
            InitializeComponent();

            #region IOS의 경우 초기화
            NavigationPage.SetHasNavigationBar(this, false); // Navigation Bar 지우는 코드 생성자에 입력
            if (Xamarin.Forms.Device.OS == TargetPlatform.iOS)
            {
                MainGrid.RowDefinitions[0].Height = 50;
            }
            #endregion

            PageTitle = "티켓룸 서비스 이용약관";
            SubTitle = subtitle;
            TermsContent = content;
            BindingContext = this;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Global.isbackbutton_clicked = true;
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            if (Global.isbackbutton_clicked)
            {
                Global.isbackbutton_clicked = false;
                Navigation.PopAsync();
            }
        }
    }
}