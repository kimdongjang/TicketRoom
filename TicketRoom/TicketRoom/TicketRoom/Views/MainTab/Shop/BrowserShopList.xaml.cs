using FFImageLoading.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketRoom.Models.Custom;
using TicketRoom.Models.ShopData;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.Shop
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BrowserShopList : ContentPage
    {
        ShopDBFunc SH_DB = ShopDBFunc.Instance();
        List<SubCate> sclist;
        Queue<Grid> SelectList_Queue = new Queue<Grid>();

        public BrowserShopList (List<SubCate> sclist)
		{
			InitializeComponent ();
            this.sclist = sclist;
            #region IOS의 경우 초기화
            NavigationPage.SetHasNavigationBar(this, false); // Navigation Bar 지우는 코드 생성자에 입력
            if (Xamarin.Forms.Device.OS == TargetPlatform.iOS)
            {
                TabGrid.RowDefinitions[0].Height = 50;
            }
            #endregion
            LoadingInit();
        }

        private async Task LoadingInit()
        {
            // 로딩 시작
            await Global.LoadingStartAsync();

            Init();

            // 로딩 완료
            await Global.LoadingEndAsync();
        }

        private void Init()
        {
            #region 베스트 쇼핑몰 레이블 추가
            // 베스트 라벨
            Grid bestlabel_grid = new Grid
            {
                BackgroundColor = Color.CornflowerBlue,
                Opacity = 0.5,
            };
            CustomLabel bestlabel = new CustomLabel
            {
                Text = "검색결과",
                Size = 14,
                TextColor = Color.White,
                VerticalOptions = LayoutOptions.Center,
                Margin = new Thickness(15, 0, 0, 0)
            };
            bestlabel_grid.Children.Add(bestlabel);
            BrowsingGrid.RowDefinitions.Add(new RowDefinition { Height = 30 });
            BrowsingGrid.Children.Add(bestlabel_grid, 0, 0);
            #endregion

            if(sclist == null)
            {
                CustomLabel errorLabel = new CustomLabel
                {
                    Text = "검색결과를 찾을 수 없습니다.",
                    Size = 14,
                    TextColor = Color.Black,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                };
                BrowsingGrid.RowDefinitions.Add(new RowDefinition { Height = 30 });
                BrowsingGrid.Children.Add(errorLabel, 0, 1);
                return;
            }

            int best_row = 1;
            for (int i = 0; i < sclist.Count; i++)
            {
                BrowsingGrid.RowDefinitions.Add(new RowDefinition { Height = 100 }); // 그리드 추가를 위한 행 추가

                // 베스트 리스트 그리드 동적으로 생성
                Grid best_rowGrid = new Grid
                {
                    ColumnDefinitions = {
                        new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) },
                        new ColumnDefinition { Width = new GridLength(7, GridUnitType.Star)  }
                        },
                };

                // 쇼핑몰 이미지
                CachedImage bestimage = new CachedImage
                {
                    LoadingPlaceholder = Global.LoadingImagePath,
                    ErrorPlaceholder = Global.NotFoundImagePath,
                    Source = ImageSource.FromUri(new Uri(sclist[i].SH_SUBCATE_IMAGE)),
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    Margin = new Thickness(20, 10, 0, 10),
                    Aspect = Aspect.AspectFill,
                };

                // 쇼핑 페이지 타이틀, 평점, 세부내용 출력을 위한 내부 그리드
                Grid best_columnGrid = new Grid
                {
                    RowDefinitions =
                        {
                            new RowDefinition {Height = 10 },
                            new RowDefinition {Height = new GridLength(1, GridUnitType.Star) },
                            new RowDefinition {Height = new GridLength(1, GridUnitType.Star) },
                            new RowDefinition {Height = 28 },
                            new RowDefinition {Height = 10 },
                        },
                    Margin = new Thickness(20, 0, 20, 0),
                };

                #region 그리드 내부 타이틀, 평점, 세부내용
                CustomLabel bestHome = new CustomLabel
                {
                    Text = sclist[i].SH_SUBCATE_NAME,
                    Size = 14,
                    TextColor = Color.Black,
                };

                #region 평점
                Grid grade_Grid = new Grid
                {
                    ColumnDefinitions =
                        {
                            new ColumnDefinition { Width = 15 },
                            new ColumnDefinition { Width = GridLength.Auto },
                        },
                    VerticalOptions = LayoutOptions.Center
                };
                CachedImage grade_image = new CachedImage
                {
                    LoadingPlaceholder = Global.LoadingImagePath,
                    ErrorPlaceholder = Global.NotFoundImagePath,
                    Source = "star.png",
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                };
                CustomLabel grade_label = new CustomLabel
                {
                    Text = "평점 : " + sclist[i].SH_SUBCATE_GRADE.ToString(),
                    Size = 10,
                    TextColor = Color.Black,
                };
                grade_Grid.Children.Add(grade_image, 0, 0);
                grade_Grid.Children.Add(grade_label, 1, 0);
                #endregion

                CustomLabel bestDetail = new CustomLabel
                {
                    Text = sclist[i].SH_SUBCATE_DETAIL,
                    Size = 10,
                    TextColor = Color.Black,
                    //MaxLines = 2,
                };
                #endregion

                #region 그리드 자식으로 추가
                best_columnGrid.Children.Add(bestHome, 0, 1);
                best_columnGrid.Children.Add(grade_Grid, 0, 2);
                best_columnGrid.Children.Add(bestDetail, 0, 3);
                best_rowGrid.Children.Add(bestimage, 0, 0);
                best_rowGrid.Children.Add(best_columnGrid, 1, 0);
                #endregion

                #region 탭 클릭시 쇼핑몰 페이지로 이동하는 이벤트
                best_rowGrid.GestureRecognizers.Add(new TapGestureRecognizer()
                {
                    Command = new Command(() =>
                    {
                        #region 네트워크 상태 확인
                        var current_network = Connectivity.NetworkAccess; // 현재 네트워크 상태
                        if (current_network != NetworkAccess.Internet) // 네트워크 연결 불가
                        {
                            DisplayAlert("알림", "네트워크에 연결할 수 없습니다!", "확인");
                            return;
                        }
                        #endregion
                        #region 네트워크 연결 가능
                        else
                        {

                            // 탭을 한번 클릭했다면 다시 열리지 않도록 제어
                            if (Global.isOpen_ShopMainPage == true)
                            {
                                return;
                            }
                            Global.isOpen_ShopMainPage = true;

                            if (SelectList_Queue.Count < 2)
                            {
                                if (SelectList_Queue.Count != 0)
                                {
                                    Grid temp = SelectList_Queue.Dequeue();
                                    temp.BackgroundColor = Color.White;
                                    temp.Opacity = 1;
                                }
                                best_rowGrid.BackgroundColor = Color.CornflowerBlue;
                                best_rowGrid.Opacity = 0.5;
                                SelectList_Queue.Enqueue(best_rowGrid);

                            }

                            int tempIndex = 0;
                            for (int j = 0; j < sclist.Count; j++)
                            {
                                if (bestHome.Text == sclist[j].SH_SUBCATE_NAME)
                                {
                                    tempIndex = sclist[j].SH_HOME_INDEX; // 홈 인덱스로 변경
                                }
                            }
                            Global.OtherIndexUpdate(tempIndex); // 다른 고객이 함께본 상품 초기화를 위한 처리
                            SH_DB.PostUpdateViewsOtherViewToIndex(Global.g_main_index, Global.g_other_index); // main인덱스와 other인덱스 서버로 전달

                            if (Global.b_user_login == true) // 회원인 상태로 로그인이 되어있다면
                            {
                                SH_DB.PostUpdateRecentViewToID(Global.ID, tempIndex.ToString()); // 최근 본 상품 목록 갱신
                            }
                            else // 비회원 상태
                            {
                                SH_DB.PostUpdateRecentViewToID(Global.non_user_id, tempIndex.ToString()); // 최근 본 상품 목록 갱신
                            }
                            Navigation.PushAsync(new ShopMainPage(tempIndex));
                        }
                        #endregion
                    })
                });
                #endregion

                BrowsingGrid.Children.Add(best_rowGrid, 0, best_row);
                best_row++;
                // 디자인을 베스트 메인 리스트 그리드에 추가하고, 행을 하나 늘린다.
            }        
        }

        private void BackButton_Clicked(object sender, EventArgs e)
        {
            //Global.isOpen_ShopListPage = false;
            Navigation.PopAsync();
        }
    }
}