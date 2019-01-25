using System;
using System.Collections.Generic;
using TicketRoom.Models.Custom;
using TicketRoom.Models.ShopData;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.Shop
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShopReviewView : ContentView
    {
        public static bool isOpenPage = false;

        ShopDBFunc SH_DB = ShopDBFunc.Instance();
        SH_Home home;
        public List<SH_Review> reviewList = new List<SH_Review>();

        ShopDataFunc dataclass = new ShopDataFunc();

        public ShopReviewView(string titleName, SH_Home home)
        {
            InitializeComponent();
            this.home = home;
            reviewList = SH_DB.PostSearchReviewToHome(home.SH_HOME_INDEX);
            Init();
        }
        public void Init()
        {
            ReviewGrid.Children.Clear();

            for (int i = 0; i < reviewList.Count; i++)
            {
                ReviewGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                // 리뷰 그리드에 내부 그리드 추가
                Grid grid = new Grid
                {
                    RowDefinitions =
                    {
                        new RowDefinition { Height = 1 }, // 구분선
                        new RowDefinition { Height = GridLength.Auto },
                        new RowDefinition { Height = GridLength.Auto },
                        new RowDefinition { Height = GridLength.Auto }
                    },
                    Margin = 1,
                    BackgroundColor = Color.White
                };
                ReviewGrid.Children.Add(grid, 0, i);



                #region 리뷰 에디터 구분선 생성
                BoxView gridbox = new BoxView
                {
                    BackgroundColor = Color.Gray,
                    Opacity = 0.5,
                };
                grid.Children.Add(gridbox, 0, 0);
                #endregion


                Label id_label = new Label
                {
                    Text = "작성자 ID : " + reviewList[i].SH_REVIEW_ID,
                    FontSize = 18,
                    TextColor = Color.Black,
                    Margin = new Thickness(15, 0, 0, 0),
                    VerticalOptions = LayoutOptions.Center
                };
                grid.Children.Add(id_label, 0, 1);

                Grid grade_Grid = new Grid
                {
                    ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = 15 },
                        new ColumnDefinition { Width = GridLength.Auto },
                    },
                    Margin = new Thickness(15, 0, 0, 0),
                    VerticalOptions = LayoutOptions.Center
                };
                Image grade_image = new Image
                {
                    Source = "star.png",
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };
                Label grade_label = new Label
                {
                    Text = "평점 : " + reviewList[i].SH_REVIEW_GRADE,
                    FontSize = 18,
                    TextColor = Color.Black,
                };
                grade_Grid.Children.Add(grade_image, 0, 0);
                grade_Grid.Children.Add(grade_label, 1, 0);
                grid.Children.Add(grade_Grid, 0, 2);

                #region 리뷰 에디터 테두리 생성
                Grid border_Grid = new Grid
                {
                    Margin = 5,
                    RowDefinitions = {
                        new RowDefinition { Height = GridLength.Auto }
                    }
                };

                BoxView boxview = new BoxView
                {
                    BackgroundColor = Color.Black,
                };
                StackLayout border_Stack = new StackLayout
                {
                    BackgroundColor = Color.White,
                    Margin = 1
                };
                border_Grid.Children.Add(boxview);
                border_Grid.Children.Add(border_Stack);

                grid.Children.Add(border_Grid, 0, 3);
                #endregion

                CustomLabel review_editor = new CustomLabel
                {
                    Text = reviewList[i].SH_REVIEW_CONTENT,
                    Size = 18,
                    TextColor = Color.Black,
                    IsEnabled = false,
                };

                border_Stack.Children.Add(review_editor);
            }
        }

        private void WriteReview_btn_Clicked(object sender, EventArgs e)
        {
            if (ShopReviewView.isOpenPage == true)
            {
                return;
            }
            ShopReviewView.isOpenPage = true;
            Navigation.PushModalAsync(new WriteReviewPage(home, this));
        }
    }
}