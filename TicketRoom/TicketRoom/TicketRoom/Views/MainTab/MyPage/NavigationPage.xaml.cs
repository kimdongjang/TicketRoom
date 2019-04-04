﻿using FFImageLoading.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketRoom.Models.Custom;
using TicketRoom.Models.Gift.Purchase;
using TicketRoom.Models.Gift.PurchaseList;
using TicketRoom.Models.PointData;
using TicketRoom.Models.Users;
using TicketRoom.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.MyPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NavagationPage : ContentPage
    {
        GiftDBFunc giftDBFunc = GiftDBFunc.Instance();
        UserDBFunc UserDB = UserDBFunc.Instance();
        List<G_PLInfo> purchaselist = new List<G_PLInfo>();

        public NavagationPage()
        {
            InitializeComponent();
            #region IOS의 경우 초기화
            NavigationPage.SetHasNavigationBar(this, false); // Navigation Bar 지우는 코드 생성자에 입력
            if (Device.OS == TargetPlatform.iOS)
            {
                TabGrid.RowDefinitions[0].Height = 50;
            }
            if (Global.ios_x_model == true) // ios X 이상의 모델일 경우
            {
                TabGrid.RowDefinitions[3].Height = 30;
            }
            #endregion
            Init();
        }
        private void Init()
        {
            Global.isbackbutton_clicked = true;
            UserInfoInit();
            if (Global.b_user_login)
            {
                PostSearchPurchaseListToIDAsync(Global.ID, -99, 0, 0);// 사용자 아이디로 구매 목록 가져옴
            }
            else
            {
                PostSearchPurchaseListToIDAsync(Global.non_user_id, -99, 0, 0);// 비 회원 아이디로 구매 목록 가져옴
            }
            PurchaseListInit();
        }
        private void UserInfoInit()
        {
            #region 네트워크 상태 확인
            var current_network = Connectivity.NetworkAccess; // 현재 네트워크 상태
            if (current_network == NetworkAccess.Internet) // 네트워크 연결 가능
            {
                if (Global.b_user_login == true)
                {
                    UserIDLabel.Text = Global.ID + " 님! 티켓룸에 어서오세요!";
                    UserPhoneLabel.Text = Global.user.PHONENUM;
                    UserPointLabel.Text = PointDBFunc.Instance().PostSearchPointListToID(Global.ID).PT_POINT_HAVEPOINT.ToString("N0");

                    #endregion
                }
                else if (Global.b_user_login == false)
                {
                    UserIDLabel.Text = "티켓룸아이디#" + Global.non_user_id + " 님! 티켓룸에 어서오세요!";
                    UserPhoneLabel.Text = "";
                    UserPointLabel.Text = "";
                }
            }
            #region 네트워크 연결 불가
            else
            {
                Global.b_user_login = false;
                UserPointLabel.Text = "";
                UserPhoneLabel.Text = "";
            }
            #endregion
        }


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


        // 구매 내역 간략하게 확인
        private void PurchaseListInit()
        {
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
                PurchaseListGrid.RowDefinitions.Clear();
                PurchaseListGrid.Children.Clear();
                PurchaseListGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                PurchaseListGrid.Children.Add(error_label, 0, 0);
                return;
            }
            #endregion

            #region 목록 검색 불가
            if (purchaselist.Count == 0)
            {
                CustomLabel nonpurchase_label = new CustomLabel
                {
                    Text = "구매내역이 없습니다!",
                    Size = 18,
                    TextColor = Color.Black,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalTextAlignment = TextAlignment.Center,
                    HorizontalTextAlignment = TextAlignment.Center
                };
                PurchaseListGrid.RowDefinitions.Clear();
                PurchaseListGrid.Children.Clear();
                PurchaseListGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                PurchaseListGrid.Children.Add(nonpurchase_label, 0, 0);
                return;
            }
            #endregion

            int success_count = 0; // 입금해야할 내역 수량 카운트
            for (int i = 0; i < purchaselist.Count; i++)
            {
                
                List<PLProInfo> productlist = giftDBFunc.SearchPurchaseListToPlnum(purchaselist[i].PL_NUM.ToString()); // 상품 리스트
                List<G_PLInfo> account_List= giftDBFunc.SearchPurchaseDetailToPlNum(purchaselist[i].PL_NUM.ToString()); // 결제 관련 리스트


                if (int.Parse(account_List[0].PL_ISSUCCESS) == 1) // 입금 대기중인 상태의 결제 내역만 보여줌
                {
                    success_count++;
                    #region 전체 그리드
                    PurchaseListGrid.RowDefinitions.Add(new RowDefinition { Height = 120 });

                    // 구분선 추가
                    BoxView row_boxview = new BoxView { BackgroundColor = Color.CornflowerBlue, Opacity = 0.5};
                    PurchaseListGrid.Children.Add(row_boxview, 0, i);


                    Grid inGrid = new Grid
                    {
                        BackgroundColor = Color.White,
                        Margin = new Thickness(2),
                        ColumnDefinitions =
                        {
                            new ColumnDefinition { Width = 80 },
                            new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                        },
                        VerticalOptions = LayoutOptions.Center,
                    };
                    PurchaseListGrid.Children.Add(inGrid, 0, i);


                    #region 상품 이미지
                    CachedImage product_image = new CachedImage
                    {
                        LoadingPlaceholder = Global.LoadingImagePath,
                        ErrorPlaceholder = Global.NotFoundImagePath,
                        Source = productlist[0].PRODUCTIMAGE,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        Aspect = Aspect.AspectFit,
                        Margin = new Thickness(15,0,0,0),
                    };

                    inGrid.Children.Add(product_image, 0, 0);
                    #endregion


                    #region 상품 설명 Labellist 그리드
                    Grid product_label_grid = new Grid
                    {
                        RowDefinitions =
                        {
                            new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }, // 주문 번호
                            new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }, // 상품 이름
                            new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }, // 입금 금액
                            new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }, // 결제 은행 , 결제 계좌
                            new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }, // 입금명
                        },
                        RowSpacing = 0,
                        VerticalOptions = LayoutOptions.Center,
                    };
                    inGrid.Children.Add(product_label_grid, 1, 0);
                    #endregion


                    #region 주문 번호 Label
                    Grid num_Grid = new Grid
                    {
                        ColumnDefinitions =
                        {
                            new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) },
                            new ColumnDefinition { Width = new GridLength(7, GridUnitType.Star) },
                        },
                        VerticalOptions = LayoutOptions.Center,
                    };
                    CustomLabel num_card = new CustomLabel
                    {
                        Text = "주문번호",
                        Size = 12,
                        BackgroundColor = Color.CornflowerBlue,
                        TextColor = Color.White,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center,
                        HeightRequest = 20,
                        WidthRequest = 60,
                        HorizontalTextAlignment = TextAlignment.Center,
                        VerticalTextAlignment = TextAlignment.Center,
                    };
                    CustomLabel num_label = new CustomLabel
                    {
                        Text = purchaselist[i].PL_NUM.ToString(),
                        Size = 12,
                        TextColor = Color.Black,
                        VerticalOptions = LayoutOptions.Center,
                    };
                    num_Grid.Children.Add(num_card, 0, 0);
                    num_Grid.Children.Add(num_label, 1, 0);
                    product_label_grid.Children.Add(num_Grid, 0, 0);
                    #endregion

                    #region 상품 이름 Label
                    Grid name_Grid = new Grid
                    {
                        ColumnDefinitions =
                        {
                            new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) },
                            new ColumnDefinition { Width = new GridLength(7, GridUnitType.Star) },
                        },
                        VerticalOptions = LayoutOptions.Center,
                    };
                    CustomLabel name_card = new CustomLabel
                    {
                        Text = "상품명",
                        Size = 12,
                        BackgroundColor = Color.CornflowerBlue,
                        TextColor = Color.White,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center,
                        HeightRequest = 20,
                        WidthRequest = 60,
                        HorizontalTextAlignment = TextAlignment.Center,
                        VerticalTextAlignment = TextAlignment.Center,
                    };

                    CustomLabel name_label = new CustomLabel
                    {
                        Text = productlist[0].PRODUCTTYPE + " " + productlist[0].PRODUCTVALUE,
                        Size = 12,
                        TextColor = Color.Black,
                        VerticalOptions = LayoutOptions.Center,
                    };
                    name_Grid.Children.Add(name_card, 0, 0);
                    name_Grid.Children.Add(name_label, 1, 0);
                    product_label_grid.Children.Add(name_Grid, 0, 1);
                    #endregion

                    #region 입금 해야할 금액 Label
                    Grid price_Grid = new Grid
                    {
                        ColumnDefinitions =
                        {
                            new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) },
                            new ColumnDefinition { Width = new GridLength(7, GridUnitType.Star) },
                        },
                        VerticalOptions = LayoutOptions.Center,
                    };
                    CustomLabel price_card = new CustomLabel
                    {
                        Text = "입금금액",
                        Size = 12,
                        BackgroundColor = Color.CornflowerBlue,
                        TextColor = Color.White,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center,
                        HeightRequest = 20,
                        WidthRequest = 60,
                        HorizontalTextAlignment = TextAlignment.Center,
                        VerticalTextAlignment = TextAlignment.Center,
                    };

                    CustomLabel price_label = new CustomLabel
                    {
                        Text = int.Parse(account_List[0].PL_PAYMENT_PRICE).ToString("N0") + "원",
                        Size = 12,
                        TextColor = Color.Black,
                        VerticalOptions = LayoutOptions.Center,
                        VerticalTextAlignment = TextAlignment.Center,
                    };
                    price_Grid.Children.Add(price_card, 0, 0);
                    price_Grid.Children.Add(price_label, 1, 0);
                    product_label_grid.Children.Add(price_Grid, 0, 2);
                    #endregion

                    List<AccountInfo> AccountValue = UserDB.GetSelectAllAccount();
                    for (int k = 0; k < AccountValue.Count; k++)
                    {
                        if(AccountValue[k].AC_NUM == account_List[0].AC_NUM)
                        {
                            #region 결제 은행, 계좌 Label
                            Grid account_Grid = new Grid
                            {
                                ColumnDefinitions =
                                {
                                    new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) },
                                    new ColumnDefinition { Width = new GridLength(7, GridUnitType.Star) },
                                },
                                VerticalOptions = LayoutOptions.Center,
                            };
                            CustomLabel account_card = new CustomLabel
                            {
                                Text = "입금계좌",
                                Size = 12,
                                BackgroundColor = Color.CornflowerBlue,
                                TextColor = Color.White,
                                HorizontalOptions = LayoutOptions.Center,
                                VerticalOptions = LayoutOptions.Center,
                                HeightRequest = 20,
                                WidthRequest = 60,
                                HorizontalTextAlignment = TextAlignment.Center,
                                VerticalTextAlignment = TextAlignment.Center,
                            };

                            CustomLabel account_label = new CustomLabel
                            {
                                Text = "[" + AccountValue[k].AC_BANKNAME + "] " + AccountValue[k].AC_ACCOUNTNUM,
                                Size = 12,
                                TextColor = Color.Black,
                                VerticalTextAlignment = TextAlignment.Center,
                            };

                            account_Grid.Children.Add(account_card, 0, 0);
                            account_Grid.Children.Add(account_label, 1, 0);
                            product_label_grid.Children.Add(account_Grid, 0, 3);
                            #endregion

                            #region 계좌주 Label
                            Grid account_name_Grid = new Grid
                            {
                                ColumnDefinitions =
                                {
                                    new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) },
                                    new ColumnDefinition { Width = new GridLength(7, GridUnitType.Star) },
                                },
                                VerticalOptions = LayoutOptions.Center,
                            };
                            CustomLabel account_name_card = new CustomLabel
                            {
                                Text = "계좌주",
                                Size = 12,
                                BackgroundColor = Color.CornflowerBlue,
                                TextColor = Color.White,
                                HorizontalOptions = LayoutOptions.Center,
                                VerticalOptions = LayoutOptions.Center,
                                HeightRequest = 20,
                                WidthRequest = 60,
                                HorizontalTextAlignment = TextAlignment.Center,
                                VerticalTextAlignment = TextAlignment.Center,
                            };

                            CustomLabel account_name_label = new CustomLabel
                            {
                                Text = AccountValue[k].AC_NAME,
                                Size = 12,
                                TextColor = Color.Black,
                                VerticalOptions = LayoutOptions.Center,
                                VerticalTextAlignment = TextAlignment.Center,
                            };

                            account_name_Grid.Children.Add(account_name_card, 0, 0);
                            account_name_Grid.Children.Add(account_name_label, 1, 0);
                            product_label_grid.Children.Add(account_name_Grid, 0, 4);
                            break;
                            #endregion
                        }
                    }
                }

            }

            // 입금대기중인 결제건이 없을 경우
            if(success_count == 0)
            {
                CustomLabel nonpurchase_label = new CustomLabel
                {
                    Text = "입금해야 할 내역이 없습니다.",
                    Size = 18,
                    TextColor = Color.Black,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalTextAlignment = TextAlignment.Center,
                    HorizontalTextAlignment = TextAlignment.Center
                };
                PurchaseListGrid.RowDefinitions.Clear();
                PurchaseListGrid.Children.Clear();
                PurchaseListGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                PurchaseListGrid.Children.Add(nonpurchase_label, 0, 0);
                return;
            }
        }
        #endregion

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            if (Global.isbackbutton_clicked)
            {
                Global.isbackbutton_clicked = false;
                Navigation.PopAsync();
            }
        }
    }
}