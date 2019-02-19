using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using TicketRoom.Models.USERS;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.Users.CreateUser
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateUserpage : ContentPage
    {
        Dictionary<string, bool> termsdata;
        public string user_adress = "";
        private InputAdress adrAPI;
        public CreateUserpage(Dictionary<string, bool> termsdata)
        {
            Xamarin.Forms.Application.Current.On<Xamarin.Forms.PlatformConfiguration.Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);
            InitializeComponent();

            #region IOS의 경우 초기화
            NavigationPage.SetHasNavigationBar(this, false); // Navigation Bar 지우는 코드 생성자에 입력
            if (Xamarin.Forms.Device.OS == TargetPlatform.iOS)
            {
                MainGrid.RowDefinitions[0].Height = 50;
            }
            #endregion

            this.termsdata = termsdata;
        }

        private void NextBtn_Clicked(object sender, EventArgs e)
        {
            if (ID_box.Text != "" && ID_box.Text != null)
            {
                if (PW_box.Text != "" && PW_box.Text != null)
                {
                    if (PWCheck_box.Text != "" && PWCheck_box.Text != null)
                    {
                        if (PW_box.Text.Equals(PWCheck_box.Text))
                        {
                            if (Email_box.Text != "" && Email_box.Text != null)
                            {
                                if (Regex.Match(Email_box.Text, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").Success)
                                {
                                    if (EntryAdress.Text != "" && EntryAdress.Text != null)
                                    {
                                        string str = @"{";
                                        str += "ID:'" + ID_box.Text + "'}";

                                        //// JSON 문자열을 파싱하여 JObject를 리턴
                                        JObject jo = JObject.Parse(str);

                                        UTF8Encoding encoder = new UTF8Encoding();
                                        byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

                                        //request.Method = "POST";
                                        HttpWebRequest request = WebRequest.Create(Global.WCFURL + "IdCheck") as HttpWebRequest;
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
                                                    case -1:
                                                        DisplayAlert("알림", "아이디가 이미 존재합니다.", "OK");
                                                        return;
                                                    case 1:
                                                        Navigation.PushAsync(new CreateUserPhoneCheckPage(new USERSData(ID_box.Text, PW_box.Text, Email_box.Text,
                                                            adrAPI.roadAddr, adrAPI.jibunAddr, adrAPI.zipNo, termsdata, Recommender_box.Text)));
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
                                        DisplayAlert("알림", "주소를 입력해세요", "OK");
                                    }
                                }
                                else
                                {
                                    DisplayAlert("알림", "이메일형식이 아닙니다", "OK");
                                }
                            }
                            else
                            {
                                DisplayAlert("알림", "이메일을 입력하세요", "OK");
                            }
                        }
                        else
                        {
                            DisplayAlert("알림", "비밀번호가 일치하지 않습니다.", "OK");
                            PW_box.Text = "";
                            PWCheck_box.Text = "";
                        }
                    }
                    else
                    {
                        DisplayAlert("알림", "비밀번호 확인을 입력하세요", "OK");
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

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            this.OnBackButtonPressed();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 주소 입력 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InputAdress_Clicked(object sender, EventArgs e)
        {
            EntryAdress.Unfocus();
            Navigation.PushAsync(adrAPI = new InputAdress(this));
        }
    }
}