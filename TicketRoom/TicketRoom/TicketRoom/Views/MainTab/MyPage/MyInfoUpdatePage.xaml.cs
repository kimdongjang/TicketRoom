using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TicketRoom.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.MyPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyInfoUpdatePage : ContentPage
    {
        MyTimer timer;
        int test;
        string ID = "";
        string Phone = "";
        string NEWPW = "";
        RSAFunc rSAFunc = RSAFunc.Instance();

        public MyInfoUpdatePage()
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

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Global.ischangemyinfobtn_clicked = true;
            Global.isbackbutton_clicked = true;
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            if (Global.isbackbutton_clicked)
            {
                Global.isbackbutton_clicked = false;
                if (timer != null)
                {
                    timer.Stop();
                }
                Navigation.PopAsync();
            }
        }

        private async void CheckNumSendBtn_Clicked(object sender, EventArgs e)
        {
            #region 네트워크 상태 확인
            var current_network = Connectivity.NetworkAccess; // 현재 네트워크 상태
            if (current_network != NetworkAccess.Internet) // 네트워크 연결 불가
            {
                DisplayAlert("알림", "네트워크에 연결할 수 없습니다. 다시 한번 시도해주세요.", "확인");
                return;
            }
            #endregion
            #region 네트워크 연결 가능
            else
            {
                if (NewPW_box.Text != "" && NewPW_box.Text != null)
                {
                    if (NewPWCheck_box.Text != "" && NewPWCheck_box.Text != null)
                    {
                        if (NewPW_box.Text.Equals(NewPWCheck_box.Text))
                        {
                            if (Phone_box.Text != "" && Phone_box.Text != null)
                            {
                                CheckNumGrid.IsVisible = true;

                                #region 인증번호 만들기
                                var bytes = new byte[4];
                                var rng = RandomNumberGenerator.Create();
                                rng.GetBytes(bytes);
                                uint random = BitConverter.ToUInt32(bytes, 0) % 100000000;
                                #endregion

                                string str = @"{";
                                str += "DATA:'" + Global.ID;  //아이디찾기에선 Name으로 
                                str += "',PHONENUM:'" + Phone_box.Text;
                                str += "',KEY:'" + String.Format("{0:D8}", random);
                                str += "',TYPE:'" + "4"; //인증 종류( 1: 회원가입, 2: ID찾기, 3: 비밀번호 찾기, 4: 개인정보 수정)
                                str += "'}";

                                //// JSON 문자열을 파싱하여 JObject를 리턴
                                JObject jo = JObject.Parse(str);

                                UTF8Encoding encoder = new UTF8Encoding();
                                byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

                                //request.Method = "POST";
                                HttpWebRequest request = WebRequest.Create(Global.WCFURL + "Certifiaction_Create") as HttpWebRequest;
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
                                            case 0:
                                                CheckNumSendBtn.Text = "인증";
                                                CheckNumGrid.IsVisible = false;
                                                DisplayAlert("알림", "일치하는 정보가 없습니다.", "OK");
                                                return;
                                            case 1:
                                                ID = Global.ID;
                                                Phone = Phone_box.Text;
                                                NEWPW = NewPW_box.Text;
                                                CheckNumSendBtn.Text = "인증번호 재전송";
                                                CheckNumGrid.IsVisible = true;
                                                #region 남은시간 타이머 
                                                await ShowMessage(String.Format("{0:D8}", random) + "인증번호가 발송 되었습니다.", "알림", "OK", async () =>
                                                {
                                                    // 타이머 생성 및 시작
                                                    test = 300;

                                                    if (timer == null)
                                                    {
                                                        timer = new MyTimer(TimeSpan.FromSeconds(1), TimerCallback_event);
                                                        timer.Start();
                                                    }
                                                    else
                                                    {
                                                        timer.Stop(); timer.Start();
                                                    }
                                                });
                                                #endregion
                                                return;
                                            default:
                                                DisplayAlert("알림", "서버 점검중입니다.", "OK");
                                                return;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                DisplayAlert("알림", "휴대폰 번호를 입력해주세요.", "OK");
                            }
                        }
                        else
                        {
                            DisplayAlert("알림", "변경 비밀번호가 일치하지 않습니다.", "OK");
                            NewPW_box.Text = "";
                            NewPWCheck_box.Text = "";
                        }
                    }
                    else
                    {
                        DisplayAlert("알림", "변경 비밀번호 확인을 입력하세요", "OK");
                    }
                }
                else
                {
                    DisplayAlert("알림", "변경 비밀번호를 입력하세요", "OK");
                }
            }
            #endregion
        }

        private void TimerCallback_event()
        {
            if (test != 0)
            {
                test--;
                int m = test / 60;
                int s = test - (60 * m);
                TimerLabel.Text = "*남은 시간 " + m + ":" + s;
            }
            else
            {
                if (timer != null)
                {
                    timer.Stop();
                }
                CheckNumSendBtn.Text = "인증";
                CheckNumGrid.IsVisible = false;
                TimerLabel.Text = "*남은 시간 " + "5" + ":" + "00";
                DisplayAlert("시간초과", "인증번호 전송을 다시 해주세요", "OK");
            }

            #region 디바이스 메인스레드 사용 방법 
            //Device.BeginInvokeOnMainThread(() =>
            //{
            //    if (test != 0)
            //    {
            //        test--;
            //        int m = test / 60;
            //        int s = test - (60 * m);
            //        TimerLabel.Text = "*남은 시간 " + m + ":" + s;
            //    }
            //    else
            //    {
            //        timer.Stop();
            //        DisplayAlert("시간초과", "시간이 초과됬엉", "OK");
            //    }
            //});
            #endregion
        }

        private void CheckNumCheckBtn_Clicked(object sender, EventArgs e)
        {
            #region 네트워크 상태 확인
            var current_network = Connectivity.NetworkAccess; // 현재 네트워크 상태
            if (current_network != NetworkAccess.Internet) // 네트워크 연결 불가
            {
                DisplayAlert("알림", "네트워크에 연결할 수 없습니다. 다시 한번 시도해주세요.", "확인");
                return;
            }
            #endregion
            #region 네트워크 연결 가능
            else
            {
                if (Global.ischangemyinfobtn_clicked)
                {
                    Global.ischangemyinfobtn_clicked = false;
                    if (CheckNum_box.Text != "" && CheckNum_box.Text != null)
                    {
                        //rsa 암호화키 생성
                        rSAFunc.SetRSA("Start");

                        string str = @"{";
                        str += "ID:'" + ID;
                        str += "',PHONE:'" + Phone;
                        str += "',NEWPW:'" + rSAFunc.RSAEncrypt(NEWPW);
                        str += "',KEY:'" + CheckNum_box.Text;
                        str += "',Rsastring:'" + rSAFunc.privateKeyText;
                        str += "'}";

                        //// JSON 문자열을 파싱하여 JObject를 리턴
                        JObject jo = JObject.Parse(str);

                        UTF8Encoding encoder = new UTF8Encoding();
                        byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

                        //request.Method = "POST";
                        HttpWebRequest request = WebRequest.Create(Global.WCFURL + "Userinfo_Update") as HttpWebRequest;
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
                                string test = JsonConvert.DeserializeObject<string>(readdata);
                                if (test.Equals("false"))
                                {
                                    DisplayAlert("알림", "인증번호가 틀렸습니다.", "OK");
                                    Global.ischangemyinfobtn_clicked = true;
                                }
                                else if (test.Equals("ex"))
                                {
                                    DisplayAlert("알림", "서버점검중입니다.", "OK");
                                    Global.ischangemyinfobtn_clicked = true;
                                }
                                else
                                {
                                    if (timer != null)
                                    {
                                        timer.Stop();
                                    }

                                    #region 로그아웃 과정
                                    Global.b_user_login = false;
                                    Global.b_auto_login = false;
                                    Global.ID = "";

                                    // config파일 재설정
                                    File.WriteAllText(Global.localPath + "app.config",
                                        "NonUserID=" + Global.non_user_id + "\n" +
                                        "IsLogin=" + Global.b_user_login.ToString() + "\n" + // 회원 로그인 false
                                        "AutoLogin=" + Global.b_auto_login.ToString() + "\n" + // 자동 로그인 false
                                        "UserID=" + Global.ID + "\n");

                                    #endregion
                                    DisplayAlert("알림", "변경되었습니다. 다시 로그인해주세요", "확인");
                                    Navigation.PopToRootAsync(); //--------수정가능성 많음 ... 로그아웃 시킬지
                                }
                            }
                        }
                    }
                    else
                    {
                        DisplayAlert("알림", "인증번호를 입력하세요", "OK");
                        Global.ischangemyinfobtn_clicked = true;
                    }
                }
            }
            #endregion
        }

        public async Task ShowMessage(string message, string title, string buttonText, Action afterHideCallback)
        {
            await DisplayAlert(title, message, buttonText);

            afterHideCallback?.Invoke();
        }

        protected override bool OnBackButtonPressed()
        {
            if (timer != null)
            {
                timer.Stop();
            }
            return base.OnBackButtonPressed();
        }
    }
}