using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Loading
    {
        public Loading()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);
        }

        /*
        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.ImInLoadingView = true;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            App.ImInLoadingView = false;
        }
        */
    }
}