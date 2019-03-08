using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
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
            //string result = await browser.EvaluateJavaScriptAsync($"func1( \"stemp\",\"stemp\",\"stemp\",\"stemp\",\"stemp\",\"stemp\",\"stemp\",\"stemp\",\"stemp\")");
            //await DisplayAlert("", $"Factorial of {stemp} is {result}.", "");
        }

        public async void WebView_Navigated(object sender, WebNavigatedEventArgs e)
        {
            if (e.Url.StartsWith("file:///android_asset/IMP.html"))
            {
                Task.Run(JSRun);
            }
            else if (e.Url.StartsWith("http://175.115.110.17:8088/Service1.svc"))
            {
                try
                {
                    string temp = e.Url.Replace("http://175.115.110.17:8088/Service1.svc?imp_uid=", "");
                    if (temp.Contains("&"))
                    {
                        string[] imp_uid = temp.Split('&');
                        //imp_uid[0]
                        //var requestUrl = "https://api.iamport.kr/users/getToken";

                        //var httpClient = new HttpClient();

                        //var userJson = await httpClient.GetStringAsync(requestUrl);

                        
                        string myJson = @"{";
                        myJson += "imp_key:'" + "0355094063652427";  //아이디찾기에선 Name으로 
                        myJson += "',imp_secret	:'" + "X99IhH4l6FSbElhjFVUSl7DJKWw7AKGxTQfbykxE0pPFK7Zq3Ujo1W8MTEUtoA0iqguYB1DBrthcAgCD";
                        myJson += "'}";

                    using (var client = new HttpClient())
                        {
                            var response = await client.PostAsync("https://api.iamport.kr/users/getToken", new StringContent(myJson, Encoding.UTF8, "application/json"));
                            string s = "d";
                        }
                        //var facebookProfile = JsonConvert.DeserializeObject<FacebookProfile>(userJson);
                    }
                }
                catch (Exception)
                {

                    throw;
                }
                
            }
        }

        public string retorno;
        public async Task JSRun()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                //retorno = await browser.EvaluateJavaScriptAsync("init();");
                IMPParam param = new IMPParam();
                param.pg = "inicis";
                param.pay_method = "card";
                param.merchant_uid = "merchant_" + System.DateTime.Now;
                param.name = "문화상품권 5천원 구매";
                param.buyer_email = "iamport@siot.do";
                param.buyer_name = "구매자이름";
                param.buyer_tel = "010-1234-5678";
                param.amount = 5000;
                //retorno = await browser.EvaluateJavaScriptAsync($"func1( \"inicis\",\"card\",\"merchant_\",\"stemp\",\"stemp\",\"stemp\",\"stemp\",\"stemp\",\"stemp\")");
                String blank = "\"";
                retorno = await browser.EvaluateJavaScriptAsync($"func4(" + blank + param.pg.ToString() + blank + "," +
                                                                                           blank + param.pay_method.ToString() + blank + "," +
                                                                                            blank + param.merchant_uid.ToString() + blank + "," +
                                                                                            blank + param.name.ToString() + blank + "," +
                                                                                            param.amount.ToString() + "," +
                                                                                            "\"stemp\",\"stemp\",\"stemp\",\"stemp\")");
            });
        }
    }

}