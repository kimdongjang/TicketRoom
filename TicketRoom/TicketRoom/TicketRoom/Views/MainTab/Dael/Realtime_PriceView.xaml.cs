using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketRoom.Models.Custom;
using TicketRoom.Models.Gift;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.Dael
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Realtime_PriceView : ContentView
	{
		public Realtime_PriceView (List<G_ProductInfo> g_ProductInfos)
		{
			InitializeComponent ();
            if (g_ProductInfos == null)
            {
                failString();
            }
            else
            {
                ShowPrice(g_ProductInfos);
            }
        }
        private void failString()
        {
            CustomLabel label = new CustomLabel
            {
                Text = "네트워크에 연결할 수 없습니다. 다시 시도해 주세요.",
                Size = 18,
                TextColor = Color.Black,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 15, 0, 0),
            };
            Price_Grid.Children.Add(label, 0, 0);         //실시간거래 그리드에 라벨추가
        }

        private void ShowPrice(List<G_ProductInfo> Pricelist)
        {
            Price_Grid.RowDefinitions.Add(new RowDefinition { Height = 40 });//new GridLength(1, GridUnitType.Star)
            CustomLabel titlename = new CustomLabel
            {
                Text = "상품명",
                Size = 14,
                TextColor = Color.Black,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.Start,
                Margin = new Thickness(15, 0, 0, 0)
            };

            CustomLabel titleb = new CustomLabel
            {
                Text = "고객판매가\n(할인율)",
                Size = 14,
                TextColor = Color.Black,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.Center
            };

            CustomLabel titler = new CustomLabel
            {
                Text = "고객구매가\n(할인율)",
                Size = 14,
                TextColor = Color.Black,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.Center
            };

            Price_Grid.Children.Add(titlename, 0, 0);
            Price_Grid.Children.Add(titleb, 1, 0);
            Price_Grid.Children.Add(titler, 2, 0);

            int row = 1;
            if (Pricelist == null) return;

            for (int i = 0; i < Pricelist.Count; i++)
            {
                Price_Grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });//new GridLength(1, GridUnitType.Star)
                Price_Grid.RowDefinitions.Add(new RowDefinition { Height = 1 });//new GridLength(1, GridUnitType.Star)
                CustomLabel n = new CustomLabel
                {
                    Text = Pricelist[i].PRODUCTTYPE + "\n" + Pricelist[i].PRODUCTVALUE,
                    Size = 14,
                    TextColor = Color.Black,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.Start,
                    Margin = new Thickness(15, 0, 0, 0)
                };

                CustomLabel b = new CustomLabel
                {
                    Text = Pricelist[i].SALEDISCOUNTPRICE + "\n(" + Pricelist[i].SALEDISCOUNTRATE + "%)",
                    Size = 14,
                    TextColor = Color.Blue,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.Center,
                };

                CustomLabel r = new CustomLabel
                {
                    Text = Pricelist[i].PURCHASEDISCOUNTPRICE + "\n(" + Pricelist[i].PURCHASEDISCOUNTRATE + "%)",
                    Size = 14,
                    TextColor = Color.Red,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.Center,
                };

                Price_Grid.Children.Add(n, 0, row);
                Price_Grid.Children.Add(b, 1, row);
                Price_Grid.Children.Add(r, 2, row);

                row++;

                BoxView gridline = new BoxView
                {
                    BackgroundColor = Color.FromHex("#f4f2f2"),
                    HeightRequest = 1,
                    VerticalOptions = LayoutOptions.End,
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };
                Price_Grid.Children.Add(gridline, 0, row);

                Grid.SetColumnSpan(gridline, 3);
                row++;
            }

        }
    }
}