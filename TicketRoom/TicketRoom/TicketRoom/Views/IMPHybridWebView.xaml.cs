using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
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
        string token = "";
        public IMPHybridWebView ()
        {
            NavigationPage.SetHasNavigationBar(this, false); // Navigation Bar 지우는 코드 생성자에 입력
            InitializeComponent();

            token = GetToken("0355094063652427", "X99IhH4l6FSbElhjFVUSl7DJKWw7AKGxTQfbykxE0pPFK7Zq3Ujo1W8MTEUtoA0iqguYB1DBrthcAgCD");
            var webViewXaml = new HybridWebView
            {
                Uri = "file:///android_asset/IMP.html",
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            Content = webViewXaml;
            //webViewXaml.EvaluateJavaScriptAsync($"func1( \"inicis\",\"card\",\"merchant_\",\"stemp\",\"stemp\",\"stemp\",\"stemp\",\"stemp\",\"stemp\")");

            webViewXaml.RegisterAction(CallbackFunc);

        }

        private void CallbackFunc(string data)
        {
            DisplayAlert("Alert", "Hello " + data, "OK");
            GetOrderInfo(data);
            Navigation.PopAsync();
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
                        var test = JsonConvert.DeserializeObject(readdata);
                        JObject applyJObj = JObject.Parse(test.ToString());
                        return applyJObj["response"]["access_token"].ToString();
                    }
                }
            }
            return "false";
        }

        public void GetOrderInfo(string imp_uid)
        {
            HttpWebRequest request = WebRequest.Create("https://api.iamport.kr/payments/"+ imp_uid + "?_token="+ token) as HttpWebRequest;
            request.Method = "GET";

            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                    Console.Out.WriteLine("Error fetching data. Server returned status code: {0}", response.StatusCode);
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    var readdata = reader.ReadToEnd();
                    //string test = JsonConvert.DeserializeObject<string>(readdata);
                }
            }
        }
    }
}