using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TicketRoom.Views.Users.Login;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.Users.FindUser
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FindIDPage : ContentPage
    {
        MyTimer timer;
        int test;
        string Name = "";
        string Phone = "";
        public FindIDPage()
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
            Global.isfindidpage_clicked = true;
        }

        protected override void OnDisappearing()
        {
            if (timer != null)
            {
                timer.Stop();
            }
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            if(Global.isfindidpage_clicked)
            {
                Global.isfindidpage_clicked = false;
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
            if (current_network == NetworkAccess.Internet) // 네트워크 연결 가능
            {
                if (Name_box.Text != "" && Name_box.Text != null)
                {
                    if (Phone_box.Text != "" && Phone_box.Text != null)
                    {
                        #region 인증번호 만들기
                        var bytes = new byte[4];
                        var rng = RandomNumberGenerator.Create();
                        rng.GetBytes(bytes);
                        uint random = BitConverter.ToUInt32(bytes, 0) % 100000000;
                        #endregion

                        string str = @"{";
                        str += "DATA:'" + Name_box.Text;  //아이디찾기에선 Name으로 
                        str += "',PHONENUM:'" + Phone_box.Text;
                        str += "',KEY:'" + String.Format("{0:D8}", random);
                        str += "',TYPE:'" + "2"; //인증 종류( 1: 회원가입, 2: ID찾기, 3: 비밀번호 찾기)
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
                                        ShowId_Grid.IsVisible = false;
                                        await DisplayAlert("알림", "일치하는 정보가 없습니다.", "OK");
                                        return;
                                    case 1:
                                        Name = Name_box.Text;
                                        Phone = Phone_box.Text;
                                        CheckNumSendBtn.Text = "인증번호 재전송";
                                        CheckNumGrid.IsVisible = true;
                                        ShowId_Grid.IsVisible = false;
                                        #region 남은시간 타이머 
                                        await ShowMessage("인증번호가 발송 되었습니다.", "알림", "OK", () =>
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
                                        await DisplayAlert("알림", "서버 점검중입니다.", "OK");
                                        return;
                                }
                            }
                        }
                    }
                    else
                    {
                        await DisplayAlert("알림", "핸드폰번호를 입력하세요", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("알림", "이름을 입력하세요", "OK");
                }
            }
            else
            {
                await DisplayAlert("알림", "네트워크에 연결할 수 없습니다. 다시 한번 시도해주세요.", "확인");
                return;
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
                timer.Stop();
                CheckNumSendBtn.Text = "인증";
                CheckNumGrid.IsVisible = false;
                ShowId_Grid.IsVisible = false;
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
            if (current_network == NetworkAccess.Internet) // 네트워크 연결 가능
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
                HttpWebRequest request = WebRequest.Create(Global.WCFURL + "Find_UserID") as HttpWebRequest;
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
                        }
                        else if (test.Equals("ex"))
                        {
                            DisplayAlert("알림", "서버점검중입니다.", "OK");
                        }
                        else
                        {
                            timer.Stop();
                            CheckNumSendBtn.Text = "인증";
                            CheckNumGrid.IsVisible = false;
                            ShowId_Grid.IsVisible = true;
                            IDHint_box.Text = readdata;
                        }
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

        private async void SendEmail_BtnBtn_Clicked(object sender, EventArgs e)
        {
            #region 네트워크 상태 확인
            var current_network = Connectivity.NetworkAccess; // 현재 네트워크 상태
            if (current_network == NetworkAccess.Internet) // 네트워크 연결 가능
            {

                if (Global.isfindidpage_clicked)
                {
                    Global.isfindidpage_clicked = false;
                    if (timer != null)
                    {
                        timer.Stop();
                    }

                    string str = @"{";
                    str += "Name:'" + Name;
                    str += "',Phone:'" + Phone;
                    str += "',Title:'" + "상품권거래 ID찾기 결과";
                    str += "',Type:'" + "ID";
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
                                await DisplayAlert("알림", "다시 시도해주세요", "OK");
                                Global.isfindidpage_clicked = true;
                            }
                            else if (test.Equals("ex"))
                            {
                                await DisplayAlert("알림", "서버점검중입니다.", "OK");
                                Global.isfindidpage_clicked = true;
                            }
                            else
                            {
                                await ShowMessage(test + "로 발송 되었습니다.", "알림", "OK", async () =>
                                {
                                    await Navigation.PopToRootAsync(); ;
                                });
                            }
                        }
                    }
                }
            }
            else
            {
                await DisplayAlert("알림", "네트워크에 연결할 수 없습니다. 다시 한번 시도해주세요.", "확인");
                return;
            }
            #endregion
        }

        public async Task ShowMessage(string message, string title, string buttonText, Action afterHideCallback)
        {
            await DisplayAlert(
                title,
                message,
                buttonText);

            afterHideCallback?.Invoke();
        }

        private void FindPWBtn_Clicked(object sender, EventArgs e)
        {
            if (Global.isfindidpage_clicked)
            {
                Global.isfindidpage_clicked = false;
                if (timer != null)
                {
                    timer.Stop();
                }
                var nav = Navigation.NavigationStack;
                int idx = nav.Count;
                this.Navigation.RemovePage(nav[idx - 1]);
                Navigation.PushAsync(new FindPWPage());
            }
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