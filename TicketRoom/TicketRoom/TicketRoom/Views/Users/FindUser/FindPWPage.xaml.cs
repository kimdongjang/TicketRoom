using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TicketRoom.Views.Users.Login;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.Users.FindUser
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FindPWPage : ContentPage
    {
        MyTimer timer;
        int test;
        string ID = "";
        string Phone = "";
        public FindPWPage()
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
            Global.isfindpwpage_clicked = true;
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            if(Global.isfindpwpage_clicked)
            {
                Global.isfindpwpage_clicked = false;
                if (timer != null)
                {
                    timer.Stop();
                }
                Navigation.PopAsync();
            }
        }

        private void FindIDPWBtn_Clicked(object sender, EventArgs e)
        {
            if (Global.isfindpwpage_clicked)
            {
                Global.isfindpwpage_clicked = false;
                if (timer != null)
                {
                    timer.Stop();
                }
                var nav = Navigation.NavigationStack;
                int idx = nav.Count;
                this.Navigation.RemovePage(nav[idx - 1]);
                Navigation.PushAsync(new FindIDPage());
            }
        }

        private async void CheckNumSendBtn_Clicked(object sender, EventArgs e)
        {
            #region 인증번호 만들기
            var bytes = new byte[4];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            uint random = BitConverter.ToUInt32(bytes, 0) % 100000000;
            #endregion

            string str = @"{";
            str += "DATA:'" + ID_box.Text;  //아이디찾기에선 Name으로 
            str += "',PHONENUM:'" + Phone_box.Text;
            str += "',KEY:'" + String.Format("{0:D8}", random);
            str += "',TYPE:'" + "3"; //인증 종류( 1: 회원가입, 2: ID찾기, 3: 비밀번호 찾기)
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
                            SendEmailPW_Grid.IsVisible = false;
                            DisplayAlert("알림", "일치하는 정보가 없습니다.", "OK");
                            return;
                        case 1:
                            ID = ID_box.Text;
                            Phone = Phone_box.Text;
                            CheckNumSendBtn.Text = "인증번호 재전송";
                            CheckNumGrid.IsVisible = true;
                            SendEmailPW_Grid.IsVisible = false;
                            #region 남은시간 타이머 
                            await ShowMessage("인증번호가 발송 되었습니다.", "알림", "OK", async () =>
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
                SendEmailPW_Grid.IsVisible = false;
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
            string str = @"{";
            str += "Phonenum:'" + Phone;
            str += "',CKey:'" + CheckNum_box.Text;
            str += "'}";

            //// JSON 문자열을 파싱하여 JObject를 리턴
            JObject jo = JObject.Parse(str);

            UTF8Encoding encoder = new UTF8Encoding();
            byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

            //request.Method = "POST";
            HttpWebRequest request = WebRequest.Create(Global.WCFURL + "Find_UserPW") as HttpWebRequest;
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
                        await DisplayAlert("알림", "인증번호가 틀렸습니다.", "OK");
                    }
                    else if (test.Equals("ex"))
                    {
                        await DisplayAlert("알림", "서버점검중입니다.", "OK");
                    }
                    else
                    {
                        timer.Stop();
                        CheckNumSendBtn.Text = "인증";
                        CheckNumGrid.IsVisible = false;
                        SendEmailPW_Grid.IsVisible = true;
                    }
                }
            }
        }

        public async Task ShowMessage(string message,
            string title,
            string buttonText,
            Action afterHideCallback)
        {
            await DisplayAlert(
                title,
                message,
                buttonText);

            afterHideCallback?.Invoke();
        }

        private async void SendEmailPW_Btn_Clicked(object sender, EventArgs e)
        {
            if (Global.isfindpwpage_clicked)
            {
                Global.isfindpwpage_clicked = false;
                if (timer != null)
                {
                    timer.Stop();
                }

                string str = @"{";
                str += "Name:'" + ID;
                str += "',Phone:'" + Phone;
                str += "',Title:'" + "상품권거래 PW찾기 결과";
                str += "',Type:'" + "PW";
                str += "'}";

                //// JSON 문자열을 파싱하여 JObject를 리턴
                JObject jo = JObject.Parse(str);

                UTF8Encoding encoder = new UTF8Encoding();
                byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

                //request.Method = "POST";
                HttpWebRequest request = WebRequest.Create(Global.WCFURL + "EmailSend") as HttpWebRequest;
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
                            DisplayAlert("알림", "다시 시도해주세요", "OK");
                            Global.isfindpwpage_clicked = true;
                        }
                        else if (test.Equals("ex"))
                        {
                            DisplayAlert("알림", "서버점검중입니다.", "OK");
                            Global.isfindpwpage_clicked = true;
                        }
                        else
                        {
                            await ShowMessage(test + "로 임시비밀번호가 발송 되었습니다.", "알림", "OK", async () =>
                            {
                                Navigation.PopToRootAsync();
                            });
                        }
                    }
                }
            }
        }

        private void EmailRadioBtn_Clicked(object sender, EventArgs e)
        {
            //DisplayAlert("라디오 버튼", "클릭", "ok");
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