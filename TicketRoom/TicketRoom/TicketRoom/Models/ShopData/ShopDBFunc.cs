using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

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
        public List<SubCate> PostSubCategoryListAsync(int main_index)
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

        // DB에서 서브 인덱스로 홈페이지 가져오기
        public SH_Home PostSearchHomeAsync(int sub_index)
        {
            SH_Home home = new SH_Home();
            string str = @"{";
            str += "subCateIndex : " + sub_index;
            str += "}";

            //// JSON 문자열을 파싱하여 JObject를 리턴
            JObject jo = JObject.Parse(str);

            UTF8Encoding encoder = new UTF8Encoding();
            byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "SH_SearchHome") as HttpWebRequest;
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
            List<SH_ImageList>  imageList = new List<SH_ImageList>();
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
        public List<SH_OtherView> PostSearchOtherViewToProductAsync(int productIndex)
        {
            List<SH_OtherView> otherList = new List<SH_OtherView>();
            string str = @"{";
            str += "productIndex : " + productIndex;
            str += "}";

            //// JSON 문자열을 파싱하여 JObject를 리턴
            JObject jo = JObject.Parse(str);

            UTF8Encoding encoder = new UTF8Encoding();
            byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "SH_SearchOtherViewToProduct") as HttpWebRequest;
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

        // DB에서 홈 상품 인덱스로 다른 고객이 본 상품을 가져오기
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
    }
}
