using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TicketRoom.Models.Users;
using TicketRoom.Views.MainTab;
using TicketRoom.Views.MainTab.MyPage;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        List<Button> tablist = new List<Button>();
        UserDBFunc USER_DB = UserDBFunc.Instance();


        public MainPage()
        {
            NavigationPage.SetHasNavigationBar(this, false); // Navigation Bar 지우는 코드 생성자에 입력

            InitializeComponent();
            Init();

            Global.user = USER_DB.PostSelectUserToID(Global.ID);
            Global.adress = USER_DB.PostSelectAdressToID(Global.ID);
        

            tablist.Add(DealTab);
            tablist.Add(ShopTab);
            tablist.Add(BasketTab);
            tablist.Add(MyPageTab);
            Tab_Changed(tablist[0], null);
        }

        private void Init()
        {
            try
            {
                if (File.Exists(Global.localPath + "app.config") == false) // 앱 설정 파일이 없다면 생성
                {
                    Global.non_user_id = USER_DB.PostInsertNonUsersID();
                    // db에서 비회원 아이디를 가져옴. 중복 검사 필요.
                    File.WriteAllText(Global.localPath + "app.config",
                        "NonUserID=" + Global.non_user_id + "\n" +
                        "IsLogin=" + Global.b_user_login.ToString() + "\n" + // 회원 로그인 false
                        "AutoLogin=" + Global.b_auto_login.ToString() + "\n" + // 자동 로그인 false
                        "UserID=" + Global.ID + "\n"); // 회원 아이디(지금은 잠시 아이디로 대체함)
                }
                else
                {
                    // config 파일 정보를 읽어옴
                    string text = File.ReadAllText(Global.localPath + "app.config");
                    string[] temp = text.Split('\n');
                    int textPoint = temp[0].IndexOf("NonUserID="); // 0행 째의 라인
                    Global.non_user_id = temp[0].Substring(textPoint).Replace("NonUserID=", "");
                    textPoint = temp[1].IndexOf("IsLogin="); // 1행 째의 라인
                    Global.b_user_login = IsBoolCheckFunc(temp[1].Substring(textPoint).Replace("IsLogin=", ""));
                    textPoint = temp[2].IndexOf("AutoLogin="); // 2행 째의 라인
                    Global.b_auto_login = IsBoolCheckFunc(temp[2].Substring(textPoint).Replace("AutoLogin=", ""));
                    if (Global.b_auto_login == true) // 자동 로그인이 되어있다면
                    {
                        textPoint = temp[3].IndexOf("UserID="); // 3행 째의 라인
                        Global.ID = temp[3].Substring(textPoint).Replace("UserID=", "");
                    }
                    else
                    {
                        Global.ID = "";
                    }
                }
            }
            catch
            {

            }

        }
        private bool IsBoolCheckFunc(string s)
        {
            if(s == "True") return true;
            else return false;
        }

        public static void ConfigUpdateIsLogin()
        {
            File.WriteAllText(Global.localPath + "app.config",
                "NonUserID=" + Global.non_user_id + "\n" +
                "IsLogin=" + Global.b_user_login.ToString() + "\n" + // 회원 로그인 false
                "AutoLogin=" + Global.b_auto_login.ToString() + "\n" + // 자동 로그인 false
                "UserID=" + Global.ID + "\n"); // 회원 아이디(지금은 잠시 아이디로 대체함)
        }


        private void Tab_Changed(object sender, EventArgs e)
        {
            Button selectedtab = (Button)sender;
            selectedtab.FontSize = 15;
            selectedtab.BackgroundColor = Color.White;
            selectedtab.TextColor = Color.LightBlue;
            if (selectedtab.Text.Equals("구매/판매"))
            {
                TabContent.Content = new DealTabPage();
                Title = "실시간 시세 표시";
            }
            else if (selectedtab.Text.Equals("쇼핑"))
            {
                TabContent.Content = new ShopTabPage();
                Title = "쇼핑 페이지";
            }
            else if (selectedtab.Text.Equals("장바구니"))
            {
                TabContent.Content = new BasketTabPage();
                Title = "장바구니";
            }
            else if (selectedtab.Text.Equals("내정보"))
            {
                TabContent.Content = new MyPageTabPage(this);
                Title = "내 정보";
            }
        }

        public void SetTabContent(Xamarin.Forms.View page)
        {
            TabContent.Content = page;
        }
        protected override bool OnBackButtonPressed()
        {
            DisplayAlert("알림", "어플리케이션을 종료하시겠습니까?", "확인", "취소");

            return base.OnBackButtonPressed();
        }
    }
}