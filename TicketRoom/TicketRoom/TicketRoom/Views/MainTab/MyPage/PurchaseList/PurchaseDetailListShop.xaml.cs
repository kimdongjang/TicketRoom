using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketRoom.Models.Custom;
using TicketRoom.Models.ShopData;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.MyPage.PurchaseList
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PurchaseDetailListShop : ContentPage
    {
        ShopDBFunc SH_DB = ShopDBFunc.Instance();

        List<SH_Pur_Delivery> pdList = new List<SH_Pur_Delivery>();
        List<SH_Pur_Pay> ppList = new List<SH_Pur_Pay>();
        List<SH_Pur_Product> proList = new List<SH_Pur_Product>();
        string pl_index = "";

        public PurchaseDetailListShop (string pl_index)
		{
			InitializeComponent ();
            // 구매 목록 인덱스를 통해 배송 관련 리스트 가져오기
            pdList = SH_DB.PostSearchPurchaseDeliveryListToIndex(pl_index);
            // 구매 목록 인덱스를 통해 결제 관련 리스트 가져오기
            ppList = SH_DB.PostSearchPurchasePayListToIndex(pl_index);
            proList = SH_DB.PostSearchPurchaseProductListToIndex(pl_index);
            this.pl_index = pl_index;
            Init();
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
                Text = proList[0].SH_PUR_PRODUCT_NAME + " 외 " + proList.Count + "개",
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
            CustomLabel input_delivery_priceLabel = new CustomLabel
            {
                Text = pdList[0].SH_PUR_DELIVERY_PAY.ToString("N0") + "원 / " + pdList[0].SH_PUR_DELIVERY_OPTION,
                Size = 14,
                TextColor = Color.DarkGray,
                VerticalOptions = LayoutOptions.CenterAndExpand,
            };
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
                Text = pdList[0].SH_PUR_DELIVERY_ADRESS,
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
                Text = pdList[0].SH_PUR_DELIVERY_PHONE,
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


            #region 배송 요청사항
            Grid delivery_detailGrid = new Grid
            {
                ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) },
                        new ColumnDefinition { Width = new GridLength(7, GridUnitType.Star) },
                    },
                RowSpacing = 0,
            };
            BoxView delivery_detailLine = new BoxView { BackgroundColor = Color.LightGray };
            StackLayout delivery_detailCover = new StackLayout { BackgroundColor = Color.White, Margin = 1 };
            CustomLabel delivery_detaileLabel = new CustomLabel
            {
                Text = "배송요청사항",
                Size = 14,
                TextColor = Color.DarkGray,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };
            CustomLabel input_delivery_detailLabel = new CustomLabel
            {
                Text = pdList[0].SH_PUR_DELIVERY_DETAIL,
                Size = 14,
                TextColor = Color.DarkGray,
                VerticalOptions = LayoutOptions.CenterAndExpand,
            };
            delivery_detailGrid.Children.Add(delivery_detailLine, 0, 0);
            delivery_detailGrid.Children.Add(delivery_detailCover, 0, 0);
            delivery_detailCover.Children.Add(delivery_detaileLabel);
            delivery_detailGrid.Children.Add(input_delivery_detailLabel, 1, 0);

            coverGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            coverGrid.Children.Add(delivery_detailGrid, 0, 10);

            BoxView borderLine6 = new BoxView { BackgroundColor = Color.LightGray };
            coverGrid.RowDefinitions.Add(new RowDefinition { Height = 1 });
            coverGrid.Children.Add(borderLine6, 0, 11);
            #endregion

            #region 결제방식
            Grid pay_optionGrid = new Grid
            {
                ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) },
                        new ColumnDefinition { Width = new GridLength(7, GridUnitType.Star) },
                    },
                RowSpacing = 0,
            };
            BoxView pay_optionLine = new BoxView { BackgroundColor = Color.LightGray };
            StackLayout pay_optionCover = new StackLayout { BackgroundColor = Color.White, Margin = 1 };
            CustomLabel pay_optionLabel = new CustomLabel
            {
                Text = "결제방식",
                Size = 14,
                TextColor = Color.DarkGray,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };
            CustomLabel input_pay_optionLabel = new CustomLabel
            {
                Text = ppList[0].SH_PUR_PAY_OPTION,
                Size = 14,
                TextColor = Color.DarkGray,
                VerticalOptions = LayoutOptions.CenterAndExpand,
            };
            pay_optionGrid.Children.Add(pay_optionLine, 0, 0);
            pay_optionGrid.Children.Add(pay_optionCover, 0, 0);
            pay_optionCover.Children.Add(pay_optionLabel);
            pay_optionGrid.Children.Add(input_pay_optionLabel, 1, 0);

            coverGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            coverGrid.Children.Add(pay_optionGrid, 0, 12);

            BoxView borderLine7 = new BoxView { BackgroundColor = Color.LightGray };
            coverGrid.RowDefinitions.Add(new RowDefinition { Height = 1 });
            coverGrid.Children.Add(borderLine7, 0, 13);
            #endregion

            #region 결제방식에 따른 레이블(카드, 은행, 핸드폰 등)
            Grid pay_bank_phone_Grid = new Grid
            {
                ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) },
                        new ColumnDefinition { Width = new GridLength(7, GridUnitType.Star) },
                    },
                RowSpacing = 0,
            };
            BoxView pay_bank_phone_Line = new BoxView { BackgroundColor = Color.LightGray };
            StackLayout pay_bank_phone_Cover = new StackLayout { BackgroundColor = Color.White, Margin = 1 };
            CustomLabel pay_bank_phone_Label = new CustomLabel
            {
                Text = "",
                Size = 14,
                TextColor = Color.DarkGray,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };
            CustomLabel input_bank_phone_Label = new CustomLabel
            {
                Text = "",
                Size = 14,
                TextColor = Color.DarkGray,
                VerticalOptions = LayoutOptions.CenterAndExpand,
            };
            pay_bank_phone_Grid.Children.Add(pay_bank_phone_Line, 0, 0);
            pay_bank_phone_Grid.Children.Add(pay_bank_phone_Cover, 0, 0);
            pay_bank_phone_Cover.Children.Add(pay_bank_phone_Label);
            pay_bank_phone_Grid.Children.Add(input_bank_phone_Label, 1, 0);

            coverGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            coverGrid.Children.Add(pay_bank_phone_Grid, 0, 14);

            BoxView borderLine8 = new BoxView { BackgroundColor = Color.LightGray };
            coverGrid.RowDefinitions.Add(new RowDefinition { Height = 1 });
            coverGrid.Children.Add(borderLine8, 0, 15);
            #endregion

            int payoption_row = 16;


            if (ppList[0].SH_PUR_PAY_OPTION == "Card")
            {
                pay_bank_phone_Label.Text = "결제카드";
                List<SH_Pay_Card> card = SH_DB.PostSearchPayCardToIndex(ppList[0].SH_PUR_PAY_INDEX.ToString());
                input_bank_phone_Label.Text = card[0].SH_PAY_CARD_KINDS;
            }
            else if (ppList[0].SH_PUR_PAY_OPTION == "Business")
            {
                pay_bank_phone_Label.Text = "결제은행";
                List<SH_Pay_Business> business = SH_DB.PostSearchPayBusinessToIndex(ppList[0].SH_PUR_PAY_INDEX.ToString());
                input_bank_phone_Label.Text = business[0].SH_PAY_BUSINESS_BANK;

                #region 사업자 등록번호
                Grid pay_num_Grid = new Grid
                {
                    ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) },
                        new ColumnDefinition { Width = new GridLength(7, GridUnitType.Star) },
                    },
                    RowSpacing = 0,
                };
                BoxView pay_num_Line = new BoxView { BackgroundColor = Color.LightGray };
                StackLayout pay_num_Cover = new StackLayout { BackgroundColor = Color.White, Margin = 1 };
                CustomLabel pay_num_Label = new CustomLabel
                {
                    Text = "사업자등록번호",
                    Size = 14,
                    TextColor = Color.DarkGray,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                };
                CustomLabel input_num_Label = new CustomLabel
                {
                    Text = business[0].SH_PAY_BUSINESS_NUM,
                    Size = 14,
                    TextColor = Color.DarkGray,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                };
                pay_num_Grid.Children.Add(pay_num_Line, 0, 0);
                pay_num_Grid.Children.Add(pay_num_Cover, 0, 0);
                pay_num_Cover.Children.Add(pay_num_Label);
                pay_num_Grid.Children.Add(input_num_Label, 1, 0);

                coverGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                coverGrid.Children.Add(pay_num_Grid, 0, payoption_row++); // 17

                BoxView borderLine9 = new BoxView { BackgroundColor = Color.LightGray };
                coverGrid.RowDefinitions.Add(new RowDefinition { Height = 1 });
                coverGrid.Children.Add(borderLine9, 0, payoption_row++); // 18
                #endregion

                #region 사업자 등록이름
                Grid pay_name_Grid = new Grid
                {
                    ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) },
                        new ColumnDefinition { Width = new GridLength(7, GridUnitType.Star) },
                    },
                    RowSpacing = 0,
                };
                BoxView pay_name_Line = new BoxView { BackgroundColor = Color.LightGray };
                StackLayout pay_name_Cover = new StackLayout { BackgroundColor = Color.White, Margin = 1 };
                CustomLabel pay_name_Label = new CustomLabel
                {
                    Text = "사업자이름",
                    Size = 14,
                    TextColor = Color.DarkGray,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                };
                CustomLabel input_name_Label = new CustomLabel
                {
                    Text = business[0].SH_PAY_BUSINESS_NAME,
                    Size = 14,
                    TextColor = Color.DarkGray,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                };
                pay_name_Grid.Children.Add(pay_name_Line, 0, 0);
                pay_name_Grid.Children.Add(pay_name_Cover, 0, 0);
                pay_name_Cover.Children.Add(pay_name_Label);
                pay_name_Grid.Children.Add(input_name_Label, 1, 0);

                coverGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                coverGrid.Children.Add(pay_name_Grid, 0, payoption_row++); // 19

                BoxView borderLine10 = new BoxView { BackgroundColor = Color.LightGray };
                coverGrid.RowDefinitions.Add(new RowDefinition { Height = 1 });
                coverGrid.Children.Add(borderLine10, 0, payoption_row++); // 20
                #endregion
            }
            else if (ppList[0].SH_PUR_PAY_OPTION == "Personal")
            {
                pay_bank_phone_Label.Text = "결제은행";
                List<SH_Pay_Personal> personal = SH_DB.PostSearchPayPersonalToIndex(ppList[0].SH_PUR_PAY_INDEX.ToString());
                input_bank_phone_Label.Text = personal[0].SH_PAY_PERSONAL_BANK;

                #region 개인 현금영수증 번호
                Grid pay_num_Grid = new Grid
                {
                    ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) },
                        new ColumnDefinition { Width = new GridLength(7, GridUnitType.Star) },
                    },
                    RowSpacing = 0,
                };
                BoxView pay_num_Line = new BoxView { BackgroundColor = Color.LightGray };
                StackLayout pay_num_Cover = new StackLayout { BackgroundColor = Color.White, Margin = 1 };
                CustomLabel pay_num_Label = new CustomLabel
                {
                    Text = "사업자등록번호",
                    Size = 14,
                    TextColor = Color.DarkGray,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                };
                CustomLabel input_num_Label = new CustomLabel
                {
                    Text = personal[0].SH_PAY_PERSONAL_NUM,
                    Size = 14,
                    TextColor = Color.DarkGray,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                };
                pay_num_Grid.Children.Add(pay_num_Line, 0, 0);
                pay_num_Grid.Children.Add(pay_num_Cover, 0, 0);
                pay_num_Cover.Children.Add(pay_num_Label);
                pay_num_Grid.Children.Add(input_num_Label, 1, 0);

                coverGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                coverGrid.Children.Add(pay_num_Grid, 0, payoption_row++); // 17

                BoxView borderLine9 = new BoxView { BackgroundColor = Color.LightGray };
                coverGrid.RowDefinitions.Add(new RowDefinition { Height = 1 });
                coverGrid.Children.Add(borderLine9, 0, payoption_row++); // 18
                #endregion

                #region 개인 이름
                Grid pay_name_Grid = new Grid
                {
                    ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) },
                        new ColumnDefinition { Width = new GridLength(7, GridUnitType.Star) },
                    },
                    RowSpacing = 0,
                };
                BoxView pay_name_Line = new BoxView { BackgroundColor = Color.LightGray };
                StackLayout pay_name_Cover = new StackLayout { BackgroundColor = Color.White, Margin = 1 };
                CustomLabel pay_name_Label = new CustomLabel
                {
                    Text = "사업자이름",
                    Size = 14,
                    TextColor = Color.DarkGray,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                };
                CustomLabel input_name_Label = new CustomLabel
                {
                    Text = personal[0].SH_PAY_PERSONAL_NAME,
                    Size = 14,
                    TextColor = Color.DarkGray,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                };
                pay_name_Grid.Children.Add(pay_name_Line, 0, 0);
                pay_name_Grid.Children.Add(pay_name_Cover, 0, 0);
                pay_name_Cover.Children.Add(pay_name_Label);
                pay_name_Grid.Children.Add(input_name_Label, 1, 0);

                coverGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                coverGrid.Children.Add(pay_name_Grid, 0, payoption_row++); // 19

                BoxView borderLine10 = new BoxView { BackgroundColor = Color.LightGray };
                coverGrid.RowDefinitions.Add(new RowDefinition { Height = 1 });
                coverGrid.Children.Add(borderLine10, 0, payoption_row++); // 20
                #endregion
            }
            else if (ppList[0].SH_PUR_PAY_OPTION == "Phone")
            {
                pay_bank_phone_Label.Text = "통신사";
                List<SH_Pay_Phone> phone = SH_DB.PostSearchPayPhoneToIndex(ppList[0].SH_PUR_PAY_INDEX.ToString());
                input_bank_phone_Label.Text = phone[0].SH_PAY_PHONE_KINDS;
            }


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
                Text = ppList[0].SH_PUR_PAY_VALUE.ToString("N0"),
                Size = 14,
                TextColor = Color.DarkGray,
                VerticalOptions = LayoutOptions.CenterAndExpand,
            };
            pay_priceGrid.Children.Add(pay_priceLine, 0, 0);
            pay_priceGrid.Children.Add(pay_priceCover, 0, 0);
            pay_priceCover.Children.Add(pay_priceLabel);
            pay_priceGrid.Children.Add(input_pay_priceLabel, 1, 0);

            coverGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            coverGrid.Children.Add(pay_priceGrid, 0, payoption_row++);

            BoxView borderLine11 = new BoxView { BackgroundColor = Color.LightGray };
            coverGrid.RowDefinitions.Add(new RowDefinition { Height = 1 });
            coverGrid.Children.Add(borderLine11, 0, payoption_row++);
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
                Text = ppList[0].SH_PUR_PAY_USEPOINT.ToString() + " point",
                Size = 14,
                TextColor = Color.DarkGray,
                VerticalOptions = LayoutOptions.CenterAndExpand,
            };
            pay_pointGrid.Children.Add(pay_pointLine, 0, 0);
            pay_pointGrid.Children.Add(pay_pointCover, 0, 0);
            pay_pointCover.Children.Add(pay_pointLabel);
            pay_pointGrid.Children.Add(input_pay_pointLabel, 1, 0);

            coverGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            coverGrid.Children.Add(pay_pointGrid, 0, payoption_row++);

            BoxView borderLine12 = new BoxView { BackgroundColor = Color.LightGray };
            coverGrid.RowDefinitions.Add(new RowDefinition { Height = 1 });
            coverGrid.Children.Add(borderLine12, 0, payoption_row++);
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
            CustomLabel input_pay_stateLabel = new CustomLabel
            {
                Text = ppList[0].SH_PUR_PAY_STATE.ToString(),
                Size = 14,
                TextColor = Color.DarkGray,
                VerticalOptions = LayoutOptions.CenterAndExpand,
            };
            pay_stateGrid.Children.Add(pay_stateLine, 0, 0);
            pay_stateGrid.Children.Add(pay_stateCover, 0, 0);
            pay_stateCover.Children.Add(pay_stateLabel);
            pay_stateGrid.Children.Add(input_pay_stateLabel, 1, 0);

            coverGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            coverGrid.Children.Add(pay_stateGrid, 0, payoption_row++);

            BoxView borderLine13 = new BoxView { BackgroundColor = Color.LightGray };
            coverGrid.RowDefinitions.Add(new RowDefinition { Height = 1 });
            coverGrid.Children.Add(borderLine13, 0, payoption_row++);
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