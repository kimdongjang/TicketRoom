using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TicketRoom.Models.Gift;
using TicketRoom.Models.Gift.Purchase;
using TicketRoom.Services;
using TicketRoom.Views.MainTab.MyPage;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.Dael.Purchase
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PurchasePage : ContentPage
    {
        MainPage mainpage;
        public string categorynum;
        G_ProductInfo productInfo = null;
        public string Purchase_Price = "";
        public string DiscountPurchase_Price = "";
        GiftDBFunc giftDBFunc = GiftDBFunc.Instance();


        public PurchasePage(MainPage mainpage,G_ProductInfo productInfo, string categorynum)
        {
            InitializeComponent();
            
            this.mainpage = mainpage;
            this.categorynum = categorynum;
            #region IOS의 경우 초기화
            NavigationPage.SetHasNavigationBar(this, false); // Navigation Bar 지우는 코드 생성자에 입력
            if (Xamarin.Forms.Device.OS == TargetPlatform.iOS)
            {
                MainGrid.RowDefinitions[0].Height = Global.title_size_value;
            }
            if (Global.ios_x_model == true) // ios X 이상의 모델일 경우
            {
                MainGrid.RowDefinitions[4].Height = 30;
            }
            #endregion
            this.productInfo = productInfo;

            Contentlabel.Text = "온라인 가격과 매장 판매가격은 왜 다른가요?\n \n할인유통상품권에는 유통 특수성이 있습니다.\n▶ 할인 상품권유통에 있어 티켓룸 당사가 발권사가 아닌 관계로 가격은보이지 않는 유통시장의 시세에 의해 많은 유동성을 갖고 있습니다.\n다시 말씀드리자면, 유통시장의 수요와 공급의 원칙에 의해 지역적 환경의 영향을 많이 받습니다.\n당사에서해당 상품권을 무한 공급 가능하다면 상품 가격은 일정한가격을 유지하여 고객에게공급하여드릴 수 \n있으나할인 상품권의 유통의 유동성이 시시각각으로 변함으로 할인상품권전체 시장 가격이 각기 다를 수밖에 없음을알려드립니다.\n\n▶ 할인 상품권 유통인 경우 티켓룸 본사가 발권사가 아니기에 모든상품권을 본사가 지사에게공급하여일률적인\n가격정책이이루어지고 있는 시스템이 아닙니다. \n티켓룸은 상권별, 시기별로 환경에 따른 나름의 유통물량 과가격을 통해운영되어 지고 \n있습니다. \n이렇듯할인 상품권 가격의 유동성은 각각의 업체마다 물류량에의해 크게 조절되고 있습니다.\n하지만 티켓룸은 보다 신뢰적인 상품권의 유통시스템을 구축하여 나가고있습니다.\n\n즉 티켓룸은 발권사와 전략적 제휴 및 직유통 계약을 통해일정한 상품 공급 물량을 통해본사든 지사든 공통된 \n단가를 적용 시켜 나아가고 있습니다\n\n \n \n \n\n\n\n \n \n\n상품권봉투는 모두 보내주나요?\n \n▶ 주문시[상품권] 봉투수량을 메모란에 기입하지 않는 경우한 장만 보내드립니다.\n\n먼저고객님께 양해 말씀 드리겠습니다.일부 상품권을 제외하고해당 상품권 봉투수량은 한정되어 있어 고객님이 \n원하시는 수량만큼 제공하지 못하는 경우가 있습니다. 이점 다시한번양해 말씀드립니다.\n봉투 30매 이상 추가 요청을 받지 않습니다.\n주문시 [상품권] 봉투 수량 요청을 하지 않으신 분은 봉투 없이 보내드립니다. \n이점 주의 하시기 바라며 봉투는 일 주문당 30장 이내로 보내드리오니 양해하여 주시기 바랍니다.\n\n\n상품권을사면 계산서를 끊어주나요?\n \n주문시 '요청하신 고객님'께 상품권 영수증을 발급, 티켓룸으로 사업자사본을 보내주시면 상품권영수증을 보내드립니다.\n▶ 본 상품권 영수증은 소득세법 제163조 1항 및 5항의 규정에 의한 매입계산서가 아니며 매입처별계산서 합계표를 제출할 의무가 없습니다. \n단순히 거래증빙 자료로 활용 할 수 있으며 원하실 경우 기존의계산서가 아닌 상품권 \n영수증(거래명세서 형식) 으로 발행해 드립니다.\n\n▶주문을 하실 때는 배송메세지에 상품권영수증 요청 후 메일로 사업자사본을 보내주시면 영수증을 동봉해서 보내드립니다. \n됩니다. \n※ 주의사항\n사업자등록증을 확인할때 착오가 없도록 보내실때 사업자등록증과 주문자성명, 연락처, 주문날짜를 \n기록해서보내주시면 더 정확하고 빨리 확인하여 처리할 수 있으니 부탁드립니다.\n\n상품권을 신용카드로 살 수 있나요?\n  \n현재 티켓룸은 현금거래를 기본으로 하고 있습니다.\n\n◎ 참고내용\n상품권은 발행사(예:교육상품권)와 가맹계약이 맺어진 업체(예:A영어사)에서 상품권을 구매하여 소지하고 있는 소비자가 현금대신 지불수단으로 사용할 수 있는 일명 유가증권입니다.\n2002년초에 입법되어져 11월에 확정되어진 상품권 판매법에 따르면 '발행사 또는 발행사와 판매대행계약이 맺어진 위탁판매자' 이외에는 신용카드로 상품권을 판매할 수없다. 로 수정이 되어 확정 발표되었습니다.\n \n위에서 언급했듯 상품권판매에서 판매대행계약이 이루어지지 않은 상품권에 대해서 카드판매는 규제되고 있습니다. 또한 위탁 판매 상품의 카드판매시 공정거래법에 의하여 할인판매를적용할수 없으므로 티켓룸은 현금거래를 기본으로 하고 있습니다.\n \n현재 티켓룸은 고객과의 거래시 현금거래를 기본으로 하고 있습니다만. 고객에게 보다 편리한 거래서비스를 제공하기 위하여 여러 발권사와위탁판매대행계약을 맺어 나가고 있으며, 이에 보다 편리한 판매서비스를 제공할 것을 약속드립니다.\n\n어플리케이션 말고 전화로 사고싶어요!\n\n대량으로 구매하실때 전화로 주문하시면 더욱 편리합니다.\n(☎. 070-4300-1383)  수량이 1매 라도 어플리케이션을 통해 직접 주문하실수 있습니다. \n";
            Pro_imgae.Source = ImageSource.FromUri(new Uri(Global.server_ipadress + productInfo.PRODUCTIMAGE));
            Pro_Name.Text = productInfo.PRODUCTTYPE + " " + productInfo.PRODUCTVALUE;
            Pro_price.Text = "고객 구매가(할인율) : " + productInfo.PURCHASEDISCOUNTPRICE + "[" + productInfo.PURCHASEDISCOUNTRATE + "%]";
            Purchase_Price = (int.Parse(productInfo.PROPRICE) * int.Parse(Count_label.Text)).ToString("N0");
            DiscountPurchase_Price = (int.Parse(productInfo.PURCHASEDISCOUNTPRICE) * int.Parse(Count_label.Text)).ToString("N0");
            Purchase_Price_span.Text = "합계 : " + Purchase_Price;
            Purchase_DiscountPrice_span.Text = "할인 금액 : " + DiscountPurchase_Price;
            DisCountRate_label.Text = productInfo.PURCHASEDISCOUNTRATE + "%";


            G_ProductCount g_count = new G_ProductCount();

            #region 네트워크 상태 확인
            var current_network = Connectivity.NetworkAccess; // 현재 네트워크 상태
            if (current_network == NetworkAccess.Internet) // 네트워크 연결 가능
            {
                g_count = giftDBFunc.Get_Product_Ccount(productInfo.PRONUM);
            }
            else
            {
                g_count = null;
            }
            #endregion

            #region 네트워크 연결 불가
            if (g_count == null) // 네트워크 연결 불가
            {
                return;
            }
            #endregion

            NavigationInit();

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

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Global.isgiftpurchasepage_clieck = true;
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            if (Global.isgiftpurchasepage_clieck)
            {
                Global.isgiftpurchasepage_clieck = false;
                Navigation.PopAsync();
            }
        }

        private void PlusBtn_Clicked(object sender, EventArgs e)
        {
            Count_label.Text = (int.Parse(Count_label.Text) + 1).ToString();
            Purchase_Price = (int.Parse(productInfo.PROPRICE) * int.Parse(Count_label.Text)).ToString("N0");
            DiscountPurchase_Price = (int.Parse(productInfo.PURCHASEDISCOUNTPRICE) * int.Parse(Count_label.Text)).ToString("N0");
            Purchase_Price_span.Text = "합계 : " + Purchase_Price;
            Purchase_DiscountPrice_span.Text = "할인 금액 : " + DiscountPurchase_Price;
            //int teqwteqw = int.Parse(Purchase_Price.Replace(",", "")); //1,000->1000
        }

        private void MinusBtn_Clicked(object sender, EventArgs e)
        {
            if (int.Parse(Count_label.Text) > 0)
            {
                Count_label.Text = (int.Parse(Count_label.Text) - 1).ToString();
                Purchase_Price = (int.Parse(productInfo.PROPRICE) * int.Parse(Count_label.Text)).ToString("N0");
                DiscountPurchase_Price = (int.Parse(productInfo.PURCHASEDISCOUNTPRICE) * int.Parse(Count_label.Text)).ToString("N0");
                Purchase_Price_span.Text = "합계 : " + Purchase_Price;
                Purchase_DiscountPrice_span.Text = "할인 금액 : " + DiscountPurchase_Price;
            }
        }

        private void DoPurchase_Clicked(object sender, EventArgs e)
        {
            G_TempBasketProduct tempBasket = new G_TempBasketProduct();
            if (Global.isgiftpurchasepage_clieck)
            {
                Global.isgiftpurchasepage_clieck = false;
                if (int.Parse(Count_label.Text) == 0)
                {
                    DisplayAlert("알림", "수량을 입력해주세요", "OK");
                    Global.isgiftpurchasepage_clieck = true;
                    return;
                }
                if (prepaymentradio.Source.ToString().Contains("radio_checked_icon.png"))
                {
                    tempBasket = new G_TempBasketProduct
                    {
                        PDL_NAME = Pro_Name.Text,
                        PDL_PRONUM = productInfo.PRONUM,
                        PDL_PROTYPE = "1",
                        PDL_PRICE = (int.Parse(productInfo.PURCHASEDISCOUNTPRICE)).ToString(), // 상품가격
                        PDL_COUNT = Count_label.Text,
                        PRODUCT_IMAGE = productInfo.PRODUCTIMAGE,
                    };
                }
                else
                {
                    tempBasket = new G_TempBasketProduct
                    {
                        PDL_NAME = Pro_Name.Text,
                        PDL_PRONUM = productInfo.PRONUM,
                        PDL_PROTYPE = "2",
                        PDL_PRICE = (int.Parse(productInfo.PURCHASEDISCOUNTPRICE)).ToString(), // 상품가격
                        PDL_COUNT = Count_label.Text,
                        PRODUCT_IMAGE = productInfo.PRODUCTIMAGE,
                    };
                }


                // 로딩 시작
                //await Global.LoadingStartAsync();

                #region 네트워크 상태 확인
                var current_network = Connectivity.NetworkAccess; // 현재 네트워크 상태
                if (current_network == NetworkAccess.Internet) // 네트워크 연결 가능
                {
                    Navigation.PushAsync(new PurchaseDetailPage(tempBasket));
                }
                else
                {
                    DisplayAlert("알림", "네트워크에 연결할 수 없습니다. 다시 한번 시도해주세요.", "확인");
                }
                #endregion



                // 로딩 완료
                //await Global.LoadingEndAsync();
            }

            #region Enable 처리로 더블 클릭 막기 -단점 버튼 텍스트가 지워짐
            //DoPurchaseBtn.IsEnabled = false;
            //if (int.Parse(Count_label.Text) == 0)
            //{
            //    DisplayAlert("알림", "수량을 입력해주세요", "OK");
            //    return;
            //}

            //List<G_PurchasedetailInfo> g_PurchasedetailInfos = new List<G_PurchasedetailInfo>();
            //if (prepaymentradio.Source.ToString().Contains("radio_checked_icon.png"))
            //{
            //    G_PurchasedetailInfo g_PurchasedetailInfo = new G_PurchasedetailInfo
            //    {
            //        PDL_PRONUM = productInfo.PRONUM,
            //        PDL_PROCOUNT = Count_label.Text,
            //        PDL_PROTYPE = "1",
            //        PDL_ALLPRICE = (int.Parse(productInfo.PURCHASEDISCOUNTPRICE) * int.Parse(Count_label.Text)).ToString(),
            //        PRODUCT_IMAGE = productInfo.PRODUCTIMAGE,
            //        PRODUCT_TYPE = productInfo.PRODUCTTYPE,
            //        PRODUCT_VALUE = productInfo.PRODUCTVALUE
            //    };
            //    g_PurchasedetailInfos.Add(g_PurchasedetailInfo);
            //}
            //else
            //{
            //    G_PurchasedetailInfo g_PurchasedetailInfo = new G_PurchasedetailInfo
            //    {
            //        PDL_PRONUM = productInfo.PRONUM,
            //        PDL_PROCOUNT = Count_label.Text,
            //        PDL_PROTYPE = "2",
            //        PDL_ALLPRICE = (int.Parse(productInfo.PURCHASEDISCOUNTPRICE) * int.Parse(Count_label.Text)).ToString(),
            //        PRODUCT_IMAGE = productInfo.PRODUCTIMAGE,
            //        PRODUCT_TYPE = productInfo.PRODUCTTYPE,
            //        PRODUCT_VALUE = productInfo.PRODUCTVALUE
            //    };
            //    g_PurchasedetailInfos.Add(g_PurchasedetailInfo);
            //}

            //// 로딩 시작
            //await Global.LoadingStartAsync();


            //await Navigation.PushAsync(new PurchaseDetailPage(g_PurchasedetailInfos));

            //// 로딩 완료
            //await Global.LoadingEndAsync();
            //DoPurchaseBtn.IsEnabled = true;
            #endregion
        }

        // 장바구니 
        private async void AddBasketBtn_Clicked(object sender, EventArgs e)
        {
            if (Global.isgiftpurchasepage_clieck)
            {
                Global.isgiftpurchasepage_clieck = false;
                if (int.Parse(Count_label.Text) == 0)
                {
                    DisplayAlert("알림", "수량을 정해주세요", "OK");
                    Global.isgiftpurchasepage_clieck = true;
                    return;
                }

                string userid = "";
                string protype = "";
                if (Global.b_user_login)
                {
                    userid = Global.ID;
                }
                else
                {
                    userid = Global.non_user_id;
                }

                if (prepaymentradio.Source.ToString().Contains("radio_checked_icon.png"))
                {
                    protype = "1";
                }
                else
                {
                    protype = "2";
                }

                string str = @"{";
                str += "ID:'" + userid;  //아이디찾기에선 Name으로 
                str += "',PRONUM:'" + productInfo.PRONUM;
                str += "',PROCOUNT:'" + Count_label.Text;
                str += "',PROTYPE:'" + protype;
                str += "'}";

                //// JSON 문자열을 파싱하여 JObject를 리턴
                JObject jo = JObject.Parse(str);

                UTF8Encoding encoder = new UTF8Encoding();
                byte[] data = encoder.GetBytes(jo.ToString()); // a json object, or xml, whatever...

                //request.Method = "POST";
                HttpWebRequest request = WebRequest.Create(Global.WCFURL + "Insert_Basketlist") as HttpWebRequest;
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
                        if (test != null && test != "")
                        {
                            if (test.Equals("true"))
                            {
                                await ShowMessage("장바구니에 추가되었습니다.", "알림", "OK", async () =>
                                {
                                    Global.InitOnAppearingBool("basket");
                                    await Navigation.PopToRootAsync();
                                });
                            }
                            else
                            {
                                await ShowMessage("서버점검중입니다.", "알림", "OK", async () =>
                                {
                                    Global.isgiftpurchasepage_clieck = true;
                                });
                            }
                        }
                    }
                }
            }
                
        }

        private void Radio1_Clicked(object sender, EventArgs e)
        {
            prepaymentradio.Source = "radio_checked_icon.png";
            Cashondeliveryradio.Source = "radio_unchecked_icon.png";
        }

        private void Radio2_Clicked(object sender, EventArgs e)
        {
            prepaymentradio.Source = "radio_unchecked_icon.png";
            Cashondeliveryradio.Source = "radio_checked_icon.png";
        }

        public async Task ShowMessage(string message, string title, string buttonText, Action afterHideCallback)
        {
            await DisplayAlert(title, message, buttonText);

            afterHideCallback?.Invoke();
        }
    }
}