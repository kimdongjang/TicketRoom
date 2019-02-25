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
            TabListColorChange(0);
            #region IOS의 경우 초기화
            NavigationPage.SetHasNavigationBar(this, false); // Navigation Bar 지우는 코드 생성자에 입력
            if (Xamarin.Forms.Device.OS == TargetPlatform.iOS)
            {
                MainGrid2.RowDefinitions[0].Height = 50;
            }
            #endregion

            if (Global.b_user_login)
            {
                PostSearchSaleListToID(Global.ID, -99, 0, 0);// 사용자 아이디로 구매 목록 가져옴
            }
            else
            {
                DisplayAlert("알림", "로그인이후에 이용해주세요", "OK");
                Navigation.PopAsync();
            }

            Init();
        }

        // 유저 아이디를 통해 상품권 구매리스트 가져오기
        public void PostSearchSaleListToID(string userid, int year, int mon, int day)
        {
            salelist.Clear();
            salelist = giftDBFunc.SearchSaleListToID(userid, year, mon, day);
        }

        private void Init()
        {
            MainGrid.Children.Clear();
            MainGrid.RowDefinitions.Clear();
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

            if (salelist != null)
            {
                for (int i = 0; i < salelist.Count; i++)
                {
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
                        BackgroundColor = Color.DarkRed,
                        TextColor = Color.White,
                        Size = 18,
                        Margin = 2,
                        BindingContext = i
                    };
                    orderLabelGrid.Children.Add(ordernumLabel, 0, 0);
                    orderLabelGrid.Children.Add(orderBtnLine, 1, 0);
                    orderLabelGrid.Children.Add(orderBtn, 1, 0);

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

                    BoxView orderLine = new BoxView { BackgroundColor = Color.Gray };

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

                        Image product_image = new Image // 상품 이미지
                        {
                            Source = salelist[i].PRODUCTIMAGE,
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
                        Text = salelist[i].SL_SALE_DATE, // 구매 날짜
                        Size = 14,
                        TextColor = Color.Black,
                        VerticalOptions = LayoutOptions.Center,
                        Margin = new Thickness(10, 0, 0, 0)
                    };
                    CustomLabel statusLabel = null;

                    if (salelist[i].SL_ISSUCCES.Equals("1"))
                    {
                        statusLabel = new CustomLabel
                        {
                            Text = "판매완료", // 구매 상태
                            Size = 18,
                            TextColor = Color.Blue,
                            VerticalOptions = LayoutOptions.Center,
                            HorizontalOptions = LayoutOptions.End,
                            Margin = new Thickness(0, 0, 10, 0)
                        };
                    }
                    else if (salelist[i].SL_ISSUCCES.Equals("2"))
                    {
                        statusLabel = new CustomLabel
                        {
                            Text = "판매실패", // 구매 상태
                            Size = 18,
                            TextColor = Color.Blue,
                            VerticalOptions = LayoutOptions.Center,
                            HorizontalOptions = LayoutOptions.End,
                            Margin = new Thickness(0, 0, 10, 0)
                        };
                    }
                    else if (salelist[i].SL_ISSUCCES.Equals("3"))
                    {
                        statusLabel = new CustomLabel
                        {
                            Text = "판매중", // 구매 상태
                            Size = 18,
                            TextColor = Color.Blue,
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

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void allbtn_clicked(object sender, EventArgs e)
        {
            TabListColorChange(0);
            ((Image)ImageGrid.Children[0]).Source = "list_all_h.png";
            ((Image)ImageGrid.Children[1]).Source = "list_week_non.png";
            ((Image)ImageGrid.Children[2]).Source = "list_month_non.png";
            ((Image)ImageGrid.Children[3]).Source = "list_year_non.png";
            PostSearchSaleListToID(Global.ID, -99, 0, 0);// 사용자 아이디로 구매 목록 가져옴
            Init();
        }

        private void weekbtn_clicked(object sender, EventArgs e)
        {
            TabListColorChange(1);
            ((Image)ImageGrid.Children[0]).Source = "list_all_non.png";
            ((Image)ImageGrid.Children[1]).Source = "list_week_h.png";
            ((Image)ImageGrid.Children[2]).Source = "list_month_non.png";
            ((Image)ImageGrid.Children[3]).Source = "list_year_non.png";
            PostSearchSaleListToID(Global.ID, 0, 0, -7);// 사용자 아이디로 구매 목록 가져옴
            Init();
        }

        private void monthbtn_clicked(object sender, EventArgs e)
        {
            TabListColorChange(2);
            ((Image)ImageGrid.Children[0]).Source = "list_all_non.png";
            ((Image)ImageGrid.Children[1]).Source = "list_week_non.png";
            ((Image)ImageGrid.Children[2]).Source = "list_month_h.png";
            ((Image)ImageGrid.Children[3]).Source = "list_year_non.png";
            PostSearchSaleListToID(Global.ID, 0, -1, 0);// 사용자 아이디로 구매 목록 가져옴
            Init();
        }

        private void yearbtn_clicked(object sender, EventArgs e)
        {
            TabListColorChange(3);
            ((Image)ImageGrid.Children[0]).Source = "list_all_non.png";
            ((Image)ImageGrid.Children[1]).Source = "list_week_non.png";
            ((Image)ImageGrid.Children[2]).Source = "list_month_non.png";
            ((Image)ImageGrid.Children[3]).Source = "list_year_h.png";
            PostSearchSaleListToID(Global.ID, -1, 0, 0);// 사용자 아이디로 구매 목록 가져옴
            Init();
        }

        // 이미지 클릭시 색상 변경
        private void TabListColorChange(int n)
        {
            for (int i = 0; i < ImageGrid.Children.Count; i++)
            {
                if (i == n)
                {
                    ImageGrid.Children[i].BackgroundColor = Color.White;
                }
                else
                {
                    ImageGrid.Children[i].BackgroundColor = Color.CornflowerBlue;
                }
            }
        }
    }
}