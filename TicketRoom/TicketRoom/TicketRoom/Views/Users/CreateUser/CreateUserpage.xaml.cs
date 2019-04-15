using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using TicketRoom.Models.USERS;
using Xamarin.Essentials;
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
                MainGrid.RowDefinitions[0].Height = Global.title_size_value;
            }
            #endregion

            this.termsdata = termsdata;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Global.iscreateusernextbtn_clicked = true;
            Global.isbackbutton_clicked = true;
        }

        private void NextBtn_Clicked(object sender, EventArgs e)
        {
            #region 네트워크 상태 확인
            var current_network = Connectivity.NetworkAccess; // 현재 네트워크 상태
            if (current_network == NetworkAccess.Internet) // 네트워크 연결 가능
            {
                if (Global.iscreateusernextbtn_clicked)
                {
                    Global.iscreateusernextbtn_clicked = false;
                    if (ID_box.Text != "" && ID_box.Text != null)
                    {
                        if (ID_box.Text.Length >= 6)
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
                                                    if (Age_picker.SelectedItem != null)
                                                    {
                                                        string str = @"{";
                                                        str += "ID:'" + ID_box.Text;
                                                        str += "',RECOMMENDER:'" + "";// 추천이 넣게 되면 수정부분 Recommender_box.Text;
                                                        str += "'}";

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
                                                                string test = JsonConvert.DeserializeObject<string>(readdata);
                                                                if (test != null && test != "")
                                                                {
                                                                    switch (int.Parse(test))
                                                                    {
                                                                        case -1:
                                                                            DisplayAlert("알림", "아이디가 이미 존재합니다.", "OK");
                                                                            Global.iscreateusernextbtn_clicked = true;
                                                                            return;
                                                                        case 1:
                                                                            Navigation.PushAsync(new CreateUserPhoneCheckPage(new USERSData(ID_box.Text, PW_box.Text, Email_box.Text,
                                                                                adrAPI.roadAddr, adrAPI.jibunAddr, adrAPI.zipNo, termsdata, "", Age_picker.SelectedItem.ToString().Replace("대", ""))));
                                                                            return;
                                                                        case 2:
                                                                            DisplayAlert("알림", "추천인 아이디가 존재하지않습니다", "OK");
                                                                            Global.iscreateusernextbtn_clicked = true;
                                                                            return;
                                                                        default:
                                                                            DisplayAlert("알림", "서버 점검중입니다.", "OK");
                                                                            Global.iscreateusernextbtn_clicked = true;
                                                                            return;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        DisplayAlert("알림", "연령을 선택하세요", "OK");
                                                        Global.iscreateusernextbtn_clicked = true;
                                                    }
                                                }
                                                else
                                                {
                                                    DisplayAlert("알림", "주소를 입력해세요", "OK");
                                                    Global.iscreateusernextbtn_clicked = true;
                                                }
                                            }
                                            else
                                            {
                                                DisplayAlert("알림", "이메일형식이 아닙니다", "OK");
                                                Global.iscreateusernextbtn_clicked = true;
                                            }
                                        }
                                        else
                                        {
                                            DisplayAlert("알림", "이메일을 입력하세요", "OK");
                                            Global.iscreateusernextbtn_clicked = true;
                                        }
                                    }
                                    else
                                    {
                                        DisplayAlert("알림", "비밀번호가 일치하지 않습니다.", "OK");
                                        PW_box.Text = "";
                                        PWCheck_box.Text = "";
                                        Global.iscreateusernextbtn_clicked = true;
                                    }
                                }
                                else
                                {
                                    DisplayAlert("알림", "비밀번호 확인을 입력하세요", "OK");
                                    Global.iscreateusernextbtn_clicked = true;
                                }
                            }
                            else
                            {
                                DisplayAlert("알림", "비밀번호를 입력하세요", "OK");
                                Global.iscreateusernextbtn_clicked = true;
                            }
                        }
                        else
                        {
                            DisplayAlert("알림", "아이디를 6글자이상으로 해주세요", "OK");
                            Global.iscreateusernextbtn_clicked = true;
                        }
                    }
                    else
                    {
                        DisplayAlert("알림", "아이디를 입력하세요", "OK");
                        Global.iscreateusernextbtn_clicked = true;
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

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            if (Global.isbackbutton_clicked)
            {
                Global.isbackbutton_clicked = false;
                Navigation.PopAsync();
            }
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
            Navigation.PushModalAsync(adrAPI = new InputAdress(this));
        }
    }
}