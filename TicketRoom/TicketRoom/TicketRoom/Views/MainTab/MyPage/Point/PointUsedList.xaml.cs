﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketRoom.Models.Custom;
using TicketRoom.Models.PointData;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.MyPage.Point
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PointUsedList : ContentView
    {
        PointDBFunc PT_DB = PointDBFunc.Instance();

        PointCheckPage pcp;
        PT_Point pp;
        int MyPoint = 0; // 내 보유 포인트


        List<PT_WithDraw> wdl = new List<PT_WithDraw>();

        public PointUsedList (PointCheckPage pcp, PT_Point pp)
		{
			InitializeComponent ();
            this.pcp = pcp;
            this.pp = pp;

            wdl = PT_DB.PostSearchWithDrawListToID(pp.USER_ID);
            MyPoint = pp.PT_POINT_HAVEPOINT;
            MyPointLabel.Text = "보유 포인트 : " + MyPoint.ToString();
            Init();

        }

        private void Init()
        {
            for (int i = 0; i < wdl.Count; i++)
            {
                MainGrid.RowDefinitions.Add(new RowDefinition { Height = 100 });

                // 박스의 구분선 생성
                BoxView BorderLine1 = new BoxView { BackgroundColor = Color.IndianRed, };
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
                        new RowDefinition { Height = GridLength.Auto }    
                    },

                };
                #region 상품권 그리드 자식 추가
                // 메인 리스트 그리드에 추가 
                MainGrid.Children.Add(inGrid, 0, i);

                inGrid.Children.Add(point_image, 0, 0);
                inGrid.Children.Add(point_label_grid, 1, 0);
                #endregion

                #region 사용 포인트 Label
                CustomLabel use_label = new CustomLabel
                {
                    Text = wdl[i].PT_WITHDRAW_POINT.ToString()/*value*/ + "포인트 사용",
                    Size = 18,
                    TextColor = Color.Black,
                };
                #endregion

                #region 사용 날짜 Label
                CustomLabel date_label = new CustomLabel
                {
                    Text = "일시 : " + wdl[i].PT_WITHDRAW_DATE/*date*/,
                    Size = 14,
                    TextColor = Color.DarkGray,
                };
                #endregion

                #region 사용 내역 Label 
                CustomLabel content_label = new CustomLabel
                {
                    Text = "내역 : " + wdl[i].PT_WITHDRAW_CONTENT + " \n[" + wdl[i].PT_WITHDRAW_BANK + "]" +
                            " \n[계좌번호:" + wdl[i].PT_WITHDRAW_ACCOUNT + "]" + "[예금주:" + wdl[i].PT_WITHDRAW_NAME + "]",
                    Size = 14,
                    TextColor = Color.Gray,
                };
                #endregion

                //상품 설명 라벨 그리드에 추가
                point_label_grid.Children.Add(use_label, 0, 0);
                point_label_grid.Children.Add(date_label, 0, 1);
                point_label_grid.Children.Add(content_label, 0, 3);
                #endregion



            }
        }
    }
}