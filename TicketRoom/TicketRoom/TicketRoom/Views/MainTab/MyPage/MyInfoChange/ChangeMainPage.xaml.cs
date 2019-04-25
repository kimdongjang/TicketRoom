using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.MyPage.MyInfoChange
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ChangeMainPage : ContentPage
	{
		public ChangeMainPage ()
		{
			InitializeComponent ();

            #region IOS의 경우 초기화
            NavigationPage.SetHasNavigationBar(this, false); // Navigation Bar 지우는 코드 생성자에 입력
            if (Xamarin.Forms.Device.OS == TargetPlatform.iOS)
            {
                MainGrid.RowDefinitions[0].Height = Global.title_size_value;
            }
            #endregion

            Init();
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

        private void Init()
        {

            PasswordChangeBtn.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(async () =>
                {
                    // 로딩 시작
                    await Global.LoadingStartAsync();

                    await Navigation.PushAsync(new PasswordChangePage());

                    // 로딩 완료
                    await Global.LoadingEndAsync();
                })
            });


            PhoneChangeBtn.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(async () =>
                {
                    // 로딩 시작
                    await Global.LoadingStartAsync();

                    await Navigation.PushAsync(new PhoneChangePage());

                    // 로딩 완료
                    await Global.LoadingEndAsync();
                })
            });
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            Global.isbackbutton_clicked = true;
            Navigation.PopAsync();
        }
    }
}