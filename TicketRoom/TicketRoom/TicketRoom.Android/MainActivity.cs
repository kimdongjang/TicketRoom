using Android.App;
using Android.Content.PM;
using Android.OS;
using Refractored.XamForms.PullToRefresh.Droid;

namespace TicketRoom.Droid
{
    [Activity(Theme = "@style/MainTheme", Icon = "@mipmap/ic_launcher", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            App.ScreenHeight = (int)(Resources.DisplayMetrics.HeightPixels / Resources.DisplayMetrics.Density);
            App.ScreenWidth = (int)(Resources.DisplayMetrics.WidthPixels / Resources.DisplayMetrics.Density);

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(enableFastRenderer: true);
            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);
            PullToRefreshLayoutRenderer.Init();
            Global.android_serial_number = Android.OS.Build.Serial;
            LoadApplication(new App());


        }
    }
}