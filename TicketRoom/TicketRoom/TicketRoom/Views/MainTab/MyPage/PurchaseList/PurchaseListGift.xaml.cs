using FFImageLoading.Forms;
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
using TicketRoom.Models.Gift.PurchaseList;
using TicketRoom.Models.ShopData;
using TicketRoom.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.MyPage.PurchaseList
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PurchaseListGift : ContentView
	{
        PurchaseListPage plp;
        GiftDBFunc giftDBFunc = GiftDBFunc.Instance();
        List<G_PLInfo> purchaselist = new List<G_PLInfo>();

        public PurchaseListGift(PurchaseListPage plp)
        {
            InitializeComponent();
            this.plp = plp;

            if (Global.b_user_login)
            {
                PostSearchPurchaseListToIDAsync(Global.ID, -99, 0, 0);// 사용자 아이디로 구매 목록 가져옴
            }
            else
            {
                PostSearchPurchaseListToIDAsync(Global.non_user_id, -99, 0, 0);// 사용자 아이디로 구매 목록 가져옴
            }
        }

        // 유저 아이디를 통해 상품권 구매리스트 가져오기
        public void PostSearchPurchaseListToIDAsync(string userid, int year, int mon, int day)
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
                purchaselist.Clear();
                purchaselist = giftDBFunc.SearchPurchaseListToID(userid, year, mon, day);
            }
            #endregion
        }

        public async Task Init()
        {
            MainGrid.Children.Clear();
            MainGrid.RowDefinitions.Clear();

            #region 네트워크 연결 불가
            if (purchaselist == null)
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

            #region 목록 검색 불가
            if (purchaselist.Count == 0)
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
                MainGrid.RowDefinitions.Clear();
                MainGrid.Children.Clear();
                MainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                MainGrid.Children.Add(nonpurchase_label,0,0);
                return;
            }
            #endregion

            for (int i = 0; i < purchaselist.Count; i++)
            {
                List<PLProInfo> productlist = new List<PLProInfo>();

                productlist = giftDBFunc.SearchPurchaseListToPlnum(purchaselist[i].PL_NUM.ToString());
                    
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
                    Text = "주문번호 : " + purchaselist[i].PL_NUM,
                    Size = 18,
                    TextColor = Color.White,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    Margin = new Thickness(15, 0, 0, 0),
                };
                BoxView orderBtnLine = new BoxView { BackgroundColor = Color.LightGray };
                CustomButton orderBtn = new CustomButton
                {
                    Text = "상세보기",
                    BackgroundColor = Color.DarkBlue,
                    TextColor = Color.White,
                    Size = 18,
                    Margin = 2,
                    BindingContext = purchaselist[i].PL_NUM
                };
                orderLabelGrid.Children.Add(ordernumLabel, 0, 0);
                orderLabelGrid.Children.Add(orderBtnLine, 1, 0);
                orderLabelGrid.Children.Add(orderBtn, 1, 0);

                // 상세보기 버튼 이벤트
                orderBtn.Clicked += (object sender, EventArgs e) =>
                {
                    System.Diagnostics.Debug.WriteLine("ta");
                    // 탭을 한번 클릭했다면 다시 열리지 않도록 제어
                    if (PurchaseListPage.isOpenPage == true)
                    {
                        return;
                    }
                    PurchaseListPage.isOpenPage = true;
                        
                    Navigation.PushAsync(new PurchaseDetailListGift(orderBtn.BindingContext.ToString()));
                };
                #endregion

                BoxView orderLine = new BoxView { BackgroundColor = Color.LightGray };

                Grid coverGrid = new Grid { };
                row_Grid.Children.Add(orderLabelGrid, 0, 0);
                row_Grid.Children.Add(orderLine, 0, 1);
                row_Grid.Children.Add(coverGrid, 0, 2);

                int product_row = 0;


                #region 주문 번호로 감싸는 실제 구매 내역
                for (int j = 0; j < productlist.Count; j++)
                {
                    coverGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                    coverGrid.RowDefinitions.Add(new RowDefinition { Height = 3 });
                    // 주문 번호로 감싸고 있는 실제 구매 내역 리스트
                    Grid inGrid = new Grid
                    {
                        ColumnDefinitions =
                        {
                            new ColumnDefinition { Width = 75 },
                            new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                            new ColumnDefinition { Width = 50 },
                        },
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Margin = new Thickness(5, 5, 5, 5),
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
                        Source = ImageSource.FromUri(new Uri(Global.server_ipadress + productlist[j].PRODUCTIMAGE)),
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

                    #region 상품별 구매 상태
                    string prostatestring = Global.StateToString(purchaselist[i].PL_ISSUCCESS);

                    CustomLabel prostatusLabel = new CustomLabel
                    {
                        Text = prostatestring, // 구매 상태
                        Size = 12,
                        TextColor = Color.Red,
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.End,
                        Margin = new Thickness(0, 0, 0, 0)
                    };
                    #endregion

                    inGrid.Children.Add(product_image, 0, 0);
                    inGrid.Children.Add(product_label_grid, 1, 0);
                    inGrid.Children.Add(prostatusLabel, 2, 0);

                    #region 상품 이름 Label
                    CustomLabel pro_label = new CustomLabel
                    {
                        Text = productlist[j].PRODUCTTYPE+" "+ productlist[j].PRODUCTVALUE,
                        Size = 18,
                        TextColor = Color.Black,
                    };
                    #endregion
                    #region 상품 종류 Label (개수(지류,핀번호))
                    CustomLabel type_label = null;

                    if (productlist[j].PDL_PROTYPE.Equals("1"))
                    {
                        type_label = new CustomLabel
                        {
                            Text = productlist[j].PDL_PROCOUNT + "개 (지류)",
                            Size = 14,
                            TextColor = Color.DarkGray,
                        };
                    }
                    else
                    {
                        type_label = new CustomLabel
                        {
                            Text = productlist[j].PDL_PROCOUNT + "개 (핀번호)",
                            Size = 14,
                            TextColor = Color.DarkGray,
                        };
                    }
                        
                    #endregion
                    #region 가격 내용 Label
                    CustomLabel price_label = new CustomLabel
                    {
                        Text = int.Parse(productlist[j].PDL_ALLPRICE).ToString("N0") + "원",
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
                    Text = purchaselist[i].PL_PURCHASE_DATE, // 구매 날짜
                    Size = 14,
                    TextColor = Color.Black,
                    VerticalOptions = LayoutOptions.Center,
                    Margin = new Thickness(10, 0, 0, 0)
                };

                string statestring = Global.StateToString(purchaselist[i].PL_ISSUCCESS);

                CustomLabel statusLabel = new CustomLabel
                {
                    Text = statestring, // 구매 상태
                    Size = 18,
                    TextColor = Color.Red,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.End,
                    Margin = new Thickness(0, 0, 10, 0)
                };
                    
                dateGrid.Children.Add(dateLabel, 0, 0);
                dateGrid.Children.Add(statusLabel, 1, 0);
                row_Grid.Children.Add(dateLine, 0, 3);
                row_Grid.Children.Add(dateGrid, 0, 4);
            }

            

            // 로딩 완료
            await Global.LoadingEndAsync();
        }
    }
}