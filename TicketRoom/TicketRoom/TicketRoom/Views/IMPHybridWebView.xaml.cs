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
using TicketRoom.Views.MainTab.MyPage.Point;
using TicketRoom.Views.MainTab.Shop;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class IMPHybridWebView : ContentPage
	{
        string token = "";
        IMPParam param;
        ShopOrderPage sop;
        PointChargeView pcv;
        IMP_RValue imp_rvalue; // 결제 내역 조회

        public IMPHybridWebView (IMPParam param, ShopOrderPage sop)
        {
            InitializeComponent();
            this.param = param;
            this.sop = sop;
            Init();
        }
        public IMPHybridWebView(IMPParam param, PointChargeView pcv)
        {
            InitializeComponent();
            this.param = param;
            this.pcv = pcv;
            Init();
        }

        // 초기화 코드
        public void Init()
        {
            token = GetToken("0355094063652427", "X99IhH4l6FSbElhjFVUSl7DJKWw7AKGxTQfbykxE0pPFK7Zq3Ujo1W8MTEUtoA0iqguYB1DBrthcAgCD");
            NavigationPage.SetHasNavigationBar(this, false); // Navigation Bar 지우는 코드 생성자에 입력 token = GetToken("0355094063652427", "X99IhH4l6FSbElhjFVUSl7DJKWw7AKGxTQfbykxE0pPFK7Zq3Ujo1W8MTEUtoA0iqguYB1DBrthcAgCD");
            var webViewXaml = new HybridWebView
            {
                Uri = "file:///android_asset/IMP.html",
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Param = param,
            };
            Content = webViewXaml;

            webViewXaml.RegisterAction(CallbackFunc);
        }

        private void CallbackFunc(string data)
        {
            GetOrderInfo(data);
            Navigation.PopAsync();
            if (pcv != null) // 포인트 뷰에서 접근했을 경우
            {
                pcv.PurchaseSuccessProcessAsync(imp_rvalue);
                return;
            }
            else if(sop != null)
            {
                sop.PurchaseSuccessProcessAsync(imp_rvalue);
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
                        var test = JsonConvert.DeserializeObject(readdata);
                        JObject applyJObj = JObject.Parse(test.ToString());
                        return applyJObj["response"]["access_token"].ToString();
                    }
                }
            }
            return "false";
        }

        // 주문 내역을 가져옴
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
                    // 결제 신청 후 파라미터값 확인
                    var test = JsonConvert.DeserializeObject(readdata);
                    JObject applyJObj = JObject.Parse(test.ToString());
                    imp_rvalue = new IMP_RValue
                    {
                        SH_IMP_UID = applyJObj["response"]["imp_uid"].ToString(),
                        SH_MERCHANT_UID = applyJObj["response"]["merchant_uid"].ToString(),
                        SH_PAY_METHOD = applyJObj["response"]["pay_method"].ToString(),
                        SH_BANK_NAME = applyJObj["response"]["bank_name"].ToString(),
                        SH_CARD_NAME = applyJObj["response"]["card_name"].ToString(),
                        SH_NAME = applyJObj["response"]["name"].ToString(),
                        SH_AMOUNT = applyJObj["response"]["amount"].ToString(),
                        SH_BUYER_NAME = applyJObj["response"]["buyer_name"].ToString(),
                        SH_BUYER_EMAIL = applyJObj["response"]["buyer_email"].ToString(),
                        SH_BUYER_TEL = applyJObj["response"]["buyer_tel"].ToString(),
                        SH_BUYER_ADDR = applyJObj["response"]["buyer_addr"].ToString(),
                        SH_BUYER_POSTCODE = applyJObj["response"]["buyer_postcode"].ToString(),
                        SH_STATUS = applyJObj["response"]["status"].ToString(), // ready(미결제), paid(결제완료), cancelled(결제취소, 부분취소포함), failed(결제실패)
                    };
                    //string test = JsonConvert.DeserializeObject<string>(readdata);
                }
            }
        }
    }
}