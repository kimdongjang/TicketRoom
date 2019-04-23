using FFImageLoading.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using TicketRoom.Models.Custom;
using TicketRoom.Models.Gift;
using TicketRoom.Services;
using TicketRoom.Views.MainTab.Dael.Purchase;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.Dael
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PurchaseTabPage : ContentView
    {
        MainPage ddp;
        GiftDBFunc giftDBFunc = GiftDBFunc.Instance();
        List<Grid> ClickTabList = new List<Grid>();

        public PurchaseTabPage(MainPage ddp, string categorynum)
        {
            InitializeComponent();
            Global.isgiftlistcliecked = true;
            this.ddp = ddp;
            ShowSubTab(Global.deal_select_category_num);
            Showlist();
            //ShowPoint(); // 잔여 포인트 인비지블
        }

        private void ShowSubTab(string categorynum)
        {
            Grid ColumnGrid = new Grid();
            StackLayout layout = new StackLayout();
            layout.Orientation = StackOrientation.Horizontal;

            List<G_CategoryInfo> CategoryList = new List<G_CategoryInfo>();
            #region 네트워크 상태 확인
            var current_network = Connectivity.NetworkAccess; // 현재 네트워크 상태
            if (current_network == NetworkAccess.Internet) // 네트워크 연결 가능
            {
                CategoryList = giftDBFunc.SelectAllCategory();
            }
            else
            {
                CategoryList = null;
            }
            #endregion

            #region 네트워크 연결 불가
            if (CategoryList == null)
            {
                CustomLabel label = new CustomLabel
                {
                    Text = "네트워크 연결 불가",
                    Size = 14,
                    TextColor = Color.Black,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    Margin = new Thickness(50, 0, 0, 0),
                };
                layout.Children.Add(label);
                TabScoll.Content = layout;
                return;
            }
            #endregion


            #region 상품권 목록 검색 불가
            if (CategoryList.Count == 0)
            {
                CustomLabel label = new CustomLabel
                {
                    Text = "상품권 검색 불가",
                    Size = 14,
                    TextColor = Color.Black,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center
                };
                layout.Children.Add(label);
                TabScoll.Content = layout;
                return;
            }
            #endregion

            #region 서브탭 상품권 목록 초기화
            for (int i = 0; i < CategoryList.Count; i++)
            {
                Grid inGrid = new Grid
                {
                    RowDefinitions =
                    {
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                        new RowDefinition { Height = 5 },
                    },
                };
                // 카테고리 이름
                CustomLabel cateName = new CustomLabel
                {
                    TextColor = Color.Black,
                    Size = 14,
                    Text = CategoryList[i].Name,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    BindingContext = i,
                    Margin = new Thickness(15, 15, 15, 0),
                };
                // 카테고리 밑줄 라인
                BoxView cateLine = new BoxView
                {
                    BackgroundColor = Color.White,
                    Margin = new Thickness(10, 0, 10, 0),
                };

                inGrid.Children.Add(cateName, 0, 0);
                inGrid.Children.Add(cateLine, 0, 1);
                layout.Children.Add(inGrid);
                ClickTabList.Add(inGrid);

                if (CategoryList[i].CategoryNum == categorynum) // 인풋된 카테고리 넘버가 일치할경우
                {
                    cateName.TextColor = Color.CornflowerBlue;
                    cateLine.BackgroundColor = Color.CornflowerBlue;
                }

                inGrid.GestureRecognizers.Add(new TapGestureRecognizer()
                {
                    Command = new Command(async () =>
                    {
                        for(int k = 0; k< ClickTabList.Count; k++)
                        {
                            if(((CustomLabel)ClickTabList[k].Children[0]).Text == cateName.Text)
                            {
                                ((CustomLabel)ClickTabList[k].Children[0]).TextColor = Color.CornflowerBlue;
                                ((BoxView)ClickTabList[k].Children[1]).BackgroundColor = Color.CornflowerBlue;

                                // 로딩 시작
                                await Global.LoadingStartAsync();
                                for (int mk = 0; mk < CategoryList.Count; mk++)
                                {
                                    if(CategoryList[mk].Name == cateName.Text)
                                    {
                                        Global.deal_select_category_num = CategoryList[mk].CategoryNum;
                                        break;
                                    }
                                }
                                // 클릭 이벤트
                                Showlist();

                                // 로딩 완료
                                await Global.LoadingEndAsync();
                            }
                            else
                            {
                                ((CustomLabel)ClickTabList[k].Children[0]).TextColor = Color.Black;
                                ((BoxView)ClickTabList[k].Children[1]).BackgroundColor = Color.White;
                            }
                        }
                    })
                });
            }
            #endregion
            TabScoll.Content = layout;
        }


        private void ShowPoint()
        {
            if (Global.b_user_login)
            {
                string str = @"{";
                str += "USER_ID:'" + Global.ID;  //아이디찾기에선 Name으로 
                str += "'}";

                //// JSON 문자열을 파싱하여 JObject를 리턴
                JObject jo = JObject.Parse(str);

                UTF8Encoding encoder = new UTF8Encoding();
                byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

                //request.Method = "POST";
                HttpWebRequest request = WebRequest.Create(Global.WCFURL + "SelectUserPoint") as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = data.Length;

                //request.Expect = "application/json";

                request.GetRequestStream().Write(data, 0, data.Length);

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        Console.Out.WriteLine("Error fetching data. Server returned status code: {0}", response.StatusCode);
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        var readdata = reader.ReadToEnd();
                        string test = JsonConvert.DeserializeObject<string>(readdata);
                        //Point_label.Text = int.Parse(test).ToString("N0");
                    }
                }
            }
            else
            {
                //Point_label.Text = int.Parse("0").ToString("N0");
            }
        }


        private void Showlist()
        {
            List<G_ProductInfo> productlist = new List<G_ProductInfo>();

            #region 네트워크 상태 확인
            var current_network = Connectivity.NetworkAccess; // 현재 네트워크 상태
            if (current_network == NetworkAccess.Internet) // 네트워크 연결 가능
            {
                productlist = giftDBFunc.PostSelectPurchaseProductToIndex(Global.deal_select_category_num); // 상품 목록 가져오기
            }
            else
            {
                productlist = null;
            }
            #endregion


            #region 네트워크 연결 불가
            if (productlist == null) // 네트워크 연결 불가
            {
                Purchaselist_Grid.Children.Clear();
                Purchaselist_Grid.RowDefinitions.Clear();
                CustomLabel label = new CustomLabel
                {
                    Text = "네트워크에 연결할 수 없습니다. 다시 시도해 주세요.",
                    Size = 18,
                    TextColor = Color.Black,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center
                };
                Purchaselist_Grid.Children.Add(label, 0, 1);         
                return;
            }
            #endregion

            #region 상품권 목록 검색 불가
            if (productlist.Count == 0)
            {
                Purchaselist_Grid.Children.Clear();
                Purchaselist_Grid.RowDefinitions.Clear();
                CustomLabel label = new CustomLabel
                {
                    Text = "상품권 목록을 불러 올 수 없습니다!",
                    Size = 18,
                    TextColor = Color.Black,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center
                };
                Purchaselist_Grid.Children.Add(label, 0, 1);         //실시간거래 그리드에 라벨추가
                return;
            }
            #endregion

            Purchaselist_Grid.Children.Clear();
            Purchaselist_Grid.RowDefinitions.Clear();

            int row = 0;

            var label_tap = new TapGestureRecognizer();
            label_tap.Tapped += async (s, e) =>
            {
                if (Global.isgiftlistcliecked)
                {
                    Global.isgiftlistcliecked = false;

                    Grid g = (Grid)s;
                    Global.deal_select_category_value = "구매";
                    await Navigation.PushAsync(new PurchasePage(ddp, productlist[int.Parse(g.BindingContext.ToString())], Global.deal_select_category_num));
                }
            };

            var label_tap2 = new TapGestureRecognizer();
            label_tap2.Tapped += async (s, e) =>
            {
                if (Global.isgiftlistcliecked)
                {
                    Global.isgiftlistcliecked = false;
                    await ddp.ShowMessage("품절상품입니다", "알림", "확인", async () =>
                    {
                        Global.isgiftlistcliecked = true;
                    });
                }
            };


            for (int i = 0; i < productlist.Count; i++)
            {
                G_ProductCount g_count = new G_ProductCount();
                #region 네트워크 상태 확인
                if (current_network == NetworkAccess.Internet) // 네트워크 연결 가능
                {
                    g_count = giftDBFunc.Get_Product_Ccount(productlist[i].PRONUM);
                }
                else
                {
                    g_count = null;
                }
                #endregion

                #region 네트워크 연결 불가
                if (g_count == null) // 네트워크 연결 불가
                {
                    CustomLabel label = new CustomLabel
                    {
                        Text = "네트워크에 연결할 수 없습니다. 다시 시도해 주세요.",
                        Size = 18,
                        TextColor = Color.Black,
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Center
                    };
                    Purchaselist_Grid.Children.Add(label, 0, 1);         //실시간거래 그리드에 라벨추가
                    return;
                }
                #endregion

                Purchaselist_Grid.RowDefinitions.Add(new RowDefinition { Height = 100 });
                Purchaselist_Grid.RowDefinitions.Add(new RowDefinition { Height = 1 });
                #region list 그리드
                Grid listgrid = new Grid
                {
                    Margin = new Thickness(15, 5, 0, 5),
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    RowSpacing = 0,
                    ColumnSpacing = 0,
                    BindingContext = i,
                    ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = 100 },
                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
                    }
                };
                #endregion

                CachedImage image = null;
                if (int.Parse(g_count.PAPER_GC_COUNT) == 0 && int.Parse(g_count.PIN_GC_COUNT) == 0)
                {
                    #region 이미지
                    image = new CachedImage
                    {
                        LoadingPlaceholder = Global.LoadingImagePath,
                        ErrorPlaceholder = Global.NotFoundImagePath,
                        Source = ImageSource.FromUri(new Uri(Global.server_ipadress + productlist[i].PRODUCTIMAGE)),
                        BackgroundColor = Color.White,
                        VerticalOptions = LayoutOptions.Center,
                        Aspect = Aspect.AspectFill,
                        Margin = 20,
                    };
                    #endregion
                }
                else
                {
                    #region 이미지
                    image = new CachedImage
                    {
                        LoadingPlaceholder = Global.LoadingImagePath,
                        ErrorPlaceholder = Global.NotFoundImagePath,
                        Source = ImageSource.FromUri(new Uri(Global.server_ipadress + productlist[i].PRODUCTIMAGE)),
                        BackgroundColor = Color.White,
                        VerticalOptions = LayoutOptions.Center,
                        Aspect = Aspect.AspectFill,
                        Margin = 20,
                    };
                    #endregion
                }

                #region label 그리드
                Grid labelgrid = new Grid
                {
                    Margin = new Thickness(10, 0, 0, 0),
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    RowSpacing = 5,
                    ColumnSpacing = 0,
                    RowDefinitions =
                    {
                        new RowDefinition { Height = GridLength.Auto },
                        new RowDefinition { Height = GridLength.Auto },
                        new RowDefinition { Height = GridLength.Auto }
                    }
                };
                #endregion

                #region 상풍권 이름 Label
                CustomLabel Name_label = new CustomLabel
                {
                    Text = productlist[i].PRODUCTTYPE + " " + productlist[i].PRODUCTVALUE,
                    Size = 18,
                    TextColor = Color.Black,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Start
                };
                #endregion

                #region 할인율 Label
                var formattedString = new FormattedString();
                formattedString.Spans.Add(new Span
                {
                    Text = "고객구매가(할인율) : ",
                    LineHeight = 1.8,
                    TextColor = Color.Black
                });

                formattedString.Spans.Add(new Span
                {
                    Text = productlist[i].PURCHASEDISCOUNTPRICE + " [" + productlist[i].PURCHASEDISCOUNTRATE + "%]",
                    LineHeight = 1.8,
                    TextColor = Color.Red
                });

                CustomLabel discountrate_label = new CustomLabel
                {
                    FormattedText = formattedString,
                    Size = 12,
                    TextColor = Color.Black,
                    VerticalOptions = LayoutOptions.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    HorizontalOptions = LayoutOptions.Start
                };
                #endregion

                #region 상풍권 수량 Label
                /*
                var CountformattedString = new FormattedString();
                CountformattedString.Spans.Add(new Span
                {
                    Text = "지류 : ",
                    LineHeight = 1.8,
                    TextColor = Color.Black
                });

                CountformattedString.Spans.Add(new Span
                {
                    Text = test.PAPER_GC_COUNT + " 개",
                    LineHeight = 1.8,
                    TextColor = Color.FromHex("#ef7d1a")
                });
                CountformattedString.Spans.Add(new Span
                {
                    Text = " 핀번호 : ",
                    LineHeight = 1.8,
                    TextColor = Color.Black
                });

                CountformattedString.Spans.Add(new Span
                {
                    Text = test.PIN_GC_COUNT + " 개",
                    LineHeight = 1.8,
                    TextColor = Color.FromHex("#ef7d1a")
                });
                CustomLabel ProCount_label = new CustomLabel
                {
                    FormattedText = CountformattedString,
                    Size = 10,
                    VerticalOptions = LayoutOptions.Center,
                    YAlign = TextAlignment.Center,
                    HorizontalOptions = LayoutOptions.Start
                };
                */
                #endregion

                #region label 그리드에 추가
                labelgrid.Children.Add(Name_label, 0, 0);         //약관 그리드에 라벨추가
                labelgrid.Children.Add(discountrate_label, 0, 1);         //약관 그리드에 Radio이미지 추가
                //labelgrid.Children.Add(ProCount_label, 0, 2);         //약관 
                #endregion

                #region label 그리드에 추가
                listgrid.Children.Add(image, 0, 0); //부모그리드에 약관 그리드 추가
                listgrid.Children.Add(labelgrid, 1, 0);         //약관 그리드에 라벨추가
                #endregion

                #region Purchaselist 그리드에 추가
                Purchaselist_Grid.Children.Add(listgrid, 0, row); //부모그리드에 약관 그리드 추가
                row++;
                #endregion

                #region list 그리드 클릭이벤트
                if (int.Parse(g_count.PAPER_GC_COUNT) != 0 || int.Parse(g_count.PIN_GC_COUNT) != 0)
                {
                    listgrid.GestureRecognizers.Add(label_tap); //라벨 클릭 이벤트 등록
                }
                else
                {
                    listgrid.GestureRecognizers.Add(label_tap2); //라벨 클릭 이벤트 등록
                }
                #endregion

                BoxView gridline = new BoxView
                {
                    BackgroundColor = Color.FromHex("#f4f2f2"),
                    HeightRequest = 1,
                    VerticalOptions = LayoutOptions.End,
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };
                Purchaselist_Grid.Children.Add(gridline, 0, row);
                row++;
            }
        }
    }
}