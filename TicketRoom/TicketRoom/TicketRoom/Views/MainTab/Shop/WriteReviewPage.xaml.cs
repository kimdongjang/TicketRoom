using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketRoom.Models.ShopData;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.Shop
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WriteReviewPage : ContentPage
    {
        ShopDBFunc SH_DB = ShopDBFunc.Instance();
        SH_Home home;
        ShopReviewView wrv;

        List<Grid> Grade_Grid = new List<Grid>();
        Queue<Grid> ColorChange_Queue = new Queue<Grid>();
        decimal gradeScore = 0;

        public WriteReviewPage(SH_Home home, ShopReviewView wrv)
        {
            InitializeComponent();
            this.home = home;
            this.wrv = wrv;
            Init();
        }

        private void Init()
        {
            #region IOS의 경우 초기화
            NavigationPage.SetHasNavigationBar(this, false); // Navigation Bar 지우는 코드 생성자에 입력
            if (Device.OS == TargetPlatform.iOS)
            {
                MainGrid.RowDefinitions[0].Height = 50;
            }
            
            if (Global.ios_x_model == true) // ios X 이상의 모델일 경우
            {
                MainGrid.RowDefinitions[3].Height = 30;
            }
            
            #endregion

            Grade_Grid.Add(OneGrade);
            Grade_Grid.Add(TwoGrade);
            Grade_Grid.Add(ThreeGrade);
            Grade_Grid.Add(FourGrade);
            Grade_Grid.Add(FiveGrade);

            for (int i = 0; i < Grade_Grid.Count; i++)
            {
                Grid tempGrid = Grade_Grid[i];
                tempGrid.GestureRecognizers.Add(new TapGestureRecognizer()
                {
                    Command = new Command(() =>
                    {
                        if (tempGrid == OneGrade)
                        {
                            gradeScore = 1;
                        }
                        else if (tempGrid == TwoGrade)
                        {
                            gradeScore = 2;
                        }
                        else if (tempGrid == ThreeGrade)
                        {
                            gradeScore = 3;
                        }
                        else if (tempGrid == FourGrade)
                        {
                            gradeScore = 4;
                        }
                        else if (tempGrid == FiveGrade)
                        {
                            gradeScore = 5;
                        }

                        if (ColorChange_Queue.Count < 2)
                        {
                            if (ColorChange_Queue.Count != 0)
                            {
                                Grid temp = ColorChange_Queue.Dequeue();
                                temp.BackgroundColor = Color.LightBlue;
                            }
                            tempGrid.BackgroundColor = Color.CornflowerBlue;
                            ColorChange_Queue.Enqueue(tempGrid);
                        }
                    })
                });
            }


        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {

        }

        private void Tab_Changed(object sender, EventArgs e)
        {

        }

        private async void ReviewWrite_ClickedAsync(object sender, EventArgs e)
        {
            if(gradeScore == 0) // 리뷰 점수가 선택되지 않았을 경우
            {
                await DisplayAlert("알림", "평점을 선택해주십시오!", "확인");
                return;
            }
            if(InputReview.Text == "")
            {
                await DisplayAlert("알림", "리뷰 내용을 입력해주십시오!", "확인");
                return;
            }

            var isAlert = await DisplayAlert("리뷰 작성", "리뷰를 작성하시겠습니까?", "확인", "취소");
            if (isAlert == false)
            {
                return;
            }

            if(Global.b_user_login == true)
            {
                #region 네트워크 상태 확인
                var current_network = Connectivity.NetworkAccess; // 현재 네트워크 상태
                if (current_network != NetworkAccess.Internet) // 네트워크 연결 불가
                {
                    await DisplayAlert("알림", "네트워크에 연결 할 수 없습니다!", "확인");
                    return;
                }
                #endregion
                #region 네트워크 연결 가능
                else
                {

                    if (SH_DB.PostInsertReviewTohome(home.SH_HOME_INDEX, Math.Round(gradeScore, 1).ToString(), Global.ID, InputReview.Text))
                    {
                        await DisplayAlert("알림", "성공적으로 작성되었습니다!", "확인");
                        wrv.reviewList = SH_DB.PostSearchReviewToHome(home.SH_HOME_INDEX);
                        wrv.Init();
                        ShopReviewView.isOpenPage = false;
                        Navigation.PopAsync();
                    }
                    else
                    {
                        await DisplayAlert("알림", "리뷰 작성에 실패했습니다. 다시 시도해 주십시오!", "확인");
                    }
                }
                #endregion
            }
        }

        private void BackButton_Clicked(object sender, EventArgs e)
        {
            ShopReviewView.isOpenPage = false;
            Navigation.PopAsync();
        }
        protected override bool OnBackButtonPressed()
        {
            ShopReviewView.isOpenPage = false;
            return base.OnBackButtonPressed();

        }
    }
}