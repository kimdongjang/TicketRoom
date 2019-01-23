using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TicketRoom.Models.USERS;
using TicketRoom.Views.MainTab;
using TicketRoom.Views.MainTab.Dael.Purchase;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.Users.CreateUser
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InputAdress : ContentPage
    {
        List<AdressAPI> adl;
        Queue<Grid> adl_queue = new Queue<Grid>();

        CreateUserpage cup;
        PurchaseDetailPage pdp;
        ShopTabPage stp;

        private bool _canClose = true;
        private bool IsInputAdress = false;
        public string roadAddr = "";
        public string jibunAddr = "";
        public string zipNo = "";
        string page_status = "";

        Thread t_loading;

        public InputAdress()
        {
            InitializeComponent();
        }
        public InputAdress(ShopTabPage s)
        {
            stp = s;
            InitializeComponent();
        }

        public InputAdress(PurchaseDetailPage p)
        {
            pdp = p;
            InitializeComponent();
        }

        public InputAdress(CreateUserpage c)
        {
            cup = c;
            InitializeComponent();
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            this.OnBackButtonPressed();
        }

        // 주소 검색 이벤트
        private void SearchBtn_Clicked(object sender, EventArgs e)
        {
            Find_AdressAsync(EntryAdress.Text);
        }
        private void SearchBtnPressed(object sender, EventArgs e)
        {
            SearchBtn.BackgroundColor = Color.FromHex("#3b5998");
        }
        private void SearchBtnReleased(object sender, EventArgs e)
        {
            SearchBtn.BackgroundColor = Color.Black;
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
            // 유저 회원 가입 주소 확인시
            if (answer && cup != null)
            {
                cup.EntryAdress.Text = EntryAdress.Text + DetailEntry.Text;
                _canClose = false;
                this.OnBackButtonPressed();
            }
            // 상품권 구매 주소 확인시
            else if (answer && pdp != null)
            {
                pdp.EntryAdress.Text = EntryAdress.Text + DetailEntry.Text;
                _canClose = false;
                this.OnBackButtonPressed();
            }
            // 쇼핑몰 메인 페이지 주소 확인시
            else if (answer && stp != null)
            {
                stp.EntryAdress.Text = EntryAdress.Text + DetailEntry.Text;
                _canClose = false;
                this.OnBackButtonPressed();
            }
        }

        private async Task Find_AdressAsync(string word)
        {
            // loading start
            Loading loadingScreen = new Loading(true);
            await Navigation.PushModalAsync(loadingScreen);



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
                Label label = new Label
                {
                    Text = "검색 결과를 찾을 수 없습니다!",
                    FontSize = 18,
                    TextColor = Color.Black,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalTextAlignment = TextAlignment.Center,
                };
                IsInputAdress = false;
                AdrListParentGrid.Children.Add(label, 0, 1);
            }

            // loading end
            await Navigation.PopModalAsync();
        }

        private void UpdateAdressList()
        {
            AdrListParentGrid.RowDefinitions.Clear();
            AdrListParentGrid.Children.Clear();

            for (int i = 0; i < adl.Count; i++)
            {
                AdrListParentGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

                Grid grid = new Grid // 주소 라벨을 묶는 그리드 생성
                {
                    RowSpacing = 0,
                    ColumnSpacing = 0,
                    BindingContext = i,
                    RowDefinitions =
                    {
                        new RowDefinition { Height = 1 },
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
                    }
                };

                AdrListParentGrid.Children.Add(grid, 0, i); //부모그리드에 약관 그리드 추가

                BoxView boxview = new BoxView
                {
                    BackgroundColor = Color.Black
                };

                // 리스트 내용은 라벨로 설정
                Label label = new Label
                {
                    Text = adl[i].roadAddr + " ( " + adl[i].zipNo + " )",
                    FontSize = 18,
                    TextColor = Color.Black,
                };

                Label label2 = new Label
                {
                    Text = adl[i].jibunAddr,
                    FontSize = 14,
                    TextColor = Color.Black,
                };

                #region 그리드에 추가
                if (i != 0) // 구분선 첫줄은 배제.
                {
                    grid.Children.Add(boxview, 0, 0); //부모그리드에 약관 그리드 추가
                }
                grid.Children.Add(label, 0, 1); //부모그리드에 약관 그리드 추가
                grid.Children.Add(label2, 0, 2); //부모그리드에 약관 그리드 추가
                #endregion

                AdrListBackColor.BackgroundColor = Color.Black;
                // 배경 컬러 검은색으로 변경


                #region 리스트 내용 클릭 이벤트
                grid.GestureRecognizers.Add(
                    new TapGestureRecognizer()
                    {
                        Command = new Command(() => {
                            var s = grid.BindingContext;
                            EntryAdress.Text = adl[int.Parse(s.ToString())].roadAddr;
                            roadAddr = adl[int.Parse(s.ToString())].roadAddr;
                            jibunAddr = adl[int.Parse(s.ToString())].jibunAddr;
                            zipNo = adl[int.Parse(s.ToString())].zipNo;
                            DetailEntry.IsEnabled = true;

                            if (adl_queue.Count < 2)
                            {
                                if (adl_queue.Count != 0)
                                {
                                    adl_queue.Dequeue().BackgroundColor = Color.White;
                                }
                                grid.BackgroundColor = Color.Gray;
                                adl_queue.Enqueue(grid);
                            }
                        })
                    }
                );
                #endregion
            }
        }

        private void EntryAdress_Focused(object sender, FocusEventArgs e)
        {
            EntryAdress.Text = "";
        }

        private void DetailEntry_Focused(object sender, FocusEventArgs e)
        {
            DetailEntry.Text = "";
        }
    }
}