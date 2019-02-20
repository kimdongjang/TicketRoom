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

        public TermsContentPage()
        {
            InitializeComponent();
            PageTitle = "상품권 거래 이용약관 동의";
            SubTitle = "상품권 거래 이용 약관";
            TermsContent = "상품권 거래 이용 약관상품권 거래 이용 약관상품권 거래 이용 약관상품권 거래 이용 약관상품권 거래 이용 약관상품권 거래 이용 약관상품권 거래 이용 약관상품권 거래 이용 약관상품권 거래 이용 약관상품권 거래 이용 약관상품권 거래 이용 약관상품권 거래 이용 약관상품권 거래 이용 약관상품권 거래 이용 약관상품권 거래 이용 약관상품권 거래 이용 약관상품권 거래 이용 약관상품권 거래 이용 약관상품권 거래 이용 약관상품권 거래 이용 약관상품권 거래 이용 약관상품권 거래 이용 약관상품권 거래 이용 약관상품권 거래 이용 약관상품권 거래 이용 약관상품권 거래 이용 약관상품권 거래 이용 약관상품권 거래 이용 약관상품권 거래 이용 약관상품권 거래 이용 약관상품권 거래 이용 약관상품권 거래 이용 약관상품권 거래 이용 약관상품권 거래 이용 약관상품권 거래 이용 약관상품권 거래 이용 약관상품권 거래 이용 약관상품권 거래 이용 약관상품권 거래 이용 약관상품권 거래 이용 약관상품권 거래 이용 약관상품권 거래 이용 약관상품권 거래 이용 약관상품권 거래 이용 약관상품권 거래 이용 약관상품권 거래 이용 약관상품권 거래 이용 약관상품권 거래 이용 약관상품권 거래 이용 약관상품권 거래 이용 약관상품권 거래 이용 약관상품권 거래 이용 약관상품권 거래 이용 약관상품권 거래 이용 약관상품권 거래 이용 약관상품권 거래 이용 약관상품권 거래 이용 약관상품권 거래 이용 약관상품권 거래 이용 약관상품권 거래 이용 약관상품권 거래 이용 약관상품권 거래 이용 약관";
            BindingContext = this;
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}