using Android.App;
using Android.Content.PM;
using Android.OS;

namespace TicketRoom.Droid
{
    [Activity(Label = "TicketRoom", Icon = "@mipmap/icon", NoHistory = true, Theme = "@style/SplashTheme",
      MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            StartActivity(typeof(MainActivity));
            Finish();

            OverridePendingTransition(0, 0);
        }
    }
}