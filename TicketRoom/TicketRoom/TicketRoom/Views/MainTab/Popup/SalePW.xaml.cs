using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TicketRoom.Views.MainTab.Dael.Purchase;
using TicketRoom.Views.MainTab.MyPage;
using TicketRoom.Views.MainTab.MyPage.SaleList;
using TicketRoom.Views.MainTab.Shop;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SalePW
    {
        SaleListPage slp;
        string slnum = "";
        public SalePW(SaleListPage slp, string slnum)
        {
            InitializeComponent();
            this.slp = slp;
            this.slnum = slnum;
        }

        private void ConfirmBtn_Clicked(object sender, EventArgs e)
        {
            string str = @"{";
            str += "slnum : '" + slnum;
            str += "',salepw:'" + MyPWEntry.Text;
            str += "'}";

            //// JSON 문자열을 파싱하여 JObject를 리턴
            JObject jo = JObject.Parse(str);

            UTF8Encoding encoder = new UTF8Encoding();
            byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "CheckSalePw") as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = data.Length;

            request.GetRequestStream().Write(data, 0, data.Length);


            try
            {
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {

                    if (response.StatusCode != HttpStatusCode.OK)
                        Console.Out.WriteLine("Error fetching data. Server returned status code: {0}", response.StatusCode);
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {

                        // readdata
                        var readdata = reader.ReadToEnd();
                        string test = JsonConvert.DeserializeObject<string>(readdata);

                        if(test !=null && test != "null")
                        {
                            if (test.Equals("true"))
                            {
                                PopupNavigation.Instance.RemovePageAsync(this);
                                slp.Navigation.PushAsync(new SaleDetailListGift(slnum));
                            }
                            else
                            {
                                PopupNavigation.Instance.RemovePageAsync(this);
                                slp.DisplayAlert("알림", "접수비밀번호가 틀렸습니다", "OK");
                            }
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {
                slp.check_salepw = false;
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }
    }
}