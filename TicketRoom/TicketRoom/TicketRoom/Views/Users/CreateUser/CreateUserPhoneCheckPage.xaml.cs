using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TicketRoom.Models.USERS;
using TicketRoom.Views.Users.Login;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.Users.CreateUser
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateUserPhoneCheckPage : ContentPage
    {
        USERSData users;
        MyTimer timer;
        int test;
        public CreateUserPhoneCheckPage(USERSData users)
        {
            InitializeComponent();
            this.users = users;

            #region IOS의 경우 초기화
            NavigationPage.SetHasNavigationBar(this, false); // Navigation Bar 지우는 코드 생성자에 입력
            if (Xamarin.Forms.Device.OS == TargetPlatform.iOS)
            {
                MainGrid.RowDefinitions[0].Height = 50;
            }
            #endregion
        }

        private async void CheckNumSendBtn_Clicked(object sender, EventArgs e)
        {
            if (Name_box.Text != "" && Name_box.Text != null)
            {
                if (Phone_box.Text != "" && Phone_box.Text != null)
                {
                    users.Name = Name_box.Text;
                    users.Phone = Phone_box.Text;
                    #region 인증번호 만들기
                    var bytes = new byte[4];
                    var rng = RandomNumberGenerator.Create();
                    rng.GetBytes(bytes);
                    uint random = BitConverter.ToUInt32(bytes, 0) % 100000000;
                    #endregion

                    string str = @"{";
                    str += "DATA:'" + Name_box.Text;  //아이디찾기에선 Name으로
                    str += "',PHONENUM:'" + users.Phone;
                    str += "',KEY:'" + String.Format("{0:D8}", random);
                    str += "',TYPE:'" + "1"; //인증 종류( 1: 회원가입, 2: ID찾기, 3: 비밀번호 찾기)
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
                                    users.Name = Name_box.Text;
                                    users.Phone = Phone_box.Text;
                                    DisplayAlert("알림", "이미 가입하신 전화번호입니다.", "OK");
                                    return;
                                case 1:
                                    CheckNumSendBtn.Text = "인증번호 재전송";
                                    CheckNumGrid.IsVisible = true;
                                    users.Name = Name_box.Text;
                                    users.Phone = Phone_box.Text;
                                    #region 남은시간 타이머 
                                    await ShowMessage(String.Format("{0:D8}", random) + "인증번호가 발송 되었습니다.", "알림", "OK", async () =>
                                    {
                                        // 타이머 생성 및 시작
                                        test = 300;

                                        if (timer == null)
                                        {
                                            timer = new MyTimer(TimeSpan.FromSeconds(1), TimerCallback_event);
                                            timer.
                                            Start();
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
                    DisplayAlert("알림", "핸드폰번호를 입력하세요", "OK");
                }
            }
            else
            {
                DisplayAlert("알림", "이름을 입력하세요", "OK");
            }
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
                timer.Stop();
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

        private async void CheckNumCheckBtn_Clicked(object sender, EventArgs e)
        {
            CheckNumSendBtn.Text = "인증번호 재전송";
            CheckNumGrid.IsVisible = true;
            users.Name = Name_box.Text;
            users.Phone = Phone_box.Text;

            string str = @"{";
            str += "ID:'" + users.ID;
            str += "',PW:'" + users.PW;
            str += "',Email:'" + users.Email;
            str += "',Recommender:'" + users.RecommenderID;
            str += "',Name:'" + users.Name;
            str += "',Phonenum:'" + users.Phone;
            str += "',roadAddr:'" + users.roadAddr;
            str += "',jibunAddr:'" + users.jibunAddr;
            str += "',zipNo:'" + users.zipNo;
            str += "',Terms1:'" + users.Termsdata.Values.ToList()[0];
            str += "',Terms2:'" + users.Termsdata.Values.ToList()[1];
            str += "',Terms3:'" + users.Termsdata.Values.ToList()[2];
            str += "',Terms4:'" + users.Termsdata.Values.ToList()[3];
            str += "',CKey:'" + CheckNum_box.Text;
            str += "'}";

            //// JSON 문자열을 파싱하여 JObject를 리턴
            JObject jo = JObject.Parse(str);

            UTF8Encoding encoder = new UTF8Encoding();
            byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

            //request.Method = "POST";
            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "Users_Create") as HttpWebRequest;
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
                            DisplayAlert("알림", "인증번호가 틀렸습니다.", "OK");
                            return;
                        case 1:
                            await ShowMessage("회원가입 되었습니다.", "알림", "OK", async () =>
                            {
                                await Navigation.PushAsync(new LoginPage());
                            });
                            return;
                        case 2:
                            DisplayAlert("알림", "추천인 아이디가 존재하지않습니다", "OK");
                            return;
                        default:
                            DisplayAlert("알림", "서버 점검중입니다.", "OK");
                            return;
                    }
                }
            }
        }

        public async Task ShowMessage(string message, string title, string buttonText, Action afterHideCallback)
        {
            await DisplayAlert(title, message, buttonText);

            afterHideCallback?.Invoke();
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}