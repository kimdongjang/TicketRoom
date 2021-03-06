﻿using FFImageLoading.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TicketRoom.Models.Custom;
using TicketRoom.Models.Gift.Purchase;
using TicketRoom.Models.Users;
using TicketRoom.Services;
using TicketRoom.Views.MainTab.MyPage;
using TicketRoom.Views.MainTab.Popup;
using TicketRoom.Views.Users.CreateUser;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.Dael.Purchase
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PurchaseDetailPage : ContentPage
    {
        InputAdress adrAPI;

        GiftDBFunc giftDBFunc = GiftDBFunc.Instance();
        PopupPhoneEntry popup_phone; // 핸드폰 번호 변경 팝업 객체
        PopupNameEntry popup_name; // 핸드폰 번호 변경 팝업 객체

        List<ADRESS> user_addrs = null;
        public string jibunAddr = ""; // 지번 주소 
        public string zipNo = "";     // 우편 번호

        bool deliveryType = false; // true : 구매리스트에 지류가 있을때 , false : 지류가 없는 상태 

        int UsedPoint = 0;
        int OldPoint = 0;
        int myPoint = 0;
        int DBGetPoint = 0;
        int tempdeliveryprice = 0;
        int price = 0;
        int deliveryprice = 3000;

        int order_price = 0;
        int order_price_to_db = 0;

        List<G_TempBasketProduct> tempBasketList = new List<G_TempBasketProduct>();
        G_TempBasketProduct tempBasket;

        // 구매페이지에서 접근
        public PurchaseDetailPage(G_TempBasketProduct tempBasket)
        {
            InitializeComponent();
            this.tempBasket = tempBasket;
            tempBasketList.Add(tempBasket);
            Init();
        }
        // 장바구니에서 접근
        public PurchaseDetailPage(List<G_TempBasketProduct> tempBasketList)
        {
            InitializeComponent();
            this.tempBasketList = tempBasketList;
            Init();
        }
        private void Init()
        {
            #region IOS의 경우 초기화
            NavigationPage.SetHasNavigationBar(this, false); // Navigation Bar 지우는 코드 생성자에 입력
            if (Xamarin.Forms.Device.OS == TargetPlatform.iOS)
            {
                MainGrid.RowDefinitions[0].Height = Global.title_size_value;
            }
            if (Global.ios_x_model == true) // ios X 이상의 모델일 경우
            {
                MainGrid.RowDefinitions[4].Height = 30;
            }
            #endregion            

            ShowUserInfo(); // 회원, 비회원에 따른 인풋 라벨 Visible 여부 확인
            ShowUserAddrlist(); // 지류, 핀번호에 따라 배송비+배송지 여부 체크
            SelectAllAccount(); // 입금계좌 초기화
            PurchaseListInit();
            Radio1_Clicked(prepaymentradio, null);  //선불 착불 기본값인 선불 선택해놈
            NavigationInit();
        }


        private void NavigationInit()
        {
            NavigationButton.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(async () =>
                {
                    // 로딩 시작
                    await Global.LoadingStartAsync();

                    await Navigation.PushAsync(new NavagationPage());

                    // 로딩 완료
                    await Global.LoadingEndAsync();
                })
            });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Global.isPurchaseDeatailBtn_clicked = true;
        }

        // 구매할 목록 초기화
        private void PurchaseListInit() 
        {
            int row = 0;
            for (int i = 0; i < tempBasketList.Count; i++)
            {
                PurchaseListGrid.RowDefinitions.Add(new RowDefinition { Height = 100 });

                Grid inGrid = new Grid
                {
                    ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = 100 },
                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                        new ColumnDefinition { Width = 30 }
                    },
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Margin = new Thickness(20, 5, 20, 5),
                    RowSpacing = 0,
                    ColumnSpacing = 0,
                };

                #region 장바구니 상품 이미지
                CachedImage product_image = new CachedImage
                {
                    LoadingPlaceholder = Global.LoadingImagePath,
                    ErrorPlaceholder = Global.NotFoundImagePath,
                    Source = ImageSource.FromUri(new Uri(Global.server_ipadress + tempBasketList[i].PRODUCT_IMAGE)),
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    Aspect = Aspect.AspectFill,
                };
                #endregion

                #region 상품 설명 Labellist 그리드
                Grid product_label_grid = new Grid
                {
                    Margin = new Thickness(10, 0, 0, 0),
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    RowSpacing = 0,
                    ColumnSpacing = 0,
                    RowDefinitions =
                    {
                        new RowDefinition { Height = GridLength.Auto },
                        new RowDefinition { Height = GridLength.Auto },
                        new RowDefinition { Height = 10 },
                        new RowDefinition { Height = GridLength.Auto }
                    },

                };

                #region 상품 제목 Label
                CustomLabel pro_label = new CustomLabel
                {
                    Text = tempBasketList[i].PDL_NAME,
                    Size = 18,
                    TextColor = Color.Black,
                };
                #endregion

                #region 상품 종류 Label (사이즈, 색상, 추가옵션)
                CustomLabel type_label = null;
                if (tempBasketList[i].PDL_PROTYPE.Equals("1"))
                {
                    type_label = new CustomLabel
                    {
                        Text = tempBasketList[i].PDL_COUNT + "개 (지류)",
                        Size = 14,
                        TextColor = Color.DarkGray,
                    };
                }
                else
                {
                    type_label = new CustomLabel
                    {
                        Text = tempBasketList[i].PDL_COUNT + "개 (핀번호)",
                        Size = 14,
                        TextColor = Color.DarkGray,
                    };
                }
                #endregion

                int product_all_price = int.Parse(tempBasketList[i].PDL_PRICE) * int.Parse(tempBasketList[i].PDL_COUNT);
                order_price += product_all_price; // 총 구매하려는 상품 가격
                #region 가격 내용 Label 및 장바구니 담은 날짜
                CustomLabel price_label = new CustomLabel
                {
                    Text = product_all_price.ToString("N0") + "원",
                    Size = 14,
                    TextColor = Color.Gray,
                };
                #endregion

                //상품 설명 라벨 그리드에 추가
                product_label_grid.Children.Add(pro_label, 0, 0);
                product_label_grid.Children.Add(type_label, 0, 1);
                product_label_grid.Children.Add(price_label, 0, 3);
                #endregion

                #region 상품권 그리드 자식 추가
                inGrid.Children.Add(product_image, 0, 0);
                inGrid.Children.Add(product_label_grid, 1, 0);
                #endregion


                //장바구니 리스트 그리드에 추가 
                PurchaseListGrid.Children.Add(inGrid, 0, row);
                row++;
                if (tempBasketList.Count > 1)
                {
                    PurchaseListGrid.RowDefinitions.Add(new RowDefinition { Height = 3 });
                    #region 구분선
                    BoxView gridline = new BoxView
                    {
                        BackgroundColor = Color.FromHex("#f4f2f2"),
                        VerticalOptions = LayoutOptions.End,
                        HorizontalOptions = LayoutOptions.FillAndExpand
                    };
                    //구분선 그리드에 추가 
                    PurchaseListGrid.Children.Add(gridline, 0, row);
                    row++;
                    #endregion
                }
            }
        }

        // 회원, 비회원 체크
        private void ShowUserInfo() 
        {
            if (Global.b_user_login)
            {
                MyNameLabel.Text = Global.user.NAME;
                MyPhoneLabel.Text = Global.user.PHONENUM;
                string test = "";
                #region 네트워크 상태 가능
                var current_network = Connectivity.NetworkAccess; // 현재 네트워크 상태
                if (current_network == NetworkAccess.Internet) // 네트워크 연결 가능
                {
                    test = giftDBFunc.PostSelectUserPoint(Global.ID); // 유저 포인트
                }
                else
                {
                    test = "";
                }
                #endregion

                #region 네트워크 연결 불가
                if (test == "") // 네트워크 연결 불가
                {
                    DisplayAlert("알림", "네트워크에 연결할 수 없습니다. 다시 한번 시도해주세요.", "확인");
                    return;
                }
                #endregion
                DBGetPoint = int.Parse(test); // 보유 포인트 갱신
                myPoint = int.Parse(test); // 보유 포인트 갱신
                Point_label.Text = int.Parse(test).ToString("N0");

            }
            else
            {
                myPoint = 0; // 보유 포인트 갱신
                Point_label.Text = int.Parse("0").ToString("N0");
                MyNameLabel.Text = "이름을 입력하세요";
                MyPhoneLabel.Text = "연락처를 입력해주세요";
                RecentAdress.IsVisible = false;
            }
        }

        // 지류, 핀번호에 따라 배송비+배송지 여부 체크
        private void ShowUserAddrlist()
        {
            for (int i = 0; i < tempBasketList.Count; i++)
            {
                if (tempBasketList[i].PDL_PROTYPE.Equals("1"))
                {
                    // 지류인 경우
                    deliveryType = true;
                }
            }
            if (tempBasket != null)
            {
                // 구매 탭에서 바로 진행된 경우
                if (tempBasket.PDL_PROTYPE == "2") // 핀번호일 경우 배송지 없음.
                {
                    DeliveryPrice_label.IsVisible = false;
                }
            }
            else
            {
                // 장바구니 탭에서 진행된 경우
                for(int i = 0; i< tempBasketList.Count; i++)
                {
                    if(tempBasketList[i].PDL_PROTYPE == "2")  // 핀번호일 경우 배송지 없음.
                    {
                        DeliveryPrice_label.IsVisible = false;
                    }
                }
            }

            if (deliveryType == true) // 지류 상태이면 배송지, 배송비가 필요함
            {
                List<ADRESS> test = giftDBFunc.ShowUserAddrlist(Global.ID); // 유저 주소 리스트

                if (test.Count >= 1)
                {
                    Addr_Picker.Items.Clear();
                    for (int i = 0; i < test.Count; i++)
                    {
                        Addr_Picker.Items.Add(test[i].ROADADDR);
                    }
                    user_addrs = test;
                    EntryAdress.Text = test[0].ROADADDR;
                    jibunAddr = test[0].JIBUNADDR;
                    zipNo = test[0].ZIPNO.ToString();
                }
                tempdeliveryprice = 3000;
            }
            else // 핀번호 상태이면 배송지 + 배송비 필요 없음
            {
                AdressListGrid.IsVisible = false;
                DV_Label.IsVisible = false;
                DV_Radio_Group.IsVisible = false;
                tempdeliveryprice = 0;
                //DeliveryPrice_label.IsVisible = false;
                DV_GridLine.IsVisible = false;
            }
        }

        // 결제금액 업데이트
        private void PriceUpdate()
        {
            if (deliveryType == true) // 지류인 경우 배송비 추가 계산
            {
                order_price_to_db/*실제주문금액*/ = order_price/*결제금액*/ + tempdeliveryprice/*배송비*/ - UsedPoint/*사용포인트*/;
                if (order_price_to_db < 0) // 결제할 금액이 마이너스가 될 경우
                {
                    DisplayAlert("알림", "입력한 포인트를 다시 한번 확인해주십시오!", "확인");
                    order_price_to_db = 0;
                }
                Purchase_AllPrice_label.Text = (order_price_to_db).ToString("N0") + " 원";
                DeliveryPrice_label.Text = "배송비 : " + tempdeliveryprice + "원";

            }
            else if(deliveryType == false) // 핀번호 인경우 배송비 제외 계산
            {
                order_price_to_db/*실제주문금액*/ = order_price/*결제금액*/ - UsedPoint/*사용포인트*/;
                if (order_price_to_db < 0) // 결제할 금액이 마이너스가 될 경우
                {
                    DisplayAlert("알림", "입력한 포인트를 다시 한번 확인해주십시오!", "확인");
                    order_price_to_db = 0;
                }
                Purchase_AllPrice_label.Text = (order_price_to_db).ToString("N0") + " 원";
                DeliveryPrice_label.Text = "배송비 : " + tempdeliveryprice + "원";
            }          

        }

        private void Radio1_Clicked(object sender, EventArgs e) // 선불 버튼
        {
            prepaymentradio.Source = "radio_checked_icon.png";
            Cashondeliveryradio.Source = "radio_unchecked_icon.png";
            tempdeliveryprice = deliveryprice; // 배송비 추가
            PriceUpdate();            
        }

        private void Radio2_Clicked(object sender, EventArgs e) // 착불 버튼
        {
            prepaymentradio.Source = "radio_unchecked_icon.png";
            Cashondeliveryradio.Source = "radio_checked_icon.png";
            tempdeliveryprice = 0; // 배송비 삭제
            PriceUpdate();
        }

        public PurchaseDetailPage()
        {
            InitializeComponent();
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            if (Global.isPurchaseDeatailBtn_clicked)
            {
                Global.isPurchaseDeatailBtn_clicked = false;
                Navigation.PopAsync();
            }
        }

        private void UsedPointBtn_Clicked(object sender, EventArgs e)
        {
            try
            {
                UsedPoint += int.Parse(Regex.Replace(Point_box.Text, @"\D", ""));
                if (UsedPoint > myPoint)
                {
                    DisplayAlert("알림", "보유한 포인트가 부족합니다.", "확인");
                    UsedPoint -= int.Parse(Point_box.Text);
                    Point_box.Text = ""; // 포인트 입력 란
                }
                else
                {
                    // 보유 포인트가 충분할 경우

                    if (deliveryType == true) // 지류인 경우 배송비 추가 계산
                    {
                        order_price_to_db/*실제주문금액*/ = order_price/*결제금액*/ + tempdeliveryprice/*배송비*/ - UsedPoint/*사용포인트*/;
                    }
                    else if (deliveryType == false) // 핀번호 인경우 배송비 제외 계산
                    {
                        order_price_to_db/*실제주문금액*/ = order_price/*결제금액*/ - UsedPoint/*사용포인트*/;
                    }
                    if (order_price_to_db < 0) // 결제할 금액이 마이너스가 될 경우
                    {
                        DisplayAlert("알림", "입력한 포인트를 다시 한번 확인해주십시오!", "확인");
                        order_price_to_db = 0;
                        UsedPoint -= int.Parse(Point_box.Text);
                        Point_box.Text = ""; // 포인트 입력 란
                        return;
                    }
                    
                    // 초기화
                    myPoint -= UsedPoint; // 보유 포인트(변수용)
                    Point_label.Text = (myPoint).ToString("N0"); // 잔여 포인트(보여주기용)
                    UsedPointLabel.Text = UsedPoint.ToString("N0"); // 사용 포인트(보여주기용)
                    Point_box.Text = ""; // 포인트 입력 란
                    // 결제금액 갱신
                    PriceUpdate();
                    DisplayAlert("알림", "포인트가 적용되었습니다.", "OK");
                }
            }
            catch
            {
                DisplayAlert("알림", "숫자만 입력해주세요", "OK");
                Point_box.Text = ""; // 포인트 입력 란
            }
        }

        private void Point_box_TextChanged(object sender, TextChangedEventArgs e)
        {
            //try
            //{
            //    if (e.NewTextValue.Contains(".") || e.NewTextValue.Contains("-"))
            //    {
            //        if (e.OldTextValue != null)
            //        {
            //            Point_box.Text = e.OldTextValue;
            //        }
            //        else
            //        {
            //            Point_box.Text = "";
            //        }
            //        return;
            //    }
            //    else
            //    {
            //        if (int.Parse(Point_box.Text) > price)
            //        {
            //            Point_box.Text = (price + tempdeliveryprice).ToString();
            //        }
            //        else
            //        {
            //            if (int.Parse(Point_box.Text) > int.Parse(Point_label.Text.Replace(",", "")))
            //            {
            //                Point_box.Text = Point_label.Text.Replace(",", "");
            //            }
            //        }

            //    }
            //}
            //catch
            //{

            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EntryAdress_Focused(object sender, FocusEventArgs e)
        {
            EntryAdress.Unfocus();

            Navigation.PushModalAsync(adrAPI = new InputAdress(this));
        }

        private async void PurchaseBtn_Clicked(object sender, EventArgs e)
        {
            if (Global.isPurchaseDeatailBtn_clicked)
            {
                Global.isPurchaseDeatailBtn_clicked = false;
                if (deliveryType) // 구매리스트에 지류가 있을때 
                {
                    if (EntryAdress.Text != "" && EntryAdress.Text != null)
                    {
                        if (MyNameLabel.Text != "" && MyNameLabel.Text != null && MyNameLabel.Text != "이름을 입력하세요")
                        {
                            if (MyPhoneLabel.Text != "" && MyPhoneLabel.Text != null && MyPhoneLabel.Text != "연락처를 입력해주세요")
                            {
                                if (Name_box.Text != "" && Name_box.Text != null)
                                {
                                    if (Combo.SelectedItem != null)
                                    {
                                        DoPurchase(EntryAdress.Text);
                                    }
                                    else
                                    {
                                        DisplayAlert("알림", "입금계좌를 선택해주세요", "OK");
                                        Global.isPurchaseDeatailBtn_clicked = true;
                                    }
                                }
                                else
                                {
                                    DisplayAlert("알림", "입금예정인을 입력해주세요", "OK");
                                    Global.isPurchaseDeatailBtn_clicked = true;
                                }
                            }
                            else
                            {
                                DisplayAlert("알림", "연락처를 입력해주세요", "OK");
                                Global.isPurchaseDeatailBtn_clicked = true;
                            }
                        }
                        else
                        {
                            DisplayAlert("알림", "수취인을 입력해주세요", "OK");
                            Global.isPurchaseDeatailBtn_clicked = true;
                        }
                    }
                    else
                    {
                        DisplayAlert("알림", "주소를 입력해주세요", "OK");
                        Global.isPurchaseDeatailBtn_clicked = true;
                    }
                }
                else// 구매리스트에 지류가 없을때 
                {
                    if (MyNameLabel.Text != "" && MyNameLabel.Text != null && MyNameLabel.Text != "이름을 입력하세요")
                    {
                        if (MyPhoneLabel.Text != "" && MyPhoneLabel.Text != null && MyPhoneLabel.Text != "연락처를 입력해주세요")
                        {
                            if (Name_box.Text != "" && Name_box.Text != null)
                            {
                                if (Combo.SelectedItem != null)
                                {
                                    DoPurchase("");
                                }
                                else
                                {
                                    DisplayAlert("알림", "입금계좌를 선택해주세요", "OK");
                                    Global.isPurchaseDeatailBtn_clicked = true;
                                }
                            }
                            else
                            {
                                DisplayAlert("알림", "입금예정인을 입력해주세요", "OK");
                                Global.isPurchaseDeatailBtn_clicked = true;
                            }
                        }
                        else
                        {
                            DisplayAlert("알림", "연락처를 입력해주세요", "OK");
                            Global.isPurchaseDeatailBtn_clicked = true;
                        }
                    }
                    else
                    {
                        DisplayAlert("알림", "수취인을 입력해주세요", "OK");
                        Global.isPurchaseDeatailBtn_clicked = true;
                    }
                }
            }

        }

        public async void DoPurchase(string address)
        {
            string userid = "";
            string isuser = "";
            string delivery_type = "";
            if (Global.b_user_login)
            {
                userid = Global.ID;
                isuser = "1";
            }
            else
            {
                userid = Global.non_user_id;
                isuser = "2";
            }

            // 선불 착불 선택
            if (prepaymentradio.Source.ToString().Contains("radio_checked_icon.png"))
            {
                delivery_type = "1";
            }
            else
            {
                delivery_type = "2";
            }

            G_PurchaseList classPurchase = new G_PurchaseList
            {
                ID = userid,  // 구매자 ID
                PL_DELIVERY_ADDRESS = address,  // 배송지 도로명 주소 
                PL_USED_POINT = UsedPoint.ToString(), // 사용 포인트
                PL_ISSUCCESS = "", // 성공여부
                PL_DELIVERYPAY_TYPE = delivery_type, // 배송타입(1: 선불 2: 착불)
                PL_PAYMENT_PRICE = order_price_to_db.ToString(),  // 총 결제금액
                AC_NUM = (Combo.SelectedIndex + 1).ToString(), // 계좌번호
                PL_ACCUSER_NAME = Name_box.Text, // 입금예정자이름
                PL_DV_NAME = MyNameLabel.Text,  // 배송받는사람이름
                PL_DV_PHONE = MyPhoneLabel.Text, // 배송시 연락처
                PL_DELIVERY_JIBUNADDR = jibunAddr,  // 배송지 지번 주소
                PL_DELIVERY_ZIPNO = zipNo,  // 배송지 우편번호
                G_TempProduct = tempBasketList, // 상품가격, 상품번호 등 상품권 정보 클래스
                ISUSER = isuser,
            };

            // 리스트를 WCF 로 보낼 방법..........................................................................................


            //// JSON 문자열을 파싱하여 JObject를 리턴
            //JObject jo = JObject.Parse(str);

            #region 네트워크 상태 확인
            var current_network = Connectivity.NetworkAccess; // 현재 네트워크 상태
            if (current_network == NetworkAccess.Internet) // 네트워크 연결 가능
            {
                var dataString = JsonConvert.SerializeObject(classPurchase);
                JObject jo = JObject.Parse(dataString);
                UTF8Encoding encoder = new UTF8Encoding();

                string str = @"{";
                str += "g_PurchaseList:" + jo.ToString();  //아이디찾기에선 Name으로
                str += "}";

                JObject jo2 = JObject.Parse(str);

                byte[] data = encoder.GetBytes(jo2.ToString()); // a json object, or xml, whatever...

                //request.Method = "POST";
                HttpWebRequest request = WebRequest.Create(Global.WCFURL + "UserAddPurchase") as HttpWebRequest;
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
                        if (test == "1") // 구매 성공했을 경우
                        {
                            for(int j=0; j< tempBasketList.Count; j++)
                            {
                                giftDBFunc.PostDeleteGiftBasketListToIndex(tempBasketList[j].BASKET_INDEX);
                            }
                            

                            await DisplayAlert("알림", "결제 완료! 구매내역에서 확인해주세요.", "확인");
                            Global.isPurchaseDeatailBtn_clicked = true;

                            Global.InitOnAppearingBool("deal");
                            await Navigation.PopToRootAsync();
                            MainPage mp = (MainPage)Application.Current.MainPage.Navigation.NavigationStack[0];
                        }
                        else if (test == "5") // 수량 부족으로 실패했을 경우
                        {
                            await DisplayAlert("알림", "수량 부족으로 결제에 실패했습니다.", "확인");
                            Global.isPurchaseDeatailBtn_clicked = true;
                        }
                        /*
                        // 3개의 아웃풋이 있는데 사실상 1개만 쓸 예정.
                        else if (int.Parse(test2[0].ToString()) == 2)
                        {
                            if (test2[1] != null && test2[1] != "")
                            {
                                string[] proinfos = test2[1].Split('@');
                                string[] procnts = test2[2].Split('@');
                                string errormessage = "";
                                for (int i = 0; i < proinfos.Length - 1; i++)
                                {
                                    errormessage += proinfos[i] + "가 " + procnts[i] + "개 있습니다";
                                }
                                DisplayAlert("알림", errormessage, "OK");
                                Global.isPurchaseDeatailBtn_clicked = true;
                            }
                        }
                        */
                        else
                        {
                            await DisplayAlert("알림", "서버점검중입니다", "확인");
                            Global.isPurchaseDeatailBtn_clicked = true;
                        }
                    }
                }
            }
            else
            {
                await DisplayAlert("알림", "네트워크에 연결할 수 없습니다. 다시 한번 시도해주세요.", "확인");
                Global.isPurchaseDeatailBtn_clicked = true;
                return;
            }
            #endregion
        }

        public async Task ShowMessage(string message, string title, string buttonText, Action afterHideCallback)
        {
            await DisplayAlert(title, message, buttonText);

            afterHideCallback?.Invoke();
        }

        private void Point_box_Focused(object sender, FocusEventArgs e)
        {
            Point_box.Text = "";
        }

        #region 입금계좌 초기화
        private void SelectAllAccount()
        {
            #region 네트워크 상태 확인
            var current_network = Connectivity.NetworkAccess; // 현재 네트워크 상태
            if (current_network == NetworkAccess.Internet) // 네트워크 연결 가능
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
                        if (test != null)
                        {
                            ShowAccount(test);
                        }
                    }
                }
            }
            else
            {
                DisplayAlert("알림", "네트워크에 연결할 수 없습니다. 다시 한번 시도해주세요.", "확인");
                return;
            }
            #endregion
        }

        private void ShowAccount(List<AccountInfo> accountlist)
        {
            if (accountlist.Count == 0)
            {
                if (accountlist[0].Error == null || accountlist[0].Error == "")
                {
                    Navigation.PopAsync();
                    return;
                }
            }

            for (int i = 0; i < accountlist.Count; i++)
            {
                Combo.Items.Clear();
                Combo.Items.Add(accountlist[i].AC_BANKNAME + ": " + accountlist[i].AC_ACCOUNTNUM + " " + accountlist[i].AC_NAME);
            }
        }
        #endregion

        private void ShowAddr_btnClicked(object sender, EventArgs e)
        {
            Addr_Picker.Focus();
        }

        private void Addr_PickerChanged(object sender, EventArgs e)
        {
            EntryAdress.Text = user_addrs[Addr_Picker.SelectedIndex].ROADADDR;
            jibunAddr = user_addrs[Addr_Picker.SelectedIndex].JIBUNADDR;
            zipNo = user_addrs[Addr_Picker.SelectedIndex].ZIPNO.ToString();
        }

        private void ChaneName_btnClicked(object sender, EventArgs e)
        {
            PopupNavigation.PushAsync(popup_name = new PopupNameEntry(this));
        }

        private void ChangePhone_btnClicked(object sender, EventArgs e)
        {
            PopupNavigation.PushAsync(popup_phone = new PopupPhoneEntry(this));
        }

        private void CanselPointBtn_Clicked(object sender, EventArgs e)
        {
            Point_box.Text = "";
            UsedPointLabel.Text = "0";
            Point_label.Text = DBGetPoint.ToString();
            UsedPoint = 0;
            myPoint = DBGetPoint;
        }
    }
}