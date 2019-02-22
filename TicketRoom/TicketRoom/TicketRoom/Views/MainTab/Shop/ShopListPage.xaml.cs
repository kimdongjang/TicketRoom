using FFImageLoading.Forms;
using FFImageLoading.Svg.Forms;
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
using TicketRoom.Models.ShopData;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.Shop
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShopListPage : ContentPage
    {
        ShopDBFunc SH_DB = ShopDBFunc.Instance();

        int main_index = 0;
        List<SubCate> sclist = new List<SubCate>();

        Queue<Grid> titleList_Queue = new Queue<Grid>();
        List<string> subCateTapList = new List<string>();
        List<Grid> ShopTapEventList = new List<Grid>();
        Queue<Grid> SelectList_Queue = new Queue<Grid>();


        public ShopListPage(int main_index)
        {
            InitializeComponent();
            this.main_index = main_index;
            #region IOS의 경우 초기화
            NavigationPage.SetHasNavigationBar(this, false); // Navigation Bar 지우는 코드 생성자에 입력
            if (Xamarin.Forms.Device.OS == TargetPlatform.iOS)
            {
                MainGrid.RowDefinitions[0].Height = 50;
            }
            #endregion
        }
        protected override void OnAppearing() // PopAsync 호출 또는 페이지 초기화때 시동
        {
            // 최상위 탭 카테고리 이름 초기화
            for (int i = 0; i < ShopTabPage.mclist.Count; i++)
            {
                if (ShopTabPage.mclist[i].SH_MAINCATE_INDEX == main_index)
                {
                    TitleName.Text = ShopTabPage.mclist[i].SH_MAINCATE_NAME;
                    Title = ShopTabPage.mclist[i].SH_MAINCATE_NAME;
                }
            }

            sclist = SH_DB.PostSubCategoryListAsync(main_index);

            TitleTapInit();
            UpdateList();
            base.OnAppearing();
        }

        // 타이틀 탭 이름 초기화 및 이벤트 등록 초기화 함수
        private void TitleTapInit()
        {
            #region 타이틀 탭 바로 밑에 탭 리스트 초기화
            for (int i = 0; i < ShopTabPage.mclist.Count; i++)
            {
                subCateTapList.Add(ShopTabPage.mclist[i].SH_MAINCATE_NAME);

                Grid inGrid = new Grid
                {
                    BackgroundColor = Color.CornflowerBlue,
                };
                CustomLabel label = new CustomLabel
                {
                    Text = ShopTabPage.mclist[i].SH_MAINCATE_NAME,
                    Size = 18,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    TextColor = Color.White,
                };

                inGrid.Children.Add(label, 0, 0);
                SubCateTapGrid.Children.Add(inGrid, i, 0);

                // 처음으로 선택한 탭 이름 초기화
                if (label.Text == TitleName.Text)
                {
                    label.TextColor = Color.CornflowerBlue;
                    inGrid.BackgroundColor = Color.White;
                    titleList_Queue.Enqueue((Grid)SubCateTapGrid.Children.ElementAt(i));
                }

                #region 탭 클릭시 색상 변경 및 밑줄 생성 이벤트
                inGrid.GestureRecognizers.Add(new TapGestureRecognizer()
                {
                    Command = new Command(() =>
                    {
                        if (titleList_Queue.Count < 2)
                        {
                            if (titleList_Queue.Count != 0)
                            {
                                // 이전 탭 색상 초기화
                                Grid temp_grid = titleList_Queue.Dequeue();
                                CustomLabel temp_label = (CustomLabel)temp_grid.Children.ElementAt(0);
                                temp_label.TextColor = Color.White;
                                temp_grid.BackgroundColor = Color.CornflowerBlue;
                                TitleName.Text = temp_label.Text;
                            }
                            // 탭 클릭시 색상 변경
                            label.TextColor = Color.CornflowerBlue;
                            inGrid.BackgroundColor = Color.White;
                            titleList_Queue.Enqueue(inGrid);
                            TitleName.Text = label.Text;
                        }
                        for (int j = 0; j < ShopTabPage.mclist.Count; j++)
                        {
                            if(ShopTabPage.mclist[j].SH_MAINCATE_NAME == TitleName.Text)
                            {
                                sclist = SH_DB.PostSubCategoryListAsync(ShopTabPage.mclist[j].SH_MAINCATE_INDEX); // 서브 카테고리 리스트 초기화
                                
                                UpdateList();
                            }
                        }
                        
                    })
                });
                #endregion
            }
            #endregion
        }

        // 메인 리스트 초기화 함수
        private async void UpdateList()
        {
            BestShopListGrid.Children.Clear();
            BestShopListGrid.RowDefinitions.Clear();
            NaturalShopListGrid.Children.Clear();
            NaturalShopListGrid.RowDefinitions.Clear();

            #region 베스트 쇼핑몰 레이블 추가
            // 베스트 라벨
            Grid bestlabel_grid = new Grid
            {
                BackgroundColor = Color.CornflowerBlue,
                Opacity = 0.5,
            };
            CustomLabel bestlabel = new CustomLabel
            {
                Text = "베스트",
                Size = 18,
                TextColor = Color.White,
                VerticalOptions = LayoutOptions.Center,
                Margin = new Thickness(15, 0, 0, 0)
            };
            bestlabel_grid.Children.Add(bestlabel);
            BestShopListGrid.RowDefinitions.Add(new RowDefinition { Height = 30 });
            BestShopListGrid.Children.Add(bestlabel_grid, 0, 0);
            #endregion            

            #region 베스트 그리드

            int best_row = 1;

            for (int i = 0; i < sclist.Count; i++)
            {
                if (sclist[i].SH_SUBCATE_ISBEST == "TRUE") // 서버에서 가져온 카테고리 리스트 중에 베스트가 등록이 되어있다면
                {
                    BestShopListGrid.RowDefinitions.Add(new RowDefinition { Height = 100 }); // 그리드 추가를 위한 행 추가

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
                        ErrorPlaceholder = Global.LoadingImagePath,
                        Source = ImageSource.FromUri(new Uri(sclist[i].SH_SUBCATE_IMAGE)),
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Center,
                        Margin = new Thickness(20, 10, 0, 10),
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
                        Size = 18,
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
                        ErrorPlaceholder = Global.LoadingImagePath,
                        Source = "star.png",
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Center,
                    };
                    CustomLabel grade_label = new CustomLabel
                    {
                        Text = "평점 : " + sclist[i].SH_SUBCATE_GRADE.ToString(),
                        Size = 14,
                        TextColor = Color.Black,
                    };
                    grade_Grid.Children.Add(grade_image, 0, 0);
                    grade_Grid.Children.Add(grade_label, 1, 0);
                    #endregion

                    CustomLabel bestDetail = new CustomLabel
                    {
                        Text = sclist[i].SH_SUBCATE_DETAIL,
                        Size = 14,
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
                            Navigation.PushAsync(new ShopMainPage(tempIndex));
                        })
                    });
                    #endregion

                    BestShopListGrid.Children.Add(best_rowGrid, 0, best_row);
                    best_row++;
                    // 디자인을 베스트 메인 리스트 그리드에 추가하고, 행을 하나 늘린다.
                }
            }
            #endregion

            #region 일반 쇼핑몰 레이블 추가
            Grid naturallabel_grid = new Grid
            {
                BackgroundColor = Color.RosyBrown,
                Opacity = 0.5,
            };
            CustomLabel naturallabel = new CustomLabel
            {
                Text = "일반",
                Size = 18,
                TextColor = Color.White,
                VerticalOptions = LayoutOptions.Center,
                Margin = new Thickness(15, 0, 0, 0)
            };
            naturallabel_grid.Children.Add(naturallabel);
            NaturalShopListGrid.RowDefinitions.Add(new RowDefinition { Height = 30 });
            NaturalShopListGrid.Children.Add(naturallabel_grid, 0, 0);
            #endregion

            #region 일반 쇼핑몰 그리드

            int normal_row = 1;

            for (int i = 0; i < sclist.Count; i++)
            {
                NaturalShopListGrid.RowDefinitions.Add(new RowDefinition { Height = 100 });

                // 일반 리스트 그리드 동적으로 생성
                Grid natural_rowGrid = new Grid
                {
                    ColumnDefinitions = {
                        new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) },
                        new ColumnDefinition { Width = new GridLength(7, GridUnitType.Star)  }
                    },
                };

                // 쇼핑몰 이미지
                CachedImage naturalimage = new CachedImage
                {
                    LoadingPlaceholder = Global.LoadingImagePath,
                    ErrorPlaceholder = Global.LoadingImagePath,
                    Source = ImageSource.FromUri(new Uri(sclist[i].SH_SUBCATE_IMAGE)),
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    Margin = new Thickness(20, 10, 0, 10),
                };

                // 쇼핑 페이지 타이틀, 평점, 세부내용 출력을 위한 내부 그리드
                Grid natural_columnGrid = new Grid
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
                CustomLabel naturalHome = new CustomLabel
                {
                    Text = sclist[i].SH_SUBCATE_NAME,
                    Size = 18,
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
                Image grade_image = new Image
                {
                    Source = "star.png",
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                };
                CustomLabel grade_label = new CustomLabel
                {
                    Text = "평점 : " + sclist[i].SH_SUBCATE_GRADE.ToString(),
                    Size = 14,
                    TextColor = Color.Black,
                };
                grade_Grid.Children.Add(grade_image, 0, 0);
                grade_Grid.Children.Add(grade_label, 1, 0);
                #endregion

                CustomLabel naturalDetail = new CustomLabel
                {
                    Text = sclist[i].SH_SUBCATE_DETAIL,
                    Size = 14,
                    TextColor = Color.Black,
                    MaxLines = 2,
                };
                #endregion

                #region 그리드 자식으로 추가
                natural_columnGrid.Children.Add(naturalHome, 0, 1);
                natural_columnGrid.Children.Add(grade_Grid, 0, 2);
                natural_columnGrid.Children.Add(naturalDetail, 0, 3);
                natural_rowGrid.Children.Add(naturalimage, 0, 0);
                natural_rowGrid.Children.Add(natural_columnGrid, 1, 0);
                #endregion


                #region 탭 클릭시 쇼핑몰 페이지로 이동하는 이벤트
                natural_rowGrid.GestureRecognizers.Add(new TapGestureRecognizer()
                {
                    Command = new Command(() =>
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
                            natural_rowGrid.BackgroundColor = Color.CornflowerBlue;
                            natural_rowGrid.Opacity = 0.5;
                            SelectList_Queue.Enqueue(natural_rowGrid);

                        }

                        int tempIndex = 0;
                        for (int j = 0; j < sclist.Count; j++)
                        {
                            if (naturalHome.Text == sclist[j].SH_SUBCATE_NAME)
                            {
                                tempIndex = sclist[j].SH_HOME_INDEX;
                            }
                        }
                        Global.OtherIndexUpdate(tempIndex); // 다른 고객이 함께본 상품 초기화를 위한 처리
                        SH_DB.PostUpdateViewsOtherViewToIndex(Global.g_main_index, Global.g_other_index); // main인덱스와 other인덱스 서버로 전달
                        Navigation.PushAsync(new ShopMainPage(tempIndex));
                    })
                });
                #endregion

                NaturalShopListGrid.Children.Add(natural_rowGrid, 0, normal_row);
                normal_row++;
            }
            #endregion
        }

        private void ChangeTitle(string name)
        {
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {

        }

        private void Tab_Changed(object sender, EventArgs e)
        {

        }

        private void BackButton_Clicked(object sender, EventArgs e)
        {
            Global.isOpen_ShopListPage = false;
            Navigation.PopAsync();
        }
        protected override bool OnBackButtonPressed()
        {
            Global.isOpen_ShopListPage = false;
            return base.OnBackButtonPressed();
        }
    }
}