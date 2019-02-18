using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using TicketRoom.Models.Gift.SaleList;

namespace TicketRoom.Services
{
    public class GiftDBFunc
    {
        private static GiftDBFunc _instance;

        //  'protected' 로 생성자를 만듦
        public GiftDBFunc() { }
        //Static으로 메서드 생성
        public static GiftDBFunc Instance()
        {
            //다중쓰레드에서는 정상적으로 동작안하는 코드입니다.
            //다중 쓰레드 경우에는 동기화가 필요합니다.
            if (_instance == null)
                _instance = new GiftDBFunc();

            // 다중 쓰레드 환경일 경우 Lock이 필요함
            //if (_instance == null)
            //{
            //    lock (_synLock)
            //    {
            //        _instance = new WBdb();
            //    }
            //}

            return _instance;
        }
        // DB에서 홈 인덱스로 홈페이지 가져오기

        public int UserAddSale(G_SaleInfo g_SaleInfo)
        {
            var dataString = JsonConvert.SerializeObject(g_SaleInfo);

            JObject jo = JObject.Parse(dataString);

            UTF8Encoding encoder = new UTF8Encoding();

            string str = @"{";
            str += "g_SaleInfo:" + jo.ToString();  //아이디찾기에선 Name으로 
            str += "}";

            JObject jo2 = JObject.Parse(str);

            byte[] data = encoder.GetBytes(jo2.ToString()); // a json object, or xml, whatever...

            //request.Method = "POST";
            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "UserAddSale") as HttpWebRequest;
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
                    string test = JsonConvert.DeserializeObject<string>(readdata);
                    if (test != null && test != "")
                    {
                        return int.Parse(test);
                    }
                    else
                    {
                        return -1;
                    }
                }
            }
        }
    }
}
