using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Loading : ContentPage
    {
        public Loading(bool _IsBusy)
        {
            NavigationPage.SetHasNavigationBar(this, false);

            BackgroundColor = Color.White;

            ActivityIndicator loading = new ActivityIndicator
            {
                Color = Color.Gray,
                IsVisible = true,
                IsRunning = true,
                VerticalOptions = LayoutOptions.StartAndExpand,
                HorizontalOptions = LayoutOptions.StartAndExpand
            };

            Label loadingLabel = new Label
            {
                Text = "Loading...",
                TextColor = Color.Black,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                //Font = Font.SystemFontOfSize(NamedSize.Large),
                //YAlign = TextAlignment.Center
            };

            StackLayout horizontalContainer = new StackLayout
            {
                //BackgroundColor = Color.Black,
                Orientation = StackOrientation.Horizontal,
                Spacing = 10,
                Padding = 10,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand

            };

            StackLayout MainContainer = new StackLayout
            {
                //BackgroundColor = Color.Black,
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Children = { horizontalContainer }
            };

            horizontalContainer.Children.Add(loading);
            //horizontalContainer.Children.Add(loadingLabel);

            MainContainer.Children.Add(horizontalContainer);

            Content = MainContainer;
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