﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TicketRoom.Models.Custom;
using TicketRoom.Models.Gift.PurchaseList;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.MyPage.PurchaseList
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PurchaseDetailListGift : ContentPage
    {
        List<G_PLInfo> pdlist = new List<G_PLInfo>();

        List<PLProInfo> productlist = new List<PLProInfo>();
        string pl_index = "";
        private object purchaseList;
        int delivery_pay = 3000; //배송비

        public PurchaseDetailListGift(string pl_index)
        {
            InitializeComponent();

            SearchPurchaseDetailToPlNum(pl_index);
            SearchPurchaseListToPlNum(pl_index);
            this.pl_index = pl_index;
            Init();
        }

        // 유저 아이디를 통해 상품권 구매리스트 가져오기
        public void SearchPurchaseDetailToPlNum(string pl_num)
        {
            string str = @"{";
            str += "plnum : '" + pl_num;
            str += "'}";

            //// JSON 문자열을 파싱하여 JObject를 리턴
            JObject jo = JObject.Parse(str);

            UTF8Encoding encoder = new UTF8Encoding();
            byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "SearchPurchaseDetailToPlnum") as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = data.Length;

            request.GetRequestStream().Write(data, 0, data.Length);


            try
            {
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {

                    if (response.StatusCode != HttpStatusCode.OK)
                        Console.Out.WriteLine("Error fetching data. Server returned status code: {0}", response.StatusCode);
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {

                        // readdata
                        var readdata = reader.ReadToEnd();
                        if(readdata!=null && readdata != "")
                        {
                            pdlist = JsonConvert.DeserializeObject<List<G_PLInfo>>(readdata);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }

        public void SearchPurchaseListToPlNum(string pl_num)
        {
            //구매내역 가져오기
            string str = @"{";
            str += "plnum : '" + pl_num;
            str += "'}";

            //// JSON 문자열을 파싱하여 JObject를 리턴
            JObject jo = JObject.Parse(str);

            UTF8Encoding encoder = new UTF8Encoding();
            byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "SearchPurchaseListToPlnum") as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = data.Length;

            request.GetRequestStream().Write(data, 0, data.Length);


            try
            {
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {

                    if (response.StatusCode != HttpStatusCode.OK)
                        Console.Out.WriteLine("Error fetching data. Server returned status code: {0}", response.StatusCode);
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        // readdata
                        var readdata = reader.ReadToEnd();
                        if (readdata != null && readdata != "")
                        {
                            productlist = JsonConvert.DeserializeObject<List<PLProInfo>>(readdata);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }

        private void Init()
        {

            Grid coverGrid = new Grid { RowSpacing = 0 };
            MainGrid.Children.Add(coverGrid, 0, 0); // 메인 그리드 추가

            #region 주문번호
            CustomLabel order_numLabel = new CustomLabel
            {
                Text = "주문 번호 : " + pl_index,
                Size = 18,
                TextColor = Color.Black,
                Margin = new Thickness(15, 0, 0, 0),
                VerticalOptions = LayoutOptions.CenterAndExpand,
            };
            coverGrid.RowDefinitions.Add(new RowDefinition { Height = 40 });
            coverGrid.Children.Add(order_numLabel, 0, 0);
            #endregion


            BoxView borderLine1 = new BoxView { BackgroundColor = Color.LightGray };
            coverGrid.RowDefinitions.Add(new RowDefinition { Height = 1 });
            coverGrid.Children.Add(borderLine1, 0, 1);

            #region 상품 이름
            Grid nameGrid = new Grid
            {
                ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) },
                        new ColumnDefinition { Width = new GridLength(7, GridUnitType.Star) },
                    },
                RowSpacing = 0,
            };
            BoxView nameLine = new BoxView { BackgroundColor = Color.LightGray };
            StackLayout nameCover = new StackLayout { BackgroundColor = Color.White, Margin = 1 };
            CustomLabel nameLabel = new CustomLabel
            {
                Text = "상품 이름",
                Size = 14,
                TextColor = Color.DarkGray,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };
            CustomLabel input_nameLabel = new CustomLabel
            {
                Text = productlist[0].PRODUCTTYPE+ " " +productlist[0].PRODUCTVALUE+" 외 "+ (productlist.Count-1)+ "개",
                Size = 14,
                TextColor = Color.DarkGray,
                VerticalOptions = LayoutOptions.CenterAndExpand,
            };
            nameGrid.Children.Add(nameLine, 0, 0);
            nameGrid.Children.Add(nameCover, 0, 0);
            nameCover.Children.Add(nameLabel);
            nameGrid.Children.Add(input_nameLabel, 1, 0);

            coverGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            coverGrid.Children.Add(nameGrid, 0, 2);
            BoxView borderLine2 = new BoxView { BackgroundColor = Color.LightGray };
            coverGrid.RowDefinitions.Add(new RowDefinition { Height = 1 });
            coverGrid.Children.Add(borderLine2, 0, 3);
            #endregion


            #region 배송비 선불/착불
            Grid delivery_priceGrid = new Grid
            {
                ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) },
                        new ColumnDefinition { Width = new GridLength(7, GridUnitType.Star) },
                    },
                RowSpacing = 0,
            };
            BoxView delivery_priceLine = new BoxView { BackgroundColor = Color.LightGray };
            StackLayout delivery_priceCover = new StackLayout { BackgroundColor = Color.White, Margin = 1 };
            CustomLabel delivery_priceLabel = new CustomLabel
            {
                Text = "배송비",
                Size = 14,
                TextColor = Color.DarkGray,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };
            CustomLabel input_delivery_priceLabel = null;
            if (pdlist[0].PL_DELIVERYPAY_TYPE.Equals("1"))
            {
                input_delivery_priceLabel = new CustomLabel
                {
                    Text = delivery_pay + "원 / 선불",
                    Size = 14,
                    TextColor = Color.DarkGray,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                };
            }
            else
            {
                input_delivery_priceLabel = new CustomLabel
                {
                    Text = "3000원 / 후불",
                    Size = 14,
                    TextColor = Color.DarkGray,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                };
            }
            
            delivery_priceGrid.Children.Add(delivery_priceLine, 0, 0);
            delivery_priceGrid.Children.Add(delivery_priceCover, 0, 0);
            delivery_priceCover.Children.Add(delivery_priceLabel);
            delivery_priceGrid.Children.Add(input_delivery_priceLabel, 1, 0);

            coverGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            coverGrid.Children.Add(delivery_priceGrid, 0, 4);
            BoxView borderLine3 = new BoxView { BackgroundColor = Color.LightGray };
            coverGrid.RowDefinitions.Add(new RowDefinition { Height = 1 });
            coverGrid.Children.Add(borderLine3, 0, 5);
            #endregion

            #region 배송지
            Grid delivery_adressGrid = new Grid
            {
                ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) },
                        new ColumnDefinition { Width = new GridLength(7, GridUnitType.Star) },
                    },
                RowSpacing = 0,
            };
            BoxView delivery_adressLine = new BoxView { BackgroundColor = Color.LightGray };
            StackLayout delivery_adressCover = new StackLayout { BackgroundColor = Color.White, Margin = 1 };
            CustomLabel delivery_adressLabel = new CustomLabel
            {
                Text = "배송지",
                Size = 14,
                TextColor = Color.DarkGray,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };
            CustomLabel input_delivery_adressLabel = new CustomLabel
            {
                Text = pdlist[0].PL_DELIVERY_ADDRESS,
                Size = 14,
                TextColor = Color.DarkGray,
                VerticalOptions = LayoutOptions.CenterAndExpand,
            };
            delivery_adressGrid.Children.Add(delivery_adressLine, 0, 0);
            delivery_adressGrid.Children.Add(delivery_adressCover, 0, 0);
            delivery_adressCover.Children.Add(delivery_adressLabel);
            delivery_adressGrid.Children.Add(input_delivery_adressLabel, 1, 0);

            coverGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            coverGrid.Children.Add(delivery_adressGrid, 0, 6);
            BoxView borderLine4 = new BoxView { BackgroundColor = Color.LightGray };
            coverGrid.RowDefinitions.Add(new RowDefinition { Height = 1 });
            coverGrid.Children.Add(borderLine4, 0, 7);
            #endregion
            
            #region 배송 연락번호
            Grid delivery_phoneGrid = new Grid
            {
                ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) },
                        new ColumnDefinition { Width = new GridLength(7, GridUnitType.Star) },
                    },
                RowSpacing = 0,
            };
            BoxView delivery_phoneLine = new BoxView { BackgroundColor = Color.LightGray };
            StackLayout delivery_phoneCover = new StackLayout { BackgroundColor = Color.White, Margin = 1 };
            CustomLabel delivery_phoneLabel = new CustomLabel
            {
                Text = "연락처",
                Size = 14,
                TextColor = Color.DarkGray,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };
            CustomLabel input_delivery_phoneLabel = new CustomLabel
            {
                Text = pdlist[0].PL_DV_PHONE,
                Size = 14,
                TextColor = Color.DarkGray,
                VerticalOptions = LayoutOptions.CenterAndExpand,
            };
            delivery_phoneGrid.Children.Add(delivery_phoneLine, 0, 0);
            delivery_phoneGrid.Children.Add(delivery_phoneCover, 0, 0);
            delivery_phoneCover.Children.Add(delivery_phoneLabel);
            delivery_phoneGrid.Children.Add(input_delivery_phoneLabel, 1, 0);

            coverGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            coverGrid.Children.Add(delivery_phoneGrid, 0, 8);
            BoxView borderLine5 = new BoxView { BackgroundColor = Color.LightGray };
            coverGrid.RowDefinitions.Add(new RowDefinition { Height = 1 });
            coverGrid.Children.Add(borderLine5, 0, 9);
            #endregion

            #region 결제금액
            Grid pay_priceGrid = new Grid
            {
                ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) },
                        new ColumnDefinition { Width = new GridLength(7, GridUnitType.Star) },
                    },
                RowSpacing = 0,
            };
            BoxView pay_priceLine = new BoxView { BackgroundColor = Color.LightGray };
            StackLayout pay_priceCover = new StackLayout { BackgroundColor = Color.White, Margin = 1 };
            CustomLabel pay_priceLabel = new CustomLabel
            {
                Text = "결제금액",
                Size = 14,
                TextColor = Color.DarkGray,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };
            CustomLabel input_pay_priceLabel = new CustomLabel
            {
                Text = int.Parse(pdlist[0].PL_PAYMENT_PRICE).ToString("N0"),
                Size = 14,
                TextColor = Color.DarkGray,
                VerticalOptions = LayoutOptions.CenterAndExpand,
            };
            pay_priceGrid.Children.Add(pay_priceLine, 0, 0);
            pay_priceGrid.Children.Add(pay_priceCover, 0, 0);
            pay_priceCover.Children.Add(pay_priceLabel);
            pay_priceGrid.Children.Add(input_pay_priceLabel, 1, 0);

            coverGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            coverGrid.Children.Add(pay_priceGrid, 0, 10);

            BoxView borderLine11 = new BoxView { BackgroundColor = Color.LightGray };
            coverGrid.RowDefinitions.Add(new RowDefinition { Height = 1 });
            coverGrid.Children.Add(borderLine11, 0, 11);
            #endregion

            #region 사용된 포인트
            Grid pay_pointGrid = new Grid
            {
                ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) },
                        new ColumnDefinition { Width = new GridLength(7, GridUnitType.Star) },
                    },
                RowSpacing = 0,
            };
            BoxView pay_pointLine = new BoxView { BackgroundColor = Color.LightGray };
            StackLayout pay_pointCover = new StackLayout { BackgroundColor = Color.White, Margin = 1 };
            CustomLabel pay_pointLabel = new CustomLabel
            {
                Text = "사용포인트",
                Size = 14,
                TextColor = Color.DarkGray,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };
            CustomLabel input_pay_pointLabel = new CustomLabel
            {
                Text = pdlist[0].PL_USED_POINT.ToString() + " point",
                Size = 14,
                TextColor = Color.DarkGray,
                VerticalOptions = LayoutOptions.CenterAndExpand,
            };
            pay_pointGrid.Children.Add(pay_pointLine, 0, 0);
            pay_pointGrid.Children.Add(pay_pointCover, 0, 0);
            pay_pointCover.Children.Add(pay_pointLabel);
            pay_pointGrid.Children.Add(input_pay_pointLabel, 1, 0);

            coverGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            coverGrid.Children.Add(pay_pointGrid, 0, 12);

            BoxView borderLine12 = new BoxView { BackgroundColor = Color.LightGray };
            coverGrid.RowDefinitions.Add(new RowDefinition { Height = 1 });
            coverGrid.Children.Add(borderLine12, 0, 13);
            #endregion

            #region 결제상태
            Grid pay_stateGrid = new Grid
            {
                ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) },
                        new ColumnDefinition { Width = new GridLength(7, GridUnitType.Star) },
                    },
                RowSpacing = 0,
            };
            BoxView pay_stateLine = new BoxView { BackgroundColor = Color.LightGray };
            StackLayout pay_stateCover = new StackLayout { BackgroundColor = Color.White, Margin = 1 };
            CustomLabel pay_stateLabel = new CustomLabel
            {
                Text = "결제상태",
                Size = 14,
                TextColor = Color.DarkGray,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };

            CustomLabel input_pay_stateLabel = null;
            if (pdlist[0].PL_ISSUCCESS.Equals("1"))
            {
                input_pay_stateLabel = new CustomLabel
                {
                    Text = "구매완료",
                    Size = 14,
                    TextColor = Color.DarkGray,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                };
            }
            else if (pdlist[0].PL_ISSUCCESS.Equals("2"))
            {
                input_pay_stateLabel = new CustomLabel
                {
                    Text = "구매실패 (수량 부족)",
                    Size = 14,
                    TextColor = Color.DarkGray,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                };
            }
            else if (pdlist[0].PL_ISSUCCESS.Equals("3"))
            {
                input_pay_stateLabel = new CustomLabel
                {
                    Text = "구매중",
                    Size = 14,
                    TextColor = Color.DarkGray,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                };
            }
            
            pay_stateGrid.Children.Add(pay_stateLine, 0, 0);
            pay_stateGrid.Children.Add(pay_stateCover, 0, 0);
            pay_stateCover.Children.Add(pay_stateLabel);
            pay_stateGrid.Children.Add(input_pay_stateLabel, 1, 0);

            coverGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            coverGrid.Children.Add(pay_stateGrid, 0, 14);

            BoxView borderLine13 = new BoxView { BackgroundColor = Color.LightGray };
            coverGrid.RowDefinitions.Add(new RowDefinition { Height = 1 });
            coverGrid.Children.Add(borderLine13, 0, 15);
            #endregion

        }

        private void PayOptionCondition(string option)
        {

        }

        private void ConfirmBtn_Clicked(object sender, EventArgs e)
        {

            PurchaseListPage.isOpenPage = false;
            Navigation.PopModalAsync();
        }

        private void ImageButton_Clicked(object sender, EventArgs e) // 백버튼 이미지
        {

            PurchaseListPage.isOpenPage = false;
            Navigation.PopModalAsync();
        }
        protected override bool OnBackButtonPressed()
        {
            PurchaseListPage.isOpenPage = false;
            return base.OnBackButtonPressed();
        }
    }
}