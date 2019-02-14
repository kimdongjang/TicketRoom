using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace TicketRoom.Models.Users
{
    public class UserDBFunc
    {
        private static UserDBFunc _instance;

        //  'protected' 로 생성자를 만듦
        public UserDBFunc() { }
        //Static으로 메서드 생성
        public static UserDBFunc Instance()
        {
            //다중쓰레드에서는 정상적으로 동작안하는 코드입니다.
            //다중 쓰레드 경우에는 동기화가 필요합니다.
            if (_instance == null)
                _instance = new UserDBFunc();

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
        public string PostInsertNonUsersID()
        {
            string retVal = "";
            string str = @"{";
            str += "";
            str += "}";

            //// JSON 문자열을 파싱하여 JObject를 리턴
            JObject jo = JObject.Parse(str);

            UTF8Encoding encoder = new UTF8Encoding();
            byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "NON_InsertNonUsersID") as HttpWebRequest;
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
                        retVal = JsonConvert.DeserializeObject<string>(readdata);
                    }
                }
                return retVal;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return null;
            }
        }


        public USERS PostSelectUserToID(string p_id)
        {
            USERS user = new USERS();
            string str = @"{";
            str += "p_id:'" + p_id;
            str += "'}";


            //// JSON 문자열을 파싱하여 JObject를 리턴
            JObject jo = JObject.Parse(str);

            UTF8Encoding encoder = new UTF8Encoding();
            byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "User_SelectUserToID") as HttpWebRequest;
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
                        user = JsonConvert.DeserializeObject<USERS>(readdata);
                    }
                }
                return user;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return user;
            }
        }


        public ADRESS PostSelectAdressToID(string p_id)
        {
            ADRESS adress = new ADRESS();
            string str = @"{";
            str += "p_id:'" + p_id;
            str += "'}";


            //// JSON 문자열을 파싱하여 JObject를 리턴
            JObject jo = JObject.Parse(str);

            UTF8Encoding encoder = new UTF8Encoding();
            byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "Adress_SelectAdressToID") as HttpWebRequest;
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
                        adress = JsonConvert.DeserializeObject<ADRESS>(readdata);
                    }
                }
                return adress;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return adress;
            }
        }

        // 상품권 장바구니 업데이트 기능
        public bool PostGiftUpdateBaskeListToID(string nonuserid, string userid)
        {
            string str2 = @"{";
            str2 += "nonuserid:'" + nonuserid;  // 비회원아이디
            str2 += "',userid:'" + userid; // 회원 ID
            str2 += "'}";

            //// JSON 문자열을 파싱하여 JObject를 리턴
            JObject jo2 = JObject.Parse(str2);

            UTF8Encoding encoder2 = new UTF8Encoding();
            byte[] data2 = encoder2.GetBytes(jo2.ToString()); // a json object, or xml, whatever...

            
            HttpWebRequest request2 = WebRequest.Create(Global.WCFURL + "Update_Basketlist") as HttpWebRequest;
            request2.Method = "POST";
            request2.ContentType = "application/json";
            request2.ContentLength = data2.Length;

            //request.Expect = "application/json";

            request2.GetRequestStream().Write(data2, 0, data2.Length);

            using (HttpWebResponse response2 = request2.GetResponse() as HttpWebResponse)
            {
                if (response2.StatusCode != HttpStatusCode.OK)
                    Console.Out.WriteLine("Error fetching data. Server returned status code: {0}", response2.StatusCode);
                using (StreamReader reader2 = new StreamReader(response2.GetResponseStream()))
                {
                    var readdata2 = reader2.ReadToEnd();
                    string test = JsonConvert.DeserializeObject<string>(readdata2);
                    if (test != null && test != "")
                    {
                        if (test.Equals("true"))
                        {
                            //상품권 장바구니 업데이트 완료 (비회원 -> 회원)
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            return false;
        }
    }
}
