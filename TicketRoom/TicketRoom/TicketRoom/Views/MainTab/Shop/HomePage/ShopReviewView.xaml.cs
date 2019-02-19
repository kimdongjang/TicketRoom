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

            if(reviewList.Count == 0)
            {
                CustomLabel errorLabel = new CustomLabel
                {
                    Text = "작성된 리뷰가 없습니다.",
                    Size = 18,
                    TextColor = Color.Gray,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                };
                ReviewGrid.Children.Add(errorLabel);
                return;
            }

            for (int i = 0; i < reviewList.Count; i++)
            {
                ReviewGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                #region 리뷰 메인 그리드
                BoxView borderLine = new BoxView { BackgroundColor = Color.LightGray, };
                // 리뷰 그리드에 내부 그리드 추가
                Grid grid = new Grid
                {
                    RowDefinitions =
                    {
                        new RowDefinition { Height = GridLength.Auto },
                        new RowDefinition { Height = GridLength.Auto },
                        new RowDefinition { Height = GridLength.Auto }
                    },
                    Margin = 3,
                    BackgroundColor = Color.White,
                    RowSpacing = 2,
                };
                ReviewGrid.Children.Add(borderLine, 0, i);
                ReviewGrid.Children.Add(grid, 0, i);
                #endregion

                #region 작성자 ID 그리드

                Grid id_Grid = new Grid
                {
                    ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = GridLength.Auto },
                        new ColumnDefinition { Width = GridLength.Auto },
                        new ColumnDefinition { Width = GridLength.Star },
                        new ColumnDefinition { Width = GridLength.Auto },
                    },
                    Margin = new Thickness(15, 15, 5, 5),
                    ColumnSpacing = 5,
                };
                CustomLabel id_btn = new CustomLabel
                {
                    Text = "작성자 ID",
                    Size = 16,
                    BackgroundColor = Color.LightBlue,
                    TextColor = Color.White,
                    HeightRequest = 30,
                    WidthRequest = 80,
                    VerticalOptions = LayoutOptions.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    HorizontalTextAlignment = TextAlignment.Center,
                };
                CustomLabel id_label = new CustomLabel
                {
                    Text = reviewList[i].SH_REVIEW_ID,
                    Size = 14,
                    TextColor = Color.Gray,
                    VerticalOptions = LayoutOptions.Center
                };

                CustomLabel date_label = new CustomLabel
                {
                    Text = reviewList[i].SH_REVIEW_DATE,
                    Size = 14,
                    TextColor = Color.Gray,
                    VerticalOptions = LayoutOptions.Center
                };
                id_Grid.Children.Add(id_btn, 0, 0);
                id_Grid.Children.Add(id_label, 1, 0);
                id_Grid.Children.Add(date_label, 3, 0);
                grid.Children.Add(id_Grid, 0, 0);

                #endregion

                #region 평점 그리드 
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
                CustomLabel grade_label = new CustomLabel
                {
                    Text = "평점 : " + reviewList[i].SH_REVIEW_GRADE.ToString("#.#"),
                    Size = 14,
                    TextColor = Color.Black,
                };
                grade_Grid.Children.Add(grade_image, 0, 0);
                grade_Grid.Children.Add(grade_label, 1, 0);
                grid.Children.Add(grade_Grid, 0, 1);
                #endregion

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
                    BackgroundColor = Color.LightGray,
                };
                StackLayout border_Stack = new StackLayout
                {
                    BackgroundColor = Color.White,
                    Margin = 1,
                    Padding = 10,
                };
                border_Grid.Children.Add(boxview);
                border_Grid.Children.Add(border_Stack);

                grid.Children.Add(border_Grid, 0, 2);
                #endregion

                CustomLabel review_editor = new CustomLabel
                {
                    Text = reviewList[i].SH_REVIEW_CONTENT,
                    Size = 14,
                    TextColor = Color.Gray,
                    IsEnabled = false,
                };

                border_Stack.Children.Add(review_editor);
            }
        }

        private async void WriteReview_btn_Clicked(object sender, EventArgs e)
        {
            if(Global.b_user_login == false)
            {
               await App.Current.MainPage.DisplayAlert("알림", "비회원은 리뷰 작성을 할 수 없습니다!", "확인");
                return;
            }
            if (ShopReviewView.isOpenPage == true)
            {
                return;
            }
            ShopReviewView.isOpenPage = true;
            await Navigation.PushAsync(new WriteReviewPage(home, this));
        }
    }
}