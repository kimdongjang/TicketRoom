using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace TicketRoom.Models.PointData
{
    class PointDBFunc
    {
        private static PointDBFunc _instance;

        //  'protected' 로 생성자를 만듦
        public PointDBFunc() { }
        //Static으로 메서드 생성
        public static PointDBFunc Instance()
        {
            //다중쓰레드에서는 정상적으로 동작안하는 코드입니다.
            //다중 쓰레드 경우에는 동기화가 필요합니다.
            if (_instance == null)
                _instance = new PointDBFunc();

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

        // 포인트 충전 및 적립되는 프로시저
        public bool PostInsertPointChargeToID(IMP_RValue rvalue, string pl_index)
        {
            var dataString = JsonConvert.SerializeObject(rvalue);
            JObject jo = JObject.Parse(dataString);
            UTF8Encoding encoder = new UTF8Encoding();

            bool isbool = false;
            string str = @"{";
            str += "rvalue:" + jo.ToString();
            str += ",pl_index:'" + pl_index;
            str += "'}";

            JObject jo2 = JObject.Parse(str);
            byte[] data = encoder.GetBytes(jo2.ToString()); // a json object, or xml, whatever...

            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "PT_InsertPointChargeToID") as HttpWebRequest;
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
                        isbool = JsonConvert.DeserializeObject<bool>(readdata);
                    }
                }
                return isbool;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return isbool;
            }
        }

        // 포인트 출금 및 사용하는 프로시저
        public bool PostInsertPointWithDrawToID(string p_content, string p_bank, string p_account, string p_name, string p_id, string p_point, string pl_index, string pl_status)
        {
            bool isbool = false;
            string str = @"{";
            str += "p_content:'" + p_content;
            str += "',p_bank:'" + p_bank;
            str += "',p_account:'" + p_account;
            str += "',p_name:'" + p_name;
            str += "',p_id:'" + p_id;
            str += "',p_point:'" + p_point;
            str += "',pl_index:'" + pl_index;
            str += "',pl_status:'" + pl_status;
            str += "'}";


            //// JSON 문자열을 파싱하여 JObject를 리턴
            JObject jo = JObject.Parse(str);

            UTF8Encoding encoder = new UTF8Encoding();
            byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "PT_InsertPointWithDrawToID") as HttpWebRequest;
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
                        isbool = JsonConvert.DeserializeObject<bool>(readdata);
                    }
                }
                return isbool;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return isbool;
            }
        }

        // 유저 아이디를 통해 리스트 인덱스와 보유 포인트 가져오기
        public PT_Point PostSearchPointListToID(string p_id)
        {
            PT_Point pp = new PT_Point();
            string str = @"{";
            str += "p_id : '" + p_id;
            str += "'}";

            //// JSON 문자열을 파싱하여 JObject를 리턴
            JObject jo = JObject.Parse(str);

            UTF8Encoding encoder = new UTF8Encoding();
            byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "PT_SearchPointListToID") as HttpWebRequest;
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
                        pp = JsonConvert.DeserializeObject<PT_Point>(readdata);
                    }
                }
                return pp;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return null;
            }
        }

        // 유저 아이디를 통해 포인트 충전 및 적립 리스트 가져오기
        public List<PT_Charge> PostSearchChargeListToID(string p_id)
        {
            List<PT_Charge> pc = new List<PT_Charge>();
            string str = @"{";
            str += "p_id : '" + p_id;
            str += "'}";

            //// JSON 문자열을 파싱하여 JObject를 리턴
            JObject jo = JObject.Parse(str);

            UTF8Encoding encoder = new UTF8Encoding();
            byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "PT_SearchChargeListToID") as HttpWebRequest;
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
                        pc = JsonConvert.DeserializeObject<List<PT_Charge>>(readdata);
                    }
                }
                return pc;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return null;
            }
        }

        // 유저 아이디를 통해 포인트 출금 및 사용 리스트 가져오기
        public List<PT_WithDraw> PostSearchWithDrawListToID(string p_id)
        {
            List<PT_WithDraw> wd = new List<PT_WithDraw>();
            string str = @"{";
            str += "p_id : '" + p_id;
            str += "'}";

            //// JSON 문자열을 파싱하여 JObject를 리턴
            JObject jo = JObject.Parse(str);

            UTF8Encoding encoder = new UTF8Encoding();
            byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "PT_SearchWithDrawListToID") as HttpWebRequest;
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
                        wd = JsonConvert.DeserializeObject<List<PT_WithDraw>>(readdata);
                    }
                }
                return wd;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return null;
            }
        }

    }
}
