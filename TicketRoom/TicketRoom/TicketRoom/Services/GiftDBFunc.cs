﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using TicketRoom.Models.Gift;
using TicketRoom.Models.Gift.Purchase;
using TicketRoom.Models.Gift.PurchaseList;
using TicketRoom.Models.Gift.SaleList;
using TicketRoom.Models.Users;

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

        // 바스켓 인덱스를 통해 수량이 0으로 변경된 장바구니 내역 삭제
        public bool PostDeleteGiftBasketListCountZero(string user_id)
        {
            bool isChecked = false;
            string str = @"{";
            str += "user_id : '" + user_id;
            str += "'}";

            //// JSON 문자열을 파싱하여 JObject를 리턴
            JObject jo = JObject.Parse(str);

            UTF8Encoding encoder = new UTF8Encoding();
            byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "PostDeleteGiftBasketListCountZero") as HttpWebRequest;
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
                        isChecked = JsonConvert.DeserializeObject<bool>(readdata);
                    }
                }
                return isChecked;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return isChecked;
            }
        }

        // 바스켓 인덱스를 통해 장바구니 리스트 한 행을 삭제함
        public bool PostDeleteGiftBasketListToIndex(string basket_index)
        {
            bool isChecked = false;
            string str = @"{";
            str += "basket_index : '" + basket_index;
            str += "'}";

            //// JSON 문자열을 파싱하여 JObject를 리턴
            JObject jo = JObject.Parse(str);

            UTF8Encoding encoder = new UTF8Encoding();
            byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "PostDeleteGiftBasketListToIndex") as HttpWebRequest;
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
                        isChecked = JsonConvert.DeserializeObject<bool>(readdata);
                    }
                }
                return isChecked;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return isChecked;
            }
        }


        // 장바구니 상품 수량 변경
        public bool UpdateGiftBasketListToIndex(string index, string count)
        {
            bool retVal = false;
            //구매내역 가져오기
            string str = @"{";
            str += "index : '" + index;
            str += "',count : '" + count;
            str += "'}";

            //// JSON 문자열을 파싱하여 JObject를 리턴
            JObject jo = JObject.Parse(str);

            UTF8Encoding encoder = new UTF8Encoding();
            byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "UpdateGiftBasketListToIndex") as HttpWebRequest;
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
                            retVal = JsonConvert.DeserializeObject<bool>(readdata);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
            return retVal;
        }


        public List<PLProInfo> SearchPurchaseListToPlnum(string plnum)
        {
            List<PLProInfo> productlist = new List<PLProInfo>();
            //구매내역 가져오기
            string str = @"{";
            str += "plnum : '" + plnum;
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

        // 상품권 할인 후 가격 가져오기(장바구니->주문시 최신가격갱신을 위해)
        public string PostSelectGiftDiscountPriceToIndex(string product_index)
        {
            try
            {
                string str = @"{";
                str += "product_index:'" + product_index;  //아이디찾기에선 Name으로 
                str += "'}";

                //// JSON 문자열을 파싱하여 JObject를 리턴
                JObject jo = JObject.Parse(str);

                UTF8Encoding encoder = new UTF8Encoding();
                byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

                //request.Method = "POST";
                HttpWebRequest request = WebRequest.Create(Global.WCFURL + "SelectGiftDiscountPriceToIndex") as HttpWebRequest;
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

        // 유저 포인트 가져오기
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

        // 구매 상세 내역 가져오기
        public List<G_PurchaseListDetail> SearchGfitDetailListToPlnumForPIN(string plnum)
        {
            List<G_PurchaseListDetail> test = new List<G_PurchaseListDetail>();
            string str = @"{";
            str += "plnum:'" + plnum;  //아이디찾기에선 Name으로 
            str += "'}";

            //// JSON 문자열을 파싱하여 JObject를 리턴
            JObject jo = JObject.Parse(str);

            UTF8Encoding encoder = new UTF8Encoding();
            byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

            //request.Method = "POST";
            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "SearchGfitDetailListToPlnumForPIN") as HttpWebRequest;
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
                        test = JsonConvert.DeserializeObject<List<G_PurchaseListDetail>>(readdata);
                        return test;
                    }
                }
            }
            return test;
        }

        // 핀 번호 목록 가져오기
        public List<G_PinNumberProduct> PostSearchPinGfitCardListToPlnum(string plnum)
        {
            List<G_PinNumberProduct> test = new List<G_PinNumberProduct>();
            string str = @"{";
            str += "plnum:'" + plnum;  //아이디찾기에선 Name으로 
            str += "'}";

            //// JSON 문자열을 파싱하여 JObject를 리턴
            JObject jo = JObject.Parse(str);

            UTF8Encoding encoder = new UTF8Encoding();
            byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

            //request.Method = "POST";
            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "SearchPinGfitCardListToPlnum") as HttpWebRequest;
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
                        test = JsonConvert.DeserializeObject<List<G_PinNumberProduct>>(readdata);
                        return test;
                    }
                }
            }
            return test;
        }


        public List<ADRESS> ShowUserAddrlist(string user_id)
        {
            List<ADRESS> test = new List<ADRESS>();
            string str = @"{";
            str += "UserID:'" + user_id;  //아이디찾기에선 Name으로 
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
                        test = JsonConvert.DeserializeObject<List<ADRESS>>(readdata);
                        return test;
                    }
                }
            }
            return test;
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
            try
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
            }
            catch
            {
                return null;
            }
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

        public List<G_PurchaseList> SearchPurchaseListToID(string userid, int year, int mon, int day)
        {
            List<G_PurchaseList> purchaselist = new List<G_PurchaseList>();
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
                            purchaselist = JsonConvert.DeserializeObject<List<G_PurchaseList>>(readdata);
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
        // 유저 아이디를 통해 상품권 구매리스트 가져오기
        public List<G_PurchaseList> SearchPurchaseDetailToPlNum(string pl_num)
        {
            try
            {
                string str = @"{";
                str += "plnum : '" + pl_num;
                str += "'}";

                //// JSON 문자열을 파싱하여 JObject를 리턴
                JObject jo = JObject.Parse(str);

                UTF8Encoding encoder = new UTF8Encoding();
                byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

                HttpWebRequest request = WebRequest.Create(Global.WCFURL + "SearchPurchaseDetailToPlnum") as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = data.Length;

                request.GetRequestStream().Write(data, 0, data.Length);


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
                            List<G_PurchaseList> pdlist = JsonConvert.DeserializeObject<List<G_PurchaseList>>(readdata);
                            return pdlist;
                        }
                    }
                }
                return null;
            }

            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return null;
            }
        }

    }
}
