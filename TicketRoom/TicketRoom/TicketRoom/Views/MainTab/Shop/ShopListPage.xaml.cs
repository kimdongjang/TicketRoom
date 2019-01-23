using System;
using System.Collections.Generic;
using System.Linq;
using TicketRoom.Models.Custom;
using TicketRoom.Models.ShopData;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.Shop
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShopListPage : ContentPage
    {
        string myShopName = "";
        int bltRow_count = 3;
        int nltRow_count = 3;
        Queue<Grid> titleList_Queue = new Queue<Grid>();
        List<string> shopList = new List<string>();
        List<Grid> ShopTapEventList = new List<Grid>();
        Queue<Grid> SelectList_Queue = new Queue<Grid>();

        ShopDataFunc sdf = new ShopDataFunc();

        public ShopListPage(string name)
        {
            InitializeComponent();
            myShopName = name;
            Init();
        }
        private void Init()
        {
            // 타이틀 탭 초기화
            TitleName.Text = myShopName;
            shopList.Add("단체복");
            shopList.Add("여성의류");
            shopList.Add("남성의류");
            shopList.Add("기프티콘");

            #region 타이틀 탭 바로 밑에 탭 리스트 초기화
            for (int i = 0; i < shopList.Count; i++)
            {
                Grid inGrid = new Grid
                {
                    BackgroundColor = Color.White,
                };
                CustomLabel label = new CustomLabel
                {
                    Text = shopList[i],
                    Size = 18,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                };
                if (label.Text == myShopName)
                {
                    label.TextColor = Color.White;
                    inGrid.BackgroundColor = Color.Black;
                }
                inGrid.Children.Add(label, 0, 0);
                ShopListTap.Children.Add(inGrid, i, 0);

                #region 탭 클릭시 색상 변경 및 밑줄 생성 이벤트
                inGrid.GestureRecognizers.Add(new TapGestureRecognizer()
                {
                    Command = new Command(() =>
                    {
                        if (titleList_Queue.Count < 2)
                        {
                            if (titleList_Queue.Count != 0)
                            {
                                Grid temp_grid = titleList_Queue.Dequeue();
                                CustomLabel temp_label = (CustomLabel)temp_grid.Children.ElementAt(0);
                                temp_label.TextColor = Color.Black;
                                temp_grid.BackgroundColor = Color.White;
                                TitleName.Text = temp_label.Text;
                            }
                            label.TextColor = Color.White;
                            inGrid.BackgroundColor = Color.Black;
                            titleList_Queue.Enqueue(inGrid);
                            TitleName.Text = label.Text;
                        }
                        UpdateList();
                    })
                });
                #endregion
            }
            #endregion


            titleList_Queue.Enqueue((Grid)ShopListTap.Children.ElementAt(0));
            UpdateList();

        }

        private void UpdateList()
        {
            BestShopListGrid.Children.Clear();
            BestShopListGrid.RowDefinitions.Clear();
            NaturalShopListGrid.Children.Clear();
            NaturalShopListGrid.RowDefinitions.Clear();

            #region 베스트 그리드
            // 베스트 라벨
            Grid bestlabel_grid = new Grid
            {
                BackgroundColor = Color.Gray,
                Opacity = 0.1,
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

            for (int i = 1; i <= bltRow_count; i++)
            {
                BestShopListGrid.RowDefinitions.Add(new RowDefinition { Height = 100 });

                // 베스트 리스트 그리드 동적으로 생성
                Grid best_rowGrid = new Grid
                {
                    ColumnDefinitions = {
                    new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(7, GridUnitType.Star)  }
                    },
                };

                // 쇼핑몰 이미지
                Image bestimage = new Image
                {
                    Source = "home.png",
                    BackgroundColor = Color.White,
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
                    Text = "Home" + i,
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
                    Text = "평점 : " + i,
                    Size = 14,
                    TextColor = Color.Black,
                };
                grade_Grid.Children.Add(grade_image, 0, 0);
                grade_Grid.Children.Add(grade_label, 1, 0);
                #endregion

                CustomLabel bestDetail = new CustomLabel
                {
                    Text = "Detail" + i + "ㅇㅁㄴㅇㅁㄴㅇㅁ너웢암나ㅜ엄누어ㅏㅁㄴㅇㅁ나아ㅓㅁㄴ뤄ㅏㄴ아ㅝㄹㅇ나ㅓㄹㄴ아ㅓㄹ",
                    Size = 14,
                    TextColor = Color.Black,
                    MaxLines = 2,
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
                        if (SelectList_Queue.Count < 2)
                        {
                            if (SelectList_Queue.Count != 0)
                            {
                                Grid temp = SelectList_Queue.Dequeue();
                                temp.BackgroundColor = Color.White;
                            }
                            best_rowGrid.BackgroundColor = Color.Gray;
                            best_rowGrid.Opacity = 0.5;
                            SelectList_Queue.Enqueue(best_rowGrid);

                        }
                        Navigation.PushModalAsync(new ShopMainPage(bestHome.Text));
                    })
                });
                #endregion

                BestShopListGrid.Children.Add(best_rowGrid, 0, i);
            }

            #endregion

            #region 일반 쇼핑몰 그리드
            Grid naturallabel_grid = new Grid
            {
                BackgroundColor = Color.Gray,
                Opacity = 0.1,
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

            for (int i = 1; i <= nltRow_count; i++)
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
                Image naturalimage = new Image
                {
                    Source = "home.png",
                    BackgroundColor = Color.White,
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
                    Text = "Home" + i,
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
                    Text = "평점 : " + i,
                    Size = 14,
                    TextColor = Color.Black,
                };
                grade_Grid.Children.Add(grade_image, 0, 0);
                grade_Grid.Children.Add(grade_label, 1, 0);
                #endregion

                CustomLabel naturalDetail = new CustomLabel
                {
                    Text = "Detail" + i + "ㅇㅁㄴㅇㅁㄴㅇㅁ너웢암나ㅜ엄누어ㅏㅁㄴㅇㅁ나아ㅓㅁㄴ뤄ㅏㄴ아ㅝㄹㅇ나ㅓㄹㄴ아ㅓㄹ",
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
                        if (SelectList_Queue.Count < 2)
                        {
                            if (SelectList_Queue.Count != 0)
                            {
                                Grid temp = SelectList_Queue.Dequeue();
                                temp.BackgroundColor = Color.White;
                            }
                            natural_rowGrid.BackgroundColor = Color.Gray;
                            natural_rowGrid.Opacity = 0.5;
                            SelectList_Queue.Enqueue(natural_rowGrid);

                        }
                        Navigation.PushModalAsync(new ShopMainPage(naturalHome.Text));
                    })
                });
                #endregion

                NaturalShopListGrid.Children.Add(natural_rowGrid, 0, i);
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
    }
}