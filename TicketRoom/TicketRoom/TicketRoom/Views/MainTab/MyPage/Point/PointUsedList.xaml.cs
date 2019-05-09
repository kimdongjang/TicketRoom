using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketRoom.Models.Custom;
using TicketRoom.Models.PointData;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.MyPage.Point
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PointUsedList : ContentView
    {
        PointDBFunc PT_DB = PointDBFunc.Instance();

        PointCheckPage pcp;
        PT_Point pp;
        int MyPoint = 0; // 내 보유 포인트


        List<PT_WithDraw> wdl = new List<PT_WithDraw>();

        public PointUsedList (PointCheckPage pcp, PT_Point pp)
		{
			InitializeComponent ();
            this.pcp = pcp;
            this.pp = pp;

            #region 네트워크 상태 확인
            var current_network = Connectivity.NetworkAccess; // 현재 네트워크 상태
            if (current_network != NetworkAccess.Internet) // 네트워크 연결 불가
            {
                wdl = null;
            }
            #endregion
            #region 네트워크 연결 가능
            else
            {
                if (Global.b_guest_login == true)
                {
                    PT_WithDraw p1 = new PT_WithDraw
                    {
                        USER_ID = "Guest",
                        PT_POINT_INDEX = 1000,
                        PT_WITHDRAW_ACCOUNT = "3154-1542-4444",
                        PT_WITHDRAW_BANK = "농협",
                        PT_WITHDRAW_CONTENT = "포인트출금 10000원",
                        PT_WITHDRAW_DATE = "2019/05/04",
                        PT_WITHDRAW_FAILDETAIL = "",
                        PT_WITHDRAW_INDEX = 1000,
                        PT_WITHDRAW_NAME = "홍길동",
                        PT_WITHDRAW_POINT = 10000,
                        PT_WITHDRAW_STATUS = "출금완료",
                    };
                    PT_WithDraw p2 = new PT_WithDraw
                    {
                        USER_ID = "Guest",
                        PT_POINT_INDEX = 1000,
                        PT_WITHDRAW_ACCOUNT = "3154-1542-4444",
                        PT_WITHDRAW_BANK = "농협",
                        PT_WITHDRAW_CONTENT = "포인트출금 20000원",
                        PT_WITHDRAW_DATE = "2019/05/04",
                        PT_WITHDRAW_FAILDETAIL = "",
                        PT_WITHDRAW_INDEX = 1000,
                        PT_WITHDRAW_NAME = "홍길동",
                        PT_WITHDRAW_POINT = 10000,
                        PT_WITHDRAW_STATUS = "출금완료",
                    };
                    PT_WithDraw p3 = new PT_WithDraw
                    {
                        USER_ID = "Guest",
                        PT_POINT_INDEX = 1000,
                        PT_WITHDRAW_ACCOUNT = "3154-1542-4444",
                        PT_WITHDRAW_BANK = "농협",
                        PT_WITHDRAW_CONTENT = "포인트출금 10000원",
                        PT_WITHDRAW_DATE = "2019/05/04",
                        PT_WITHDRAW_FAILDETAIL = "",
                        PT_WITHDRAW_INDEX = 1000,
                        PT_WITHDRAW_NAME = "홍길동",
                        PT_WITHDRAW_POINT = 10000,
                        PT_WITHDRAW_STATUS = "출금대기",
                    };
                    wdl.Add(p1);
                    wdl.Add(p2);
                    wdl.Add(p3);
                }
                else
                {
                    wdl = PT_DB.PostSearchWithDrawListToID(pp.USER_ID);
                }
            }
            #endregion
            MyPoint = pp.PT_POINT_HAVEPOINT;
            MyPointLabel.Text = "보유 포인트 : " + MyPoint.ToString();
            Init();

        }

        private void Init()
        {
            #region 네트워크 연결 불가
            if (wdl == null)
            {
                MainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

                CustomLabel nullproduct = new CustomLabel
                {
                    Text = "네트워크에 연결할 수 없습니다. 다시 시도해 주세요.",
                    Size = 18,
                    TextColor = Color.Black,
                    VerticalOptions = LayoutOptions.Center,
                    YAlign = TextAlignment.Center,
                    HorizontalOptions = LayoutOptions.Center
                };
                MainGrid.Children.Clear();
                MainGrid.RowDefinitions.Clear();
                MainGrid.Children.Add(nullproduct, 0, 0);
                return;
            }
            if (wdl.Count == 0)
            {
                MainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

                CustomLabel nullproduct = new CustomLabel
                {
                    Text = "사용내역이 없습니다.",
                    Size = 18,
                    TextColor = Color.Black,
                    VerticalOptions = LayoutOptions.Center,
                    YAlign = TextAlignment.Center,
                    HorizontalOptions = LayoutOptions.Center
                };
                MainGrid.Children.Clear();
                MainGrid.RowDefinitions.Clear();
                MainGrid.Children.Add(nullproduct, 0, 0);
                return;
            }
            #endregion
            for (int i = 0; i < wdl.Count; i++)
            {
                MainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

                // 박스의 구분선 생성
                BoxView BorderLine1 = new BoxView { BackgroundColor = Color.CornflowerBlue, };
                StackLayout BorderStack1 = new StackLayout { BackgroundColor = Color.LightGray, Margin = 3, };
                StackLayout BorderStack2 = new StackLayout { BackgroundColor = Color.White, Margin = 6, };
                MainGrid.Children.Add(BorderLine1, 0, i);
                MainGrid.Children.Add(BorderStack1, 0, i);
                MainGrid.Children.Add(BorderStack2, 0, i);


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
                    Margin = new Thickness(0, 5, 0, 5),
                    RowSpacing = 0,
                    ColumnSpacing = 0,
                };

                inGrid.GestureRecognizers.Add(new TapGestureRecognizer()
                {
                    Command = new Command(async () =>
                    {
                        if (Global.isNavigation_clicked == true)
                        {
                            Global.isNavigation_clicked = false;
                            await Navigation.PushAsync(new NavagationPage("포인트출금"));
                        }

                    })
                });

                #region 포인트 이미지
                Image point_image = new Image
                {
                    Source = "point_icon.png",
                    BackgroundColor = Color.White,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    Aspect = Aspect.AspectFit,
                    Margin = 10,
                };
                #endregion

                #region 포인트 설명 그리드
                Grid point_label_grid = new Grid
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
                        new RowDefinition { Height = GridLength.Auto },
                        new RowDefinition { Height = 10 },
                        new RowDefinition { Height = GridLength.Auto },
                        new RowDefinition { Height = GridLength.Auto },
                        new RowDefinition { Height = 10 },
                    },

                };
                #region 상품권 그리드 자식 추가
                // 메인 리스트 그리드에 추가 
                MainGrid.Children.Add(inGrid, 0, i);

                inGrid.Children.Add(point_image, 0, 0);
                inGrid.Children.Add(point_label_grid, 1, 0);
                #endregion

                #region 사용 포인트 Label
                CustomLabel use_label = new CustomLabel
                {
                    Text = wdl[i].PT_WITHDRAW_POINT.ToString()/*value*/ + "포인트 사용",
                    Size = 18,
                    TextColor = Color.Black,
                };
                #endregion

                #region 사용 날짜 Label
                CustomLabel date_label = new CustomLabel
                {
                    Text = "일시 : " + wdl[i].PT_WITHDRAW_DATE/*date*/,
                    Size = 12,
                    TextColor = Color.DarkGray,
                };
                #endregion

                #region 사용 내역 Label
                CustomLabel content_label = new CustomLabel
                {
                    Size = 14,
                    TextColor = Color.Gray,
                };
                if (wdl[i].PT_WITHDRAW_BANK == "" && wdl[i].PT_WITHDRAW_ACCOUNT == "" && wdl[i].PT_WITHDRAW_NAME == "")
                {
                    content_label.Text = "내역 : " + wdl[i].PT_WITHDRAW_CONTENT;
                }
                else
                {
                    content_label.Text = "내역 : " + wdl[i].PT_WITHDRAW_CONTENT + " \n[" + wdl[i].PT_WITHDRAW_BANK + "]" +
                            " \n[계좌번호:" + wdl[i].PT_WITHDRAW_ACCOUNT + "]" + "[예금주:" + wdl[i].PT_WITHDRAW_NAME + "]";
                }
                #endregion

                #region 출금 상태 Label
                CustomLabel status_label = new CustomLabel
                {
                    Size = 14,
                    TextColor = Color.Orange,
                    Text = "출금상태 : " + wdl[i].PT_WITHDRAW_STATUS,
                };

                if (wdl[i].PT_WITHDRAW_STATUS == "출금대기")
                {
                    status_label.TextColor = Color.Orange;
                }
                else if (wdl[i].PT_WITHDRAW_STATUS == "출금완료")
                {
                    status_label.TextColor = Color.Blue;
                }
                else if (wdl[i].PT_WITHDRAW_STATUS == "출금실패")
                {
                    status_label.TextColor = Color.Red;
                    CustomLabel fail_label = new CustomLabel
                    {
                        Text = "실패사유 : " + wdl[i].PT_WITHDRAW_FAILDETAIL,
                        Size = 14,
                        TextColor = Color.Black,
                    };
                    point_label_grid.Children.Add(fail_label, 0, 6);
                }
                #endregion

                //상품 설명 라벨 그리드에 추가
                point_label_grid.Children.Add(use_label, 0, 0);
                point_label_grid.Children.Add(date_label, 0, 1);
                point_label_grid.Children.Add(content_label, 0, 3);
                point_label_grid.Children.Add(status_label, 0, 5);
                #endregion



            }
        }
    }
}