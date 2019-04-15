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
using TicketRoom.Models.Gift.SaleList;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.MyPage.SaleList
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SaleDetailListGift : ContentPage
	{
        List<G_SaleInfo> salelist = new List<G_SaleInfo>();
        string slnum = "";
		public SaleDetailListGift (string slnum)
		{
			InitializeComponent ();
            this.slnum = slnum;

            #region IOS의 경우 초기화
            NavigationPage.SetHasNavigationBar(this, false); // Navigation Bar 지우는 코드 생성자에 입력
            if (Xamarin.Forms.Device.OS == TargetPlatform.iOS)
            {
                MainGrid2.RowDefinitions[0].Height = Global.title_size_value;
            }
            if (Global.ios_x_model == true) // ios X 이상의 모델일 경우
            {
                MainGrid2.RowDefinitions[5].Height = 30;
            }
            #endregion

            PostSearchSaleListToSlNum(slnum);
            Init();
            NavigationInit();
        }

        private void NavigationInit()
        {
            NavigationButton.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(async () =>
                {
                    // 로딩 시작
                    await Global.LoadingStartAsync();

                    await Navigation.PushAsync(new NavagationPage());

                    // 로딩 완료
                    await Global.LoadingEndAsync();
                })
            });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Global.isbackbutton_clicked = true;
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            if(Global.isbackbutton_clicked)
            {
                Global.isbackbutton_clicked = false;
                Navigation.PopAsync();
            }
        }

        private void ConfirmBtn_Clicked(object sender, EventArgs e)
        {

        }

        // 판매번호를 통해 상품권 판매 상세리스트 가져오기
        public void PostSearchSaleListToSlNum(string slnum)
        {
            #region 네트워크 상태 확인
            var current_network = Connectivity.NetworkAccess; // 현재 네트워크 상태
            if (current_network != NetworkAccess.Internet) // 네트워크 연결 불가
            {
                DisplayAlert("알림", "네트워크에 연결할 수 없습니다. 다시 한번 시도해주세요.", "확인");
                salelist = null;
                return;
            }
            #endregion
            #region 네트워크 연결 가능
            else
            {
                string str = @"{";
                str += "slnum : '" + slnum;
                str += "'}";

                //// JSON 문자열을 파싱하여 JObject를 리턴
                JObject jo = JObject.Parse(str);

                UTF8Encoding encoder = new UTF8Encoding();
                byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

                HttpWebRequest request = WebRequest.Create(Global.WCFURL + "SearchSaleListToSlNum") as HttpWebRequest;
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
                            salelist = JsonConvert.DeserializeObject<List<G_SaleInfo>>(readdata);
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                }
            }
            #endregion
        }

        private void Init()
        {
            #region 네트워크 연결 불가
            if (salelist == null) // 네트워크 연결 불가
            {
                MainGrid.Children.Clear();
                MainGrid.RowDefinitions.Clear();
                CustomLabel label = new CustomLabel
                {
                    Text = "네트워크에 연결할 수 없습니다. 다시 시도해 주세요.",
                    Size = 18,
                    TextColor = Color.Black,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center
                };
                MainGrid.Children.Add(label, 0, 0);
                return;
            }
            #endregion

            Grid coverGrid = new Grid { RowSpacing = 0 };
            MainGrid.Children.Add(coverGrid, 0, 0); // 메인 그리드 추가

            #region 주문번호
            CustomLabel order_numLabel = new CustomLabel
            {
                Text = "주문 번호 : " + slnum,
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
                Text = salelist[0].PRODUCTTYPE + " " + salelist[0].PRODUCTVALUE,
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
                Text = int.Parse(salelist[0].SL_TOTAL_PRICE).ToString("N0"),
                Size = 14,
                TextColor = Color.DarkGray,
                VerticalOptions = LayoutOptions.CenterAndExpand,
            };
            pay_priceGrid.Children.Add(pay_priceLine, 0, 0);
            pay_priceGrid.Children.Add(pay_priceCover, 0, 0);
            pay_priceCover.Children.Add(pay_priceLabel);
            pay_priceGrid.Children.Add(input_pay_priceLabel, 1, 0);

            coverGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            coverGrid.Children.Add(pay_priceGrid, 0, 4);

            BoxView borderLine5 = new BoxView { BackgroundColor = Color.LightGray };
            coverGrid.RowDefinitions.Add(new RowDefinition { Height = 1 });
            coverGrid.Children.Add(borderLine5, 0, 5);
            #endregion

            #region 은행명
            Grid bankname_grid = new Grid
            {
                ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) },
                        new ColumnDefinition { Width = new GridLength(7, GridUnitType.Star) },
                    },
                RowSpacing = 0,
            };
            BoxView bankname_Line = new BoxView { BackgroundColor = Color.LightGray };
            StackLayout bankname_Cover = new StackLayout { BackgroundColor = Color.White, Margin = 1 };
            CustomLabel bankname_Label = new CustomLabel
            {
                Text = "은행",
                Size = 14,
                TextColor = Color.DarkGray,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };
            CustomLabel input_bankname_Label = new CustomLabel
            {
                Text = salelist[0].SL_BANK_NAME,
                Size = 14,
                TextColor = Color.DarkGray,
                VerticalOptions = LayoutOptions.CenterAndExpand,
            };
            bankname_grid.Children.Add(bankname_Line, 0, 0);
            bankname_grid.Children.Add(bankname_Cover, 0, 0);
            bankname_Cover.Children.Add(bankname_Label);
            bankname_grid.Children.Add(input_bankname_Label, 1, 0);

            coverGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            coverGrid.Children.Add(bankname_grid, 0, 6);

            BoxView borderLine7 = new BoxView { BackgroundColor = Color.LightGray };
            coverGrid.RowDefinitions.Add(new RowDefinition { Height = 1 });
            coverGrid.Children.Add(borderLine7, 0, 7);
            #endregion

            #region 예금자명
            Grid accname_grid = new Grid
            {
                ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) },
                        new ColumnDefinition { Width = new GridLength(7, GridUnitType.Star) },
                    },
                RowSpacing = 0,
            };
            BoxView accname_Line = new BoxView { BackgroundColor = Color.LightGray };
            StackLayout accname_Cover = new StackLayout { BackgroundColor = Color.White, Margin = 1 };
            CustomLabel accname_Label = new CustomLabel
            {
                Text = "예금자명",
                Size = 14,
                TextColor = Color.DarkGray,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };
            CustomLabel input_accname_Label = new CustomLabel
            {
                Text = salelist[0].SL_ACC_NAME,
                Size = 14,
                TextColor = Color.DarkGray,
                VerticalOptions = LayoutOptions.CenterAndExpand,
            };
            accname_grid.Children.Add(accname_Line, 0, 0);
            accname_grid.Children.Add(accname_Cover, 0, 0);
            accname_Cover.Children.Add(accname_Label);
            accname_grid.Children.Add(input_accname_Label, 1, 0);

            coverGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            coverGrid.Children.Add(accname_grid, 0, 8);

            BoxView borderLine9 = new BoxView { BackgroundColor = Color.LightGray };
            coverGrid.RowDefinitions.Add(new RowDefinition { Height = 1 });
            coverGrid.Children.Add(borderLine9, 0, 9);
            #endregion

            #region 계좌번호
            Grid accnum_grid = new Grid
            {
                ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) },
                        new ColumnDefinition { Width = new GridLength(7, GridUnitType.Star) },
                    },
                RowSpacing = 0,
            };
            BoxView accnum_Line = new BoxView { BackgroundColor = Color.LightGray };
            StackLayout accnum_Cover = new StackLayout { BackgroundColor = Color.White, Margin = 1 };
            CustomLabel accnum_Label = new CustomLabel
            {
                Text = "계좌번호",
                Size = 14,
                TextColor = Color.DarkGray,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };
            CustomLabel input_accnum_Label = new CustomLabel
            {
                Text = salelist[0].SL_ACC_NUM,
                Size = 14,
                TextColor = Color.DarkGray,
                VerticalOptions = LayoutOptions.CenterAndExpand,
            };
            accnum_grid.Children.Add(accnum_Line, 0, 0);
            accnum_grid.Children.Add(accnum_Cover, 0, 0);
            accnum_Cover.Children.Add(accnum_Label);
            accnum_grid.Children.Add(input_accnum_Label, 1, 0);

            coverGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            coverGrid.Children.Add(accnum_grid, 0, 10);

            BoxView borderLine11 = new BoxView { BackgroundColor = Color.LightGray };
            coverGrid.RowDefinitions.Add(new RowDefinition { Height = 1 });
            coverGrid.Children.Add(borderLine11, 0, 11);
            #endregion

            #region 전달사항
            Grid sendstring_grid = new Grid
            {
                ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) },
                        new ColumnDefinition { Width = new GridLength(7, GridUnitType.Star) },
                    },
                RowSpacing = 0,
            };
            BoxView sendstring_Line = new BoxView { BackgroundColor = Color.LightGray };
            StackLayout sendstring_Cover = new StackLayout { BackgroundColor = Color.White, Margin = 1 };
            CustomLabel sendstring_Label = new CustomLabel
            {
                Text = "전달사항",
                Size = 14,
                TextColor = Color.DarkGray,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };
            CustomLabel input_sendstring_Label = new CustomLabel
            {
                Text = salelist[0].SL_SENDSTRING,
                Size = 14,
                TextColor = Color.DarkGray,
                VerticalOptions = LayoutOptions.CenterAndExpand,
            };
            sendstring_grid.Children.Add(sendstring_Line, 0, 0);
            sendstring_grid.Children.Add(sendstring_Cover, 0, 0);
            sendstring_Cover.Children.Add(sendstring_Label);
            sendstring_grid.Children.Add(input_sendstring_Label, 1, 0);

            coverGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            coverGrid.Children.Add(sendstring_grid, 0, 12);

            BoxView borderLine13 = new BoxView { BackgroundColor = Color.LightGray };
            coverGrid.RowDefinitions.Add(new RowDefinition { Height = 1 });
            coverGrid.Children.Add(borderLine13, 0, 13);
            #endregion

            #region 판매상태
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

            CustomLabel input_pay_stateLabel = new CustomLabel();
            if (salelist[0].SL_ISSUCCES.Equals("1"))
            {
                input_pay_stateLabel = new CustomLabel
                {
                    Text = "판매완료",
                    Size = 14,
                    TextColor = Color.DarkGray,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                };
            }
            else if (salelist[0].SL_ISSUCCES.Equals("2"))
            {
                input_pay_stateLabel = new CustomLabel
                {
                    Text = "판매실패",
                    Size = 14,
                    TextColor = Color.DarkGray,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                };
            }
            else if (salelist[0].SL_ISSUCCES.Equals("3"))
            {
                input_pay_stateLabel = new CustomLabel
                {
                    Text = "판매중",
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

            BoxView borderLine15 = new BoxView { BackgroundColor = Color.LightGray };
            coverGrid.RowDefinitions.Add(new RowDefinition { Height = 1 });
            coverGrid.Children.Add(borderLine15, 0, 15);
            #endregion

            if (salelist[0].SL_ISSUCCES.Equals("2"))
            {
                #region 실패사유
                Grid failstring_Grid = new Grid
                {
                    ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) },
                        new ColumnDefinition { Width = new GridLength(7, GridUnitType.Star) },
                    },
                    RowSpacing = 0,
                };
                BoxView failstring_Line = new BoxView { BackgroundColor = Color.LightGray };
                StackLayout failstring_Cover = new StackLayout { BackgroundColor = Color.White, Margin = 1 };
                CustomLabel failstring_Label = new CustomLabel
                {
                    Text = "결제상태",
                    Size = 14,
                    TextColor = Color.DarkGray,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                };

                CustomLabel input_failstring_Label =  new CustomLabel
                {
                    Text = salelist[0].SL_FAILSTRING,
                    Size = 14,
                    TextColor = Color.DarkGray,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                };

                failstring_Grid.Children.Add(failstring_Line, 0, 0);
                failstring_Grid.Children.Add(failstring_Cover, 0, 0);
                failstring_Cover.Children.Add(failstring_Label);
                failstring_Grid.Children.Add(input_failstring_Label, 1, 0);

                coverGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                coverGrid.Children.Add(failstring_Grid, 0, 16);

                BoxView borderLine17 = new BoxView { BackgroundColor = Color.LightGray };
                coverGrid.RowDefinitions.Add(new RowDefinition { Height = 1 });
                coverGrid.Children.Add(borderLine17, 0, 17);
                #endregion
            }
        }
    }
}