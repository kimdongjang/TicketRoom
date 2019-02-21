using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TicketRoom.Models.Custom;
using TicketRoom.Models.Users;
using TicketRoom.Views.MainTab.Dael.Purchase;
using TicketRoom.Views.MainTab.Shop;
using TicketRoom.Views.Users.CreateUser;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopupRecentAdress
    {
        UserDBFunc USER_DB = UserDBFunc.Instance();
        List<ADRESS> recentList = new List<ADRESS>();
        ADRESS myAdress = new ADRESS();
        Queue<Grid> adl_queue = new Queue<Grid>();
        InputAdress returnInputAdress;
        bool isClicked = false;

        public PopupRecentAdress(InputAdress ia)
        {
            InitializeComponent();
            recentList = USER_DB.PostRecentAdressToID(Global.ID);
            this.returnInputAdress = ia;
            Init();
        }

        private void Init()
        {
            if (Global.b_user_login == false) // 비회원 상태일 경우
            {
                CustomLabel errorLabel = new CustomLabel
                {
                    Text = "최근 주소를 찾을 수 없습니다!",
                    Size = 14,
                    TextColor = Color.Gray,
                    VerticalOptions = LayoutOptions.Center,
                };
                RecentAdressGrid.Children.Add(errorLabel);
                return;
            }
            RecentAdressGrid.RowDefinitions.Clear();
            RecentAdressGrid.Children.Clear();

            for (int i = 0; i < recentList.Count; i++)
            {
                Grid grid = new Grid // 주소 라벨을 묶는 그리드 생성
                {
                    RowSpacing = 10,
                    ColumnSpacing = 0,
                    BindingContext = i,
                    RowDefinitions =
                    {
                        new RowDefinition { Height = 3 },
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                        new RowDefinition { Height = 3 },
                    }
                };

                RecentAdressGrid.Children.Add(grid, 0, i); //

                BoxView borderLine = new BoxView { BackgroundColor = Color.LightGray, Margin = new Thickness(10, 0, 10, 0) };
                if (i != 0) // 구분선 첫줄은 배제.
                {
                    grid.Children.Add(borderLine, 0, 0); //
                }
                #region 도로명 구역
                Grid roadGrid = new Grid
                {
                    ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) },
                        new ColumnDefinition { Width = new GridLength(7, GridUnitType.Star) },
                    }
                };
                CustomButton roadButton = new CustomButton
                {
                    Text = "도로명",
                    Size = 14,
                    BackgroundColor = Color.CornflowerBlue,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    HeightRequest = 25,
                    TextColor = Color.White,
                };
                // 리스트 내용은 라벨로 설정
                CustomLabel roadLabel = new CustomLabel
                {
                    Text = recentList[i].ROADADDR + " ( " + recentList[i].ZIPNO + " )",
                    Size = 14,
                    TextColor = Color.Gray,
                    VerticalOptions = LayoutOptions.Center,
                };
                roadGrid.Children.Add(roadButton, 0, 0);
                roadGrid.Children.Add(roadLabel, 1, 0);
                #endregion

                #region 지번 구역
                Grid jibunGrid = new Grid
                {
                    ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) },
                        new ColumnDefinition { Width = new GridLength(7, GridUnitType.Star) },
                    }
                };
                CustomButton jibunButton = new CustomButton
                {
                    Text = "지번",
                    Size = 14,
                    BackgroundColor = Color.CornflowerBlue,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    HeightRequest = 30,
                    TextColor = Color.White,
                };
                CustomLabel jibunLabel = new CustomLabel
                {
                    Text = recentList[i].JIBUNADDR,
                    Size = 14,
                    TextColor = Color.Gray,
                    VerticalOptions = LayoutOptions.Center,
                };
                jibunGrid.Children.Add(jibunButton, 0, 0);
                jibunGrid.Children.Add(jibunLabel, 1, 0);
                #endregion

                #region 그리드에 추가
                grid.Children.Add(roadGrid, 0, 1); //
                grid.Children.Add(jibunGrid, 0, 2); //
                #endregion


                #region 리스트 내용 클릭 이벤트
                grid.GestureRecognizers.Add(
                    new TapGestureRecognizer()
                    {
                        Command = new Command(() => {
                            isClicked = true; // 주소 하나라도 클릭했을 경우 최근 주소 데이터를 -> 인풋 어드레스로 이동
                            var s = grid.BindingContext;
                            // ADRESS 객체에 도로명,지번,우편번호 초기화
                            myAdress.ROADADDR = recentList[int.Parse(s.ToString())].ROADADDR;
                            myAdress.JIBUNADDR = recentList[int.Parse(s.ToString())].JIBUNADDR;
                            myAdress.ZIPNO = recentList[int.Parse(s.ToString())].ZIPNO;

                            if (adl_queue.Count < 2)
                            {
                                if (adl_queue.Count != 0)
                                {
                                    Grid tempGrid = adl_queue.Dequeue();
                                    if (tempGrid.Children.Count != 3) // 첫줄 구분선 제어
                                    {
                                        ((CustomButton)((Grid)tempGrid.Children.ElementAt(0)).Children.ElementAt(0)).BackgroundColor = Color.CornflowerBlue;
                                        ((CustomButton)((Grid)tempGrid.Children.ElementAt(1)).Children.ElementAt(0)).BackgroundColor = Color.CornflowerBlue;
                                    }
                                    else
                                    {
                                        ((CustomButton)((Grid)tempGrid.Children.ElementAt(1)).Children.ElementAt(0)).BackgroundColor = Color.CornflowerBlue;
                                        ((CustomButton)((Grid)tempGrid.Children.ElementAt(2)).Children.ElementAt(0)).BackgroundColor = Color.CornflowerBlue;
                                    }
                                }
                                if (int.Parse(s.ToString()) != 0) // 첫줄 구분선 제어
                                {
                                    ((CustomButton)((Grid)grid.Children.ElementAt(1)).Children.ElementAt(0)).BackgroundColor = Color.Blue;
                                    ((CustomButton)((Grid)grid.Children.ElementAt(2)).Children.ElementAt(0)).BackgroundColor = Color.Blue;
                                }
                                else
                                {
                                    ((CustomButton)((Grid)grid.Children.ElementAt(0)).Children.ElementAt(0)).BackgroundColor = Color.Blue;
                                    ((CustomButton)((Grid)grid.Children.ElementAt(1)).Children.ElementAt(0)).BackgroundColor = Color.Blue;
                                }
                                adl_queue.Enqueue(grid);
                            }
                        })
                    }
                );
                #endregion

            }
        }
        
        // 확인 버튼 눌렀을시
        private async void ConfirmBtn_Clicked(object sender, EventArgs e)
        {
            if (isClicked == true)
            {
                var answer = await DisplayAlert(myAdress.ROADADDR, "해당 주소로 변경하시겠습니까?", "확인", "취소");
                if (answer == false) return;
                Global.adress.ROADADDR = myAdress.ROADADDR;
                Global.adress.JIBUNADDR = myAdress.JIBUNADDR;
                Global.adress.ZIPNO = myAdress.ZIPNO;

                // 유저 회원 가입 주소 확인시
                if (returnInputAdress.cup != null)
                {
                    returnInputAdress.cup.EntryAdress.Text = Global.adress.ROADADDR;
                }
                // 상품권 구매 주소 확인시
                else if (returnInputAdress.pdp != null)
                {
                    returnInputAdress.pdp.EntryAdress.Text = Global.adress.ROADADDR;
                }
                // 쇼핑몰 탭 페이지 주소 확인시
                else if (returnInputAdress.stp != null)
                {
                    returnInputAdress.stp.EntryAdress.Text = Global.adress.ROADADDR;
                }
                // 쇼핑몰 주문 페이지 주소 변경시
                else if (returnInputAdress.sop != null)
                {
                    returnInputAdress.sop.AdressLabel.Text = Global.adress.ROADADDR;
                }
                PopupNavigation.Instance.RemovePageAsync(this);
                Navigation.PopModalAsync();
            }
            else
            {
                PopupNavigation.Instance.RemovePageAsync(this);
            }
        }
    }
}