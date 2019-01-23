using Rg.Plugins.Popup.Services;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.Shop.GridImage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PictureList : ContentPage
    {
        int picture_count = 6;
        PopupImage popup_image;
        public PictureList()
        {
            InitializeComponent();
            Init();
        }
        private void Init()
        {
            for (int i = 0; i < picture_count / 2; i++)
            {
                ScrollGrid.RowDefinitions.Add(new RowDefinition { Height = 200 });

                Grid pictureGrid = new Grid
                {
                    ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star)  },
                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star)  },
                    }
                };
                ScrollGrid.Children.Add(pictureGrid, 0, i);
                Image image1 = new Image
                {
                    Source = "shop_clothes1.jpg",
                    Aspect = Aspect.AspectFill,
                };
                Image image2 = new Image
                {
                    Source = "shop_clothes1.jpg",
                    Aspect = Aspect.AspectFill,
                };
                pictureGrid.Children.Add(image1, 0, 0);
                pictureGrid.Children.Add(image2, 1, 0);
                image1.GestureRecognizers.Add(new TapGestureRecognizer()
                {
                    Command = new Command(() =>
                    {
                        PopupNavigation.PushAsync(popup_image = new PopupImage(image1.Source.ToString()));
                    })
                });
                image2.GestureRecognizers.Add(new TapGestureRecognizer()
                {
                    Command = new Command(() =>
                    {
                        PopupNavigation.PushAsync(popup_image = new PopupImage(image2.Source.ToString()));
                    })
                });
            }
        }
        protected override bool OnBackButtonPressed()
        {
            if (PopupNavigation.Instance.PopupStack.Count != 0)
            {
                PopupNavigation.Instance.RemovePageAsync(popup_image);
                return true;
            }
            else
            {
                return base.OnBackButtonPressed();
            }
        }


        private void ImageButton_Clicked(object sender, EventArgs e)
        {

        }
    }
}