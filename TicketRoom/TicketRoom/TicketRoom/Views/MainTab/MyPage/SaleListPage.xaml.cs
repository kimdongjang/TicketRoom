using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.MyPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SaleListPage : ContentPage
    {
        List<string> imagelist = new List<string> { "Departmentstore_pro.png", "Departmentstore_pro.png", "Departmentstore_pro.png",
                                                     "Departmentstore_pro.png", "Departmentstore_pro.png", "Departmentstore_pro.png",
                                                     "Departmentstore_pro.png", "Departmentstore_pro.png"};
        List<string> productnamelist = new List<string> { "롯데백화점상품권", "롯데백화점상품권", "롯데백화점상품권",
                                                     "롯데백화점상품권", "롯데백화점상품권", "롯데백화점상품권",
                                                     "롯데백화점상품권", "롯데백화점상품권"};
        List<string> productkindlist = new List<string> { "50만원권", "50만원권", "50만원권",
                                                     "50만원권", "50만원권", "50만원권",
                                                     "50만원권", "50만원권"};
        List<string> saledatalist = new List<string> { "판매일: 2018-11-27", "판매일: 2018-11-27", "판매일: 2018-11-27",
                                                     "판매일: 2018-11-27", "판매일: 2018-11-27", "판매일: 2018-11-27",
                                                     "판매일: 2018-11-27", "판매일: 2018-11-27"};
        List<string> productstatelist = new List<string> { "접수중", "판매완료", "접수거절",
                                                     "접수중", "판매완료", "접수거절",
                                                     "접수중", "판매완료"};
        List<string> pricelist = new List<string> { "$ 490,000", "$ 490,000", "$ 490,000",
                                                     "$ 490,000", "$ 490,000", "$ 490,000",
                                                     "$ 490,000", "$ 490,000"};
        public SaleListPage()
        {
            InitializeComponent();
            Showimge();

        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            this.OnBackButtonPressed();
        }

        private void Showimge()
        {
            for (int i = 0; i < imagelist.Count; i++)
            {
                Grid saleproduct_grid = new Grid
                {
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    RowSpacing = 0,
                    ColumnSpacing = 0,
                    RowDefinitions =
                    {
                        new RowDefinition { Height = 100 },
                        new RowDefinition { Height = GridLength.Auto },
                        new RowDefinition { Height = GridLength.Auto },
                        new RowDefinition { Height = GridLength.Auto },
                        new RowDefinition { Height = GridLength.Auto },
                        new RowDefinition { Height = GridLength.Auto },
                    }
                };

                Image imgae = new Image
                {
                    Source = imagelist[i],
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Aspect = Aspect.AspectFit
                };

                Label productname = new Label
                {
                    Text = productnamelist[i],
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    FontSize = 15,
                    TextColor = Color.Black
                };

                Label productkind = new Label
                {
                    Text = productkindlist[i],
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    FontSize = 15
                };

                Label saledate = new Label
                {
                    Text = saledatalist[i],
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    FontSize = 15
                };

                Label productstate = new Label
                {
                    Text = productstatelist[i],
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    TextColor = Color.Blue,
                    FontSize = 15
                };

                Label price = new Label
                {
                    Text = pricelist[i],
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    FontSize = 15
                };

                saleproduct_grid.Children.Add(imgae, 0, 0);
                saleproduct_grid.Children.Add(productname, 0, 1);
                saleproduct_grid.Children.Add(productkind, 0, 2);
                saleproduct_grid.Children.Add(saledate, 0, 3);
                saleproduct_grid.Children.Add(productstate, 0, 4);
                saleproduct_grid.Children.Add(price, 0, 5);

                if (i == 0)
                {
                    Salelist_Grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                    Salelist_Grid.Children.Add(saleproduct_grid, 0, 0);
                }
                else
                {
                    if ((i % 2) == 0)
                    {
                        Salelist_Grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                    }
                    Salelist_Grid.Children.Add(saleproduct_grid, (i % 2), (i / 2));         //실시간거래 그리드에 라벨추가
                }

                TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += (s, e) =>
                {
                    //Grid g = (Grid)s;
                    //Label l = (Label)g.Children[2];
                    //DisplayAlert("TEST", l.Text+"접수취소????넣을꺼얌???힘둔뎅..", "OK");
                };
                imgae.GestureRecognizers.Add(tapGestureRecognizer);
            }
            Salelist_Grid.RowDefinitions.Add(new RowDefinition { Height = 5 });
        }
    }
}