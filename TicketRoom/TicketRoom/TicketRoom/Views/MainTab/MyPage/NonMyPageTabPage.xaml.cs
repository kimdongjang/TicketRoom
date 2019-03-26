using System;
using TicketRoom.Views.Users.Login;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.MyPage
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NonMyPageTabPage : ContentView
	{
		public NonMyPageTabPage ()
		{
			InitializeComponent ();
            NavigationInit();
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

        private void LoginCreate_Clicked(object sender, EventArgs e)
        {
            //로그인 페이지 이동
            Navigation.PushAsync(new LoginPage());
        }
    }
}