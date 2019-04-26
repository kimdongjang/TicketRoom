using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TicketRoom.Models.Custom;
using TicketRoom.Models.Gift.Purchase;
using TicketRoom.Models.Gift.PurchaseList;
using TicketRoom.Models.Users;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.MyPage.PurchaseList
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PurchaseDetailListGift : ContentPage
    {
        List<G_PurchaseList> purchaselist = new List<G_PurchaseList>();
        List<PLProInfo> productlist = new List<PLProInfo>();
        List<AccountInfo> accountlist = new List<AccountInfo>();

        G_PinNumberProduct pinData = null;
        G_PurchaseListDetail product_detail = null;
        string pl_index = "";
        int delivery_pay = 3000; //배송비

        public PurchaseDetailListGift(string pl_index)
        {
            InitializeComponent();
            this.pl_index = pl_index;
            Init();
        }

        public PurchaseDetailListGift(string pl_index, G_PurchaseListDetail p_detail) // 핀번호 생성자
        {
            InitializeComponent();
            this.pl_index = pl_index;
            this.product_detail = p_detail;
            //this.pinData = pinData;
            Init();

        }
        private void Init()
        {
            #region IOS의 경우 초기화
            NavigationPage.SetHasNavigationBar(this, false); // Navigation Bar 지우는 코드 생성자에 입력
            if (Xamarin.Forms.Device.OS == TargetPlatform.iOS)
            {
                MainGrid2.RowDefinitions[0].Height = Global.title_size_value;
            }
            if (Global.ios_x_model == true) // ios X 이상의 모델일 경우
            {
                MainGrid2.RowDefinitions[5].Height = 30;
            }
            #endregion

            SearchAccountList();
            SearchPurchaseDetailToPlNum(pl_index);
            SearchPurchaseListToPlNum(pl_index);
            if(product_detail != null) { SearchPinListToPlNum(product_detail.PDL_PINNUM); }            
            ListInit();
            NavigationInit();
        }

        // 네비게이션 바 초기화
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

        // 계좌 리스트 검색
        public void SearchAccountList()
        {
            #region 네트워크 상태 확인
            var current_network = Connectivity.NetworkAccess; // 현재 네트워크 상태
            if (current_network != NetworkAccess.Internet) // 네트워크 연결 불가
            {
                accountlist = null;
                return;
            }
            #endregion
            #region 네트워크 연결 가능
            else
            {
                accountlist = UserDBFunc.Instance().GetSelectAllAccount();
            }
            #endregion
        }
        // 구매 번호를 통해 핀번호 관련 상세구매리스트 가져오기
        public void SearchPurchaseDetailToPlNum(string pl_num)
        {
            #region 네트워크 상태 확인
            var current_network = Connectivity.NetworkAccess; // 현재 네트워크 상태
            if (current_network != NetworkAccess.Internet) // 네트워크 연결 불가
            {
                purchaselist = null;
                return;
            }
            #endregion
            #region 네트워크 연결 가능
            else
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
            }
            #endregion
        }

        // 구매 번호를 통해 구매리스트 가져오기
        public void SearchPurchaseListToPlNum(string pl_num)
        {
            #region 네트워크 상태 확인
            var current_network = Connectivity.NetworkAccess; // 현재 네트워크 상태
            if (current_network != NetworkAccess.Internet) // 네트워크 연결 불가
            {
                DisplayAlert("알림", "네트워크에 연결할 수 없습니다. 다시 한번 시도해주세요.", "확인");
                productlist = null;
                return;
            }
            #endregion
            #region 네트워크 연결 가능
            else
            {
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
            }
            #endregion
        }

        // 핀 상품번호를 통해 핀번호리스트 가져오기
        public void SearchPinListToPlNum(string plnum)
        {
            #region 네트워크 상태 확인
            var current_network = Connectivity.NetworkAccess; // 현재 네트워크 상태
            if (current_network != NetworkAccess.Internet) // 네트워크 연결 불가
            {
                DisplayAlert("알림", "네트워크에 연결할 수 없습니다. 다시 한번 시도해주세요.", "확인");
                productlist = null;
                return;
            }
            #endregion
            #region 네트워크 연결 가능
            else
            {
                //구매내역 가져오기
                string str = @"{";
                str += "plnum : '" + plnum;
                str += "'}";

                //// JSON 문자열을 파싱하여 JObject를 리턴
                JObject jo = JObject.Parse(str);

                UTF8Encoding encoder = new UTF8Encoding();
                byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

                HttpWebRequest request = WebRequest.Create(Global.WCFURL + "SearchPinListToPlNum") as HttpWebRequest;
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
                                pinData = JsonConvert.DeserializeObject<G_PinNumberProduct>(readdata);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                }
            }
            #endregion
        }


        private void ListInit()
        {
            int coverGridRow = 0;
            #region 네트워크 연결 불가
            if (productlist == null)
            {
                CustomLabel error_label = new CustomLabel
                {
                    Text = "네트워크에 연결할 수 없습니다. 다시 시도해 주세요.",
                    Size = 18,
                    TextColor = Color.Black,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalTextAlignment = TextAlignment.Center,
                    HorizontalTextAlignment = TextAlignment.Center
                };
                MainGrid.RowDefinitions.Clear();
                MainGrid.Children.Clear();
                MainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                MainGrid.Children.Add(error_label, 0, 0);
                return;
            }
            #endregion

            #region 구매내역 없음
            if (productlist.Count == 0)
            {
                CustomLabel error_label = new CustomLabel
                {
                    Text = "구매 내역이 없습니다!",
                    Size = 18,
                    TextColor = Color.Black,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalTextAlignment = TextAlignment.Center,
                    HorizontalTextAlignment = TextAlignment.Center
                };
                MainGrid.RowDefinitions.Clear();
                MainGrid.Children.Clear();
                MainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                MainGrid.Children.Add(error_label, 0, 0);
                return;
            }
            #endregion.

            Grid coverGrid = new Grid { RowSpacing = 0 };
            MainGrid.Children.Add(coverGrid, 0, 0); // 메인 그리드 추가

            #region 주문번호
            CustomLabel order_numLabel = new CustomLabel
            {
                Text = "주문 번호 : " + pl_index,
                Size = 18,
                TextColor = Color.Black,
                Margin = new Thickness(15, 0, 0, 0),
                VerticalOptions = LayoutOptions.CenterAndExpand,
            };
            coverGrid.RowDefinitions.Add(new RowDefinition { Height = 40 });
            coverGrid.Children.Add(order_numLabel, 0, coverGridRow++);
            #endregion
            
            BoxView borderLine1 = new BoxView { BackgroundColor = Color.LightGray };
            coverGrid.RowDefinitions.Add(new RowDefinition { Height = 1 });
            coverGrid.Children.Add(borderLine1, 0, coverGridRow++);

            #region 상품 이름
            Grid nameGrid = new Grid
            {
                ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = 100 },
                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    },
                RowSpacing = 0,
            };
            BoxView nameLine = new BoxView { BackgroundColor = Color.LightGray };
            StackLayout nameCover = new StackLayout { BackgroundColor = Color.White, Margin = 1 };
            CustomLabel nameLabel = new CustomLabel
            {
                Text = "상품이름",
                Size = 14,
                TextColor = Color.Gray,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };
            CustomLabel input_nameLabel = new CustomLabel
            {
                Text = productlist[0].PRODUCTTYPE + " " + productlist[0].PRODUCTVALUE + " 외 " + (productlist.Count - 1) + "개",
                Size = 14,
                TextColor = Color.Gray,
                VerticalOptions = LayoutOptions.CenterAndExpand,
            };
            if(productlist.Count == 1) // 수량이 1개일 경우 외 ~개 표시 x
            {
                input_nameLabel.Text = productlist[0].PRODUCTTYPE + " " + productlist[0].PRODUCTVALUE; 
            }

            nameGrid.Children.Add(nameLine, 0, 0);
            nameGrid.Children.Add(nameCover, 0, 0);
            nameCover.Children.Add(nameLabel);
            nameGrid.Children.Add(input_nameLabel, 1, 0);

            coverGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            coverGrid.Children.Add(nameGrid, 0, coverGridRow++);
            BoxView borderLine2 = new BoxView { BackgroundColor = Color.LightGray };
            coverGrid.RowDefinitions.Add(new RowDefinition { Height = 1 });
            coverGrid.Children.Add(borderLine2, 0, coverGridRow++);
            #endregion



            /* 배송지 초기화 및 핀번호 상품번호, 발행날짜 파기날짜 초기화 */
            // 지류일 경우
            if (productlist[0].PDL_PROTYPE == "1")
            {
                #region 배송비 선불/착불
                Grid delivery_priceGrid = new Grid
                {
                    ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = 100 },
                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    },
                    RowSpacing = 0,
                };
                BoxView delivery_priceLine = new BoxView { BackgroundColor = Color.LightGray };
                delivery_priceGrid.Children.Add(delivery_priceLine, 0, 0);
                StackLayout delivery_priceCover = new StackLayout { BackgroundColor = Color.White, Margin = 1 };
                delivery_priceGrid.Children.Add(delivery_priceCover, 0, 0);
                CustomLabel delivery_priceLabel = new CustomLabel
                {
                    Text = "배송비",
                    Size = 14,
                    TextColor = Color.Gray,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                };
                delivery_priceCover.Children.Add(delivery_priceLabel);
                CustomLabel input_delivery_priceLabel = null;
                // 선불
                if (purchaselist[0].PL_DELIVERYPAY_TYPE.Equals("1"))
                {
                    input_delivery_priceLabel = new CustomLabel
                    {
                        Text = delivery_pay + "원 / 선불",
                        Size = 14,
                        TextColor = Color.Gray,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                    };
                }
                // 착불
                else
                {
                    input_delivery_priceLabel = new CustomLabel
                    {
                        Text = "3000원 / 착불",
                        Size = 14,
                        TextColor = Color.Gray,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                    };
                }
                delivery_priceGrid.Children.Add(input_delivery_priceLabel, 1, 0);


                // 구분선
                coverGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                coverGrid.Children.Add(delivery_priceGrid, 0, coverGridRow++);
                BoxView borderLine3 = new BoxView { BackgroundColor = Color.LightGray };
                coverGrid.RowDefinitions.Add(new RowDefinition { Height = 1 });
                coverGrid.Children.Add(borderLine3, 0, coverGridRow++);
                #endregion

                #region 배송지
                Grid delivery_adressGrid = new Grid
                {
                    ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = 100 },
                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    },
                    RowSpacing = 0,
                };
                BoxView delivery_adressLine = new BoxView { BackgroundColor = Color.LightGray };
                delivery_adressGrid.Children.Add(delivery_adressLine, 0, 0);
                StackLayout delivery_adressCover = new StackLayout { BackgroundColor = Color.White, Margin = 1 };
                delivery_adressGrid.Children.Add(delivery_adressCover, 0, 0);
                CustomLabel delivery_adressLabel = new CustomLabel
                {
                    Text = "배송지",
                    Size = 14,
                    TextColor = Color.Gray,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                };
                delivery_adressCover.Children.Add(delivery_adressLabel);
                CustomLabel input_delivery_adressLabel = new CustomLabel
                {
                    Text = purchaselist[0].PL_DELIVERY_ADDRESS,
                    Size = 14,
                    TextColor = Color.Gray,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                };
                delivery_adressGrid.Children.Add(input_delivery_adressLabel, 1, 0);

                // 구분선
                coverGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                coverGrid.Children.Add(delivery_adressGrid, 0, coverGridRow++);
                BoxView borderLine4 = new BoxView { BackgroundColor = Color.LightGray };
                coverGrid.RowDefinitions.Add(new RowDefinition { Height = 1 });
                coverGrid.Children.Add(borderLine4, 0, coverGridRow++);
                #endregion

                #region 배송 연락번호
                Grid delivery_phoneGrid = new Grid
                {
                    ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = 100 },
                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    },
                    RowSpacing = 0,
                };
                BoxView delivery_phoneLine = new BoxView { BackgroundColor = Color.LightGray };
                delivery_phoneGrid.Children.Add(delivery_phoneLine, 0, 0);
                StackLayout delivery_phoneCover = new StackLayout { BackgroundColor = Color.White, Margin = 1 };
                delivery_phoneGrid.Children.Add(delivery_phoneCover, 0, 0);
                CustomLabel delivery_phoneLabel = new CustomLabel
                {
                    Text = "연락처",
                    Size = 14,
                    TextColor = Color.Gray,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                };
                delivery_phoneCover.Children.Add(delivery_phoneLabel);
                CustomLabel input_delivery_phoneLabel = new CustomLabel
                {
                    Text = purchaselist[0].PL_DV_PHONE,
                    Size = 14,
                    TextColor = Color.Gray,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                };
                delivery_phoneGrid.Children.Add(input_delivery_phoneLabel, 1, 0);

                // 구분선
                coverGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                coverGrid.Children.Add(delivery_phoneGrid, 0, coverGridRow++);
                BoxView borderLine5 = new BoxView { BackgroundColor = Color.LightGray };
                coverGrid.RowDefinitions.Add(new RowDefinition { Height = 1 });
                coverGrid.Children.Add(borderLine5, 0, coverGridRow++);
                #endregion
            }
            // 핀 번호 일경우 배송지 없음. 대신 상품 번호, 발행날짜, 파기날짜 확인
            else if (productlist[0].PDL_PROTYPE == "2")
            {
                #region 핀번호 고유 번호
                Grid pin_gcnumGrid = new Grid
                {
                    ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = 100 },
                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    },
                    RowSpacing = 0,
                };
                BoxView pin_gcnumLine = new BoxView { BackgroundColor = Color.LightGray };
                pin_gcnumGrid.Children.Add(pin_gcnumLine, 0, 0);
                StackLayout pin_gcnumCover = new StackLayout { BackgroundColor = Color.White, Margin = 1 };
                pin_gcnumGrid.Children.Add(pin_gcnumCover, 0, 0);
                CustomLabel pin_gcnumLabel = new CustomLabel
                {
                    Text = "고유번호",
                    Size = 14,
                    TextColor = Color.Gray,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                };
                pin_gcnumCover.Children.Add(pin_gcnumLabel);
                CustomLabel input_pin_gcnumLabel = new CustomLabel
                {
                    Text = pinData.PIN_GC_NUM.ToString(),
                    Size = 14,
                    TextColor = Color.Gray,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                };
                pin_gcnumGrid.Children.Add(input_pin_gcnumLabel, 1, 0);


                // 구분선
                coverGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                coverGrid.Children.Add(pin_gcnumGrid, 0, coverGridRow++);
                BoxView borderLine3 = new BoxView { BackgroundColor = Color.LightGray };
                coverGrid.RowDefinitions.Add(new RowDefinition { Height = 1 });
                coverGrid.Children.Add(borderLine3, 0, coverGridRow++);
                #endregion

                #region 사용여부
                Grid pin_isusedateGrid = new Grid
                {
                    ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = 100 },
                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    },
                    RowSpacing = 0,
                };
                BoxView pin_isusedateLine = new BoxView { BackgroundColor = Color.LightGray };
                pin_isusedateGrid.Children.Add(pin_isusedateLine, 0, 0);
                StackLayout pin_isusedateCover = new StackLayout { BackgroundColor = Color.White, Margin = 1 };
                pin_isusedateGrid.Children.Add(pin_isusedateCover, 0, 0);
                CustomLabel pin_isusedateLabel = new CustomLabel
                {
                    Text = "사용여부",
                    Size = 14,
                    TextColor = Color.Gray,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                };
                pin_isusedateCover.Children.Add(pin_isusedateLabel);
                CustomLabel input_pin_isusedateLabel = new CustomLabel
                {
                    Size = 14,
                    TextColor = Color.Gray,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                };
                if(pinData.PIN_GC_ISUSED == "1") { input_pin_isusedateLabel.Text = "사용안함"; }
                else if (pinData.PIN_GC_ISUSED == "2") { input_pin_isusedateLabel.Text = "사용함"; }
                else if (pinData.PIN_GC_ISUSED == "3") { input_pin_isusedateLabel.Text = "사용대기"; }
                pin_isusedateGrid.Children.Add(input_pin_isusedateLabel, 1, 0);

                // 구분선
                coverGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                coverGrid.Children.Add(pin_isusedateGrid, 0, coverGridRow++);
                BoxView borderLine4 = new BoxView { BackgroundColor = Color.LightGray };
                coverGrid.RowDefinitions.Add(new RowDefinition { Height = 1 });
                coverGrid.Children.Add(borderLine4, 0, coverGridRow++);
                #endregion

                #region 파기날짜
                Grid pin_destructGrid = new Grid
                {
                    ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = 100 },
                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    },
                    RowSpacing = 0,
                };
                BoxView pin_destructLine = new BoxView { BackgroundColor = Color.LightGray };
                pin_destructGrid.Children.Add(pin_destructLine, 0, 0);
                StackLayout pin_destructCover = new StackLayout { BackgroundColor = Color.White, Margin = 1 };
                pin_destructGrid.Children.Add(pin_destructCover, 0, 0);
                CustomLabel pin_destructLabel = new CustomLabel
                {
                    Text = "파기날짜",
                    Size = 14,
                    TextColor = Color.Gray,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                };
                pin_destructCover.Children.Add(pin_destructLabel);
                CustomLabel input_pin_destructLabel = new CustomLabel
                {
                    Text = pinData.PIN_GC_DESTRUCTIONDATE,
                    Size = 14,
                    TextColor = Color.Gray,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                };
                pin_destructGrid.Children.Add(input_pin_destructLabel, 1, 0);

                // 구분선
                coverGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                coverGrid.Children.Add(pin_destructGrid, 0, coverGridRow++);
                BoxView borderLine5 = new BoxView { BackgroundColor = Color.LightGray };
                coverGrid.RowDefinitions.Add(new RowDefinition { Height = 1 });
                coverGrid.Children.Add(borderLine5, 0, coverGridRow++);
                #endregion

            }

            #region 결제금액
            Grid pay_priceGrid = new Grid
            {
                ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = 100 },
                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    },
                RowSpacing = 0,
            };
            BoxView pay_priceLine = new BoxView { BackgroundColor = Color.LightGray };
            pay_priceGrid.Children.Add(pay_priceLine, 0, 0);
            StackLayout pay_priceCover = new StackLayout { BackgroundColor = Color.White, Margin = 1 };
            pay_priceGrid.Children.Add(pay_priceCover, 0, 0);
            CustomLabel pay_priceLabel = new CustomLabel
            {
                Text = "결제금액",
                Size = 14,
                TextColor = Color.Gray,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };
            pay_priceCover.Children.Add(pay_priceLabel);
            CustomLabel input_pay_priceLabel = new CustomLabel
            {
                Size = 14,
                TextColor = Color.Gray,
                VerticalOptions = LayoutOptions.CenterAndExpand,
            };
            pay_priceGrid.Children.Add(input_pay_priceLabel, 1, 0);
            if (productlist[0].PDL_PROTYPE == "1") // 지류일 경우 전체 금액
            {
                input_pay_priceLabel.Text = int.Parse(purchaselist[0].PL_PAYMENT_PRICE).ToString("N0") + "원";
            }
            else if (productlist[0].PDL_PROTYPE == "2") // 핀번호일 경우 각각의 금액
            {
                input_pay_priceLabel.Text = int.Parse(product_detail.PDL_PRICE).ToString("N0") + "원";
            }

            // 구분선
            coverGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            coverGrid.Children.Add(pay_priceGrid, 0, coverGridRow++);
            BoxView borderLine11 = new BoxView { BackgroundColor = Color.LightGray };
            coverGrid.RowDefinitions.Add(new RowDefinition { Height = 1 });
            coverGrid.Children.Add(borderLine11, 0, coverGridRow++);
            #endregion


            #region 입금계좌번호 + 입금주 + 입금은행
            Grid account_num_Grid = new Grid
            {
                ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = 100 },
                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    },
                RowSpacing = 0,
            };
            BoxView account_num_Line = new BoxView { BackgroundColor = Color.LightGray };
            account_num_Grid.Children.Add(account_num_Line, 0, 0);
            StackLayout account_num_Cover = new StackLayout { BackgroundColor = Color.White, Margin = 1 };
            account_num_Grid.Children.Add(account_num_Cover, 0, 0);
            CustomLabel account_num_Label = new CustomLabel
            {
                Text = "입금계좌번호",
                Size = 14,
                TextColor = Color.Gray,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };
            account_num_Cover.Children.Add(account_num_Label);
            CustomLabel input_account_num_Label = new CustomLabel
            {   
                Size = 14,
                TextColor = Color.Gray,
                VerticalOptions = LayoutOptions.CenterAndExpand,
            };
            account_num_Grid.Children.Add(input_account_num_Label, 1, 0);

            AccountInfo accountData = new AccountInfo();
            // acnum과 일치하는 내역 검색
            for(int k = 0; k<accountlist.Count; k++)
            {
                if(accountlist[k].AC_NUM == purchaselist[0].AC_NUM)
                {
                    accountData = accountlist[k];
                    break;
                }
            }
            // 계좌번호, 은행, 예금주 초기화
            input_account_num_Label.Text = accountData.AC_ACCOUNTNUM + " [" + accountData.AC_BANKNAME + "] [" + accountData.AC_NAME + "]";

            // 구분선
            coverGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            coverGrid.Children.Add(account_num_Grid, 0, coverGridRow++);
            BoxView accountnumborderLine = new BoxView { BackgroundColor = Color.LightGray };
            coverGrid.RowDefinitions.Add(new RowDefinition { Height = 1 });
            coverGrid.Children.Add(accountnumborderLine, 0, coverGridRow++);
            #endregion

            #region 사용된 포인트
            Grid pay_pointGrid = new Grid
            {
                ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = 100 },
                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    },
                RowSpacing = 0,
            };
            BoxView pay_pointLine = new BoxView { BackgroundColor = Color.LightGray };
            pay_pointGrid.Children.Add(pay_pointLine, 0, 0);
            StackLayout pay_pointCover = new StackLayout { BackgroundColor = Color.White, Margin = 1 };
            pay_pointGrid.Children.Add(pay_pointCover, 0, 0);
            CustomLabel pay_pointLabel = new CustomLabel
            {
                Text = "사용포인트",
                Size = 14,
                TextColor = Color.Gray,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };
            pay_pointCover.Children.Add(pay_pointLabel);
            CustomLabel input_pay_pointLabel = new CustomLabel
            {
                Text = purchaselist[0].PL_USED_POINT.ToString() + " point",
                Size = 14,
                TextColor = Color.Gray,
                VerticalOptions = LayoutOptions.CenterAndExpand,
            };
            pay_pointGrid.Children.Add(input_pay_pointLabel, 1, 0);

            // 구분선
            coverGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            coverGrid.Children.Add(pay_pointGrid, 0, coverGridRow++);
            BoxView borderLine12 = new BoxView { BackgroundColor = Color.LightGray };
            coverGrid.RowDefinitions.Add(new RowDefinition { Height = 1 });
            coverGrid.Children.Add(borderLine12, 0, coverGridRow++);
            #endregion

            #region 결제상태 지류/핀번호
            Grid pay_stateGrid = new Grid
            {
                ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = 100 },
                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    },
                RowSpacing = 0,
            };
            BoxView pay_stateLine = new BoxView { BackgroundColor = Color.LightGray };
            pay_stateGrid.Children.Add(pay_stateLine, 0, 0);
            StackLayout pay_stateCover = new StackLayout { BackgroundColor = Color.White, Margin = 1 };
            pay_stateGrid.Children.Add(pay_stateCover, 0, 0);
            CustomLabel pay_stateLabel = new CustomLabel
            {
                Text = "결제상태",
                Size = 14,
                TextColor = Color.Gray,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };
            pay_stateCover.Children.Add(pay_stateLabel);

            CustomLabel input_pay_stateLabel = new CustomLabel
            {
                Size = 14,
                TextColor = Color.Gray,
                VerticalOptions = LayoutOptions.CenterAndExpand,
            };
            /* 구매 상태 초기화 */
            // 지류일 경우
            if (productlist[0].PDL_PROTYPE == "1")
            {
                string prostatestring = Global.StateToString(purchaselist[0].PL_ISSUCCESS);
                input_pay_stateLabel.Text = prostatestring;
            }
            // 핀 번호 일경우
            else if (productlist[0].PDL_PROTYPE == "2")
            {
                string prostatestring = Global.StateToString(product_detail.PDL_PIN_STATE);
                input_pay_stateLabel.Text = prostatestring;

            }
            pay_stateGrid.Children.Add(input_pay_stateLabel, 1, 0);
            coverGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            coverGrid.Children.Add(pay_stateGrid, 0, coverGridRow++);

            BoxView borderLine13 = new BoxView { BackgroundColor = Color.LightGray };
            coverGrid.RowDefinitions.Add(new RowDefinition { Height = 1 });
            coverGrid.Children.Add(borderLine13, 0, coverGridRow++);
            #endregion


            #region 운송장번호 및 핀번호 조회
            /* 운송장번호 및 핀번호 조회 초기화 */
            // 지류일 경우
            if (productlist[0].PDL_PROTYPE == "1")
            {
                Grid deliveryNumberGrid = new Grid
                {
                    ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = 100 },
                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                        new ColumnDefinition { Width = 100 },
                    },
                    RowSpacing = 0,
                };
                BoxView deliveryNumber_Line = new BoxView { BackgroundColor = Color.LightGray };
                deliveryNumberGrid.Children.Add(deliveryNumber_Line, 0, 0);
                StackLayout deliveryNumber_Cover = new StackLayout { BackgroundColor = Color.White, Margin = 1 };
                deliveryNumberGrid.Children.Add(deliveryNumber_Cover, 0, 0);
                CustomLabel deliveryNumber_Label = new CustomLabel
                {
                    Text = "운송장번호",
                    Size = 14,
                    TextColor = Color.Gray,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                };
                deliveryNumber_Cover.Children.Add(deliveryNumber_Label);
                CustomLabel input_deliveryNumber_Label = new CustomLabel // 실제 등기번호 데이터베이스에서 가져올것!
                {
                    Text = purchaselist[0].PL_PAPER_DVNUM, // 등기번호
                    Size = 14,
                    TextColor = Color.Gray,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                };
                deliveryNumberGrid.Children.Add(input_deliveryNumber_Label, 1, 0);
                CustomButton deliveryLookup_Button = new CustomButton
                {
                    Text = "배송조회",
                    Size = 14,
                    TextColor = Color.White,
                    BackgroundColor = Color.Gray,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                };
                deliveryNumberGrid.Children.Add(deliveryLookup_Button, 2, 0);
                // 운송장 번호 조회 버튼
                deliveryLookup_Button.Clicked += (sender, args) =>
                {
                    if (input_deliveryNumber_Label.Text == "") // 운송장 번호가 없을경우
                    {
                        DisplayAlert("알림", "송장번호가 존재하지 않습니다!", "확인");
                        return;
                    }
                    else
                    {
                        Navigation.PushAsync(new DeliveryLookup(input_deliveryNumber_Label.Text));
                    }
                };


                // 구분선
                coverGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                coverGrid.Children.Add(deliveryNumberGrid, 0, coverGridRow++);

                BoxView borderLine14 = new BoxView { BackgroundColor = Color.LightGray };
                coverGrid.RowDefinitions.Add(new RowDefinition { Height = 1 });
                coverGrid.Children.Add(borderLine14, 0, coverGridRow++);
            }
            // 핀 번호 일경우 핀번호 보여주기
            else if (productlist[0].PDL_PROTYPE == "2")
            {
                Grid pin_gcpinGrid = new Grid
                {
                    ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = 100 },
                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    },
                    RowSpacing = 0,
                };
                BoxView pin_gcpin_Line = new BoxView { BackgroundColor = Color.LightGray };
                pin_gcpinGrid.Children.Add(pin_gcpin_Line, 0, 0);
                StackLayout pin_gcpin_Cover = new StackLayout { BackgroundColor = Color.White, Margin = 1 };
                pin_gcpinGrid.Children.Add(pin_gcpin_Cover, 0, 0);
                CustomLabel pin_gcpin_Label = new CustomLabel
                {
                    Text = "핀번호확인",
                    Size = 14,
                    TextColor = Color.Gray,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                };
                pin_gcpin_Cover.Children.Add(pin_gcpin_Label);
                CustomLabel input_pin_gcpin_Label = new CustomLabel
                {
                    Text = "결제하기 전에는 확인할 수 없습니다.",
                    Size = 14,
                    TextColor = Color.Gray,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                };
                pin_gcpinGrid.Children.Add(input_pin_gcpin_Label, 1, 0);

                if (product_detail.PDL_PIN_STATE == "2") // 구매 완료시 핀번호 출력
                {
                    input_pin_gcpin_Label.Text = pinData.PIN_GC_PINNUM1 + "-" + pinData.PIN_GC_PINNUM2 + "-" +
                        pinData.PIN_GC_PINNUM3 + "-" + pinData.PIN_GC_PINNUM4 + "(" + pinData.PIN_GC_CERTIFINUM + ")"; // 핀번호
                }

                // 구분선
                coverGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                coverGrid.Children.Add(pin_gcpinGrid, 0, coverGridRow++);

                BoxView borderLine14 = new BoxView { BackgroundColor = Color.LightGray };
                coverGrid.RowDefinitions.Add(new RowDefinition { Height = 1 });
                coverGrid.Children.Add(borderLine14, 0, coverGridRow++);
            }
            #endregion

        }

        private void PayOptionCondition(string option)
        {

        }

        private void ConfirmBtn_Clicked(object sender, EventArgs e)
        {

            PurchaseListPage.isOpenPage = false;
            Navigation.PopAsync();
        }

        private void ImageButton_Clicked(object sender, EventArgs e) // 백버튼 이미지
        {
            PurchaseListPage.isOpenPage = false;
            Navigation.PopAsync();
        }
        protected override bool OnBackButtonPressed()
        {
            PurchaseListPage.isOpenPage = false;
            return base.OnBackButtonPressed();
        }
    }
}