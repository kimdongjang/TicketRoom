﻿using FFImageLoading.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TicketRoom.Models.Custom;
using TicketRoom.Models.Gift;
using TicketRoom.Models.Gift.Purchase;
using TicketRoom.Services;
using TicketRoom.Views.MainTab.Dael.Purchase;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.Basket
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BasketGiftView : ContentView
    {
        BasketTabPage btp;
        GiftDBFunc GIFT_DB = GiftDBFunc.Instance();
        List<Grid> productgridlist = new List<Grid>();
        List<G_BasketInfo> BasketList = new List<G_BasketInfo>();



        public BasketGiftView(BasketTabPage btp)
        {
            InitializeComponent();
            ShowBasketlist();
            ListInit();
            this.btp = btp;
            Global.isgiftbastketorderbtn_clicked = true;
        }

        private void ShowBasketlist()
        {
            string userid = "null";

            if (Global.b_user_login)
            {
                userid = Global.ID;
            }
            else
            {
                userid = Global.non_user_id;
            }
            #region 네트워크 상태 확인
            var current_network = Connectivity.NetworkAccess; // 현재 네트워크 상태
            if (current_network != NetworkAccess.Internet) // 네트워크 연결 불가
            {
                BasketList = null;
            }
            #endregion
            #region 네트워크 연결 가능
            else
            {
                string str = @"{";
                str += "ID:'" + userid;  //아이디찾기에선 Name으로 
                str += "'}";

                //// JSON 문자열을 파싱하여 JObject를 리턴
                JObject jo = JObject.Parse(str);

                UTF8Encoding encoder = new UTF8Encoding();
                byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

                //request.Method = "POST";
                HttpWebRequest request = WebRequest.Create(Global.WCFURL + "Select_Basketlist") as HttpWebRequest;
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
                        if (readdata != null && readdata != "")
                        {
                            BasketList = JsonConvert.DeserializeObject<List<G_BasketInfo>>(readdata);
                        }
                    }
                }
            }
            #endregion

        }
        private void ListInit()
        {     
            Basketlist_Grid.Children.Clear();
            Basketlist_Grid.RowDefinitions.Clear();

            int row = 0;
            int result_price = 0;

            #region 네트워크 연결 불가
            if (BasketList == null)
            {
                Basketlist_Grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

                CustomLabel nullproduct = new CustomLabel
                {
                    Text = "네트워크에 연결할 수 없습니다. 다시 시도해 주세요.",
                    Size = 18,
                    TextColor = Color.Black,
                    VerticalOptions = LayoutOptions.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    HorizontalOptions = LayoutOptions.Center
                };
                Basketlist_Grid.Children.Add(nullproduct, 0, 0);
                Bottom_Grid.IsVisible = false;
                return;
            }
            #endregion

            #region 상품이 준비중
            if (BasketList.Count == 0)
            {
                Basketlist_Grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

                CustomLabel nullproduct = new CustomLabel
                {
                    Text = "장바구니에 상품이 없습니다",
                    Size = 25,
                    TextColor = Color.Black,
                    VerticalOptions = LayoutOptions.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    HorizontalOptions = LayoutOptions.Center
                };
                Basketlist_Grid.Children.Add(nullproduct, 0, 0);
                Bottom_Grid.IsVisible = false;
                return;
            }
            #endregion

            for (int i = 0; i < BasketList.Count; i++)
            {
                Basketlist_Grid.RowDefinitions.Add(new RowDefinition { Height = 100 });
                Basketlist_Grid.RowDefinitions.Add(new RowDefinition { Height = 3 });

                #region 상품 그리드
                Grid product_grid = new Grid
                {
                    Margin = new Thickness(0, 10, 0, 10),
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    BackgroundColor = Color.White,
                    RowSpacing = 0,
                    ColumnSpacing = 0,
                    ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = 100 },
                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                        new ColumnDefinition { Width = 40 },
                        new ColumnDefinition { Width = 20 }
                    },
                    BindingContext = BasketList[i].BASKETLISTTABLE_NUM,
                };
                productgridlist.Add(product_grid);

                #region 장바구니 상품 이미지
                CachedImage product_image = new CachedImage
                {
                    LoadingPlaceholder = Global.LoadingImagePath,
                    ErrorPlaceholder = Global.NotFoundImagePath,
                    Source = ImageSource.FromUri(new Uri(Global.server_ipadress + BasketList[i].BK_PRODUCT_IMAGE)),
                    BackgroundColor = Color.White,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    Aspect = Aspect.AspectFill,
                    Margin = 20,
                };
                #endregion

                #region 상품 설명 Labellist 그리드
                Grid product_label_grid = new Grid
                {
                    Margin = new Thickness(10, 0, 0, 0),
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    RowSpacing = 0,
                    ColumnSpacing = 0,
                    RowDefinitions =
                    {
                        new RowDefinition { Height = GridLength.Auto },
                        new RowDefinition { Height = GridLength.Auto },
                        new RowDefinition { Height = 20 },
                        new RowDefinition { Height = GridLength.Auto }
                    }
                };

                #region 상품 제목 Label
                CustomLabel pro_label = null;
                if (BasketList[i].BK_TYPE.Equals("1"))
                {
                    pro_label = new CustomLabel
                    {
                        Text = BasketList[i].BK_PRODUCT_TYPE + " (지류)",
                        Size = 16,
                        TextColor = Color.Black,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        HorizontalOptions = LayoutOptions.StartAndExpand
                    };
                }
                else
                {
                    pro_label = new CustomLabel
                    {
                        Text = BasketList[i].BK_PRODUCT_TYPE + " (핀번호)",
                        Size = 16,
                        TextColor = Color.Black,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        HorizontalOptions = LayoutOptions.StartAndExpand
                    };
                }
                #endregion

                #region 상품 종류 Label
                CustomLabel type_label = new CustomLabel
                {
                    Text = BasketList[i].BK_PRODUCT_VALUE,
                    Size = 14,
                    TextColor = Color.Black,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalTextAlignment = TextAlignment.Start,
                    VerticalTextAlignment = TextAlignment.Center,
                    HorizontalOptions = LayoutOptions.StartAndExpand
                };
                #endregion

                #region 가격 내용 Label
                CustomLabel price_label = new CustomLabel
                {
                    Text = BasketList[i].BK_PRODUCT_PURCHASE_DISCOUNTPRICE + "원",
                    Size = 14,
                    TextColor = Color.Gray,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.StartAndExpand
                };
                #endregion

                //상품 설명 라벨 그리드에 추가
                product_label_grid.Children.Add(pro_label, 0, 0);
                product_label_grid.Children.Add(type_label, 0, 1);
                product_label_grid.Children.Add(price_label, 0, 4);
                #endregion

                #region 상품 수량 그리드
                Grid product_count_grid = new Grid
                {
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    RowSpacing = 0,
                    ColumnSpacing = 0,
                    //BindingContext = i,
                    RowDefinitions =
                    {
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) }
                    }
                };

                #region 플러스 버튼 
                Image Plus_btn = new Image
                {
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Source = "plus.png",
                    Aspect = Aspect.AspectFit,
                    HeightRequest = 25,
                };
                #endregion

                #region 상품 수량 label
                CustomEntry Count_label = new CustomEntry
                {
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    HorizontalTextAlignment = TextAlignment.Center,
                    Text = BasketList[i].BK_PROCOUNT,
                    Keyboard = Keyboard.Numeric,
                    Size = 14,
                    TextColor = Color.Black,
                };
                #endregion

                #region 마이너스 버튼
                Image minus_btn = new Image
                {
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Source = "minus.png",
                    Aspect = Aspect.AspectFit,
                    HeightRequest = 25,
                };
                #endregion

                //상품 수량 그리드에 추가
                product_count_grid.Children.Add(Plus_btn, 0, 0);
                product_count_grid.Children.Add(Count_label, 0, 1);
                product_count_grid.Children.Add(minus_btn, 0, 2);
                #endregion

                #region 상품권 그리드 자식 추가
                product_grid.Children.Add(product_image, 0, 0);
                product_grid.Children.Add(product_label_grid, 1, 0);
                product_grid.Children.Add(product_count_grid, 2, 0);
                #endregion
                #endregion

                //장바구니 리스트 그리드에 추가 
                Basketlist_Grid.Children.Add(product_grid, 0, row);
                row++;

                #region 장바구니 삭제 버튼
                Grid deleteGrid = new Grid
                {
                    RowDefinitions =
                    {
                         new RowDefinition { Height = new GridLength(2, GridUnitType.Star) },
                         new RowDefinition { Height = new GridLength(8, GridUnitType.Star) },
                    }
                };
                Image deleteImage = new Image
                {
                    BindingContext = i,
                    Source = "x.png",
                    HeightRequest = 40,
                    WidthRequest = 40,
                };


                deleteGrid.Children.Add(deleteImage, 0, 0);
                product_grid.Children.Add(deleteGrid, 3, 0);

                // X버튼 누를시에 해당 리스트 삭제 이벤트
                #region 장바구니 삭제 이벤트
                // Your label tap event
                var deletebtn_Clicked = new TapGestureRecognizer();
                deletebtn_Clicked.Tapped += async (s, e) =>
                {
                    bool check = await App.Current.MainPage.DisplayAlert("삭제", "장바구니 항목을 삭제하시겠습니까?", "확인", "취소");
                    if (check == false)
                    {
                        return;
                    }
                    #region 네트워크 상태 확인
                    var current_network = Connectivity.NetworkAccess; // 현재 네트워크 상태
                    if (current_network != NetworkAccess.Internet) // 네트워크 연결 불가
                    {
                        await App.Current.MainPage.DisplayAlert("알림", "네트워크에 연결할 수 없습니다. 다시 한번 시도해주세요.", "확인");
                        return;
                    }
                    #endregion
                    #region 네트워크 연결 가능
                    else
                    {
                        Image deletegrid = (Image)s;
                        //Global.BasketList.RemoveAt(int.Parse(deletegrid.BindingContext.ToString()));

                        string str2 = @"{";
                        str2 += "basketlistnum:'" + BasketList[int.Parse(deletegrid.BindingContext.ToString())].BASKETLISTTABLE_NUM;  //아이디찾기에선 Name으로 
                        str2 += "'}";

                        //// JSON 문자열을 파싱하여 JObject를 리턴
                        JObject jo2 = JObject.Parse(str2);

                        UTF8Encoding encoder2 = new UTF8Encoding();
                        byte[] data2 = encoder2.GetBytes(jo2.ToString()); // a json object, or xml, whatever...

                        //request.Method = "POST";
                        HttpWebRequest request2 = WebRequest.Create(Global.WCFURL + "Delete_Basketlist") as HttpWebRequest;
                        request2.Method = "POST";
                        request2.ContentType = "application/json";
                        request2.ContentLength = data2.Length;

                        //request.Expect = "application/json";

                        request2.GetRequestStream().Write(data2, 0, data2.Length);

                        using (HttpWebResponse response2 = request2.GetResponse() as HttpWebResponse)
                        {
                            if (response2.StatusCode != HttpStatusCode.OK)
                                Console.Out.WriteLine("Error fetching data. Server returned status code: {0}", response2.StatusCode);
                            using (StreamReader reader = new StreamReader(response2.GetResponseStream()))
                            {
                                var readdata = reader.ReadToEnd();
                                string test = JsonConvert.DeserializeObject<string>(readdata);
                                if (test != null && test != "")
                                {
                                    if (test.Equals("true"))
                                    {
                                        await App.Current.MainPage.DisplayAlert("알림", "장바구니 항목이 삭제되었습니다.", "확인");
                                    }
                                    else
                                    {
                                        await App.Current.MainPage.DisplayAlert("알림", "서버점검중입니다", "확인");
                                    }
                                }
                            }
                        }
                        ShowBasketlist();
                        ListInit();
                    }
                    #endregion
                };
                #endregion
                deleteImage.GestureRecognizers.Add(deletebtn_Clicked);
                #endregion


                #region 이미지 클릭 이벤트
                // Your label tap event
                var plus_tap = new TapGestureRecognizer();
                plus_tap.Tapped += (s, e) =>
                {
                    plusBtn_Clicked(s, e);
                };
                #endregion

                #region 이미지 클릭 이벤트
                // Your label tap event
                var minus_tap = new TapGestureRecognizer();
                minus_tap.Tapped += (s, e) =>
                {
                    minusBtn_Clicked(s, e);
                };
                #endregion

                Plus_btn.GestureRecognizers.Add(plus_tap);
                minus_btn.GestureRecognizers.Add(minus_tap);

                BoxView gridline = new BoxView
                {
                    BackgroundColor = Color.LightGray,
                    HeightRequest = 1,
                    VerticalOptions = LayoutOptions.End,
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };
                Basketlist_Grid.Children.Add(gridline, 0, row);
                row++;

                result_price += int.Parse(BasketList[i].BK_PROCOUNT) * int.Parse(BasketList[i].BK_PRODUCT_PURCHASE_DISCOUNTPRICE);
            }

            ResultPrice_label.Text = "합계 : " + result_price.ToString("N0") + "원";
        }

        private void plusBtn_Clicked(object s, EventArgs e)
        {
            #region 네트워크 상태 확인
            var current_network = Connectivity.NetworkAccess; // 현재 네트워크 상태
            if (current_network != NetworkAccess.Internet) // 네트워크 연결 불가
            {
            }
            #endregion

            #region 네트워크 연결 가능
            else
            {
                Image button = (Image)s;
                //countlabelist[int.Parse(button.Parent.BindingContext.ToString())].Text = (int.Parse(countlabelist[int.Parse(button.Parent.BindingContext.ToString())].Text)+1).ToString();
                Grid g = (Grid)button.Parent;
                List<Xamarin.Forms.View> b = g.Children.ToList();
                CustomEntry count = (CustomEntry)b[1];
                count.Text = (int.Parse(count.Text) + 1).ToString();
                Grid product_grid = (Grid)g.Parent; // 메인 로우 그리드

                GIFT_DB.UpdateGiftBasketListToIndex(product_grid.BindingContext.ToString(), count.Text); // 수량 변경 요청



                Grid g2 = (Grid)g.Parent;
                List<Xamarin.Forms.View> b2 = g2.Children.ToList();
                Grid g3 = (Grid)b2[1];
                List<Xamarin.Forms.View> b3 = g3.Children.ToList();
                CustomLabel price = (CustomLabel)b3[2];
                ResultPrice_label.Text = "합계 : " + (int.Parse(ResultPrice_label.Text.Replace("합계 : ", "").Replace(",", "").Replace("원", "")) + int.Parse(price.Text.Replace("원", ""))).ToString("N0") + "원";
            }
            #endregion
        }

        private void minusBtn_Clicked(object s, EventArgs e)
        {
            #region 네트워크 상태 확인
            var current_network = Connectivity.NetworkAccess; // 현재 네트워크 상태
            if (current_network != NetworkAccess.Internet) // 네트워크 연결 불가
            {
            }
            #endregion

            #region 네트워크 연결 가능
            else
            {
                Image button = (Image)s;
                //countlabelist[int.Parse(button.Parent.BindingContext.ToString())].Text = (int.Parse(countlabelist[int.Parse(button.Parent.BindingContext.ToString())].Text) - 1).ToString();

                Grid g = (Grid)button.Parent;
                List<Xamarin.Forms.View> b = g.Children.ToList();
                CustomEntry count = (CustomEntry)b[1];

                if (int.Parse(count.Text) > 0)
                {
                    count.Text = (int.Parse(count.Text) - 1).ToString();
                    Grid product_grid = (Grid)g.Parent; // 메인 로우 그리드
                    GIFT_DB.UpdateGiftBasketListToIndex(product_grid.BindingContext.ToString(), count.Text); // 수량 변경 요청

                    Grid g2 = (Grid)g.Parent;
                    List<Xamarin.Forms.View> b2 = g2.Children.ToList();
                    Grid g3 = (Grid)b2[1];
                    List<Xamarin.Forms.View> b3 = g3.Children.ToList();
                    CustomLabel price = (CustomLabel)b3[2];
                    ResultPrice_label.Text = "합계 : " + (int.Parse(ResultPrice_label.Text.Replace("합계 : ", "").Replace(",", "").Replace("원", "")) - int.Parse(price.Text.Replace("원", ""))).ToString("N0") + "원";
                }
            }
            #endregion
        }

        private void OrderBtn_Clicked(object sender, EventArgs e)
        {
            if (Global.b_guest_login == true)
            {
                App.Current.MainPage.DisplayAlert("알림", "회원가입 후에 이용해주세요!", "확인");
                return;
            }

            #region 네트워크 상태 확인
            var current_network = Connectivity.NetworkAccess; // 현재 네트워크 상태
            if (current_network != NetworkAccess.Internet) // 네트워크 연결 불가
            {
                App.Current.MainPage.DisplayAlert("알림", "네트워크 연결이 원활하지 않습니다.", "확인");
            }
            #endregion

            #region 네트워크 연결 가능
            else
            {
                if (Global.b_user_login == true) // 회원으로 로그인 되어있는 경우
                {
                    GIFT_DB.PostDeleteGiftBasketListCountZero(Global.ID);
                }
                else
                {
                    GIFT_DB.PostDeleteGiftBasketListCountZero(Global.non_user_id);
                }
                ShowBasketlist();

                if (Global.isgiftbastketorderbtn_clicked)
                {
                    Global.isgiftbastketorderbtn_clicked = false;
                    List<G_TempBasketProduct> tempBasketList = new List<G_TempBasketProduct>();
                    for (int i = 0; i < BasketList.Count; i++)
                    {
                        Grid g = productgridlist[i];
                        List<Xamarin.Forms.View> b = g.Children.ToList();
                        Grid g2 = (Grid)b[2];
                        List<Xamarin.Forms.View> b2 = g2.Children.ToList();
                        CustomEntry g3 = (CustomEntry)b2[1];

                        if (int.Parse(g3.Text) != 0)
                        {
                            G_TempBasketProduct tempBasket = new G_TempBasketProduct
                            {
                                PDL_NAME = BasketList[i].BK_PRODUCT_TYPE + BasketList[i].BK_PRODUCT_VALUE, // 상품이름
                                PDL_PRONUM = BasketList[i].BK_PRONUM,
                                PDL_PROTYPE = BasketList[i].BK_TYPE,
                                // 주문할때 상품 가격 갱신해서 가져오기
                                PDL_PRICE = GIFT_DB.PostSelectGiftDiscountPriceToIndex(BasketList[i].BK_PRONUM), // 상품가격
                                PDL_COUNT = BasketList[i].BK_PROCOUNT,
                                PRODUCT_IMAGE = BasketList[i].BK_PRODUCT_IMAGE,
                                BASKET_INDEX = BasketList[i].BASKETLISTTABLE_NUM,
                            };
                            tempBasketList.Add(tempBasket);
                        }
                    }

                    Navigation.PushAsync(new PurchaseDetailPage(tempBasketList));
                }
                #endregion
            }
        }
    }
}