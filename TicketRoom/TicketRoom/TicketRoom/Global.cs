using System;
using System.Collections.Generic;
using TicketRoom.Models.Gift;
using TicketRoom.Models.Users;
using TicketRoom.Views.MainTab.Shop;
using Xamarin.Forms;

namespace TicketRoom
{
    public class Global
    {
        public static string WCFURL = @"http://221.141.58.49:8088/Service1.svc/";
        //public static string WCFURL = @"http://52.231.66.251/Service1.svc/";
        
        //public static string WCFURL = @"http://52.231.66.251/Service1.svc/";
        //public static string WCFURL = @"http://220.90.190.218/Service1.svc/";
        //public static string WCFURL = @"http://localhost:65192/Service1.svc/";
        //운기 로컬 Services
        //public static string WCFURL = @"http://220.90.190.218:8081/Service1.svc/";

        public static string ID = "";

        public static string non_user_id = "";
        public static bool b_user_login = false; // 회원 로그인 상태
        public static bool b_auto_login = false; // 자동 로그인 상태

        public static string localPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        public static string LoadingImagePath = "load.png";

        public static bool ISLOGIN = true;

        // 다른 고객이 본 상품 인덱스 초기화 전역 변수        
        public static int g_main_index = -1;
        public static int g_other_index = -1;
        public static int g_SetUsedValue = 0;


        // 페이지 두번 클릭 제한 bool 변수(쇼핑)
        // false는 열리지 않은 상태, true는 열려있는 상태
        public static bool isOpen_ShopListPage = false; // ShopTabPage -> ShopListPage
        public static bool isOpen_ShopMainPage = false; // ShopListPage -> ShopMainPage(SaleView)
        public static bool isOpen_ShopDetailPage = false; // ShopMainPage(SaleView) -> ShopDetailPage
        public static bool isOpen_ShopOtherPage = false; // ShopDetailPage -> ShopOtherPage
        public static bool isOpen_PictureList = false; // ShopDetailPage -> PictureList


        // 페이지 두번 클릭 제한 bool 변수(주소창)
        public static bool isOpen_AdressModal = false; // ShopTabPage -> ShopListPage

        public static USERS user = new USERS();
        public static ADRESS adress = new ADRESS();


        // 메인 페이지 OnAppearing시 열려 있는 탭 확인용 전역 변수
        public static bool isMainDeal = true;
        public static bool isMainShop = false;
        public static bool isMainBasket = false;
        public static bool isMainMyinfo = false;

        public static void InitOnAppearingBool(string name)
        {
            if(name == "deal")
            {
                isMainDeal = true;
                isMainShop = false;
                isMainBasket = false;
                isMainMyinfo = false;
            }
            else if (name == "shop")
            {
                isMainDeal = false;
                isMainShop = true;
                isMainBasket = false;
                isMainMyinfo = false;
            }
            else if (name == "basket")
            {
                isMainDeal = false;
                isMainShop = false;
                isMainBasket = true;
                isMainMyinfo = false;
            }
            else if (name == "myinfo")
            {
                isMainDeal = false;
                isMainShop = false;
                isMainBasket = false;
                isMainMyinfo = true;
            }
        }
        // 장바구니 페이지 OnAppearing시 열려 있는 탭 확인용 전역 변수
        public static bool isBasketDeal = true;
        public static bool isBasketShop = false;

        public static void InitBasketOnAppearingBool(string name)
        {
            if (name == "deal")
            {
                isBasketDeal = true;
                isBasketShop = false;
            }
            else if (name == "shop")
            {
                isBasketDeal = false;
                isBasketShop = true;
            }
        }
        /// <summary>
        /// 다른 고객이 본 상품으로 연결하는 인덱스 업데이트 ( main페이지1 -> other페이지3 , main페이지3 -> other페이지2 )
        /// other이 된 페이지는 main이 된다.
        /// </summary>
        /// <param name="index"></param>
        public static void OtherIndexUpdate(int input)
        {
            if (Global.g_SetUsedValue == 0) // 첫 페이지를 열 때만( num1, x)
            {
                Global.g_main_index = input;
                Global.g_SetUsedValue = 1;
            }
            else if (Global.g_SetUsedValue == 1) // 두번째 페이지 스왑(num1, num2)
            {
                Global.g_other_index = input;
                Global.g_SetUsedValue = 2;
            }
            else if (Global.g_SetUsedValue == 2)// 세번째 부터 스왑의 연속(num2, num1)
            {
                Global.g_main_index = Global.g_other_index;
                Global.g_other_index = input;
            }
        }

    }

}
