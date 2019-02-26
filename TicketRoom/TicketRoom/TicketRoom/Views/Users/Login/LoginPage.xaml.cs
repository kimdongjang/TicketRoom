using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Text;
using TicketRoom.Models.ShopData;
using TicketRoom.Models.Users;
using TicketRoom.Views.Users.CreateUser;
using TicketRoom.Views.Users.FindUser;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.Users.Login
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        ShopDBFunc SH_DB = ShopDBFunc.Instance();
        UserDBFunc USER_DB = UserDBFunc.Instance();

        public LoginPage()
        {
            InitializeComponent();
            #region IOS의 경우 초기화
            NavigationPage.SetHasNavigationBar(this, false); // Navigation Bar 지우는 코드 생성자에 입력
            if (Xamarin.Forms.Device.OS == TargetPlatform.iOS)
            {
                MainGrid.RowDefinitions[0].Height = 50;
            }
            #endregion
        }
        private void Button_Clicked(object sender, EventArgs e)
        {
            if (id_box.Text != "" && id_box.Text != null)
            {
                if (pw_box.Text != "" && pw_box.Text != null)
                {
                    string str = @"{";
                    str += "ID:'" + id_box.Text;  //아이디찾기에선 Name으로 
                    str += "',PW:'" + pw_box.Text;
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
                                case -1: DisplayAlert("알림", "비밀번호가 틀렸습니다", "OK"); return;
                                case 0: DisplayAlert("알림", "아이디가 존재하지 않습니다.", "OK"); return;
                                case 1: // 로그인 성공시
                                    Global.b_user_login = true; // 회원 로그인 상태
                                    Global.b_auto_login = true; // 자동 로그인 상태
                                    Global.ID = id_box.Text; // 회원 아이디
                                    MainPage.ConfigUpdateIsLogin(); // 회원 로그인 상태 Config 업데이트

                                    Global.user = USER_DB.PostSelectUserToID(Global.ID);
                                    Global.adress = USER_DB.PostSelectAdressToID(Global.ID);
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
                }
            }
            else
            {
                DisplayAlert("알림", "아이디를 입력하세요", "OK");
            }
        }

        

        private void FindIDPWBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FindIDPage());
        }

        private void CreateUserBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AcceptTermsPage());
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            this.OnBackButtonPressed();
        }

        private void GoogleLogin_Clicked(object sender, EventArgs e)
        {

        }
        private void LoginWithFacebook_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FacebookProfileCsPage());
            //App.Current.MainPage = new FacebookProfileCsPage();
        }
    }
}