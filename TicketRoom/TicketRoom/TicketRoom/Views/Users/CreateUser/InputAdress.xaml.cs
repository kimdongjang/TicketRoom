using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TicketRoom.Models.Custom;
using TicketRoom.Models.Users;
using TicketRoom.Models.USERS;
using TicketRoom.Views.MainTab;
using TicketRoom.Views.MainTab.Dael.Purchase;
using TicketRoom.Views.MainTab.Popup;
using TicketRoom.Views.MainTab.Shop;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.Users.CreateUser
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InputAdress : ContentPage
    {
        List<AdressAPI> adl;
        public ADRESS myAdress = new ADRESS();
        Queue<Grid> adl_queue = new Queue<Grid>();

        public CreateUserpage cup;
        public PurchaseDetailPage pdp;
        public ShopTabPage stp; // 쇼핑 탭 페이지
        public ShopOrderPage sop; // 쇼핑 주문 페이지

        PopupRecentAdress pra;

        private bool IsInputAdress = false;
        public string roadAddr = "";
        public string jibunAddr = "";
        public string zipNo = "";

        #region 생성자
        public InputAdress()
        {
            InitializeComponent();
            Init();
        }
        public InputAdress(ShopTabPage s)
        {
            stp = s;
            InitializeComponent();
            EntryAdress.Focus();
            Init();
        }

        public InputAdress(PurchaseDetailPage p)
        {
            pdp = p;
            InitializeComponent();
            Init();
        }

        public InputAdress(CreateUserpage c)
        {
            cup = c;
            InitializeComponent();
            Init();
        }

        public InputAdress(ShopOrderPage sop)
        {
            this.sop = sop;
            InitializeComponent();
            Init();
        }


        #endregion

        private void Init()
        {
            NavigationPage.SetHasNavigationBar(this, false); // Navigation Bar 지우는 코드 생성자에 입력
            #region IOS의 경우 초기화
            if (Xamarin.Forms.Device.OS == TargetPlatform.iOS)
            {
                MainGrid.RowDefinitions[0].Height = Global.title_size_value;
            }
            if (Global.ios_x_model == true) // ios X 이상의 모델일 경우
            {
                MainGrid.RowDefinitions[4].Height = 30;
            }
            #endregion
            BackButtonImage.Source = "backbutton_icon.png";


            
        }
        #region 최근 주소 검색

        #endregion

        #region Back버튼 처리
        protected override bool OnBackButtonPressed()
        {
            if(Global.isloading_block == true)
            {
                return true;
            }
            BackButtonFunc();
            return base.OnBackButtonPressed();
        }
        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            BackButtonFunc();
            this.OnBackButtonPressed();
        }
        // 뒤로 가기 버튼을 눌렀을 시의 Entry 처리
        private void BackButtonFunc()
        {
            // 유저 회원 가입 주소 확인시
            if (cup != null)
            {
                cup.EntryAdress.Text = Global.adress.ROADADDR;
            }
            // 상품권 구매 주소 확인시
            else if (pdp != null)
            {
                pdp.EntryAdress.Text = Global.adress.ROADADDR;
                pdp.jibunAddr = Global.adress.JIBUNADDR;
                pdp.zipNo = Global.adress.ZIPNO.ToString();
            }
            // 쇼핑몰 주문 페이지 주소 변경시
            else if (sop != null)
            {
                sop.AdressLabel.Text = Global.adress.ROADADDR;
            }
            Global.isOpen_AdressModal = false;
            Navigation.PopModalAsync();
        }
        #endregion

        // 주소 검색 이벤트
        private void SearchBtn_Clicked(object sender, EventArgs e)
        {
            Find_AdressAsync(EntryAdress.Text);
        }


        // 입력 주소 확인 이벤트
        private void CheckAdress_Clicked(object sender, EventArgs e)
        {
            if (IsInputAdress == false)
            {
                DisplayAlert("주소가 입력되지 않았습니다!", "", "확인");
                return;
            }
            ShowConfirmDialog();
        }

        /// <summary>
        /// 주소 검색 후 마지막 결정 확인 이벤트
        /// </summary>
        private async void ShowConfirmDialog()
        {
            if (DetailEntry.Text == "상세 주소 입력")
            {
                // 상세 주소 입력을 안햇다는 이야기 => 공백 허용 가능하게 할거?
                DetailEntry.Text = "";
            }
            var answer = await DisplayAlert(EntryAdress.Text + " , " + DetailEntry.Text, "입력된 주소가 맞습니까?", "확인", "취소");
            if (answer == false) return;

            // 입력된 주소로 전역 변수 초기화
            Global.adress.ROADADDR = myAdress.ROADADDR + DetailEntry.Text;
            Global.adress.JIBUNADDR = myAdress.JIBUNADDR + DetailEntry.Text;
            Global.adress.ZIPNO = myAdress.ZIPNO;

            this.OnBackButtonPressed();
        }

        
        private async Task Find_AdressAsync(string word)
        {
            // loading start
            Loading loadingScreen = new Loading();

            await PopupNavigation.PushAsync(loadingScreen);
            //await Navigation.PushModalAsync(loadingScreen);
            Global.isloading_block = true;


            adl = new List<AdressAPI>();
            string str = @"{";
            str += "word : ' " + word;
            str += " '}";

            //// JSON 문자열을 파싱하여 JObject를 리턴
            JObject jo = JObject.Parse(str);

            UTF8Encoding encoder = new UTF8Encoding();
            byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "Find_Adress") as HttpWebRequest;
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
                        adl = JsonConvert.DeserializeObject<List<AdressAPI>>(readdata);

                    }

                    IsInputAdress = true;
                    UpdateAdressList();

                }
            }
            catch
            {
                CustomLabel label = new CustomLabel
                {
                    Text = "검색 결과를 찾을 수 없습니다!",
                    Size = 18,
                    TextColor = Color.Black,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalTextAlignment = TextAlignment.Center,
                };
                IsInputAdress = false;
                AdrListParentGrid.Children.Add(label, 0, 1);
            }

            // loading end
            await PopupNavigation.PopAsync();
            Global.isloading_block = false;
            //await Navigation.PopModalAsync();
        }

        // 주소 리스트 갱신
        private void UpdateAdressList()
        {
            AdrListParentGrid.RowDefinitions.Clear();
            AdrListParentGrid.Children.Clear();

            for (int i = 0; i < adl.Count; i++)
            {
                //AdrListParentGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

                Grid grid = new Grid // 주소 라벨을 묶는 그리드 생성
                {
                    RowSpacing = 10,
                    ColumnSpacing = 0,
                    BindingContext = i,
                    RowDefinitions =
                    {
                        new RowDefinition { Height = 3 },
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                        new RowDefinition { Height = 3 },
                    }
                };

                AdrListParentGrid.Children.Add(grid, 0, i); //

                BoxView borderLine = new BoxView { BackgroundColor = Color.LightGray };
                if (i != 0) // 구분선 첫줄은 배제.
                {
                    grid.Children.Add(borderLine, 0, 0); //
                }
                #region 도로명 구역
                Grid roadGrid = new Grid
                {
                    ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) },
                        new ColumnDefinition { Width = new GridLength(7, GridUnitType.Star) },
                    }
                };
                CustomButton roadButton = new CustomButton
                {
                    Text = "도로명",
                    Size = 14,
                    BackgroundColor = Color.CornflowerBlue,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    HeightRequest = 25,
                    TextColor = Color.White,
                };
                // 리스트 내용은 라벨로 설정
                CustomLabel roadLabel = new CustomLabel
                {
                    Text = adl[i].roadAddr + " ( " + adl[i].zipNo + " )",
                    Size = 14,
                    TextColor = Color.Gray,
                    VerticalOptions = LayoutOptions.Center,
                };
                roadGrid.Children.Add(roadButton, 0, 0);
                roadGrid.Children.Add(roadLabel, 1, 0);
                #endregion

                #region 지번 구역
                Grid jibunGrid = new Grid
                {
                    ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) },
                        new ColumnDefinition { Width = new GridLength(7, GridUnitType.Star) },
                    }
                };
                CustomButton jibunButton = new CustomButton
                {
                    Text = "지번",
                    Size = 14,
                    BackgroundColor = Color.CornflowerBlue,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    HeightRequest = 30,
                    TextColor = Color.White,
                };
                CustomLabel jibunLabel = new CustomLabel
                {
                    Text = adl[i].jibunAddr,
                    Size = 14,
                    TextColor = Color.Gray,
                    VerticalOptions = LayoutOptions.Center,
                };
                jibunGrid.Children.Add(jibunButton, 0, 0);
                jibunGrid.Children.Add(jibunLabel, 1, 0);
                #endregion

                #region 그리드에 추가
                grid.Children.Add(roadGrid, 0, 1); //
                grid.Children.Add(jibunGrid, 0, 2); //
                #endregion

                AdrListBackColor.BackgroundColor = Color.LightGray; // 버튼 클릭시 검색결과 색상 처리

                #region 리스트 내용 클릭 이벤트
                grid.GestureRecognizers.Add(
                    new TapGestureRecognizer()
                    {
                        Command = new Command(() => {
                            var s = grid.BindingContext;
                            // ADRESS 객체에 도로명,지번,우편번호 초기화
                            myAdress.ROADADDR = adl[int.Parse(s.ToString())].roadAddr;
                            myAdress.JIBUNADDR = adl[int.Parse(s.ToString())].jibunAddr;
                            myAdress.ZIPNO = int.Parse(adl[int.Parse(s.ToString())].zipNo);

                            // xaml UI에 도로명,지번,우편번호 초기화
                            EntryAdress.Text = adl[int.Parse(s.ToString())].roadAddr;
                            roadAddr = adl[int.Parse(s.ToString())].roadAddr;
                            jibunAddr = adl[int.Parse(s.ToString())].jibunAddr;
                            zipNo = adl[int.Parse(s.ToString())].zipNo;
                            DetailEntry.IsEnabled = true;


                            if (adl_queue.Count < 2)
                            {
                                if (adl_queue.Count != 0)
                                {
                                    Grid tempGrid = adl_queue.Dequeue();
                                    if (tempGrid.Children.Count != 3) // 첫줄 구분선 제어
                                    {
                                        ((CustomButton)((Grid)tempGrid.Children.ElementAt(0)).Children.ElementAt(0)).BackgroundColor = Color.CornflowerBlue;
                                        ((CustomButton)((Grid)tempGrid.Children.ElementAt(1)).Children.ElementAt(0)).BackgroundColor = Color.CornflowerBlue;
                                    }
                                    else
                                    {
                                        ((CustomButton)((Grid)tempGrid.Children.ElementAt(1)).Children.ElementAt(0)).BackgroundColor = Color.CornflowerBlue;
                                        ((CustomButton)((Grid)tempGrid.Children.ElementAt(2)).Children.ElementAt(0)).BackgroundColor = Color.CornflowerBlue;
                                    }
                                }
                                if (int.Parse(s.ToString()) != 0) // 첫줄 구분선 제어
                                {
                                    ((CustomButton)((Grid)grid.Children.ElementAt(1)).Children.ElementAt(0)).BackgroundColor = Color.Blue;
                                    ((CustomButton)((Grid)grid.Children.ElementAt(2)).Children.ElementAt(0)).BackgroundColor = Color.Blue;
                                }
                                else
                                {
                                    ((CustomButton)((Grid)grid.Children.ElementAt(0)).Children.ElementAt(0)).BackgroundColor = Color.Blue;
                                    ((CustomButton)((Grid)grid.Children.ElementAt(1)).Children.ElementAt(0)).BackgroundColor = Color.Blue;
                                }
                                adl_queue.Enqueue(grid);
                                MainScroll.ScrollToAsync(0,0,true);
                            }
                        })
                    }
                );
                #endregion
            }
        }

        private void RecentAdressButton_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.PushAsync(pra = new PopupRecentAdress(this));
        }
    }
}