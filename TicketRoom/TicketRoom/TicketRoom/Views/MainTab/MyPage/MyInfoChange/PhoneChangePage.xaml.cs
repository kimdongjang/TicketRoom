using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.MyPage.MyInfoChange
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PhoneChangePage : ContentPage
    {

        MyTimer timer;
        int timer_count;
        bool isEntryFocus = false;

        public PhoneChangePage()
        {
            InitializeComponent();
            #region IOS의 경우 초기화
            NavigationPage.SetHasNavigationBar(this, false); // Navigation Bar 지우는 코드 생성자에 입력
            if (Xamarin.Forms.Device.OS == TargetPlatform.iOS)
            {
                MainGrid.RowDefinitions[0].Height = Global.title_size_value;
            }
            #endregion
            NavigationInit();
            Init();
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
        private void Init()
        {
            MyPhoneLabel.Text = Global.user.PHONENUM;
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            Global.isbackbutton_clicked = true;
            Navigation.PopAsync();
        }

        // 인증 버튼 클릭
        private void CertificationBtn_Clicked(object sender, EventArgs e)
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
                if (InputPhoneEntry.Text != "") // 변경된 핸드폰 번호가 널이 아닌 경우
                {
                    CheckNumGrid.IsVisible = true;

                    string str = @"{";
                    str += "DATA:'" + Global.ID;  //아이디찾기에선 Name으로 
                    str += "',PHONENUM:'" + InputPhoneEntry.Text; // 입력된 핸드폰 번호로 메세지 전송
                    str += "',TYPE:'" + "5"; //인증 종류( 1: 회원가입, 2: ID찾기, 3: 비밀번호 찾기, 4: 비밀번호 수정, 5: 핸드폰 번호 수정)
                    str += "'}";

                    //// JSON 문자열을 파싱하여 JObject를 리턴
                    JObject jo = JObject.Parse(str);

                    UTF8Encoding encoder = new UTF8Encoding();
                    byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

                    HttpWebRequest request = WebRequest.Create(Global.WCFURL + "Certifiaction_Create") as HttpWebRequest;
                    request.Method = "POST";
                    request.ContentType = "application/json";
                    request.ContentLength = data.Length;

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
                                    CertificationBtn.Text = "인증";
                                    CheckNumGrid.IsVisible = false;
                                    DisplayAlert("알림", "일치하는 정보가 없습니다.", "확인");
                                    return;
                                case 1:
                                    CertificationBtn.Text = "인증번호 재전송";
                                    CheckNumGrid.IsVisible = true;
                                    isEntryFocus = true;

                                    #region 남은시간 타이머 
                                    DisplayAlert("알림", "인증번호가 발송 되었습니다.", "확인");

                                    // 타이머 생성 및 시작
                                    timer_count = 300;

                                    if (timer == null)
                                    {
                                        timer = new MyTimer(TimeSpan.FromSeconds(1), TimerCallback_event);
                                        timer.Start();
                                    }
                                    else
                                    {
                                        timer.Stop();
                                        timer.Start();
                                    }
                                    #endregion
                                    return;
                                default:
                                    DisplayAlert("알림", "서버 점검중입니다.", "확인");
                                    Navigation.PopAsync();
                                    return;
                            }
                        }
                    }
                }
            }
            #endregion

        }

        // 타이머 콜백 이벤트
        private void TimerCallback_event()
        {
            if (timer_count != 0)
            {
                timer_count--;
                int m = timer_count / 60;
                int s = timer_count - (60 * m);
                TimerLabel.Text = "*남은 시간 " + m + ":" + s;
            }
            else
            {
                if (timer != null)
                {
                    timer.Stop();
                }
                CertificationBtn.Text = "인증";
                CheckNumGrid.IsVisible = false;
                TimerLabel.Text = "*남은 시간 " + "5" + ":" + "00";
                DisplayAlert("시간초과", "인증번호 전송을 다시 해주세요", "확인");
            }
        }

        // 인증번호 입력후 최종 확인 버튼
        private void ConfirmBtn_Clicked(object sender, EventArgs e)
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
                if(CertificationEntry.Text != "")
                {
                    string str = @"{";
                    str += "p_id:'" + Global.ID;
                    str += "',p_phonenum:'" + InputPhoneEntry.Text;
                    str += "',p_key:'" + CertificationEntry.Text;
                    str += "'}";

                    //// JSON 문자열을 파싱하여 JObject를 리턴
                    JObject jo = JObject.Parse(str);

                    UTF8Encoding encoder = new UTF8Encoding();
                    byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

                    //request.Method = "POST";
                    HttpWebRequest request = WebRequest.Create(Global.WCFURL + "UserPhoneUpdateToID") as HttpWebRequest;
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
                                DisplayAlert("알림", "인증번호가 틀렸습니다.", "확인");
                                Global.ischangemyinfobtn_clicked = true;
                            }
                            else if (test.Equals("true"))
                            {

                                if (timer != null)
                                {
                                    timer.Stop();
                                }
                                DisplayAlert("알림", "핸드폰 번호가 정상적으로 변경되었습니다.", "확인");
                                Global.ischangemyinfobtn_clicked = true;
                                Navigation.PopAsync();
                            }
                            else
                            {
                                DisplayAlert("알림", "서버에 연결할 수 없습니다.", "확인");
                                Global.ischangemyinfobtn_clicked = true;
                                Navigation.PopAsync();

                            }
                        }
                    }
                }
            }
            #endregion
        }

        private void InputPhoneEntry_Focused(object sender, FocusEventArgs e)
        {
            if(isEntryFocus == true) // 인증버튼을 눌렀을 경우 수정하지 못하게 제어
            {
                InputPhoneEntry.Unfocus();
            }
        }
    }
}