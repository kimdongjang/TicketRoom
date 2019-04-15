using Plugin.DeviceInfo;
using TicketRoom.Views;
using TicketRoom.Views.MainTab.Shop;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace TicketRoom
{
    public partial class App : Application
    {
        public static int ScreenHeight { get; set; }
        public static int ScreenWidth { get; set; }

        public App()
        {
            InitializeComponent();
            GetDeviceNameInit();
            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        // IOS 디바이스 종류에 따른 초기화 진행
        private void GetDeviceNameInit()
        {
            string device_name = CrossDeviceInfo.Current.DeviceName.ToString();
            //string device_name = UIDevice.CurrentDevice.Name.ToString();
            // s7 -> 1440x2560
            // 대체로  1242x2688(1), 1125x2436(2), 1080x1920(2), 828x1792(3),  750x1334(3), 640x1136(4)
            if (device_name == "iPad") // 아이패드
            {
                Global.font_size_minus_value = -5;
                Global.ios_x_model = true; // X모델은 하단 탭에 ㅡ바가 생기기 때문에 처리를 해줘야함.
            }
            else if (device_name == "iPad Pro (12.9-inch) (3rd generation)") // 아이패드
            {
                Global.font_size_minus_value = -5;
                Global.ios_x_model = true; // X모델은 하단 탭에 ㅡ바가 생기기 때문에 처리를 해줘야함.
            }
            else if (device_name == "iPad Pro (12.9-inch) (2nd generation)") // 아이패드
            {
                Global.font_size_minus_value = -5;
                Global.ios_x_model = true; // X모델은 하단 탭에 ㅡ바가 생기기 때문에 처리를 해줘야함.
            }

            else if (device_name == "iPad Pro (12.9-inch)") // 아이패드
            {
                Global.font_size_minus_value = -5;
                Global.ios_x_model = true; // X모델은 하단 탭에 ㅡ바가 생기기 때문에 처리를 해줘야함.
            }

            else if (device_name == "iPad Pro (11-inch)") // 아이패드
            {
                Global.font_size_minus_value = -4;
                Global.ios_x_model = true; // X모델은 하단 탭에 ㅡ바가 생기기 때문에 처리를 해줘야함.
            }

            else if (device_name == "iPad Pro (10.5-inch)") // 아이패드
            {
                Global.font_size_minus_value = -4;
                Global.ios_x_model = true; // X모델은 하단 탭에 ㅡ바가 생기기 때문에 처리를 해줘야함.
            }

            else if (device_name == "iPad Pro (9.7-inch)") // 아이패드
            {
                Global.font_size_minus_value = -4;
                Global.ios_x_model = true; // X모델은 하단 탭에 ㅡ바가 생기기 때문에 처리를 해줘야함.
            }
            else if (device_name == "iPhone XS") // 1125x2436
            {
                Global.font_size_minus_value = 2;
                Global.ios_x_model = true; // X모델은 하단 탭에 ㅡ바가 생기기 때문에 처리를 해줘야함.
            }
            else if (device_name == "iPhone XS Max") // 1242x2688
            {
                Global.font_size_minus_value = 1;
                Global.ios_x_model = true;
            }
            else if (device_name == "iPhone XR Max") // 828x1792
            {
                Global.font_size_minus_value = 3;
                Global.ios_x_model = true;
            }
            else if (device_name == "iPhone X") // 1125x2436
            {
                Global.font_size_minus_value = 2;
                Global.ios_x_model = true;
            }
            else if (device_name == "iPhone 8 Plus") // 1080x1920
            {
                Global.font_size_minus_value = 2;
                Global.title_size_value = 30;
            }
            else if (device_name == "iPhone 8") // 750x1334
            {
                Global.font_size_minus_value = 3;
                Global.title_size_value = 30;
            }
            else if (device_name == "iPhone 7 Plus") // 1080x1920
            {
                Global.font_size_minus_value = 2;
                Global.title_size_value = 30;
            }
            else if (device_name == "iPhone 7") // 750x1334
            {
                Global.font_size_minus_value = 3;
                Global.title_size_value = 30;
            }
            else if (device_name == "iPhone 6s Plus") // 1080x1920
            {
                Global.font_size_minus_value = 2;
                Global.title_size_value = 30;
            }
            else if (device_name == "iPhone 6s") // 750x1334
            {
                Global.font_size_minus_value = 3;
                Global.title_size_value = 30;
            }
            else if (device_name == "iPhone 6 Plus") // 1080x1920
            {
                Global.font_size_minus_value = 2;
                Global.title_size_value = 30;
            }
            else if (device_name == "iPhone 6") //  750x1334
            {
                Global.font_size_minus_value = 3;
                Global.title_size_value = 30;
            }
            else if (device_name == "iPhone SE") //  640x1136
            {
                Global.font_size_minus_value = 3;
                Global.title_size_value = 30;
            }
            else Global.font_size_minus_value = 0; // 그외 디바이스들
        }
    }
}
