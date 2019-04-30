using FFImageLoading.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Refractored.XamForms.PullToRefresh;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using TicketRoom.Models;
using TicketRoom.Models.Custom;
using TicketRoom.Models.Gift;
using TicketRoom.Services;
using TicketRoom.Views.MainTab.Dael;
using TicketRoom.Views.MainTab.MyPage;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DealTabPage : ContentView
    {
        MainPage mainPage;
        GiftDBFunc giftDBFunc = GiftDBFunc.Instance();
        List<G_DealInfo> g_DealInfolist = new List<G_DealInfo>();
        public DealTabPage(MainPage mainPage)
        {
            InitializeComponent();
            this.mainPage = mainPage;
            #region IOS의 경우 초기화
            if (Device.OS == TargetPlatform.iOS)
            {
                TabGrid.RowDefinitions[0].Height = Global.title_size_value;
            }
            #endregion

            ScrollRefresh();
            LoadingInitAsync();
        }
        private async void LoadingInitAsync()
        {
            // 로딩 시작
            await Global.LoadingStartAsync();
            NavigationInit();
            Showdeal();
            ShowPoint();
            Showimge();
            // 로딩 완료
            await Global.LoadingEndAsync();
        }

        private void NavigationInit()
        {
            NavigationButton.GestureRecognizers.Add(new TapGestureRecognizer()
             {
                 Command = new Command(async () =>
                 {
                     // 로딩 시작
                     await Global.LoadingStartAsync();

                     await Navigation.PushAsync(new NavagationPage());

                     // 로딩 완료
                     await Global.LoadingEndAsync();
                 })
             });
        }

        private void ScrollRefresh()
        {
            var refreshView = new PullToRefreshLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Content = MainScroll,
                RefreshColor = Color.FromHex("#3498db")
            };

            refreshView.SetBinding<ScrollModel>(PullToRefreshLayout.IsRefreshingProperty, vm => vm.IsBusy, BindingMode.OneWay);
            refreshView.SetBinding<ScrollModel>(PullToRefreshLayout.RefreshCommandProperty, vm => vm.RefreshCommand);

            TabGrid.Children.Add(refreshView, 0, 3);

            refreshView.RefreshCommand = new Command(() =>
            {
                refreshView.IsRefreshing = false;
                
                LoadingInitAsync();

            });
        }

        private void ShowPoint()
        {
            #region 네트워크 상태 확인
            var current_network = Connectivity.NetworkAccess; // 현재 네트워크 상태
            if (current_network == NetworkAccess.Internet) // 네트워크 연결 가능
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

                            if (test != null && test != "null")
                            {
                                MyPointLabel.Text = int.Parse(test).ToString("N0") + " Point";
                            }
                            else
                            {
                                MyPointLabel.Text = "0 Point";
                            }

                        }
                    }
                }
                else
                {
                    MyPointLabel.Text = int.Parse("0").ToString("N0") + " Point";
                }
            } // 네트워크 연결 가능
            else // 연결 불가 -> 포인트 0으로 처리
            {
                MyPointLabel.Text = "0 Point";
            }
            #endregion
        }


        // 실시간 거래
        private void Showdeal()
        {
            RealTimeGrid.Children.Clear();
            RealTimeGrid.RowDefinitions.Clear();

            #region 네트워크 상태 확인
            var current_network = Connectivity.NetworkAccess; // 현재 네트워크 상태
            if (current_network == NetworkAccess.Internet) // 네트워크 연결 가능
            {
                g_DealInfolist = giftDBFunc.SelectDealList(); // 실시간 거래 리스트 검색
            }
            else
            {
                g_DealInfolist = null;
            }
            #endregion

            #region 네트워크 연결 불가
            if (g_DealInfolist == null) // 네트워크 연결 불가
            {
                RealTimeGrid.RowDefinitions.Add(new RowDefinition { Height = 30 });
                CustomLabel label = new CustomLabel
                {
                    //Text = "네트워크에 연결할 수 없습니다. 다시 시도해 주세요.", // 위치가 좀 달라서 굳이 띄우진 않겠음
                    Size = 14,
                    TextColor = Color.Black,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center
                };
                RealTimeGrid.Children.Add(label, 0, 0);   //실시간거래 그리드에 라벨추가
                return;
            }
            #endregion

            #region 실시간 거래 내역 검색 불가
            if (g_DealInfolist==null)
            {
                RealTimeGrid.RowDefinitions.Add(new RowDefinition { Height = 30 });
                CustomLabel label = new CustomLabel
                {
                    Text = "조회실패",
                    Size = 14,
                    TextColor = Color.Black,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center
                };
                RealTimeGrid.Children.Add(label, 0, 0);         //실시간거래 그리드에 라벨추가
                return;
            }
            if (g_DealInfolist.Count == 0)
            {
                RealTimeGrid.RowDefinitions.Add(new RowDefinition{ Height = 30 });
                CustomLabel label = new CustomLabel
                {
                    //Text = "거래내역이 없습니다",
                    Size = 14,
                    TextColor = Color.Black,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center
                };
                RealTimeGrid.Children.Add(label, 0, 0);         //실시간거래 그리드에 라벨추가
                return;
            }
            #endregion

            #region 실시간 거래 내역 가져오기

            for (int i = 0; i < g_DealInfolist.Count; i++)
            {
                RealTimeGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(3, GridUnitType.Auto) });
                Grid inGrid = new Grid
                {
                    ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) },
                        new ColumnDefinition { Width = new GridLength(8, GridUnitType.Star) },
                    },
                };

                BoxView statusLine = new BoxView
                {
                    BackgroundColor = Color.CornflowerBlue,
                    Margin = 2,
                };
                // 결제 상태 레이블
                CustomLabel statusLabel = new CustomLabel
                {
                    TextColor = Color.White,
                    Size = 14,
                    Text = "거래완료",
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,

                };
                inGrid.Children.Add(statusLine, 0, 0);
                inGrid.Children.Add(statusLabel, 0, 0);

                string name = "";
                string title = g_DealInfolist[i].TITLE;
                if(g_DealInfolist[i].NAME!=null&& g_DealInfolist[i].NAME != "")
                {
                    if (g_DealInfolist[i].NAME.Length == 2)
                    {
                        name = g_DealInfolist[i].NAME.Remove(1) + "*";
                    }
                    else if (g_DealInfolist[i].NAME.Length == 3)
                    {
                        name = g_DealInfolist[i].NAME.Remove(1) + "*" + g_DealInfolist[i].NAME.Remove(0, 2);
                    }
                    else if (g_DealInfolist[i].NAME.Length == 4)
                    {
                        name = g_DealInfolist[i].NAME.Remove(1) + "**" + g_DealInfolist[i].NAME.Remove(0, 3);
                    }
                    else
                    {
                        name = "비회원";
                    }
                }

                if (g_DealInfolist[i].ISCHECK.Equals("1"))
                {
                    title += " 구매";
                }
                else
                {
                    title += " 판매";
                }

                DateTime date = DateTime.ParseExact(g_DealInfolist[i].TOTALDATE, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                string time = date.TimeOfDay.ToString();
                string s4 = date.Day.ToString();
                string hour = date.Hour.ToString();
                string minute = date.Minute.ToString();

                CustomLabel dateLabel = new CustomLabel
                {
                    Text = name + " " + title + "["+ hour+":"+minute+"]",
                    Size = 14,
                    TextColor = Color.Black,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Start
                };
                inGrid.Children.Add(dateLabel, 1, 0);
                RealTimeGrid.Children.Add(inGrid, 0, i);         //실시간거래 그리드에 행 추가
            }

            #endregion
        }


        // 상품권 목록
        private void Showimge()
        {
            CategoryGrid.Children.Clear();
            CategoryGrid.RowDefinitions.Clear();

            List<G_CategoryInfo> categories = new List<G_CategoryInfo>();

            #region 네트워크 상태 확인
            var current_network = Connectivity.NetworkAccess; // 현재 네트워크 상태
            if (current_network == NetworkAccess.Internet) // 네트워크 연결 가능
            {
                categories = giftDBFunc.SelectAllCategory(); // 상품권 목록 가져오기
            }
            else
            {
                categories = null;
            }
            #endregion

            #region 네트워크 연결 불가
            if (categories == null) // 네트워크 연결 불가
            {
                CustomLabel label = new CustomLabel
                {
                    Text = "네트워크에 연결할 수 없습니다. 다시 시도해 주세요.",
                    Size = 18,
                    TextColor = Color.Black,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center
                };
                CategoryGrid.Children.Add(label, 0, 0);         //실시간거래 그리드에 라벨추가
                return;
            }
            #endregion

            #region 상품권 목록 검색 불가
            if (categories.Count == 0)
            {
                if (categories[0].Error == null || categories[0].Error == "")
                {
                    CustomLabel label = new CustomLabel
                    {
                        Text = "상품권 목록을 불러 올 수 없습니다!",
                        Size = 10,
                        TextColor = Color.Black,
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Center
                    };
                    CategoryGrid.Children.Add(label, 0, 0);         //실시간거래 그리드에 라벨추가
                    return;
                }
            }
            #endregion

            #region 상품권 목록 가져오기
            int columnindex = 2;
            int rowindex = 0;
            Grid ColumnGrid = new Grid();

            for (int i = 0; i < categories.Count;)
            {          
                if (columnindex > 1) // 열 그리드
                {
                    columnindex = 0;
                    CategoryGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                    ColumnGrid = new Grid
                    {
                        ColumnDefinitions =
                        {
                            new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star)},
                            new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star)}
                        },
                        RowSpacing = 0,
                        Margin = new Thickness(10, 0, 10, 0),
                    };
                    CategoryGrid.Children.Add(ColumnGrid, 0, rowindex);
                    rowindex+=1;
                }

                BoxView borderLine = new BoxView
                {
                    BackgroundColor = Color.FromHex("#ebecf9"),
                };
                // 내부 그리드
                Grid inGrid = new Grid
                {
                    RowDefinitions =
                        {
                        new RowDefinition {  Height = 60},
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Auto)},
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Auto)},
                        },
                    BindingContext = i + 1,
                    BackgroundColor = Color.White,
                    Margin = 1,
                };
                ColumnGrid.Children.Add(borderLine, columnindex, 0);
                ColumnGrid.Children.Add(inGrid, columnindex, 0);



                CachedImage image = new CachedImage
                {
                    LoadingPlaceholder = Global.LoadingImagePath,
                    ErrorPlaceholder = Global.NotFoundImagePath,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Start,
                    Aspect = Aspect.AspectFill,
                    Margin = new Thickness(15,0,0,0),
                    Source = ImageSource.FromUri(new Uri(Global.server_ipadress + categories[i].Image)),
                    //Source = "test_icon.png",
                };
                inGrid.Children.Add(image, 0, 0);

                CustomLabel nameLabel = new CustomLabel
                {
                    Text = categories[i].Name,
                    Size = 14,
                    TextColor = Color.Black,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Start,
                    FontAttributes = FontAttributes.Bold,
                    Margin = new Thickness(15,0,0,0),
                };
                inGrid.Children.Add(nameLabel, 0, 1);

                CustomLabel detailLabel = new CustomLabel
                {
                    Size = 14,
                    TextColor = Color.Gray,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Start,
                    FontAttributes = FontAttributes.Bold,
                    Margin = new Thickness(15, 0, 0, 5),
                };

                // 상품권 하위 카테고리 초기화
                List<DetailCategory> giftNameList = new List<DetailCategory>();
                #region 네트워크 상태 확인
                if (current_network == NetworkAccess.Internet) // 네트워크 연결 가능
                {
                    giftNameList = giftDBFunc.PostSelectDetailCategoryToIndex(categories[i].CategoryNum);
                }
                else
                {
                    giftNameList = null;
                }
                #endregion

                if(giftNameList != null)
                {
                    string named = "";
                    for (int k = 0; k < giftNameList.Count; k++)
                    {
                        named += giftNameList[k].PRODUCTTYPE;
                        if (k != giftNameList.Count - 1) // 마지막일 경우 쉼표를 붙히지 않음.
                        {
                            named += ", ";
                        }
                    }
                    detailLabel.Text = named;
                }

                inGrid.Children.Add(detailLabel, 0, 2);

                inGrid.GestureRecognizers.Add(new TapGestureRecognizer()
                {
                    Command = new Command(() =>
                    {
                        Global.deal_select_category_value = "구매"; // 상품권 탭 클릭시 디폴트 상태는 구매.
                        mainPage.ShowDealDetailAsync(inGrid.BindingContext.ToString());
                    })
                });
                i++;
                columnindex++;
            }

            #endregion
        }

        private void Arrow_Clicked(object sender, EventArgs e)
        {
        }

        private void RealTimeBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Realtime_Price());
        }
    }
}