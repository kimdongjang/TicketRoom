using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using TicketRoom.Models.Gift.Purchase;

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
        // 장치 고유 번호 서버로 전송
        public string PostDeviceSerialNumber(string serial_number)
        {
            string retVal = "";
            string str = @"{";
            str += "serial_number:'" + serial_number;
            str += "'}";

            //// JSON 문자열을 파싱하여 JObject를 리턴
            JObject jo = JObject.Parse(str);

            UTF8Encoding encoder = new UTF8Encoding();
            byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "PostDeviceSerialNumber") as HttpWebRequest;
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
                        // 1: 고유번호없음(어플첫시작=>비회원아이디발급), 2: 고유번호있음(회원아이디없음=>비회원정보가져옴), 3: 고유번호있음(회원아이디있음=>회원정보가져옴) <-사용안함
                        // u# << user아이디, n# 비회원 아이디
                        // -1: 실패
                        retVal = JsonConvert.DeserializeObject<string>(readdata);
                    }
                }
                return retVal;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return retVal;
            }
        }

        // 비회원 아이디 고유 장치 번호로 얻어오기
        public string GetNonUserIDToSerial(string serial_number)
        {
            string retVal = "";
            string str = @"{";
            str += "serial_number:'" + serial_number;
            str += "'}";

            //// JSON 문자열을 파싱하여 JObject를 리턴
            JObject jo = JObject.Parse(str);

            UTF8Encoding encoder = new UTF8Encoding();
            byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "GetNonUserIDToSerial") as HttpWebRequest;
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
                        // 1: 고유번호없음(어플첫시작=>비회원아이디발급), 2: 고유번호있음(회원아이디없음=>비회원정보가져옴), 3: 고유번호있음(회원아이디있음=>회원정보가져옴) <-사용안함
                        // u# << user아이디, n# 비회원 아이디
                        // -1: 실패
                        retVal = JsonConvert.DeserializeObject<string>(readdata);
                    }
                }
                return retVal;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return retVal;
            }
        }
        

        // 회원가입, 회원 로그인시 자동 로그인을 위해 시리얼 넘버, 아이디 전송
        public string PostAutoLoginSerialNumber(string serial_number, string user_id)
        {
            string retVal = "";
            string str = @"{";
            str += "serial_number:'" + serial_number;
            str += "',user_id:'" + user_id; // 회원 ID
            str += "'}";

            //// JSON 문자열을 파싱하여 JObject를 리턴
            JObject jo = JObject.Parse(str);

            UTF8Encoding encoder = new UTF8Encoding();
            byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "PostAutoLoginSerialNumber") as HttpWebRequest;
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
                        // 1:성공, 0:실패
                        retVal = JsonConvert.DeserializeObject<string>(readdata);
                    }
                }
                return retVal;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return retVal;
            }
        }

        public List<AccountInfo> GetSelectAllAccount() // 계좌 리스트 검색
        {
            try
            {

                //request.Method = "POST";
                HttpWebRequest request = WebRequest.Create(Global.WCFURL + "SelectAllAccount") as HttpWebRequest;
                request.Method = "GET";

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        Console.Out.WriteLine("Error fetching data. Server returned status code: {0}", response.StatusCode);
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        var readdata = reader.ReadToEnd();
                        List<AccountInfo> test = JsonConvert.DeserializeObject<List<AccountInfo>>(readdata);
                        return test;
                    }
                }
            }
            catch
            {
                return null;
            }
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

        // 최근 주소 검색
        public List<ADRESS> PostRecentAdressToID(string UserID)
        {
            string str = @"{";
            str += "UserID:'" + UserID;  //아이디찾기에선 Name으로 
            str += "'}";

            //// JSON 문자열을 파싱하여 JObject를 리턴
            JObject jo = JObject.Parse(str);

            UTF8Encoding encoder = new UTF8Encoding();
            byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

            //request.Method = "POST";
            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "SelectUserAddr") as HttpWebRequest;
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
                        List<ADRESS> test = JsonConvert.DeserializeObject<List<ADRESS>>(readdata);
                        return test;
                    }
                }
            }
            return null;
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


        // 결제 정보 및 디바이스 아이피 정보 전송
        public bool PostPurchaseDeviceInfo(string ipadress)
        {
            string str = @"{";
            str += "ipadress:'" + ipadress;
            str += "'}";

            //// JSON 문자열을 파싱하여 JObject를 리턴
            JObject jo = JObject.Parse(str);

            UTF8Encoding encoder = new UTF8Encoding();
            byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...


            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "PostPurchaseDeviceInfo") as HttpWebRequest;
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

        public bool PostCrawlingCheckPinNumber(string packet)
        {
            string str = @"{";
            str += "packet:'" + packet;
            str += "'}";

            //// JSON 문자열을 파싱하여 JObject를 리턴
            JObject jo = JObject.Parse(str);

            UTF8Encoding encoder = new UTF8Encoding();
            byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...


            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "CrawlingCheckPinNumber") as HttpWebRequest;
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


                }
            }
            return false;
        }
    }
}
