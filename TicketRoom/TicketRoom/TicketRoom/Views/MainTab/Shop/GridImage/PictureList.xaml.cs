using FFImageLoading.Forms;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using TicketRoom.Models.ShopData;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.Shop.GridImage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PictureList : ContentPage
    {
        List<SH_ImageList> imageList;
        
        PopupImage popup_image;
        public PictureList(List<SH_ImageList> imageList)
        {
            InitializeComponent();
            this.imageList = imageList;
            Init();
        }
        private void Init()
        {
            #region IOS의 경우 초기화
            NavigationPage.SetHasNavigationBar(this, false); // Navigation Bar 지우는 코드 생성자에 입력
            if (Device.OS == TargetPlatform.iOS)
            {
                MainGrid.RowDefinitions[0].Height = 50;
            }
            #endregion
            BackButtonImage.Source = ImageSource.FromUri(new Uri("http://221.141.58.49:8088/img/default/backbutton_icon.png"));

            int row = 0;
            int column = 2;
            Grid pictureGrid = new Grid();

            for (int i = 0; i < imageList.Count; i++)
            {
                if (column > 1)
                {
                    column = 0;

                    ScrollGrid.RowDefinitions.Add(new RowDefinition { Height = 200 });
                    pictureGrid = new Grid
                    {
                        ColumnDefinitions = {
                            new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                            new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                        },
                    };

                    ScrollGrid.Children.Add(pictureGrid, 0, row);
                    row++;

                }

                CachedImage image = new CachedImage
                {
                    LoadingPlaceholder = Global.LoadingImagePath,
                    ErrorPlaceholder = Global.LoadingImagePath,
                    Source = ImageSource.FromUri(new Uri(imageList[i].SH_IMAGELIST_SOURCE)),
                    Aspect = Aspect.AspectFill,
                };

                // 컬럼은 0번 1번이 고정이다.
                pictureGrid.Children.Add(image, column, 0);
                column++;

                image.GestureRecognizers.Add(new TapGestureRecognizer()
                {
                    Command = new Command(() =>
                    {
                        PopupNavigation.PushAsync(popup_image = new PopupImage(image.Source.ToString()));
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
                Global.isOpen_PictureList = false;
                return base.OnBackButtonPressed();
            }
        }

        private void BackButton_Clicked(object sender, EventArgs e)
        {
            Global.isOpen_PictureList = false;
            base.OnBackButtonPressed();
        }
    }
}