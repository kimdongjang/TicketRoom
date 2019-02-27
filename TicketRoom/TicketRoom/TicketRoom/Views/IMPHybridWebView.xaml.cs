using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketRoom.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class IMPHybridWebView : ContentPage
	{
		public IMPHybridWebView ()
        {
            NavigationPage.SetHasNavigationBar(this, false); // Navigation Bar 지우는 코드 생성자에 입력
            InitializeComponent();

            var webViewXaml = new HybridWebView
            {
                Uri = "IMP.html",
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            Padding = new Thickness(0, 20, 0, 0);
            Content = webViewXaml;

            //webViewXaml.RegisterAction();
            webViewXaml.InvokeAction("");
            
            
            


        }
        private void Func1(string s)
        {

        }
	}
}