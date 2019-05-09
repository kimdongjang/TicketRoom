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
using TicketRoom.Models.Gift.Purchase;
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
        public List<G_PurchaseList> purchaselist = new List<G_PurchaseList>();


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
                if(Global.b_guest_login == true)
                {

                }
                else
                {
                    PostSearchPurchaseListToIDAsync(Global.non_user_id, -99, 0, 0);// 사용자 아이디로 구매 목록 가져옴
                }
                
            }
        }

        // 유저 아이디를 통해 상품권 구매리스트 가져오기
        public void PostSearchPurchaseListToIDAsync(string userid, int year, int mon, int day)
        {
            #region 네트워크 상태 불가
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
                bool isPaperPurchase = false;
                bool isPinPurchase = false;

                List<PLProInfo> productlist = new List<PLProInfo>();
                List<G_PurchaseListDetail> detail_list = new List<G_PurchaseListDetail>();
                if(Global.b_guest_login == true)
                {
                    PLProInfo p1 = new PLProInfo
                    {
                        PDL_PRICE = "10000",
                        PDL_PROCOUNT = "1",
                        PDL_PROTYPE = "1",
                        PRODUCTIMAGE = "img/Gift/Category/culture_gift.png",
                        PRODUCTTYPE = "문화상품권",
                        PRODUCTVALUE = "1만원권",
                    };
                    productlist.Add(p1);
                    G_PurchaseListDetail d1 = new G_PurchaseListDetail
                    {
                        PDL_PROTYPE ="1",
                        PDL_PRICE = "10000",
                        PDL_NUM="1000",
                        PDL_ISAVAILABLE="1",
                        PDL_PINNUM="",
                        PDL_PAPERNUM="100",
                        PDL_PIN_STATE="1",
                        PDL_PLNUM ="1",
                        PDL_PRONUM="1",
                    };
                    detail_list.Add(d1);
                }
                else
                {
                    productlist = giftDBFunc.SearchPurchaseListToPlnum(purchaselist[i].PL_NUM.ToString()); // 구매내역 가져오기

                    detail_list = giftDBFunc.SearchGfitDetailListToPlnumForPIN(purchaselist[i].PL_NUM.ToString()); // 핀번호 관련 구매 상세 내역 가져오기
                }

                #region 전체 그리드
                MainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                BoxView row_boxview = new BoxView { BackgroundColor = Color.Blue, Opacity = 0.2, Margin = new Thickness(10), };


                Grid row_Grid = new Grid
                {
                    RowDefinitions =
                    {
                        new RowDefinition { Height = 30 }, // 주문 번호
                        new RowDefinition { Height = 3 },
                        new RowDefinition { Height = 30 }, // 지류 레이블
                        new RowDefinition { Height = 3 },
                        new RowDefinition { Height = GridLength.Auto }, // 지류 구매내역 행
                        new RowDefinition { Height = 3 },
                        new RowDefinition { Height = 30 }, // 핀 번호레이블
                        new RowDefinition { Height = 3 },
                        new RowDefinition { Height = GridLength.Auto }, // 핀번호 내역 행
                        new RowDefinition { Height = 3 },
                        new RowDefinition { Height = 30 }, // 구매날짜 결제 상태
                    },
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    RowSpacing = 0,
                    Margin = new Thickness(15),
                    BackgroundColor = Color.White,
                    BindingContext = purchaselist[i].PL_NUM,
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
                row_Grid.Children.Add(orderLabelGrid, 0, 0);

                CustomLabel ordernumLabel = new CustomLabel
                {
                    Text = "주문번호 : " + purchaselist[i].PL_NUM,
                    Size = 16,
                    TextColor = Color.White,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    Margin = new Thickness(15, 0, 0, 0),
                };
                orderLabelGrid.Children.Add(ordernumLabel, 0, 0);

                BoxView orderLine = new BoxView { BackgroundColor = Color.LightGray };
                row_Grid.Children.Add(orderLine, 0, 1);

                #endregion
                // =========================================================
                // 주문 번호 밑, 지류 라벨 및 상태보기 행  ( 2번 )
                Grid paper_area_grid = new Grid
                {
                    ColumnDefinitions =
                        {
                            new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) },
                            new ColumnDefinition { Width = new GridLength(4, GridUnitType.Star) },
                            new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) },
                        },
                    BackgroundColor = Color.CornflowerBlue,
                };
                row_Grid.Children.Add(paper_area_grid, 0, 2);

                CustomLabel paper_area_label = new CustomLabel
                {
                    Text = "지류",
                    Size = 16,
                    TextColor = Color.White,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    Margin = new Thickness(15, 0, 0, 0),
                };
                paper_area_grid.Children.Add(paper_area_label, 0, 0);

                //// 지류 관련 상세보기 버튼
                //BoxView orderBtnLine = new BoxView { BackgroundColor = Color.LightGray };
                //CustomButton orderBtn = new CustomButton
                //{
                //    Text = "주문상세",
                //    BackgroundColor = Color.White,
                //    TextColor = Color.CornflowerBlue,
                //    Size = 16,
                //    Margin = 2,
                //};
                //paper_area_grid.Children.Add(orderBtnLine, 2, 0);
                //paper_area_grid.Children.Add(orderBtn, 2, 0);

                // =========================================================

                // 지류로 감싸는 실제 구매 내역 행 ( 4번 )
                Grid paper_area_row_grid = new Grid {};
                row_Grid.Children.Add(paper_area_row_grid, 0, 4);


                // =========================================================
                // 주문 번호 밑 핀번호 내역보기 행 ( 6번 )
                Grid pin_area_grid = new Grid
                {
                    ColumnDefinitions =
                            {
                                new ColumnDefinition { Width = new GridLength(7, GridUnitType.Star) },
                                new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) },
                            },
                    BackgroundColor = Color.CornflowerBlue,
                };
                row_Grid.Children.Add(pin_area_grid, 0, 6);

                CustomLabel pin_area_label = new CustomLabel
                {
                    Text = "핀번호",
                    Size = 16,
                    TextColor = Color.White,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    Margin = new Thickness(15, 0, 0, 0),
                };
                pin_area_grid.Children.Add(pin_area_label, 0, 0);
                // =========================================================


                // 핀번호로 감싸는 실제 구매 내역 행 ( 8번)
                Grid pin_area_row_grid = new Grid { };
                row_Grid.Children.Add(pin_area_row_grid, 0, 8);
                // =========================================================


                int paper_product_row = 0; // 추가 되는 상품 카운트 로우
                int pin_product_row = 0; // 추가 되는 상품 카운트 로우

                int pin_index = 0; // pin 번호 찾기 위한 인덱스
                
                #region 주문 번호로 감싸는 실제 구매 내역
                for (int j = 0; j < productlist.Count; j++)
                {
                    // 지류일 경우 1번 행 밑에 추가
                    if (productlist[j].PDL_PROTYPE == "1" && isPaperPurchase == false)
                    {
                        isPaperPurchase = true; // 지류로 구매한 상품이 하나라도 있다면 true

                        paper_area_row_grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                        paper_area_row_grid.RowDefinitions.Add(new RowDefinition { Height = 3 });

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
                        paper_area_row_grid.Children.Add(inGrid, 0, paper_product_row);
                        paper_product_row++;

                        BoxView productLine = new BoxView { BackgroundColor = Color.LightGray }; // 구분선
                        paper_area_row_grid.Children.Add(productLine, 0, paper_product_row);
                        paper_product_row++;

                        // 상품 이미지
                        CachedImage product_image = new CachedImage
                        {
                            LoadingPlaceholder = Global.LoadingImagePath,
                            ErrorPlaceholder = Global.NotFoundImagePath,
                            Source = ImageSource.FromUri(new Uri(Global.server_ipadress + productlist[j].PRODUCTIMAGE)),
                            BackgroundColor = Color.White,
                            VerticalOptions = LayoutOptions.CenterAndExpand,
                            HorizontalOptions = LayoutOptions.CenterAndExpand,
                            Aspect = Aspect.AspectFill,
                        };
                        inGrid.Children.Add(product_image, 0, 0);

                        // 상품 상세 설명(상품이름, 옵션, 금액) 그리드
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
                        inGrid.Children.Add(product_label_grid, 1, 0);

                        #region 상품 이름 Label
                        CustomLabel pro_label = new CustomLabel
                        {
                            Text = productlist[j].PRODUCTTYPE + " " + productlist[j].PRODUCTVALUE,
                            Size = 16,
                            TextColor = Color.Black,
                        };
                        product_label_grid.Children.Add(pro_label, 0, 0);
                        #endregion

                        #region 상품 가격 + 개수(지류,핀번호) Label
                        CustomLabel type_label = null;

                        if (productlist[j].PDL_PROTYPE.Equals("1"))
                        {
                            type_label = new CustomLabel
                            {
                                Text = int.Parse(purchaselist[i].PL_PAYMENT_PRICE).ToString("N0") + "원" + " / " + purchaselist[i].PL_PAPER_COUNT + "개 (지류)",
                                Size = 14,
                                TextColor = Color.DarkGray,
                            };
                        }
                        product_label_grid.Children.Add(type_label, 0, 1);

                        #endregion
                        #region 구매 상태
                        string prostatestring = Global.StateToString(purchaselist[i].PL_PAPERSTATE);
                        CustomLabel price_label = new CustomLabel
                        {
                            Text = prostatestring,
                            Size = 14,
                            TextColor = Color.Gray,
                        };
                        product_label_grid.Children.Add(price_label, 0, 3);

                        // 구매 상태에 따라 핀번호 색상 변경
                        if (prostatestring == "구매대기") { price_label.TextColor = Color.Orange; }
                        else if (prostatestring == "구매중") { price_label.TextColor = Color.Orange; }
                        else if (prostatestring == "구매실패") { price_label.TextColor = Color.Red; }
                        else if (prostatestring == "구매완료") { price_label.TextColor = Color.Blue; }
                        #endregion


                        #region inGrid 지류 상세보기 클릭 이벤트
                        inGrid.GestureRecognizers.Add(new TapGestureRecognizer()
                        {
                            Command = new Command(() =>
                            {
                                var s = inGrid.BindingContext;

                                // 탭을 한번 클릭했다면 다시 열리지 않도록 제어
                                if (PurchaseListPage.isOpenPage == true)
                                {
                                    return;
                                }
                                PurchaseListPage.isOpenPage = true;
                                Navigation.PushAsync(new PurchaseDetailListGift(row_Grid.BindingContext.ToString()));

                            })
                        });
                        #endregion
                    }

                    // =================================================================


                    // 핀번호일 경우 3번 행 밑에 추가
                    else if (productlist[j].PDL_PROTYPE == "2")
                    {
                        isPinPurchase = true; // 핀으로 구매한 상품이 하나라도 있다면 true

                        pin_area_row_grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                        pin_area_row_grid.RowDefinitions.Add(new RowDefinition { Height = 3 });

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

                            BindingContext = detail_list[pin_index].PDL_PINNUM, // 총 주문 번호 인덱스 (ex, 333번)를 통해 각 pin번호 그리드 마다 핀번호 테이블에서 주문번호와 일치하는 상태값 조회
                        };
                        pin_area_row_grid.Children.Add(inGrid, 0, pin_product_row);
                        pin_product_row++;

                        BoxView productLine = new BoxView { BackgroundColor = Color.LightGray }; // 구분선
                        pin_area_row_grid.Children.Add(productLine, 0, pin_product_row);
                        pin_product_row++;

                        // 상품 이미지
                        CachedImage product_image = new CachedImage
                        {
                            LoadingPlaceholder = Global.LoadingImagePath,
                            ErrorPlaceholder = Global.NotFoundImagePath,
                            Source = ImageSource.FromUri(new Uri(Global.server_ipadress + productlist[j].PRODUCTIMAGE)),
                            BackgroundColor = Color.White,
                            VerticalOptions = LayoutOptions.CenterAndExpand,
                            HorizontalOptions = LayoutOptions.CenterAndExpand,
                            Aspect = Aspect.AspectFill,
                        };
                        inGrid.Children.Add(product_image, 0, 0);

                        // 상품 상세 설명(상품이름, 옵션, 금액) 그리드
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
                        inGrid.Children.Add(product_label_grid, 1, 0);


                        #region 상품 이름 Label
                        CustomLabel pro_label = new CustomLabel
                        {
                            Text = productlist[j].PRODUCTTYPE + " " + productlist[j].PRODUCTVALUE,
                            Size = 16,
                            TextColor = Color.Black,
                        };
                        product_label_grid.Children.Add(pro_label, 0, 0);
                        #endregion
                        #region 가격 내용 Label
                        CustomLabel price_label = new CustomLabel
                        {
                            Text = int.Parse(detail_list[pin_index].PDL_PRICE).ToString("N0") + "원",
                            Size = 14,
                            TextColor = Color.Gray,
                        };
                        product_label_grid.Children.Add(price_label, 0, 1);
                        #endregion


                        #region 상품별 구매 상태
                        string prostatestring = Global.StateToString(detail_list[pin_index].PDL_PIN_STATE);

                        // 핀번호의 구매 상태
                        CustomLabel prostatusLabel = new CustomLabel
                        {
                            Text = prostatestring, // 수정 필요
                            Size = 14,
                            TextColor = Color.Red,
                            Margin = new Thickness(0, 0, 0, 0)
                        };
                        // 구매 상태에 따라 핀번호 색상 변경
                        if (prostatestring == "구매대기") { prostatusLabel.TextColor = Color.Orange; }
                        else if (prostatestring == "구매중") { prostatusLabel.TextColor = Color.Orange; }
                        else if (prostatestring == "구매실패") { prostatusLabel.TextColor = Color.Red; }
                        else if (prostatestring == "구매완료") { prostatusLabel.TextColor = Color.Blue; }
                        product_label_grid.Children.Add(prostatusLabel, 0, 3);
                        #endregion

                        #region inGrid Pin번호 클릭 이벤트
                        inGrid.GestureRecognizers.Add(new TapGestureRecognizer()
                        {
                            Command = new Command(() =>
                            {
                                var s = inGrid.BindingContext;

                                if (PurchaseListPage.isOpenPage == true)
                                {
                                    return;
                                }
                                PurchaseListPage.isOpenPage = true;
                                for(int k = 0; k< detail_list.Count; k++)
                                {
                                    if (detail_list[k].PDL_PINNUM.ToString() == s.ToString())
                                    {
                                        Navigation.PushAsync(new PurchaseDetailListGift(row_Grid.BindingContext.ToString(), detail_list[k])); // 주문번호
                                        break;
                                    }
                                }

                            })
                        });
                        #endregion

                        pin_index++;
                        
                    }
                }
                #endregion

                #region 지류 , 핀 구매내역 없다는 라벨 표시
                if(isPaperPurchase == false) // 지류 구매 내역이 1개도 없을 경우 에러 라벨 표시
                {
                    paper_area_row_grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                    CustomLabel null_paper_label = new CustomLabel
                    {
                        Text = "구매한 지류 내역이 없습니다!",
                        Size = 14,
                        TextColor = Color.Black,
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Center,
                        Margin = new Thickness(0, 10, 0, 10),
                    };
                    paper_area_row_grid.Children.Add(null_paper_label, 0, 0);
                }
                else if(isPinPurchase == false) // 핀 구매 내역이 1개도 없을 경우 에러 라벨 표시
                {
                    pin_area_row_grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                    CustomLabel null_paper_label = new CustomLabel
                    {
                        Text = "구매한 핀번호 내역이 없습니다!",
                        Size = 14,
                        TextColor = Color.Black,
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Center,
                        Margin = new Thickness(0, 10, 0, 10),
                    };
                    pin_area_row_grid.Children.Add(null_paper_label, 0, 0);
                }
                #endregion

                #region 지류 관련 상세보기 버튼 이벤트
                //if (isPaperPurchase == true) // 지류 목록이 추가되었다면
                //{
                //    // 상세보기 버튼 이벤트
                //    orderBtn.Clicked += (object sender, EventArgs e) =>
                //    {
                //        System.Diagnostics.Debug.WriteLine("ta");
                //        // 탭을 한번 클릭했다면 다시 열리지 않도록 제어
                //        if (PurchaseListPage.isOpenPage == true)
                //        {
                //            return;
                //        }
                //        PurchaseListPage.isOpenPage = true;

                //        Navigation.PushAsync(new PurchaseDetailListGift(orderBtn.BindingContext.ToString()));
                //    };
                //}
                //else // 지류 목록이 추가되지 않았다면 안보이게 처리
                //{
                //    orderBtn.IsVisible = false;
                //    orderBtnLine.IsVisible = false;
                //}
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
                dateGrid.Children.Add(dateLabel, 0, 0);
                string state_main_text = Global.StateToString(purchaselist[i].PL_ISSUCCESS);
                CustomLabel main_state_label = new CustomLabel
                {
                    Text = state_main_text, // 전체 구매 상태
                    Size = 14,
                    TextColor = Color.Black,
                    VerticalOptions = LayoutOptions.Center,
                    Margin = new Thickness(10, 0, 0, 0)
                };


                // 구매 상태에 따라 핀번호 색상 변경
                if (state_main_text == "구매대기") { main_state_label.TextColor = Color.Orange; }
                else if (state_main_text == "구매중") { main_state_label.TextColor = Color.Orange; }
                else if (state_main_text == "구매실패") { main_state_label.TextColor = Color.Red; }
                else if (state_main_text == "구매완료") { main_state_label.TextColor = Color.Blue; }

                dateGrid.Children.Add(main_state_label, 1, 0);
                row_Grid.Children.Add(dateLine, 0, 9);
                row_Grid.Children.Add(dateGrid, 0, 10);
            }

            

            // 로딩 완료
            await Global.LoadingEndAsync();
        }
    }
}