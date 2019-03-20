using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using TicketRoom.Models.Gift;
using TicketRoom.Models.Gift.PurchaseList;
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

        public string PostSelectUserPoint(string user_id)
        {
            try
            {
                string str = @"{";
                str += "USER_ID:'" + user_id;  //아이디찾기에선 Name으로 
                str += "'}";

                //// JSON 문자열을 파싱하여 JObject를 리턴
                JObject jo = JObject.Parse(str);

                UTF8Encoding encoder = new UTF8Encoding();
                byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

                //request.Method = "POST";
                HttpWebRequest request = WebRequest.Create(Global.WCFURL + "SelectUserPoint") as HttpWebRequest;
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
                        return test;
                    }
                }
            }
            catch
            {
                return "";
            }
        }

        public G_ProductCount Get_Product_Ccount(string pro_num)
        {
            try
            {
                G_ProductCount test = null;
                string str = @"{";
                str += "ProNum:'" + pro_num;  //아이디찾기에선 Name으로 
                str += "'}";

                //// JSON 문자열을 파싱하여 JObject를 리턴
                JObject jo = JObject.Parse(str);

                UTF8Encoding encoder = new UTF8Encoding();
                byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

                //request.Method = "POST";
                HttpWebRequest request = WebRequest.Create(Global.WCFURL + "Get_Product_Ccount") as HttpWebRequest;
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
                        test = JsonConvert.DeserializeObject<G_ProductCount>(readdata);
                        return test;
                    }
                }
            }
            catch
            {
                return null;
            }
        }


        // 카테고리 넘버로 카테고리 구매 리스트 가져오기
        public List<G_ProductInfo> PostSelectSaleCategory(string category_num)
        {
            try
            {
                string str = @"{";
                str += "CategoryNum:'" + category_num;  //아이디찾기에선 Name으로 
                str += "'}";

                //// JSON 문자열을 파싱하여 JObject를 리턴
                JObject jo = JObject.Parse(str);

                UTF8Encoding encoder = new UTF8Encoding();
                byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

                //request.Method = "POST";
                HttpWebRequest request = WebRequest.Create(Global.WCFURL + "SelectSaleProduct") as HttpWebRequest;
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
                        List<G_ProductInfo> test = JsonConvert.DeserializeObject<List<G_ProductInfo>>(readdata);
                        return test;
                    }
                }
            }
            catch
            {
                return null;
            }

        }

        // 카테고리 넘버로 카테고리 구매 리스트 가져오기
        public List<G_ProductInfo> PostSelectPurchaseProductToIndex(string category_num)
        {
            try
            {
                string str = @"{";
                str += "CategoryNum:'" + category_num;  //아이디찾기에선 Name으로 
                str += "'}";

                //// JSON 문자열을 파싱하여 JObject를 리턴
                JObject jo = JObject.Parse(str);

                UTF8Encoding encoder = new UTF8Encoding();
                byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

                //request.Method = "POST";
                HttpWebRequest request = WebRequest.Create(Global.WCFURL + "SelectPurchaseProduct") as HttpWebRequest;
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
                        List<G_ProductInfo> test = JsonConvert.DeserializeObject<List<G_ProductInfo>>(readdata);
                        return test;
                    }
                }
            }
            catch
            {
                return null;
            }
            
        }

        // 카테고리 넘버로 카테고리 리스트 가져오기
        public List<DetailCategory> PostSelectDetailCategoryToIndex(string category_num)
        {
            string str = @"{";
            str += "category_num:'" + category_num;  //아이디찾기에선 Name으로 
            str += "'}";

            //// JSON 문자열을 파싱하여 JObject를 리턴
            JObject jo = JObject.Parse(str);

            UTF8Encoding encoder = new UTF8Encoding();
            byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

            //request.Method = "POST";
            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "GIFT_SelectDetailCategoryToIndex") as HttpWebRequest;
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
                    List<DetailCategory> test = JsonConvert.DeserializeObject<List<DetailCategory>>(readdata);
                    return test;
                }
            }
            return null;
        }


        public List<G_CategoryInfo> SelectAllCategory()
        {
            //request.Method = "POST";
            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "SelectAllCategory") as HttpWebRequest;
            request.Method = "GET";

            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                    Console.Out.WriteLine("Error fetching data. Server returned status code: {0}", response.StatusCode);
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    var readdata = reader.ReadToEnd();
                    List<G_CategoryInfo> test = JsonConvert.DeserializeObject<List<G_CategoryInfo>>(readdata);
                    if (test != null)
                    {
                        return test;
                    }
                }
            }
            return null;
        }

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

        public List<G_SaleInfo> SearchSaleListToID(string userid, int year, int mon, int day)
        {
            List<G_SaleInfo> salelist = new List<G_SaleInfo>();
            string str = @"{";
            str += "userid : '" + userid;
            str += "',year:'" + year;
            str += "',mon:'" + mon;
            str += "',day:'" + day;
            str += "'}";

            //// JSON 문자열을 파싱하여 JObject를 리턴
            JObject jo = JObject.Parse(str);

            UTF8Encoding encoder = new UTF8Encoding();
            byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "SearchSaleListToID") as HttpWebRequest;
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
                        if (readdata != null && readdata != "")
                        {
                            salelist = JsonConvert.DeserializeObject<List<G_SaleInfo>>(readdata);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
            return salelist;
        }

        internal List<G_DealInfo> SelectDealList()
        {
            List<G_DealInfo> purchaselist = new List<G_DealInfo>();
            string str = @"{}";

            //// JSON 문자열을 파싱하여 JObject를 리턴
            JObject jo = JObject.Parse(str);

            UTF8Encoding encoder = new UTF8Encoding();
            byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "SelectDealList") as HttpWebRequest;
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
                        if (readdata != null && readdata != "")
                        {
                            purchaselist = JsonConvert.DeserializeObject<List<G_DealInfo>>(readdata);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
            return purchaselist;
        }

        public List<G_PLInfo> SearchPurchaseListToID(string userid, int year, int mon, int day)
        {
            List<G_PLInfo> purchaselist = new List<G_PLInfo>();
            string str = @"{";
            str += "userid : '" + userid;
            str += "',year:'" + year;
            str += "',mon:'" + mon;
            str += "',day:'" + day;
            str += "'}";

            //// JSON 문자열을 파싱하여 JObject를 리턴
            JObject jo = JObject.Parse(str);

            UTF8Encoding encoder = new UTF8Encoding();
            byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "SearchPurchaseListToID") as HttpWebRequest;
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
                        if (readdata != null && readdata != "")
                        {
                            purchaselist = JsonConvert.DeserializeObject<List<G_PLInfo>>(readdata);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
            return purchaselist;
        }

        public List<PLProInfo> SearchPurchaseListToPlnum(string pl_num)
        {
            List<PLProInfo> productlist = new List<PLProInfo>();
            //구매내역 가져오기
            string str = @"{";
            str += "plnum : '" + pl_num;
            str += "'}";

            //// JSON 문자열을 파싱하여 JObject를 리턴
            JObject jo = JObject.Parse(str);

            UTF8Encoding encoder = new UTF8Encoding();
            byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "SearchPurchaseListToPlnum") as HttpWebRequest;
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
                        if (readdata != null && readdata != "")
                        {
                            productlist = JsonConvert.DeserializeObject<List<PLProInfo>>(readdata);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
            return productlist;
        }
    }
}
