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
                Uri = @"http://192.168.0.2:8080/solindex_mvc/NewFile.jsp?p1= + ""\" + "inicis" + "\"" + "&p2=2&p3=3&p4=4&p5=5&p6=6&p7=7&p8=8&p9=9",
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