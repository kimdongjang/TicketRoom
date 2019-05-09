using FFImageLoading.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TicketRoom.Models.Custom;
using TicketRoom.Models.Gift.SaleList;
using TicketRoom.Services;
using TicketRoom.Views.MainTab.MyPage.SaleList;
using TicketRoom.Views.MainTab.Popup;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.MyPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SaleListPage : ContentPage
    {

        List<G_SaleInfo> salelist = new List<G_SaleInfo>();
        GiftDBFunc giftDBFunc = GiftDBFunc.Instance();
        SalePW popup_name; // 핸드폰 번호 변경 팝업 객체
        public bool check_salepw = false;
        public SaleListPage()
        {
            InitializeComponent();
            #region IOS의 경우 초기화
            NavigationPage.SetHasNavigationBar(this, false); // Navigation Bar 지우는 코드 생성자에 입력
            if (Xamarin.Forms.Device.OS == TargetPlatform.iOS)
            {
                MainGrid2.RowDefinitions[0].Height = Global.title_size_value;
            }
            #endregion

            if (Global.b_guest_login == true)
            {
                G_SaleInfo g1 = new G_SaleInfo
                {
                    SL_NUM = "1000",
                    SL_USERID = "Guest",
                    SL_SALE_DATE = "2019/05/03",
                    SL_ISSUCCES = "11",
                    SL_FAILSTRING = "",
                    SL_TOTAL_PRICE = "30000",
                    SL_ACC_NAME = "홍길동",
                    SL_ACC_NUM = "1101465983",
                    SL_SALEPRO_TYPE = "1",
                    SL_SEND_DATE = "2019/05/03",
                    SL_SENDSTRING ="이상없음",
                    SL_SALE_PW = "1",
                    SL_PRONUM = "1",
                    SL_PROCOUNT= "1",
                    SL_BANK_NAME = "신한",
                    PRODUCTTYPE = "문화 상품권",
                    PRODUCTIMAGE = "img/Gift/Category/culture_gift.png",
                    PRODUCTVALUE = "3만원 권",
                };
                G_SaleInfo g2 = new G_SaleInfo
                {
                    SL_NUM = "1001",
                    SL_USERID = "Guest",
                    SL_SALE_DATE = "2019/05/03",
                    SL_ISSUCCES = "11",
                    SL_FAILSTRING = "",
                    SL_TOTAL_PRICE = "10000",
                    SL_ACC_NAME = "김길동",
                    SL_ACC_NUM = "1101465983",
                    SL_SALEPRO_TYPE = "1",
                    SL_SEND_DATE = "2019/05/03",
                    SL_SENDSTRING = "이상없음",
                    SL_SALE_PW = "1",
                    SL_PRONUM = "1",
                    SL_PROCOUNT = "1",
                    SL_BANK_NAME = "신한",
                    PRODUCTTYPE = "문화 상품권",
                    PRODUCTIMAGE = "img/Gift/Category/culture_gift.png",
                    PRODUCTVALUE = "1만원 권",
                };
                G_SaleInfo g3 = new G_SaleInfo
                {
                    SL_NUM = "1002",
                    SL_USERID = "Guest",
                    SL_SALE_DATE = "2019/05/03",
                    SL_ISSUCCES = "11",
                    SL_FAILSTRING = "",
                    SL_TOTAL_PRICE = "10000",
                    SL_ACC_NAME = "김길동",
                    SL_ACC_NUM = "1101465983",
                    SL_SALEPRO_TYPE = "1",
                    SL_SEND_DATE = "2019/05/03",
                    SL_SENDSTRING = "모서리 부분에 살짝 접힌 흔적",
                    SL_SALE_PW = "1",
                    SL_PRONUM = "1",
                    SL_PROCOUNT = "1",
                    SL_BANK_NAME = "신한",
                    PRODUCTTYPE = "문화 상품권",
                    PRODUCTIMAGE = "img/Gift/Category/culture_gift.png",
                    PRODUCTVALUE = "1만원 권",
                };
                salelist.Add(g1);
                salelist.Add(g2);
                salelist.Add(g3);
            }
            else
            {
                if (Global.b_user_login)
                {
                    PostSearchSaleListToID(Global.ID, -99, 0, 0);// 사용자 아이디로 구매 목록 가져옴
                }
                else
                {
                    DisplayAlert("알림", "로그인이후에 이용해주세요", "OK");
                    Navigation.PopAsync();
                }
            }

            LoadingInit();
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
            Global.isbackbutton_clicked = true;
        }

        private async void LoadingInit()
        {
            // 로딩 시작
            await Global.LoadingStartAsync();

            ListUpdate();
            if(Global.b_guest_login == false)
            {
                Init();
            }           

            // 로딩 완료
            await Global.LoadingEndAsync();

        }

        // 유저 아이디를 통해 상품권 구매리스트 가져오기
        public void PostSearchSaleListToID(string userid, int year, int mon, int day)
        {
            #region 네트워크 상태 확인
            var current_network = Connectivity.NetworkAccess; // 현재 네트워크 상태
            if (current_network != NetworkAccess.Internet) // 네트워크 연결 불가
            {
                DisplayAlert("알림", "네트워크에 연결할 수 없습니다. 다시 한번 시도해주세요.", "확인");
                salelist = null;
                return;
            }
            #endregion
            #region 네트워크 연결 가능
            else
            {
                salelist.Clear();
                salelist = giftDBFunc.SearchSaleListToID(userid, year, mon, day);
            }
            #endregion
        }

        private async void Init()
        {
            #region 전체 목록 보기 클릭 이벤트
            ListAllGrid.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(async () =>
                {
                    // 로딩 시작
                    await Global.LoadingStartAsync();

                    PostSearchSaleListToID(Global.ID, -99, 0, 0);// 사용자 아이디로 구매 목록 가져옴
                    ListUpdate();

                    ((CustomLabel)ListAllGrid.Children[0]).TextColor = Color.CornflowerBlue;
                    ((BoxView)ListAllGrid.Children[1]).BackgroundColor = Color.CornflowerBlue;
                    ((CustomLabel)ListYearGrid.Children[0]).TextColor = Color.Black;
                    ((BoxView)ListYearGrid.Children[1]).BackgroundColor = Color.White;
                    ((CustomLabel)ListMonthGrid.Children[0]).TextColor = Color.Black;
                    ((BoxView)ListMonthGrid.Children[1]).BackgroundColor = Color.White;
                    ((CustomLabel)ListDayGrid.Children[0]).TextColor = Color.Black;
                    ((BoxView)ListDayGrid.Children[1]).BackgroundColor = Color.White;
                    // 로딩 시작
                    await Global.LoadingEndAsync();
                })
            });
            #endregion

            #region 일주일 목록 보기 클릭 이벤트
            ListYearGrid.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(async () =>
                {
                    // 로딩 시작
                    await Global.LoadingStartAsync();

                    PostSearchSaleListToID(Global.ID, -1, 0, 0);// 사용자 아이디로 구매 목록 가져옴
                    ListUpdate();

                    ((CustomLabel)ListAllGrid.Children[0]).TextColor = Color.Black;
                    ((BoxView)ListAllGrid.Children[1]).BackgroundColor = Color.White;
                    ((CustomLabel)ListYearGrid.Children[0]).TextColor = Color.CornflowerBlue;
                    ((BoxView)ListYearGrid.Children[1]).BackgroundColor = Color.CornflowerBlue;
                    ((CustomLabel)ListMonthGrid.Children[0]).TextColor = Color.Black;
                    ((BoxView)ListMonthGrid.Children[1]).BackgroundColor = Color.White;
                    ((CustomLabel)ListDayGrid.Children[0]).TextColor = Color.Black;
                    ((BoxView)ListDayGrid.Children[1]).BackgroundColor = Color.White;

                    // 로딩 시작
                    await Global.LoadingEndAsync();
                })
            });
            #endregion

            #region 달 목록 보기 클릭 이벤트
            ListMonthGrid.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(async () =>
                {
                    // 로딩 시작
                    await Global.LoadingStartAsync();

                    PostSearchSaleListToID(Global.ID, 0, -1, 0);// 사용자 아이디로 구매 목록 가져옴
                    ListUpdate();

                    ((CustomLabel)ListAllGrid.Children[0]).TextColor = Color.Black;
                    ((BoxView)ListAllGrid.Children[1]).BackgroundColor = Color.White;
                    ((CustomLabel)ListYearGrid.Children[0]).TextColor = Color.Black;
                    ((BoxView)ListYearGrid.Children[1]).BackgroundColor = Color.White;
                    ((CustomLabel)ListMonthGrid.Children[0]).TextColor = Color.CornflowerBlue;
                    ((BoxView)ListMonthGrid.Children[1]).BackgroundColor = Color.CornflowerBlue;
                    ((CustomLabel)ListDayGrid.Children[0]).TextColor = Color.Black;
                    ((BoxView)ListDayGrid.Children[1]).BackgroundColor = Color.White;

                    // 로딩 시작
                    await Global.LoadingEndAsync();
                })
            });
            #endregion

            #region 년 목록 보기 클릭 이벤트
            ListDayGrid.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(async () =>
                {
                    // 로딩 시작
                    await Global.LoadingStartAsync();


                    PostSearchSaleListToID(Global.ID, 0, 0, -7);// 사용자 아이디로 구매 목록 가져옴
                    ListUpdate();

                    ((CustomLabel)ListAllGrid.Children[0]).TextColor = Color.Black;
                    ((BoxView)ListAllGrid.Children[1]).BackgroundColor = Color.White;
                    ((CustomLabel)ListYearGrid.Children[0]).TextColor = Color.Black;
                    ((BoxView)ListYearGrid.Children[1]).BackgroundColor = Color.White;
                    ((CustomLabel)ListMonthGrid.Children[0]).TextColor = Color.Black;
                    ((BoxView)ListMonthGrid.Children[1]).BackgroundColor = Color.White;
                    ((CustomLabel)ListDayGrid.Children[0]).TextColor = Color.CornflowerBlue;
                    ((BoxView)ListDayGrid.Children[1]).BackgroundColor = Color.CornflowerBlue;

                    // 로딩 시작
                    await Global.LoadingEndAsync();
                })
            });
            #endregion
        }
        private void ListUpdate() { 
            MainGrid.Children.Clear();
            MainGrid.RowDefinitions.Clear();

            #region 네트워크 연결 불가
            if (salelist == null)
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
                RootGird.RowDefinitions.Clear();
                RootGird.Children.Clear();
                RootGird.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                RootGird.Children.Add(error_label, 0, 0);
                return;
            }
            #endregion

            #region 목록 검색 불가
            if (salelist.Count == 0)
            {
                CustomLabel nonpurchase_label = new CustomLabel
                {
                    Text = "구매내역이 없습니다",
                    Size = 18,
                    TextColor = Color.Black,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalTextAlignment = TextAlignment.Center,
                    HorizontalTextAlignment = TextAlignment.Center
                };
                RootGird.RowDefinitions.Clear();
                RootGird.Children.Clear();
                RootGird.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                RootGird.Children.Add(nonpurchase_label, 0, 0);
                return;
            }
            #endregion

            for (int i = 0; i < salelist.Count; i++)
            {
                #region 전체 그리드
                MainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                BoxView row_boxview = new BoxView { BackgroundColor = Color.Blue, Opacity = 0.2, Margin = new Thickness(10), };


                Grid row_Grid = new Grid
                {
                    RowDefinitions =
                    {
                        new RowDefinition { Height = 30 }, // 주문 번호
                        new RowDefinition { Height = 3 },
                        new RowDefinition { Height = GridLength.Auto }, // 구매내역 행
                        new RowDefinition { Height = 3 },
                        new RowDefinition { Height = 30 }, // 구매날짜 결제 상태
                    },
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    RowSpacing = 0,
                    Margin = new Thickness(15),
                    BackgroundColor = Color.White,
                };
                // 그리드를 감싸는 구분선 정의 및 구매내역 그리드 정의
                MainGrid.Children.Add(row_boxview, 0, i);
                MainGrid.Children.Add(row_Grid, 0, i);
                #endregion

                #region 주문 번호 Label
                Grid orderLabelGrid = new Grid
                {
                    ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = new GridLength(7, GridUnitType.Star) },
                        new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) },
                    },
                    BackgroundColor = Color.CornflowerBlue,
                };

                CustomLabel ordernumLabel = new CustomLabel
                {
                    Text = "주문번호 : " + salelist[i].SL_NUM,
                    Size = 18,
                    TextColor = Color.White,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    Margin = new Thickness(15, 0, 0, 0),
                };
                BoxView orderBtnLine = new BoxView { BackgroundColor = Color.Black };
                CustomButton orderBtn = new CustomButton
                {
                    Text = "상세보기",
                    BackgroundColor = Color.DarkBlue,
                    TextColor = Color.White,
                    Size = 18,
                    Margin = 2,
                    BindingContext = i
                };
                orderLabelGrid.Children.Add(ordernumLabel, 0, 0);
                orderLabelGrid.Children.Add(orderBtnLine, 1, 0);
                orderLabelGrid.Children.Add(orderBtn, 1, 0);
                if(Global.b_guest_login == true)
                {
                    orderBtn.IsVisible = false;
                    orderBtnLine.IsVisible = false;
                }

                // 상세보기 버튼 이벤트
                orderBtn.Clicked += (object sender, EventArgs e) =>
                {
                    if (salelist[int.Parse(orderBtn.BindingContext.ToString())].SL_SALEPRO_TYPE.Equals("2"))
                    {
                        PopupNavigation.PushAsync(popup_name = new SalePW(this, salelist[int.Parse(orderBtn.BindingContext.ToString())].SL_NUM));
                    }
                    else
                    {
                        Navigation.PushAsync(new SaleDetailListGift(salelist[int.Parse(orderBtn.BindingContext.ToString())].SL_NUM));
                    }
                        
                };
                #endregion

                BoxView orderLine = new BoxView { BackgroundColor = Color.LightGray };

                Grid coverGrid = new Grid { };
                row_Grid.Children.Add(orderLabelGrid, 0, 0);
                row_Grid.Children.Add(orderLine, 0, 1);
                row_Grid.Children.Add(coverGrid, 0, 2);

                int product_row = 0;


                #region 주문 번호로 감싸는 실제 구매 내역
                for (int j = 0; j < 1; j++)
                {
                    coverGrid.RowDefinitions.Add(new RowDefinition { Height = 75 });
                    coverGrid.RowDefinitions.Add(new RowDefinition { Height = 3 });
                    // 주문 번호로 감싸고 있는 실제 구매 내역 리스트
                    Grid inGrid = new Grid
                    {
                        ColumnDefinitions =
                        {
                            new ColumnDefinition { Width = 100 },
                            new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                        },
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Margin = new Thickness(20, 5, 20, 5),
                        RowSpacing = 0,
                        ColumnSpacing = 0,
                    };

                    BoxView productLine = new BoxView { BackgroundColor = Color.LightGray }; // 구분선

                    coverGrid.Children.Add(inGrid, 0, product_row);
                    product_row++;
                    coverGrid.Children.Add(productLine, 0, product_row);
                    product_row++;

                    CachedImage product_image = new CachedImage  // 상품 이미지
                    {
                        LoadingPlaceholder = Global.LoadingImagePath,
                        ErrorPlaceholder = Global.NotFoundImagePath,
                        Source = ImageSource.FromUri(new Uri(Global.server_ipadress + salelist[i].PRODUCTIMAGE)),
                        BackgroundColor = Color.White,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        Aspect = Aspect.AspectFill,
                    };

                    Grid product_label_grid = new Grid // 상품 상세 설명(상품이름, 옵션, 금액)
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
                    inGrid.Children.Add(product_image, 0, 0);
                    inGrid.Children.Add(product_label_grid, 1, 0);

                    #region 상품 이름 Label
                    CustomLabel pro_label = new CustomLabel
                    {
                        Text = salelist[i].PRODUCTTYPE + " " + salelist[i].PRODUCTVALUE,
                        Size = 18,
                        TextColor = Color.Black,
                    };
                    #endregion
                    #region 상품 종류 Label (개수(지류,핀번호))
                    CustomLabel type_label = null;

                    if (salelist[i].SL_SALEPRO_TYPE.Equals("1"))
                    {
                        type_label = new CustomLabel
                        {
                            Text = salelist[i].SL_PROCOUNT + "개 (지류)",
                            Size = 14,
                            TextColor = Color.DarkGray,
                        };
                    }
                    else
                    {
                        type_label = new CustomLabel
                        {
                            Text = salelist[i].SL_PROCOUNT + "개 (핀번호)",
                            Size = 14,
                            TextColor = Color.DarkGray,
                        };
                    }

                    #endregion
                    #region 가격 내용 Label
                    CustomLabel price_label = new CustomLabel
                    {
                        Text = int.Parse(salelist[i].SL_TOTAL_PRICE).ToString("N0") + "원",
                        Size = 14,
                        TextColor = Color.Gray,
                    };
                    #endregion
                    product_label_grid.Children.Add(pro_label, 0, 0);
                    product_label_grid.Children.Add(type_label, 0, 1);
                    product_label_grid.Children.Add(price_label, 0, 3);
                }
                #endregion

                BoxView dateLine = new BoxView { BackgroundColor = Color.LightGray };

                Grid dateGrid = new Grid
                {
                    ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = new GridLength(7, GridUnitType.Star) },
                        new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) },
                    },
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.Center,

                    RowSpacing = 0,
                    ColumnSpacing = 0,
                };
                CustomLabel dateLabel = new CustomLabel
                {
                    Text = salelist[i].SL_SALE_DATE, // 구매 날짜
                    Size = 14,
                    TextColor = Color.Black,
                    VerticalOptions = LayoutOptions.Center,
                    Margin = new Thickness(10, 0, 0, 0)
                };

                string statestring = Global.StateToString(salelist[i].SL_ISSUCCES);

                CustomLabel statusLabel = new CustomLabel
                {
                    Text = statestring, // 구매 상태
                    Size = 18,
                    TextColor = Color.Blue,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.End,
                    Margin = new Thickness(0, 0, 10, 0)
                };

                dateGrid.Children.Add(dateLabel, 0, 0);
                dateGrid.Children.Add(statusLabel, 1, 0);
                row_Grid.Children.Add(dateLine, 0, 3);
                row_Grid.Children.Add(dateGrid, 0, 4);
            }
            
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            if (Global.isbackbutton_clicked)
            {
                Global.isbackbutton_clicked = false;
                Global.ismypagebtns_clicked = true;
                Navigation.PopAsync();
            }
        }

        protected override bool OnBackButtonPressed()
        {
            Global.ismypagebtns_clicked = true;
            return base.OnBackButtonPressed();
        }
    }
}