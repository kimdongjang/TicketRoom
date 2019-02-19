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
                PostSearchPurchaseListToID(Global.ID, -99, 0, 0);// 사용자 아이디로 구매 목록 가져옴
            }
            else
            {
                PostSearchPurchaseListToID(Global.non_user_id, -99, 0, 0);// 사용자 아이디로 구매 목록 가져옴
            }
            
            Init();
        }

        // 유저 아이디를 통해 상품권 구매리스트 가져오기
        public void PostSearchPurchaseListToID(string userid, int year, int mon, int day)
        {
            purchaselist.Clear();
            purchaselist = giftDBFunc.SearchPurchaseListToID(userid, year, mon, day);
        }

        public void Init()
        {
            MainGrid.Children.Clear();
            MainGrid.RowDefinitions.Clear();
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
                RootGird.RowDefinitions.Clear();
                RootGird.Children.Clear();
                RootGird.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                RootGird.Children.Add(nonpurchase_label,0,0);
                return;
            }

            if (purchaselist != null)
            {
                for (int i = 0; i < purchaselist.Count; i++)
                {
                    List<PLProInfo> productlist = new List<PLProInfo>();

                    productlist = giftDBFunc.SearchPurchaseListToPlnum(purchaselist[i].PL_NUM.ToString());
                    
                    #region 전체 그리드
                    MainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                    BoxView row_boxview = new BoxView { BackgroundColor = Color.Red, Opacity = 0.2, Margin = new Thickness(10), };


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
                        BackgroundColor = Color.IndianRed,
                    };

                    CustomLabel ordernumLabel = new CustomLabel
                    {
                        Text = "주문번호 : " + purchaselist[i].PL_NUM,
                        Size = 18,
                        TextColor = Color.White,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        Margin = new Thickness(15, 0, 0, 0),
                    };
                    BoxView orderBtnLine = new BoxView { BackgroundColor = Color.Black };
                    CustomButton orderBtn = new CustomButton
                    {
                        Text = "상세보기",
                        BackgroundColor = Color.DarkRed,
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

                    BoxView orderLine = new BoxView { BackgroundColor = Color.Gray };

                    Grid coverGrid = new Grid { };
                    row_Grid.Children.Add(orderLabelGrid, 0, 0);
                    row_Grid.Children.Add(orderLine, 0, 1);
                    row_Grid.Children.Add(coverGrid, 0, 2);

                    int product_row = 0;


                    #region 주문 번호로 감싸는 실제 구매 내역
                    for (int j = 0; j < productlist.Count; j++)
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

                        Image product_image = new Image // 상품 이미지
                        {
                            Source = productlist[j].PRODUCTIMAGE,
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

                    BoxView dateLine = new BoxView { BackgroundColor = Color.Gray };

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
                    CustomLabel statusLabel = null;

                    if (purchaselist[i].PL_ISSUCCESS.Equals("1"))
                    {
                        statusLabel = new CustomLabel
                        {
                            Text = "구매완료", // 구매 상태
                            Size = 18,
                            TextColor = Color.Red,
                            VerticalOptions = LayoutOptions.Center,
                            HorizontalOptions = LayoutOptions.End,
                            Margin = new Thickness(0, 0, 10, 0)
                        };
                    }
                    else if (purchaselist[i].PL_ISSUCCESS.Equals("2"))
                    {
                        statusLabel = new CustomLabel
                        {
                            Text = "구매실패", // 구매 상태
                            Size = 18,
                            TextColor = Color.Red,
                            VerticalOptions = LayoutOptions.Center,
                            HorizontalOptions = LayoutOptions.End,
                            Margin = new Thickness(0, 0, 10, 0)
                        };
                    }
                    else if (purchaselist[i].PL_ISSUCCESS.Equals("3"))
                    {
                        statusLabel = new CustomLabel
                        {
                            Text = "구매중", // 구매 상태
                            Size = 18,
                            TextColor = Color.Red,
                            VerticalOptions = LayoutOptions.Center,
                            HorizontalOptions = LayoutOptions.End,
                            Margin = new Thickness(0, 0, 10, 0)
                        };
                    }
                    
                    dateGrid.Children.Add(dateLabel, 0, 0);
                    dateGrid.Children.Add(statusLabel, 1, 0);
                    row_Grid.Children.Add(dateLine, 0, 3);
                    row_Grid.Children.Add(dateGrid, 0, 4);
                }

            }
        }
    }
}