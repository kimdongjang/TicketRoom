﻿using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TicketRoom.Models;
using TicketRoom.Models.Gift;
using TicketRoom.Models.Users;
using TicketRoom.Views;
using TicketRoom.Views.MainTab.Shop;
using Xamarin.Forms;

namespace TicketRoom
{
    public class Global
    {
        public static string WCFURL = @"http://124.62.109.137:8088/Service1.svc/";
        public static string server_ipadress = @"http://124.62.109.137:8088/";

        //public static string WCFURL = @"http://52.231.66.251/Service1.svc/";

        //운기 로컬 Services
        //public static string WCFURL = @"http://220.90.190.218:8081/Service1.svc/";

        public static string ID = "";

        public static string non_user_id = "";
        public static bool b_user_login = false; // 회원 로그인 상태
        public static bool b_auto_login = false; // 자동 로그인 상태
        public static bool b_guest_login = false; // 게스트 로그인 상태

        public static string android_serial_number = "";
        public static string ios_serial_number = "";

        public static string localPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        public static string LoadingImagePath = "load.png";
        public static string NotFoundImagePath = "not_image.png";

        public static bool ISLOGIN = true;

        #region IOS 최적화 관련
        public static int font_size_minus_value = 0; // IOS 디바이스 종류에 따라 레이블 크기 조정
        public static int title_size_value = 50; // 타이틀 사이즈 IOS 디바이스 종류에 따라 조정 기본 50
        public static bool ios_x_model = false;
        #endregion


        // 상품권 탭 클릭시의 카테고리 번호
        public static string deal_select_category_num = "";
        // 상품권 탭 클릭시의 카테고리 종류(구매/판매)
        public static string deal_select_category_value = "";

        // loading창 back button blocking하는 bool 변수
        public static bool isloading_block = false;

        #region 백버튼 이미지 더블클릭 방지 ????하나만 써도 될듯한데
        public static bool isbackbutton_clicked = true;
        #endregion

        #region 구매/판매 관련 버튼 더블클릭 방지 변수 
        //구매판매 상세 리스트 더블클릭 막는 bool변수
        public static bool isgiftlistcliecked = true;
        //구매 페이지 버튼 더블클릭 막는 bool변수
        public static bool isgiftpurchasepage_clieck = true;
        //구매 판매 탭 더블클릭 막는 bool변수
        public static bool isDealTabCliecked = true;
        //구매 상세 페이지 구매버튼 더블클릭 막는 bool변수
        public static bool isPurchaseDeatailBtn_clicked = true;
        //판매 페이지 판매버튼 더블클릭 막는 bool변수
        public static bool isSaleBtnclicked = true;
        #endregion
        
        #region 장바구니 관련 버튼 더블클릭 방지 변수 
        //장바구니 주문하기 버튼 더블클릭 막는 bool변수
        public static bool isgiftbastketorderbtn_clicked = true;
        #endregion

        #region 마이페이지 관련 버튼 더블클릭 방지 변수 
        //마이페이지 버튼들 버튼 더블 클릭 막는 변수
        public static bool ismypagebtns_clicked = true;
        //내정보 수정 완료 버튼 더블 클릭 막는 변수
        public static bool ischangemyinfobtn_clicked = true;
        #endregion

        #region 네비게이션 관련 버튼 더블클릭 방지 변수
        //네비게이션 관련 버튼 더블 클릭 막는 변수
        public static bool isNavigation_clicked = true;
        #endregion

        #region 로그인 및 Users 관련 페이지 더블클릭 방지 변수
        //로그인 버튼 더블 클릭 막는 변수
        public static bool isloginbtn_clicked = true;
        //약관 동의 페이지 버튼 더블 클릭 막는 변수
        public static bool isaccepttermsnextbtn_clicked = true;
        //회원가입 정보 페이지 버튼 더블 클릭 막는 변수
        public static bool iscreateusernextbtn_clicked = true;
        //회원가입 폰정보 페이지 버튼 더블 클릭 막는 변수
        public static bool iscreateuserphonenextbtn_clicked = true;
        //아이디찾기 페이지 버튼 더블 클릭 막는 변수
        public static bool isfindidpage_clicked = true;
        //비번찾기 페이지 버튼 더블 클릭 막는 변수
        public static bool isfindpwpage_clicked = true;

        #endregion
        
        #region Loading
        public static Loading loadingScreen;
        public static async Task LoadingStartAsync()
        {
            loadingScreen = new Loading();
            await PopupNavigation.PushAsync(loadingScreen);
            Global.isloading_block = true;
        }
        public static async Task LoadingEndAsync()
        {
            if (PopupNavigation.PopupStack.Count != 0)
            {
                await PopupNavigation.PopAllAsync();
            }
            Global.isloading_block = false;
        }
        #endregion

        #region 상품권 사용여부 조회 크롤링 패킷 확인
            // 해피머니 확인
        // happy_money_gift_num_none
        // happy_money_gift_num_use
        // happy_money_gift_num_error

            // 북앤라이프 사용 불가
        // book_and_life_book_pin_none
        // book_and_life_book_inherence_none
        // book_and_life_mobile_none
        // book_and_life_online_none

            // 북앤 라이프 사용 가능
        // book_and_life_book_pin_use
        // book_and_life_book_inherence_use
        // book_and_life_mobile_use
        // book_and_life_online_use
        public static string CrawlingReturnValueCheck(string packet)
        {
            // 사용가능
            if(packet == "happy_money_gift_num_use" || packet == "book_and_life_book_pin_use" || packet == "book_and_life_book_inherence_use"
                || packet == "book_and_life_mobile_use" || packet == "book_and_life_online_use")
            {
                return "사용가능";
            }
            else if (packet == "happy_money_gift_num_none" || packet == "book_and_life_book_pin_none" || packet == "book_and_life_book_inherence_none"
                || packet == "book_and_life_mobile_none" || packet == "book_and_life_online_none")
            {

                return "사용불가";
            }
            else
            {
                return "에러";
            }            
        }
        #endregion

