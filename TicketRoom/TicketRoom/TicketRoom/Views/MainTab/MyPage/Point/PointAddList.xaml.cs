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
	public partial class PointAddList : ContentView
    {
        PointDBFunc PT_DB = PointDBFunc.Instance();

        PointCheckPage pcp;
        PT_Point pp;
        int MyPoint = 0; // 내 보유 포인트


        List<PT_Charge> pcl = new List<PT_Charge>();

        public PointAddList (PointCheckPage pcp, PT_Point pp) // 포인트 적립내역
		{
			InitializeComponent ();
            this.pcp = pcp;
            this.pp = pp;
            #region 네트워크 상태 확인
            var current_network = Connectivity.NetworkAccess; // 현재 네트워크 상태
            if (current_network != NetworkAccess.Internet) // 네트워크 연결 불가
            {
                pcl = null;
            }
            #endregion
            #region 네트워크 연결 가능
            else
            {
                pcl = PT_DB.PostSearchChargeListToID(pp.USER_ID);
            }
            #endregion
            MyPoint = pp.PT_POINT_HAVEPOINT;
            MyPointLabel.Text = "보유 포인트 : " + MyPoint.ToString();
            Init();

        }
        private void Init()
        {
            #region 네트워크 오류 
            if (pcl == null)
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
            #endregion
            #region 수량 0일떄
            if (pcl.Count == 0)
            {
                MainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

                CustomLabel nullproduct = new CustomLabel
                {
                    Text = "적립내역이 없습니다.",
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
            for (int i = 0; i < pcl.Count; i++)
            {
                MainGrid.RowDefinitions.Add(new RowDefinition { Height = 100 });

                // 박스의 구분선 생성
                BoxView BorderLine1 = new BoxView { BackgroundColor =  Color.CornflowerBlue, };
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
                        if(Global.isNavigation_clicked == true)
                        {
                            Global.isNavigation_clicked = false;
                            await Navigation.PushAsync(new NavagationPage("포인트입금"));
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
                        new RowDefinition { Height = GridLength.Auto }
                    },

                };
                #region 상품권 그리드 자식 추가
                // 메인 리스트 그리드에 추가 
                MainGrid.Children.Add(inGrid, 0, i);

                inGrid.Children.Add(point_image, 0, 0);
                inGrid.Children.Add(point_label_grid, 1, 0);
                #endregion

                #region 적립 포인트 Label
                CustomLabel add_label = new CustomLabel
                {
                    Text = pcl[i].PT_CHARGE_POINT.ToString()/*value*/ + "포인트 적립",
                    Size = 18,
                    TextColor = Color.Black,
                };
                #endregion

                #region 적립 날짜 Label
                CustomLabel date_label = new CustomLabel
                {
                    Text = "일시 : " + pcl[i].PT_CHARGE_DATE/*date*/,
                    Size = 12,
                    TextColor = Color.DarkGray,
                };
                #endregion

                #region 적립 내역 Label 
                CustomLabel content_label = new CustomLabel
                {
                    Text = "",
                    Size = 14,
                    TextColor = Color.Gray,
                };
                if(pcl[i].PT_CHARGE_BANK == "" && pcl[i].PT_CHARGE_CARD == "") // 은행, 카드 충전이 모두 아니라면
                {
                    content_label.Text = "내역 : " + pcl[i].PT_CHARGE_CONTENT;
                }
                else if (pcl[i].PT_CHARGE_BANK != "" && pcl[i].PT_CHARGE_CARD == "") // 은행으로 충전한 내역이라면
                {
                    content_label.Text = "내역 : " + pcl[i].PT_CHARGE_CONTENT + "[" + pcl[i].PT_CHARGE_BANK + "]";
                }
                else if (pcl[i].PT_CHARGE_BANK == "" && pcl[i].PT_CHARGE_CARD != "") // 카드로 충전한 내역이라면
                {
                    content_label.Text = "내역 : " + pcl[i].PT_CHARGE_CONTENT + "[" + pcl[i].PT_CHARGE_CARD + "]";
                }
                #endregion


                CustomLabel status_label = new CustomLabel
                {
                    Text = "상태 : " + pcl[i].PT_CHARGE_STATUS,
                    Size = 14,
                    TextColor = Color.Orange,
                };
                if(pcl[i].PT_CHARGE_STATUS == "입금대기")
                {
                    status_label.TextColor = Color.Orange;
                }
                else if(pcl[i].PT_CHARGE_STATUS == "입금완료")
                {
                    status_label.TextColor = Color.Blue;
                }
                else if (pcl[i].PT_CHARGE_STATUS == "입금실패")
                {
                    status_label.TextColor = Color.Red;
                    point_label_grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                    CustomLabel fail_label = new CustomLabel
                    {
                        Text = "실패사유 : " + pcl[i].PT_FAIL_DETAIL,
                        Size = 14,
                        TextColor = Color.Black,
                    };
                    point_label_grid.Children.Add(fail_label, 0, 5);
                }

                //상품 설명 라벨 그리드에 추가
                point_label_grid.Children.Add(add_label, 0, 0);
                point_label_grid.Children.Add(date_label, 0, 1);
                point_label_grid.Children.Add(content_label, 0, 3);
                point_label_grid.Children.Add(status_label, 0, 4);
                #endregion
            }
        }
	}
}