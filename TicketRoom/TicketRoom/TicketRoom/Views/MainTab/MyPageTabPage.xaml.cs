﻿using System;
using TicketRoom.Views.MainTab.MyPage;
using TicketRoom.Views.Users.Login;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyPageTabPage : ContentView
    {
        public MyPageTabPage()
        {
            InitializeComponent();
            Init();
        }


        private void Init()
        { 
            if (Global.b_user_login == true)
            {
                UserIDLabel.Text =  Global.ID;
                IsLoginBtn.Text = "로그아웃";                
            }
            else if (Global.b_user_login == false)
            {
                UserIDLabel.Text = "티켓룸아이디#" + Global.non_user_id;
                IsLoginBtn.Text = "로그인";
            }
        }
        private async void MyInfoUpdate_Clicked(object sender, EventArgs e)
        {
            if (Global.b_user_login == false)
            {
                await App.Current.MainPage.DisplayAlert("알림", "로그인 후에 이용이 가능합니다.", "확인");
                return;
            }
            await Navigation.PushModalAsync(new MyInfoUpdatePage());
        }

        private async void SaleList_Clicked(object sender, EventArgs e)
        {
            if (Global.b_user_login == false)
            {
                await App.Current.MainPage.DisplayAlert("알림", "로그인 후에 이용이 가능합니다.", "확인");
                return;
            }
            await Navigation.PushModalAsync(new SaleListPage());
        }

        private void PurchaseList_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new PurchaseListPage());
        }

        private async void PointCheck_Clicked(object sender, EventArgs e)
        {
            if (Global.b_user_login == false)
            {
                await App.Current.MainPage.DisplayAlert("알림", "로그인 후에 이용이 가능합니다.", "확인");
                return;
            }
            await Navigation.PushModalAsync(new PointCheckPage());
        }

        private async void IsLoginBtn_ClickedAsync(object sender, EventArgs e)
        {
            if (Global.b_user_login == true) // 로그아웃 버튼
            {
                if(await App.Current.MainPage.DisplayAlert("알림", "로그아웃 하시겠습니까?", "확인", "취소") == true)
                {
                    Global.b_user_login = false;
                    Global.ID = "";
                    await App.Current.MainPage.DisplayAlert("알림", "성공적으로 로그아웃 되었습니다.", "확인");
                    Init();
                }                
            }
            else if (Global.b_user_login == false)
            {
                await Navigation.PushModalAsync(new LoginPage()); // 로그인 페이지로 이동
            }
            
        }

        private void IsLoginBtn_Clicked(object sender, EventArgs e)
        {

        }
    }
}