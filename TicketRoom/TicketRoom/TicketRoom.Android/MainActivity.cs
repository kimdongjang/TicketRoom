using Android.App;
using Android.Content.PM;
using Android.OS;
using Refractored.XamForms.PullToRefresh.Droid;

namespace TicketRoom.Droid
{
    [Activity(Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(enableFastRenderer: true);
            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);
            PullToRefreshLayoutRenderer.Init();
            LoadApplication(new App());

        }
    }
}