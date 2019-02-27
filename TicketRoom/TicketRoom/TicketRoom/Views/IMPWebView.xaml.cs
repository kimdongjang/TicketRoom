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
    public partial class IMPWebView : ContentPage
    {
        public IMPWebView()
        {
            NavigationPage.SetHasNavigationBar(this, false); // Navigation Bar 지우는 코드 생성자에 입력
            InitializeComponent();
            Init();
        }
        private async Task Init()
        {
            browser.Navigated += WebView_Navigated;

            string stemp = "asdasd";
            //string result = await browser.EvaluateJavaScriptAsync($"func1( \"stemp\",\"stemp\",\"stemp\",\"stemp\",\"stemp\",\"stemp\",\"stemp\",\"stemp\",\"stemp\")");
            //await DisplayAlert("", $"Factorial of {stemp} is {result}.", "");
        }
        public void WebView_Navigated(object sender, WebNavigatedEventArgs e)
        {
            Task.Run(JSRun);
        }

        public string retorno;
        public async Task JSRun()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                IMPParam param = new IMPParam();
                param.pg = "inicis";
                param.pay_method = "card";
                param.merchant_uid = "merchant_" + System.DateTime.Now;
                param.name = "최태영";
                param.amount = 5000;
                //retorno = await browser.EvaluateJavaScriptAsync($"func1( \"inicis\",\"card\",\"merchant_\",\"stemp\",\"stemp\",\"stemp\",\"stemp\",\"stemp\",\"stemp\")");
                String blank = "\"";
                /*retorno = await browser.EvaluateJavaScriptAsync($"func1(" + blank + param.pg.ToString() + blank + "," +
                                                                                           blank + param.pay_method.ToString() + blank + "," +
                                                                                            blank + param.merchant_uid.ToString() + blank + "," +
                                                                                            blank + param.name.ToString() + blank + "," +
                                                                                            param.amount.ToString() + "," +
                                                                                            "\"stemp\",\"stemp\",\"stemp\",\"stemp\")");*/
            });
        }
    }

}