        #region 구매/판매 상태값 -> 문자열 반환 함수
        public static string StateToString(string state)
        {
            #region 판매(지류)
            if (state.Equals("11"))
            {
                return "판매대기";
            }
            else if (state.Equals("12"))
            {
                return "판매가능";//(배송중)
            }
            else if (state.Equals("13"))
            {
                return "판매실패";//(전달사항 기준 판매거절)
            }
            else if (state.Equals("14"))
            {
                return "배송완료";
            }
            else if (state.Equals("15"))
            {
                return "판매실패";//(배송실패)
            }
            else if (state.Equals("16"))
            {
                return "판매완료";//(입금완료)
            }
            else if (state.Equals("17"))
            {
                return "판매실패";//(잘못된 입금정보 입력 판매거절)
            }
            else if (state.Equals("18"))
            {
                return "판매실패";//(지류상태보고 판매거절)
            }
            else if (state.Equals("19"))
            {
                return "판매실패";//(시스템 오류)
            }
            #endregion
            #region 판매(핀번호)
            else if (state.Equals("21"))
            {
                return "판매대기";
            }
            else if (state.Equals("22"))
            {
                return "판매가능";//(입금대기)
            }
            else if (state.Equals("23"))
            {
                return "판매실패";//(유효하지않은 핀번호 포함됨)
            }
            else if (state.Equals("24"))
            {
                return "판매완료";//(입금완료)
            }
            else if (state.Equals("25"))
            {
                return "판매실패";//(잘못된 계좌정보)
            }
            else if (state.Equals("26"))
            {
                return "판매실패";//(잘못된 핀번호)
            }
            else if (state.Equals("27"))
            {
                return "판매실패";//(시스템오류)
            }
            #endregion
            #region 구매(주문번호에 대한 상태)
            //성공여부(1: 구매대기(입금대기) 2: 구매중(배송 / 발송대기, 발송중) 3: 구매실패(입금시간초과) 4: 구매완료 5: 구매실패(수량부족) 6: 시스템에러)
            else if (state.Equals("1"))
            {
                return "구매대기";
            }
            else if (state.Equals("2"))
            {
                return "구매중";
            }
            else if (state.Equals("3"))
            {
                return "구매실패";
            }
            else if (state.Equals("4"))
            {
                return "구매완료";
            }
            else if (state.Equals("5"))
            {
                return "구매실패";//(수량부족)
            }
            else if (state.Equals("6"))
            {
                return "구매실패";//(시스템에러)
            }
            #endregion
            #region 구매(상세 삼품 지류 상태)
            //지류 배송상태(30: 구매 대기 31: 배송중 32:배송 완료 33: 구매실패(반송) 34: 구매실패(수량부족) 35: 구매실패(시스템에러) 36: 교환 37: 환불)
            else if (state.Equals("30"))
            {
                return "구매대기";
            }
            else if (state.Equals("31"))
            {
                return "배송중";
            }
            else if (state.Equals("32"))
            {
                return "배송완료";
            }
            else if (state.Equals("33"))
            {
                return "구매실패";//(반송)
            }
            else if (state.Equals("34"))
            {
                return "구매실패";//수량부족
            }
            else if (state.Equals("35"))
            {
                return "구매실패";//(시스템에러)
            }
            else if (state.Equals("36"))
            {
                return "교환";//(교환)
            }
            else if (state.Equals("37"))
            {
                return "환불";
            }
            #endregion
            #region 구매(상세 삼품 핀번호 상태)
            //핀번호 상태값  41: 구매 대기 42:배송/발송 완료 43: 구매실패(시스템에러)
            else if (state.Equals("41"))
            {
                return "구매대기";
            }
            else if (state.Equals("42"))
            {
                return "발송완료";
            }
            else if (state.Equals("43"))
            {
                return "발송실패";
            }
            #endregion
            return "예외 없는 상태값";
        }
        #endregion

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

        //구매 판매 현재탭 기억 변수
        public static string current_Tab_P_or_S = "P";

        // 메인 페이지 OnAppearing시 열려 있는 탭 확인용 전역 변수
        public static bool isMainDeal = true;
        public static bool isMainShop = false;
        public static bool isMainBasket = false;
        public static bool isMainMyinfo = false;
        public static bool isMainDealDeatil = false;

        // 쇼핑 리스트 페이지 OnAppearing시 열려 있는 탭 확인용 전역 변수
        public static int OnShopListTapIndex = 0;

        public static void InitOnAppearingBool(string name)
        {
            if(name == "deal")
            {
                isMainDeal = true;
                isMainShop = false;
                isMainBasket = false;
                isMainMyinfo = false;
                isMainDealDeatil = false;
            }
            else if (name == "shop")
            {
                isMainDeal = false;
                isMainShop = true;
                isMainBasket = false;
                isMainMyinfo = false;
                isMainDealDeatil = false;
            }
            else if (name == "basket")
            {
                isMainDeal = false;
                isMainShop = false;
                isMainBasket = true;
                isMainMyinfo = false;
                isMainDealDeatil = false;
            }
            else if (name == "myinfo")
            {
                isMainDeal = false;
                isMainShop = false;
                isMainBasket = false;
                isMainMyinfo = true;
                isMainDealDeatil = false;
            }
            else if (name == "dealdetail")
            {
                isMainDeal = false;
                isMainShop = false;
                isMainBasket = false;
                isMainMyinfo = false;
                isMainDealDeatil = true;
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
