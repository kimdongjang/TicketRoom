using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
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
                Uri = "http://192.168.0.2:8080/solindex_mvc/NewFile.jsp?p1=%27inicis%27&p2=2&p3=3&p4=444444&p5=555555&p6=6&p7=7&p8=8&p9=9",
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            Content = webViewXaml;
            //webViewXaml.EvaluateJavaScriptAsync($"func1( \"inicis\",\"card\",\"merchant_\",\"stemp\",\"stemp\",\"stemp\",\"stemp\",\"stemp\",\"stemp\")");

            webViewXaml.RegisterAction(data => DisplayAlert("Alert", "Hello " + data, "OK"));

        }
	}
}