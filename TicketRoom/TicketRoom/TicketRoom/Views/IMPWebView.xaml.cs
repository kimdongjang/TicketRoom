using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
        string token = "";
        public IMPWebView()
        {
            NavigationPage.SetHasNavigationBar(this, false); // Navigation Bar 지우는 코드 생성자에 입력
            InitializeComponent();
            Init();
        }
        private async Task Init()
        {
            browser.Navigated += WebView_Navigated;

            token = GetToken("0355094063652427", "X99IhH4l6FSbElhjFVUSl7DJKWw7AKGxTQfbykxE0pPFK7Zq3Ujo1W8MTEUtoA0iqguYB1DBrthcAgCD");
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

                        
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                
            }
        }

        public string GetToken(string imp_key, string imp_secret)
        {
            string str = @"{";
            str += "imp_key:'" + "0355094063652427";  //아이디찾기에선 Name으로 
            str += "',imp_secret	:'" + "X99IhH4l6FSbElhjFVUSl7DJKWw7AKGxTQfbykxE0pPFK7Zq3Ujo1W8MTEUtoA0iqguYB1DBrthcAgCD";
            str += "'}";

            //// JSON 문자열을 파싱하여 JObject를 리턴
            JObject jo = JObject.Parse(str);

            UTF8Encoding encoder = new UTF8Encoding();
            byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

            //request.Method = "POST";
            HttpWebRequest request = WebRequest.Create("https://api.iamport.kr/users/getToken") as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = data.Length;

            //request.Expect = "application/json";

            request.GetRequestStream().Write(data, 0, data.Length);

            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                    Console.Out.WriteLine("Error fetching data. Server returned status code: {0}", response.StatusCode);
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    var readdata = reader.ReadToEnd();
                    if (readdata != null && readdata != "")
                    {
                        return JsonConvert.DeserializeObject<string>(readdata);
                    }
                }
            }
            return "false";
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