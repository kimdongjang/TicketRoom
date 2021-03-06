﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TicketRoom.Models.ShopData
{
    public class ShopDBFunc
    {
        private static ShopDBFunc _instance;

        //  'protected' 로 생성자를 만듦
        public ShopDBFunc() { }
        //Static으로 메서드 생성
        public static ShopDBFunc Instance()
        {
            //다중쓰레드에서는 정상적으로 동작안하는 코드입니다.
            //다중 쓰레드 경우에는 동기화가 필요합니다.
            if (_instance == null)
                _instance = new ShopDBFunc();

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



        public List<MainCate> GetCategoryListAsync()
        {
            #region 서버에서 GET메소드/메인 카테고리 리스트 요청
            List<MainCate> mclist = new List<MainCate>();

            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "SH_SearchMainCate") as HttpWebRequest;
            request.Method = "GET";
            request.ContentType = "application/json";
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
                        mclist = JsonConvert.DeserializeObject<List<MainCate>>(readdata);
                    }

                    return mclist;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);

                return null;
            }
            #endregion
        }

        // DB에서 메인 인덱스로 서브 카테고리 목록을 가져오기
        public List<SubCate> PostSubCategoryListToIndex(int main_index)
        {
            #region POST형식으로 서브 카테고리 리스트를 받아옴
            List<SubCate> sclist = new List<SubCate>();
            string str = @"{";
            str += "mainCateIndex : " + main_index;
            str += "}";

            //// JSON 문자열을 파싱하여 JObject를 리턴
            JObject jo = JObject.Parse(str);

            UTF8Encoding encoder = new UTF8Encoding();
            byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "SH_SearchSubCate") as HttpWebRequest;
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
                        sclist = JsonConvert.DeserializeObject<List<SubCate>>(readdata);
                    }
                }
                return sclist;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);

                return null;
            }
            #endregion
        }

        // DB에서 홈 인덱스로 홈페이지 가져오기
        public SH_Home PostSearchHomeToHome(int homeIndex)
        {
            SH_Home home = new SH_Home();
            string str = @"{";
            str += "homeIndex : " + homeIndex;
            str += "}";

            //// JSON 문자열을 파싱하여 JObject를 리턴
            JObject jo = JObject.Parse(str);

            UTF8Encoding encoder = new UTF8Encoding();
            byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "SH_SearchHomeToHome") as HttpWebRequest;
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
                        home = JsonConvert.DeserializeObject<SH_Home>(readdata);
                    }
                }
                return home;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return null;
            }
        }

        // DB에서 홈 인덱스로 상품 목록을 가져오기
        public List<SH_Product> PostSearchProductToHome(int homeIndex)
        {
            List<SH_Product> productList = new List<SH_Product>();
            string str = @"{";
            str += "homeIndex : " + homeIndex;
            str += "}";

            //// JSON 문자열을 파싱하여 JObject를 리턴
            JObject jo = JObject.Parse(str);

            UTF8Encoding encoder = new UTF8Encoding();
            byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "SH_SearchProductToHome") as HttpWebRequest;
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
                        productList = JsonConvert.DeserializeObject<List<SH_Product>>(readdata);
                    }
                }
                return productList;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return null;
            }
        }

        // DB에서 상품 인덱스로 상품 목록을 가져오기
        public SH_Product PostSearchProductToProduct(int productIndex)
        {
            SH_Product product = new SH_Product();
            string str = @"{";
            str += "productIndex : " + productIndex;
            str += "}";

            //// JSON 문자열을 파싱하여 JObject를 리턴
            JObject jo = JObject.Parse(str);

            UTF8Encoding encoder = new UTF8Encoding();
            byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "SH_SearchProductToProduct") as HttpWebRequest;
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
                        product = JsonConvert.DeserializeObject<SH_Product>(readdata);
                    }
                }
                return product;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return null;
            }
        }

        public List<SH_Review> PostSearchReviewToHome(int homeIndex)
        {

            List<SH_Review> reviewList = new List<SH_Review>();
            string str = @"{";
            str += "homeIndex : " + homeIndex;
            str += "}";

            //// JSON 문자열을 파싱하여 JObject를 리턴
            JObject jo = JObject.Parse(str);

            UTF8Encoding encoder = new UTF8Encoding();
            byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "SH_SearchReviewToHome") as HttpWebRequest;
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
                        reviewList = JsonConvert.DeserializeObject<List<SH_Review>>(readdata);
                    }
                }
                return reviewList;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return null;
            }
        }


        // DB에서 상품 인덱스로 이미지 목록을 가져오기
        public List<SH_ImageList> PostSearchImageListToProductAsync(int productIndex)
        {
            List<SH_ImageList> imageList = new List<SH_ImageList>();
            string str = @"{";
            str += "productIndex : " + productIndex;
            str += "}";

            //// JSON 문자열을 파싱하여 JObject를 리턴
            JObject jo = JObject.Parse(str);

            UTF8Encoding encoder = new UTF8Encoding();
            byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "SH_SearchImageListToProduct") as HttpWebRequest;
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
                        imageList = JsonConvert.DeserializeObject<List<SH_ImageList>>(readdata);
                    }
                }
                return imageList;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return null;
            }
        }

        // DB에서 홈 상품 인덱스로 다른 고객이 본 상품을 가져오기
        public List<SH_OtherView> PostSearchOtherViewToHome(int homeIndex)
        {
            List<SH_OtherView> otherList = new List<SH_OtherView>();
            string str = @"{";
            str += "homeIndex : " + homeIndex;
            str += "}";

            //// JSON 문자열을 파싱하여 JObject를 리턴
            JObject jo = JObject.Parse(str);

            UTF8Encoding encoder = new UTF8Encoding();
            byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "SH_SearchOtherViewToHome") as HttpWebRequest;
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
                        otherList = JsonConvert.DeserializeObject<List<SH_OtherView>>(readdata);
                    }
                }
                return otherList;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return null;
            }
        }

        // DB에서 홈 상품 인덱스로 상품 옵션을 가져오기
        public List<SH_Pro_Option> PostSearchProOptionToProductAsync(int productIndex)
        {
            List<SH_Pro_Option> optionList = new List<SH_Pro_Option>();
            string str = @"{";
            str += "productIndex : " + productIndex;
            str += "}";

            //// JSON 문자열을 파싱하여 JObject를 리턴
            JObject jo = JObject.Parse(str);

            UTF8Encoding encoder = new UTF8Encoding();
            byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "SH_SearchProOptionToProduct") as HttpWebRequest;
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
                        optionList = JsonConvert.DeserializeObject<List<SH_Pro_Option>>(readdata);
                    }
                }
                return optionList;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return null;
            }
        }


        // DB에 페이지 리뷰 결과 입력하기
        public bool PostInsertReviewTohome(int homeIndex, string grade, string userid, string u_contents)
        {
            bool isResult = false;
            string str = @"{";
            str += "homeIndex:'" + homeIndex.ToString();
            str += "',grade:'" + grade;
            str += "',userid:'" + userid;
            str += "',u_contents:'" + u_contents;
            str += "'}";

            //// JSON 문자열을 파싱하여 JObject를 리턴
            JObject jo = JObject.Parse(str);

            UTF8Encoding encoder = new UTF8Encoding();
            byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "SH_InsertReviewTohome") as HttpWebRequest;
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
                        isResult = JsonConvert.DeserializeObject<bool>(readdata);
                    }
                }
                return isResult;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return isResult;
            }
        }

        // DB에 장바구니 내용 담기
        public bool PostInsertBasketListToHome(string sh_homeIndex, string sh_price, string sh_count, string sh_color, string sh_size, string sh_id, string sh_name, string sh_date, string sh_image, string sh_prdouct_index)
        {
            bool isResult = false;
            string str = @"{";
            str += "sh_homeIndex:'" + sh_homeIndex;
            str += "',sh_price:'" + sh_price;
            str += "',sh_count:'" + sh_count;
            str += "',sh_color:'" + sh_color;
            str += "',sh_size:'" + sh_size;
            str += "',sh_id:'" + sh_id;
            str += "',sh_name:'" + sh_name;
            str += "',sh_date:'" + sh_date;
            str += "',sh_image:'" + sh_image;
            str += "',sh_prdouct_index:'" + sh_prdouct_index;
            str += "'}";

            //// JSON 문자열을 파싱하여 JObject를 리턴
            JObject jo = JObject.Parse(str);

            UTF8Encoding encoder = new UTF8Encoding();
            byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "SH_InsertBasketListToHome") as HttpWebRequest;
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
                        isResult = JsonConvert.DeserializeObject<bool>(readdata);
                    }
                }
                return isResult;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return isResult;
            }
        }


        // DB에서 장바구니 리스트 가져오기
        public List<SH_BasketList> PostSearchBasketListToID(string id)
        {
            List<SH_BasketList> basketList = new List<SH_BasketList>();
            string str = @"{";
            str += "id : '" + id;
            str += "'}";

            //// JSON 문자열을 파싱하여 JObject를 리턴
            JObject jo = JObject.Parse(str);

            UTF8Encoding encoder = new UTF8Encoding();
            byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "SH_SearchBasketListToID") as HttpWebRequest;
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
                        basketList = JsonConvert.DeserializeObject<List<SH_BasketList>>(readdata);
                    }
                }
                return basketList;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return null;
            }
        }


        // 바스켓 인덱스를 통해 장바구니 리스트 한 행을 삭제함
        public bool PostDeleteBasketListToBasket(string basket_index)
        {
            bool isChecked = false; 
            string str = @"{";
            str += "basket_index : '" + basket_index;
            str += "'}";

            //// JSON 문자열을 파싱하여 JObject를 리턴
            JObject jo = JObject.Parse(str);

            UTF8Encoding encoder = new UTF8Encoding();
            byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "SH_DeleteBasketListToBasket") as HttpWebRequest;
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

        // 구매시 구매 리스트 정보 생성(return 값은 주문 번호)
        public int PostInsertPurchaseListToID(SH_Pur_Delivery delivery, IMP_RValue rvalue, string p_p_point, string p_id, string p_isuser) // 유저 아이디 , 비회원 회원 상태 확인
        {
            var dataString = JsonConvert.SerializeObject(delivery);
            JObject jo = JObject.Parse(dataString);
            var dataString2 = JsonConvert.SerializeObject(rvalue);
            JObject jo2 = JObject.Parse(dataString2);
            UTF8Encoding encoder = new UTF8Encoding();

            int result = -1;
            string str = @"{";
            str += "delivery:" + jo.ToString();
            str += ",rvalue:" + jo2.ToString();
            str += ",p_p_point:'" + p_p_point;
            str += "',p_id:'" + p_id;
            str += "',p_isuser:'" + p_isuser;
            str += "'}";

            JObject jo3 = JObject.Parse(str);
            byte[] data = encoder.GetBytes(jo3.ToString()); // a json object, or xml, whatever...

            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "SH_InsertPurchaseListToID") as HttpWebRequest;
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
                        result = JsonConvert.DeserializeObject<int>(readdata);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return result;
            }
        }


        // 구매 리스트 생성을 위해 상품 목록을 생성
        public bool PostInsertProductToPurchaseList(string p_homeindex, string p_image, string p_count, string p_color, string p_size, string p_name,
                                                string p_id, string p_index, string p_price, string p_productindex)
        {
            bool isResult = false;
            string str = @"{";
            str += "p_homeindex:'" + p_homeindex;
            str += "',p_image:'" + p_image;
            str += "',p_count:'" + p_count;
            str += "',p_color:'" + p_color;
            str += "',p_size:'" + p_size;
            str += "',p_name:'" + p_name;
            str += "',p_id:'" + p_id;
            str += "',p_index:'" + p_index;
            str += "',p_price:'" + p_price;
            str += "',p_productindex:'" + p_productindex;
            str += "'}";

            //// JSON 문자열을 파싱하여 JObject를 리턴
            JObject jo = JObject.Parse(str);

            UTF8Encoding encoder = new UTF8Encoding();
            byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "SH_InsertProductToPurchaseList") as HttpWebRequest;
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
                        isResult = JsonConvert.DeserializeObject<bool>(readdata);
                    }
                }
                return isResult;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return isResult;
            }
        }



        // 유저 아이디를 통해 구매리스트 가져오기
        public List<SH_Purchace> PostSearchPurchaseListToID(string userid, int year, int month, int day)
        {
            List<SH_Purchace> purList = new List<SH_Purchace>();
            string str = @"{";
            str += "userid : '" + userid;
            str += "',year:'" + year;
            str += "',month:'" + month;
            str += "',day:'" + day;
            str += "'}";

            //// JSON 문자열을 파싱하여 JObject를 리턴
            JObject jo = JObject.Parse(str);

            UTF8Encoding encoder = new UTF8Encoding();
            byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "SH_SearchPurchaseListToID") as HttpWebRequest;
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
                        purList = JsonConvert.DeserializeObject<List<SH_Purchace>>(readdata);
                    }
                }
                return purList;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return null;
            }
        }

        // 구매 목록 인덱스를 통해 배송 관련 리스트 가져오기
        public List<SH_Pur_Delivery> PostSearchPurchaseDeliveryListToIndex(string pl_index)
        {
            List<SH_Pur_Delivery> delList = new List<SH_Pur_Delivery>();
            string str = @"{";
            str += "pl_index : '" + pl_index;
            str += "'}";

            //// JSON 문자열을 파싱하여 JObject를 리턴
            JObject jo = JObject.Parse(str);

            UTF8Encoding encoder = new UTF8Encoding();
            byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "SH_SearchPurchaseDeliveryListToIndex") as HttpWebRequest;
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
                        delList = JsonConvert.DeserializeObject<List<SH_Pur_Delivery>>(readdata);
                    }
                }
                return delList;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return null;
            }
        }

        // 구매 목록 인덱스를 통해 구매 상품 리스트 가져오기
        public List<SH_Pur_Product> PostSearchPurchaseProductListToIndex(string pl_index)
        {
            List<SH_Pur_Product> proList = new List<SH_Pur_Product>();
            string str = @"{";
            str += "pl_index : '" + pl_index;
            str += "'}";

            //// JSON 문자열을 파싱하여 JObject를 리턴
            JObject jo = JObject.Parse(str);

            UTF8Encoding encoder = new UTF8Encoding();
            byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "SH_SearchPurchaseProductListToIndex") as HttpWebRequest;
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
                        proList = JsonConvert.DeserializeObject<List<SH_Pur_Product>>(readdata);
                    }
                }
                return proList;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return null;
            }
        }
        
        // 
        public bool SH_UpdateProductCountToIndex(string p_product_index, string p_count)
        {
            bool isResult = false;
            string str = @"{";
            str += "p_product_index:'" + p_product_index;
            str += "',p_count:'" + p_count;
            str += "'}";

            //// JSON 문자열을 파싱하여 JObject를 리턴
            JObject jo = JObject.Parse(str);

            UTF8Encoding encoder = new UTF8Encoding();
            byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "SH_UpdateProductCountToIndex") as HttpWebRequest;
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
                        isResult = JsonConvert.DeserializeObject<bool>(readdata);
                    }
                }
                return isResult;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return isResult;
            }
        }

        // DB에 장바구니 내용 담기
        public bool PostUpdateBasketUserToID(string p_id, string p_nonid)
        {
            bool isResult = false;
            string str = @"{";
            str += "p_id:'" + p_id;
            str += "',p_nonid:'" + p_nonid;
            str += "'}";

            //// JSON 문자열을 파싱하여 JObject를 리턴
            JObject jo = JObject.Parse(str);

            UTF8Encoding encoder = new UTF8Encoding();
            byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "SH_UpdateBasketUserToID") as HttpWebRequest;
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
                        isResult = JsonConvert.DeserializeObject<bool>(readdata);
                    }
                }
                return isResult;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return isResult;
            }
        }

        // 다른 고객이 본 상품 테이블 업데이트 및 초기화
        public bool PostUpdateViewsOtherViewToIndex(int p_home_index, int p_other_index)
        {
            if(p_other_index == -1) // other index가 초기화가 안되어 있을 경우 리턴
            {
                return false;
            }

            bool isResult = false;
            string str = @"{";
            str += "p_home_index:'" + p_home_index.ToString();
            str += "',p_other_index:'" + p_other_index.ToString();
            str += "'}";

            //// JSON 문자열을 파싱하여 JObject를 리턴
            JObject jo = JObject.Parse(str);

            UTF8Encoding encoder = new UTF8Encoding();
            byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "SH_UpdateViewsOtherViewToIndex") as HttpWebRequest;
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
                        isResult = JsonConvert.DeserializeObject<bool>(readdata);
                    }
                }
                return isResult;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return isResult;
            }
        }

        // 최근 본 상품 목록 앱 실행시 초기화
        public bool PostInsertRecentViewToID(string p_id)
        {
            bool isResult = false;
            string str = @"{";
            str += "p_id:'" + p_id;
            str += "'}";

            //// JSON 문자열을 파싱하여 JObject를 리턴
            JObject jo = JObject.Parse(str);

            UTF8Encoding encoder = new UTF8Encoding();
            byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "SH_InsertRecentViewToID") as HttpWebRequest;
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
                        isResult = JsonConvert.DeserializeObject<bool>(readdata);
                    }
                }
                return isResult;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return isResult;
            }
        }


        // 최근 본 상품 목록 홈 페이지 이동시 갱신
        public bool PostUpdateRecentViewToID(string p_id, string p_home_index)
        {
            bool isResult = false;
            string str = @"{";
            str += "p_id:'" + p_id;
            str += "',p_home_index:'" + p_home_index;
            str += "'}";

            //// JSON 문자열을 파싱하여 JObject를 리턴
            JObject jo = JObject.Parse(str);

            UTF8Encoding encoder = new UTF8Encoding();
            byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "SH_UpdateRecentViewToID") as HttpWebRequest;
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
                        isResult = JsonConvert.DeserializeObject<bool>(readdata);
                    }
                }
                return isResult;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return isResult;
            }
        }

        // 결제 인덱스를 통해 카드 결제 방법 리스트 가져오기
        public SH_RecentView PostSelectRecentViewToID(string p_id)
        {
            SH_RecentView recentList = new SH_RecentView();
            string str = @"{";
            str += "p_id : '" + p_id;
            str += "'}";

            //// JSON 문자열을 파싱하여 JObject를 리턴
            JObject jo = JObject.Parse(str);

            UTF8Encoding encoder = new UTF8Encoding();
            byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "SH_SelectRecentViewToID") as HttpWebRequest;
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
                        recentList = JsonConvert.DeserializeObject<SH_RecentView>(readdata);
                    }
                }
                return recentList;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return null;
            }
        }


        // 쇼핑 탭에서 쇼핑몰 이름 검색하기
        public List<SubCate> PostBrowsingShopListToName(string p_name)
        {
            List<SubCate> sclist = new List<SubCate>();
            string str = @"{";
            str += "p_name : '" + p_name;
            str += "'}";

            //// JSON 문자열을 파싱하여 JObject를 리턴
            JObject jo = JObject.Parse(str);

            UTF8Encoding encoder = new UTF8Encoding();
            byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "SH_BrowsingShopListToName") as HttpWebRequest;
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
                        sclist = JsonConvert.DeserializeObject<List<SubCate>>(readdata);
                    }
                }
                return sclist;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return null;
            }
        }

    }
}
