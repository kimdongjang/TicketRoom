using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Text;
using TicketRoom.Models.ShopData;
using TicketRoom.Models.Users;
using TicketRoom.Services;
using TicketRoom.Views.MainTab.MyPage;
using TicketRoom.Views.Users.CreateUser;
using TicketRoom.Views.Users.FindUser;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.Users.Login
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        ShopDBFunc SH_DB = ShopDBFunc.Instance();
        UserDBFunc USER_DB = UserDBFunc.Instance();
        RSAFunc rSAFunc = RSAFunc.Instance();

        public LoginPage()
        {
            InitializeComponent();
            #region IOS의 경우 초기화
            NavigationPage.SetHasNavigationBar(this, false); // Navigation Bar 지우는 코드 생성자에 입력
            if (Xamarin.Forms.Device.OS == TargetPlatform.iOS)
            {
                MainGrid.RowDefinitions[0].Height = Global.title_size_value;
            }
            #endregion

            Init();
        }
        private void Init()
        {
            guestGrid.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(async () =>
                {
                    if (Global.isloginbtn_clicked)
                    {
                        Global.isloginbtn_clicked = false;
                        await DisplayAlert("알림", "Guest 로그인에 성공했습니다!", "확인");
                        Global.b_guest_login = true;
                        await Navigation.PopAsync();
                    }
                })
            });
            googleGrid.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(async () =>
                {
                    if (Global.isloginbtn_clicked)
                    {
                        
                        Global.isloginbtn_clicked = false;
                        await DisplayAlert("알림", "준비중입니다!", "확인");
                        Global.isloginbtn_clicked = true;
                        return;

                        await Navigation.PushAsync(new FacebookProfileCsPage());
                        //Navigation.PushAsync(new FacebookProfilePage());
                    }
                })
            });

            facebookGrid.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(async () =>
                {
                    if (Global.isloginbtn_clicked)
                    {
                        /*
                        Global.isloginbtn_clicked = false;
                        await DisplayAlert("알림", "준비중입니다!", "확인");
                        Global.isloginbtn_clicked = true;
                        return;*/

                        await Navigation.PushAsync(new FacebookProfileCsPage());
                        //Navigation.PushAsync(new FacebookProfilePage());
                    }
                })
            });

            LoginButton.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(async () =>
                {
                    // 로딩 시작
                    await Global.LoadingStartAsync();

                    Login();

                    // 로딩 완료
                    await  Global.LoadingEndAsync();
                })
            });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Global.isloginbtn_clicked = true;
            Global.isbackbutton_clicked = true;
        }

        private void Login()
        {
            #region 네트워크 상태 확인
            var current_network = Connectivity.NetworkAccess; // 현재 네트워크 상태
            if (current_network == NetworkAccess.Internet) // 네트워크 연결 가능
            {
                if (Global.isloginbtn_clicked)
                {
                    Global.isloginbtn_clicked = false;
                    if (id_box.Text != "" && id_box.Text != null)
                    {
                        if (pw_box.Text != "" && pw_box.Text != null)
                        {
                            rSAFunc.SetRSA("Start");
                            string str = @"{";
                            str += "ID:'" + id_box.Text;  //아이디찾기에선 Name으로 
                            str += "',PW:'" + rSAFunc.RSAEncrypt(pw_box.Text);
                            str += "',Rsastring:'" + rSAFunc.privateKeyText;
                            str += "'}";

                            //// JSON 문자열을 파싱하여 JObject를 리턴
                            JObject jo = JObject.Parse(str);

                            UTF8Encoding encoder = new UTF8Encoding();
                            byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

                            //request.Method = "POST";
                            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "Login_User") as HttpWebRequest;
                            request.Method = "POST";
                            request.ContentType = "application/json";
                            request.ContentLength = data.Length;

                            //request.Expect = "application/json";

                            request.GetRequestStream().Write(data, 0, data.Length);

                            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                            {
                                if (response.StatusCode != HttpStatusCode.OK)
                                    Console.Out.WriteLine("Error fetching data. Server returned status code: {0}", response.StatusCode);
                                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                                {
                                    var readdata = reader.ReadToEnd();
                                    //Stuinfo test = JsonConvert.DeserializeObject<Stuinfo>(readdata);
                                    switch (int.Parse(readdata))
                                    {
                                        case -1: DisplayAlert("알림", "비밀번호가 틀렸습니다", "OK"); Global.isloginbtn_clicked = true; return;
                                        case 0: DisplayAlert("알림", "아이디가 존재하지 않습니다.", "OK"); Global.isloginbtn_clicked = true; return;
                                        case 1: // 로그인 성공시(아이디와 비밀번호가 일치할 경우 처리)
                                            #region 네트워크 상태 확인
                                            if (current_network == NetworkAccess.Internet) // 네트워크 연결 가능
                                            {
                                                // 네트워크 연결 성공시 유저 데이터와 유저 주소 초기화
                                                Global.user = USER_DB.PostSelectUserToID(id_box.Text);
                                                Global.adress = USER_DB.PostSelectAdressToID(id_box.Text);
                                            }
                                            else
                                            {
                                                // 문제 발생시 리턴, -> 로그인 성공 실패
                                                DisplayAlert("알림", "네트워크에 연결할 수 없습니다. 로그인에 실패했습니다.", "확인");
                                                return;
                                            }
                                            #endregion

                                            Global.b_user_login = true; // 회원 로그인 상태
                                            Global.b_auto_login = true; // 자동 로그인 상태
                                            Global.ID = id_box.Text; // 회원 아이디

                                            if (Global.android_serial_number != "") // 안드로이드 기종으로 실행시
                                            {
                                                USER_DB.PostAutoLoginSerialNumber(Global.android_serial_number, Global.ID);
                                            }
                                            else if(Global.ios_serial_number != "") // ios 기종으로 실행시
                                            {
                                                USER_DB.PostAutoLoginSerialNumber(Global.ios_serial_number, Global.ID);
                                            }
                                            //MainPage.ConfigUpdateIsLogin(); // 회원 로그인 상태 Config 업데이트

                                            DisplayAlert("알림", "로그인에 성공했습니다.", "확인");

                                            if (SH_DB.PostUpdateBasketUserToID(Global.ID, Global.non_user_id) == false) // 비회원 -> 회원 로그인시 장바구니 목록 이동
                                            {
                                                DisplayAlert("알림", "쇼핑몰 장바구니 목록을 옮기는 과정에 문제가 발생했습니다.", "확인");
                                            }

                                            if (USER_DB.PostGiftUpdateBaskeListToID(Global.non_user_id, Global.ID) == false)
                                            {
                                                App.Current.MainPage.DisplayAlert("알림", "서버점검중입니다", "확인");
                                            }

                                            Navigation.PopToRootAsync();
                                            MainPage mp = (MainPage)Application.Current.MainPage.Navigation.NavigationStack[0];
                                            return;
                                    }
                                }
                            }
                        }
                        else
                        {
                            DisplayAlert("알림", "비밀번호를 입력하세요", "OK");
                            Global.isloginbtn_clicked = true;
                        }
                    }
                    else
                    {
                        DisplayAlert("알림", "아이디를 입력하세요", "OK");
                        Global.isloginbtn_clicked = true;
                    }
                }
            }
            else
            {
                DisplayAlert("알림", "네트워크에 연결할 수 없습니다. 다시 한번 시도해주세요.", "확인");
                return;
            }
            #endregion
            
        }

        

        private void FindIDPWBtn_Clicked(object sender, EventArgs e)
        {
            if (Global.isloginbtn_clicked)
            {
                Global.isloginbtn_clicked = false;
                Navigation.PushAsync(new FindIDPage());
            }
        }

        private void CreateUserBtn_Clicked(object sender, EventArgs e)
        {
            if (Global.isloginbtn_clicked)
            {
                Global.isloginbtn_clicked = false;
                Navigation.PushAsync(new AcceptTermsPage());
            }
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            Global.isbackbutton_clicked = true;
            Navigation.PopAsync();
        }


        protected override bool OnBackButtonPressed()
        {
            Global.ismypagebtns_clicked = true;
            return base.OnBackButtonPressed();
        }

    }
}