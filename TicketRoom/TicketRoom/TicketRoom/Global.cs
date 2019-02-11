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
        //public static string WCFURL = @"http://221.141.58.49:8088/Service1.svc/";
        //public static string WCFURL = @"http://52.231.66.251/Service1.svc/";

        public static List<G_BasketInfo> BasketList = new List<G_BasketInfo>();
        //public static string WCFURL = @"http://52.231.66.251/Service1.svc/";
        //public static string WCFURL = @"http://220.90.190.218/Service1.svc/";
        //public static string WCFURL = @"http://localhost:65192/Service1.svc/";
        //운기 로컬 Services
        public static string WCFURL = @"http://220.90.190.218:8081/Service1.svc/";

        public static string ID = "";

        public static string non_user_id = "";
        public static bool b_user_login = false; // 회원 로그인 상태
        public static bool b_auto_login = false; // 자동 로그인 상태

        public static string localPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        public static bool ISLOGIN = true;


        // 페이지 두번 클릭 제한 bool 변수(쇼핑)
        // false는 열리지 않은 상태, true는 열려있는 상태
        public static bool isOpen_ShopListPage = false; // ShopTabPage -> ShopListPage
        public static bool isOpen_ShopMainPage = false; // ShopListPage -> ShopMainPage(SaleView)
        public static bool isOpen_ShopDetailPage = false; // ShopMainPage(SaleView) -> ShopDetailPage
        public static bool isOpen_ShopOtherPage = false; // ShopDetailPage -> ShopOtherPage
        public static bool isOpen_PictureList = false; // ShopDetailPage -> PictureList


        public static USERS user;
        public static ADRESS adress;

    }
}
