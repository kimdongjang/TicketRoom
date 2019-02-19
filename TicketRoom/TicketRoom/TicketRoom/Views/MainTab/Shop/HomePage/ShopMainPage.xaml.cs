﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using TicketRoom.Models.Custom;
using TicketRoom.Models.ShopData;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.Shop
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShopMainPage : ContentPage
    {
        ShopDBFunc SH_DB = ShopDBFunc.Instance();
        SH_Home home;

        Uri instaUri;
        Uri webUri;

        ShopSaleView ssv;
        ShopInfoView siv;
        ShopReviewView srv;

        List<Button> tablist = new List<Button>();
        Queue<CustomButton> SelectTap_Queue = new Queue<CustomButton>();
        CustomButton selectedtab;

        public ShopMainPage(int home_index)
        {
            InitializeComponent();
            Global.isOpen_ShopOtherPage = false; // 다른 고객이 본 상품을 클릭했을 경우 false처리를 더해야 연속해서 창을 볼 수 있다.
            Global.isOpen_ShopDetailPage = false;
            home = SH_DB.PostSearchHomeToHome(home_index);
            Init();
        }

        // DB에서 가져온 홈 페이지 정보로 초기화 진행
        private void Init()
        {
            #region IOS의 경우 초기화
            NavigationPage.SetHasNavigationBar(this, false); // Navigation Bar 지우는 코드 생성자에 입력
            if (Device.OS == TargetPlatform.iOS)
            {
                MainGrid.RowDefinitions[0].Height = 50;
            }
            #endregion
            BackButtonImage.Source = ImageSource.FromUri(new Uri("http://221.141.58.49:8088/img/default/backbutton_icon.png"));

            // 타이틀 탭 초기화
            TitleName.Text = home.SH_HOME_NAME;

            // 리스트에 버튼 추가
            tablist.Add(Content_Sale);
            tablist.Add(Content_Info);
            tablist.Add(Content_Review);

            MainShopImage.Source = ImageSource.FromUri(new Uri(home.SH_HOME_IMAGE));
            MainShopGrade.Text = home.SH_HOME_GRADE.ToString();
            MainShopDelivery.Text = "무료 배송 금액 : " + home.SH_HOME_FREEDELEVERY.ToString("N0") + "원";
            MainShopPay.Text = "결제 방법 : " + home.SH_HOME_PAYWAY;
            instaUri = new Uri(home.SH_HOME_INSTA);
            webUri = new Uri(home.SH_HOME_WEB);


            Content_Changed(tablist[0], null); // Default로 보여질 콘텐츠 뷰
        }



        /// <summary>
        /// 콘텐츠 뷰 변경 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Content_Changed(object sender, EventArgs e)
        {
            selectedtab = (CustomButton)sender;

            if (SelectTap_Queue.Count < 2)
            {
                if (SelectTap_Queue.Count != 0)
                {
                    CustomButton temp = SelectTap_Queue.Dequeue();
                    temp.TextColor = Color.White;
                    temp.BackgroundColor = Color.LightBlue;
                }
                selectedtab.TextColor = Color.LightBlue;
                selectedtab.BackgroundColor = Color.White;
                SelectTap_Queue.Enqueue(selectedtab);
            }

            if (selectedtab.Text.Equals("판매품"))
            {
                ShopContentView.Content = ssv = new ShopSaleView(TitleName.Text, home);
            }
            else if (selectedtab.Text.Equals("정보"))
            {
                ShopContentView.Content = siv = new ShopInfoView(TitleName.Text, home);
            }
            else if (selectedtab.Text.Equals("리뷰"))
            {
                ShopContentView.Content = srv = new ShopReviewView(TitleName.Text, home);
            }

        }

        private void Tab_Changed(object sender, EventArgs e)
        {

        }

        private void CallBtn_Clicked(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri("tel:010-9257-8836"));
        }

        private void BasketBtn_Clicked(object sender, EventArgs e)
        {

        }

        private void Insta_btn_Clicked(object sender, EventArgs e)
        {
            Device.OpenUri(instaUri);
        }

        private void MoveShop_btn_Clicked(object sender, EventArgs e)
        {
            Device.OpenUri(webUri);
        }

        private void BackButton_Clicked(object sender, EventArgs e)
        {
            Global.isOpen_ShopMainPage = false;
            Global.isOpen_ShopOtherPage = false;
            Global.isOpen_ShopDetailPage = false;
            
            Navigation.PopAsync();
        }
        protected override bool OnBackButtonPressed()
        {
            if (ShopContentView.Content != ssv) // 컨텐츠뷰가 메인으로 활성화 되어있지 않으면 메인으로 활성화 시킴
            {
                // 선택 탭 컬러 변경
                if (SelectTap_Queue.Count < 2)
                {
                    if (SelectTap_Queue.Count != 0)
                    {
                        CustomButton temp = SelectTap_Queue.Dequeue();
                        temp.TextColor = Color.Black;
                        temp.BackgroundColor = Color.White;
                    }
                    Content_Sale.TextColor = Color.White;
                    Content_Sale.BackgroundColor = Color.Black;
                    SelectTap_Queue.Enqueue(Content_Sale);
                }
                ShopContentView.Content = ssv = new ShopSaleView(TitleName.Text, home);
                return true;
            }
            else
            {
                Global.isOpen_ShopMainPage = false;
                Global.isOpen_ShopOtherPage = false;
                Global.isOpen_ShopDetailPage = false;
                return base.OnBackButtonPressed();
            }
        }
    }
}