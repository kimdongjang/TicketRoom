using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TicketRoom.Models.Custom;
using TicketRoom.Models.PointData;
using TicketRoom.Models.ShopData;
using TicketRoom.Models.Users;
using TicketRoom.Views.MainTab.Popup;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.Shop
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShopOrderPage : ContentPage
    {
        ShopDBFunc SH_DB = ShopDBFunc.Instance();
        PointDBFunc PT_DB = PointDBFunc.Instance();
        UserDBFunc USER_DB = UserDBFunc.Instance();
        List<SH_BasketList> basketList = new List<SH_BasketList>();
        List<SH_Home> homeList = new List<SH_Home>();

        List<string> SH_ProductNameList = new List<string>();
        List<string> DeliveryPicker = new List<string>();

        // 배송비 선불 착불 라디오 부울 변수
        string DeliveryOption = "선불";

        // 결제 방식 라디오 부울 변수
        string payOption = ""; // Personal, Card, Business, Phone

        // 현금 영수증 라디오 부울 변수
        bool b_Personal = true;
        bool b_Business = false;

        // 포인트 사용할때의 엔트리 포커스 여부 확인
        bool b_pointLabel = false;
        // 배송 관련 피커 오픈 여부 확인
        bool b_deliveryPicker = false;
        // 배송 요청사항 직접사항 엔트리 생성 여부
        bool b_deliveryEntry = false;

        string ShopOrderPage_ID = "";

        CustomPicker card_picker = new CustomPicker();
        CustomPicker cash_picker = new CustomPicker();
        CustomPicker phone_picker = new CustomPicker();

        Xamarin.Forms.Entry phoneEntry = new Xamarin.Forms.Entry();
        Xamarin.Forms.Entry nameEntry = new Xamarin.Forms.Entry();
        Xamarin.Forms.Entry deliveryEntry = new Xamarin.Forms.Entry();

        int MyPoint = 100; // 잔여 포인트
        int MyUsePoint = 0; // 사용 포인트
        int AmountOfPay = 0; // 결제금액
        int DeliveryPrice = 0; // 배송비



        PopupPhoneEntry popup_phone; // 핸드폰 번호 변경 팝업 객체
        PopupNameEntry popup_name; // 핸드폰 번호 변경 팝업 객체

        // 결제할 금액을 생성자로 받아와야함
        #region 생성자
        public ShopOrderPage(List<SH_BasketList> basketList)
        {
            Xamarin.Forms.Application.Current.On<Xamarin.Forms.PlatformConfiguration.Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);
            InitializeComponent();
            this.basketList = basketList;

            if (Global.b_user_login == false) // 로그인이 안되있을 경우
            {
                ShopOrderPage_ID = Global.non_user_id;
                MyPoint = 0;
                AdressLabel.Text = "";
                MyPhoneLabel.Text = "";
            }
            else if (Global.b_user_login == true) // 로그인이 되어있을 경우
            {
                ShopOrderPage_ID = Global.ID;
                MyPoint = PT_DB.PostSearchPointListToID(ShopOrderPage_ID).PT_POINT_HAVEPOINT;

                AdressLabel.Text = Global.adress.ROADADDR; // 도로명 주소
                MyPhoneLabel.Text = Global.user.PHONENUM; // 폰 넘버 초기화
            }


            PurchaseListInit();
            Init();
        }
        #endregion



        private void PurchaseListInit() // 구매할 목록 초기화
        {
            int row = 0;
            for (int i = 0; i < basketList.Count; i++)
            {
                PurchaseListGrid.RowDefinitions.Add(new RowDefinition { Height = 100 });
                PurchaseListGrid.RowDefinitions.Add(new RowDefinition { Height = 3 });
                Grid inGrid = new Grid
                {
                    ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = 100 },
                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                        new ColumnDefinition { Width = 30 }
                    },
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Margin = new Thickness(20,0,20,0),
                    RowSpacing = 0,
                    ColumnSpacing = 0,
                };

                #region 장바구니 상품 이미지
                Image product_image = new Image
                {
                    Source = basketList[i].SH_BASKET_IMAGE,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    Aspect = Aspect.AspectFill,
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
                        new RowDefinition { Height = 10 },
                        new RowDefinition { Height = GridLength.Auto }
                    },

                };

                #region 상품 제목 Label
                CustomLabel pro_label = new CustomLabel
                {
                    Text = basketList[i].SH_BASKET_NAME,
                    Size = 18,
                    TextColor = Color.Black,
                };
                #endregion

                #region 상품 종류 Label (사이즈, 색상, 추가옵션)
                CustomLabel type_label = new CustomLabel
                {
                    Text = "색상 : " + basketList[i].SH_BASKET_COLOR + ", 사이즈 : " + basketList[i].SH_BASKET_SIZE + ", " + basketList[i].SH_BASKET_COUNT + "개",
                    Size = 14,
                    TextColor = Color.DarkGray,
                };
                #endregion

                #region 가격 내용 Label 및 장바구니 담은 날짜
                CustomLabel price_label = new CustomLabel
                {
                    Text = basketList[i].SH_BASKET_PRICE.ToString("N0") + "원",
                    Size = 14,
                    TextColor = Color.Gray,
                };
                #endregion

                //상품 설명 라벨 그리드에 추가
                product_label_grid.Children.Add(pro_label, 0, 0);
                product_label_grid.Children.Add(type_label, 0, 1);
                product_label_grid.Children.Add(price_label, 0, 3);
                #endregion

                #region 상품권 그리드 자식 추가
                inGrid.Children.Add(product_image, 0, 0);
                inGrid.Children.Add(product_label_grid, 1, 0);
                #endregion


                //장바구니 리스트 그리드에 추가 
                PurchaseListGrid.Children.Add(inGrid, 0, row);
                row++;


                #region 구분선
                BoxView gridline = new BoxView
                {
                    BackgroundColor = Color.FromHex("#f4f2f2"),
                    VerticalOptions = LayoutOptions.End,
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };
                //구분선 그리드에 추가 
                PurchaseListGrid.Children.Add(gridline, 0, row);
                row++;
                #endregion
            }
        }

        private void Init()
        {
            #region 배송비 착불 선불 선택
            PayRadioGrid.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    PayRadioImage.Source = "radio_checked_icon.png";
                    ArriveRadioImage.Source = "radio_unchecked_icon.png";
                    DeliveryOption = "선불";
                    deliveryPayLabel.Text = "배송비: " + DeliveryPrice;
                    DeliveryPayUpdate();
                    AmountOfPayUpdate();
                })
            });
            ArriveRadioGrid.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    ArriveRadioImage.Source = "radio_checked_icon.png";
                    PayRadioImage.Source = "radio_unchecked_icon.png";
                    DeliveryOption = "착불";
                    deliveryPayLabel.Text = "";
                    DeliveryPrice = 0;
                    AmountOfPayUpdate();
                })
            });
            #endregion

            #region 카드 현금 휴대폰 결제 방식 선택
            CardPay.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    CardRadio.Source = "radio_checked_icon.png";
                    CashRadio.Source = "radio_unchecked_icon.png";
                    PhoneRadio.Source = "radio_unchecked_icon.png";
                    payOption = "Card";
                    PhoneOptionGrid.Children.Clear();
                    CardOptionEnable();
                })
            });
            CashPay.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    CardRadio.Source = "radio_unchecked_icon.png";
                    CashRadio.Source = "radio_checked_icon.png";
                    PhoneRadio.Source = "radio_unchecked_icon.png";
                    payOption = "Personal";
                    PhoneOptionGrid.Children.Clear();
                    CashOptionEnable();
                })
            });
            PhonePay.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    CardRadio.Source = "radio_unchecked_icon.png";
                    CashRadio.Source = "radio_unchecked_icon.png";
                    PhoneRadio.Source = "radio_checked_icon.png";
                    payOption = "Phone";
                    PhoneOptionGrid.Children.Clear();
                    PhoneOptionEnable();
                })
            });
            #endregion

            #region 배송 선택사항 피커 초기화
            DeliveryPicker.Add("부재시 경비실에 맡겨주세요.");
            DeliveryPicker.Add("현관 앞에 놓아주세요.");
            DeliveryPicker.Add("배송 전 연락 부탁드립니다.");
            DeliveryPicker.Add("(배송 선택 사항)직접 입력");

            foreach (string name in DeliveryPicker)
            {
                DeliveryContentPicker.Items.Add(name);
            }

            // 직접입력 피커가 선택되었을 경우
            DeliveryContentPicker.Focused += (object sender, FocusEventArgs e) =>
            {
                if (b_deliveryPicker == false)
                {
                    b_deliveryPicker = true;
                }
            };
            DeliveryContentPicker.Unfocused += (object sender, FocusEventArgs e) =>
            {
                b_deliveryPicker = false;
                DeliveryContentPicker.IsEnabled = true;
            };
            DeliveryContentPicker.SelectedIndexChanged += (object sender, EventArgs e) =>
            {
                if (DeliveryContentPicker.SelectedIndex == 3 && b_deliveryEntry == false) // 직접 입력이 선택되었을 경우
                {
                    deliveryEntry = new Xamarin.Forms.Entry
                    {

                    };
                    DeliveryGrid.Children.Add(deliveryEntry, 0, 1); // 피커 바로 아래에 입력사항 엔트리 추가
                    b_deliveryEntry = true;
                    deliveryEntry.Focus();
                }
                else if (b_deliveryEntry == true) // 직접 입력을 선택하지 않을 경우 엔트리 삭제
                {
                    b_deliveryEntry = false;
                    DeliveryGrid.Children.RemoveAt(1);
                }
            };
            #endregion

            DeliveryPayUpdate();
            PointUpdate();
            AmountOfPayUpdate();
        }


        private void CardOptionEnable()
        {
            PhoneOptionGrid.Children.Clear();
            card_picker.Items.Clear();
            PhoneOptionGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            PhoneOptionGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            CustomLabel label = new CustomLabel
            {
                Text = "결제 카드 선택",
                Size = 18,
            };
            card_picker = new CustomPicker
            {
                Title = "카드를 선택해주세요.",
                FontSize = 18,
            };
            PhoneOptionGrid.Children.Add(label, 0, 0);
            PhoneOptionGrid.Children.Add(card_picker, 0, 1);

            #region 카드결제 피커 초기화
            card_picker.Items.Add("농협카드");
            card_picker.Items.Add("신한카드");
            card_picker.Items.Add("하나카드");
            card_picker.Items.Add("국민카드");
            card_picker.Items.Add("기업카드");
            card_picker.Items.Add("현대카드");
            card_picker.Items.Add("삼성카드");
            #endregion
        }
        private void CashOptionEnable()
        {
            PhoneOptionGrid.Children.Clear();
            PhoneOptionGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            PhoneOptionGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            CustomLabel label = new CustomLabel
            {
                Text = "은행명",
                Size = 18,
            };
            cash_picker = new CustomPicker
            {
                Title = "은행을 선택해주세요.",
                FontSize = 18,
            };
            PhoneOptionGrid.Children.Add(label, 0, 0);
            PhoneOptionGrid.Children.Add(cash_picker, 0, 1);

            #region 결제 피커 초기화
            cash_picker.Items.Add("농협");
            cash_picker.Items.Add("신한은행");
            cash_picker.Items.Add("하나은행");
            cash_picker.Items.Add("국민은행");
            cash_picker.Items.Add("기업은행");
            #endregion

            #region 발급정보
            PhoneOptionGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            CustomLabel label2 = new CustomLabel
            {
                Text = "발급정보",
                Size = 18,
            };
            PhoneOptionGrid.Children.Add(label2, 0, 2);
            #endregion

            PhoneOptionGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            Grid inGrid = new Grid
            {
                ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    },
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(15, 0, 0, 0),
                RowSpacing = 0,
                ColumnSpacing = 0,
            };

            #region 현금영수증 개인 선택 라디오
            Grid perGrid = new Grid
            {
                ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = 20 },
                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    },
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                RowSpacing = 0,
                ColumnSpacing = 0,
            };
            Image perImage = new Image
            {
                Source = "radio_checked_icon.png",
                VerticalOptions= LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Aspect = Aspect.AspectFit,
                HeightRequest = 40,
                WidthRequest = 40,
            };
            CustomLabel perLabel = new CustomLabel
            {
                Text = "개인소득공제",
                Size = 18,
                VerticalOptions = LayoutOptions.Center,
            };
            perGrid.Children.Add(perImage, 0, 0);
            perGrid.Children.Add(perLabel, 1, 0);

            #endregion
            #region 현금영수증 사업자 선택 라디오
            Grid busGrid = new Grid
            {
                ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = 20 },
                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    },
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                RowSpacing = 0,
                ColumnSpacing = 0,
            };
            Image busImage = new Image
            {
                Source = "radio_unchecked_icon.png",
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Aspect = Aspect.AspectFit,
                HeightRequest = 40,
                WidthRequest = 40,
            };
            CustomLabel busLabel = new CustomLabel
            {
                Text = "사업자 지출증빙",
                Size = 18,
                VerticalOptions = LayoutOptions.Center,
            };
            busGrid.Children.Add(busImage, 0, 0);
            busGrid.Children.Add(busLabel, 1, 0);

            #endregion

            #region 라디오 선택 이벤트
            perGrid.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    perImage.Source = "radio_checked_icon.png";
                    busImage.Source = "radio_unchecked_icon.png";
                    payOption = "Personal";
                    b_Business = false;
                    b_Personal = true;
                })
            });
            inGrid.Children.Add(perGrid, 0, 0);
            busGrid.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    busImage.Source = "radio_checked_icon.png";
                    perImage.Source = "radio_unchecked_icon.png";
                    payOption = "Business";
                    b_Business = true;
                    b_Personal = false;
                })
            });
            inGrid.Children.Add(busGrid, 1, 0);
            PhoneOptionGrid.Children.Add(inGrid, 0, 3);
            #endregion

            #region 현금영수증 레이블
            PhoneOptionGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            PhoneOptionGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            PhoneOptionGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            PhoneOptionGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            CustomLabel phoneLabel = new CustomLabel
            {
                Size = 18,
            };
            PhoneOptionGrid.Children.Add(phoneLabel, 0, 4);
            phoneEntry = new Xamarin.Forms.Entry
            {
                FontSize = 18,
            };
            PhoneOptionGrid.Children.Add(phoneEntry, 0, 5);
            CustomLabel nameLabel = new CustomLabel
            {
                Size = 18,
            };
            PhoneOptionGrid.Children.Add(nameLabel, 0, 6);
            nameEntry = new Xamarin.Forms.Entry
            {
                FontSize = 18,
            };
            PhoneOptionGrid.Children.Add(nameEntry, 0, 7);
            if (b_Personal == true) // 현금 영수증 개인이 선택되었을 경우
            {
                phoneLabel.Text = "휴대폰(- 빼고 입력 해 주세요)";
                phoneEntry.Placeholder = "ex) 01012340000";
                nameLabel.Text = "이름";
                nameEntry.Placeholder = "ex) 홍길동";
            }
            else if (b_Business == true) // 현금 영수증 사업자가 선택되었을 경우
            {
                phoneLabel.Text = "사업자 등록 번호(- 빼고 입력 해 주세요)";
                phoneEntry.Placeholder = "ex) 1113330000";
                nameLabel.Text = "사업자 이름";
                nameEntry.Placeholder = "ex) 홍길동";
            }
            #endregion
        }
        private void PhoneOptionEnable()
        {
            PhoneOptionGrid.Children.Clear();
            phone_picker.Items.Clear();
            PhoneOptionGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            PhoneOptionGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            CustomLabel label = new CustomLabel
            {
                Text = "통신사 선택",
                Size = 18,
            };
            phone_picker = new CustomPicker
            {
                Title = "통신사를 선택해주세요.",
                FontSize = 18,
            };
            PhoneOptionGrid.Children.Add(label, 0, 0);
            PhoneOptionGrid.Children.Add(phone_picker, 0, 1);

            #region 통신사 피커 초기화
            phone_picker.Items.Add("KT");
            phone_picker.Items.Add("LGU+");
            phone_picker.Items.Add("SKT");
            #endregion
        }

        // 배송비 갱신 함수
        private void DeliveryPayUpdate()
        {
            DeliveryPrice = 0;
            for (int i = 0; i < basketList.Count; i++)
            {
                SH_Home home = SH_DB.PostSearchHomeToHome(basketList[i].SH_HOME_INDEX);
                if (home != null)
                {
                    homeList.Add(home);
                }
            }

            List<int> countList = new List<int>();
            for (int i = 0; i < homeList.Count; i++) // Soruce
            {
                bool bVal = false;
                for (int j = 0; j < countList.Count; j++) // A
                {
                    if (homeList[i].SH_HOME_INDEX == countList[j]) // 중복된 쇼핑몰이 있다면 브레이크.
                    {
                        bVal = true;
                        break;
                    }
                }
                if (bVal == false) // 중복되지 않은 쇼핑몰이 있다면
                {
                    countList.Add(homeList[i].SH_HOME_INDEX);
                    DeliveryPrice += homeList[i].SH_HOME_DELEVERY; // 배송비 갱신
                }
            }
        }

        // 포인트 갱신 함수 (미완성)
        private void PointUpdate()
        {
            // 잔여 포인트 갱신
            RestPoint.Text = "잔여 포인트 : " + MyPoint.ToString();
        }

        // 상품 가격 갱신 함수
        private void AmountOfPayUpdate() // 상품 가격
        {
            AmountOfPay = 0;
            for (int i = 0; i < basketList.Count; i++)
            {
                AmountOfPay += basketList[i].SH_BASKET_PRICE;
            }
            AmountOfPay -= DeliveryPrice; // 결제금액 - 배송비
            AmountOfPay -= MyUsePoint; // 결제금액 - 사용 포인트
            PriceLabel.Text = AmountOfPay.ToString("N0") + "원";
        }


        // 포인트 사용
        private async void PointUseBtn_ClickedAsync(object sender, EventArgs e)
        {
            if (await DisplayAlert("포인트 사용", "포인트를 사용하시겠습니까?", "확인", "취소") == false)
            {
                return;
            }

            if (InputPointEntry.Text == "") // 포인트 칸이 빈칸일 경우,
            {
                await DisplayAlert("주의", "사용할 포인트를 입력해주십시오!", "확인");
                return;
            }
            else
            {
                if (int.Parse(Regex.Replace(InputPointEntry.Text, @"\D", "")) > MyPoint) // 보유 포인트가 모자를 경우
                {
                    await DisplayAlert("주의", "사용 가능한 포인트 금액을 초과했습니다!", "확인");
                    return;
                }
                else
                {
                    MyUsePoint += int.Parse(InputPointEntry.Text); // 사용 포인트 엔트리 -> int 변수

                    UsedPointLabel.Text = "포인트 사용 : " + MyUsePoint.ToString() + " Point";
                    MyPoint -= MyUsePoint; // 소유한 포인트 갱신
                    PointUpdate(); // xaml 잔여 포인트 갱신
                    AmountOfPayUpdate(); // 결제금액 갱신
                }
            }
        }

        // 주문 버튼 클릭했을시
        private async void PaymentBtn_Clicked(object sender, EventArgs e)
        {
            if (AdressLabel.Text == "")
            {
                await DisplayAlert("알림", "배송지가 입력되지 않았습니다!", "확인"); return;
            }
            if (card_picker.SelectedIndex == -1 && cash_picker.SelectedIndex == -1 && phone_picker.SelectedIndex == -1)
            {
                await DisplayAlert("알림", "결제수단이 선택되지 않았습니다!", "확인"); return;
            }
            if (cash_picker.SelectedIndex != -1 && phoneEntry.Text == "" && nameEntry.Text == "")
            {
                await DisplayAlert("알림", "빈 칸을 채워주십시오!", "확인"); return;
            }
            else
            {
                //장바구니로 이동
                var answer = await DisplayAlert("결제금액 : " + PriceLabel.Text, "결제 정보가 맞습니까?", "확인", "취소");
                if (answer)
                {
                    if (DeliveryContentPicker.SelectedIndex != -1) // 배송 선택사항이 선택되지 않았을 경우
                    {
                        int OrderIndex = SH_DB.PostInsertPurchaseListToID(DeliveryPrice.ToString()/*배송비*/, DeliveryOption/*선불착불*/, ""/*배송선택사항*/,
                            AdressLabel.Text/*배송지*/, MyPhoneLabel.Text/*휴대폰번호*/, "상품준비중"/*배송상태*/, payOption/*결제수단*/,
                            AmountOfPay.ToString()/*결제금액*/, MyPoint.ToString()/*사용포인트*/, "결제대기중"/*결제상태*/,
                            ShopOrderPage_ID/*아이디*/, System.DateTime.Now.ToString());
                        if (OrderIndex == -1)
                        {
                            await DisplayAlert("알림", "오류가 발생했습니다. 다시 한번 시도해주십시오.", "확인"); return;
                        }
                        else
                        {
                            for (int i = 0; i < basketList.Count; i++)
                            {

                                if (SH_DB.SH_UpdateProductCountToIndex(basketList[i].SH_PRODUCT_INDEX.ToString(), basketList[i].SH_BASKET_COUNT.ToString()) == false)
                                {
                                    await DisplayAlert("알림", "구매 가능한 수량이 부족합니다.", "확인"); return;
                                }

                                // 구매 목록에 장바구니에 저장했던 상품들 추가
                                if (SH_DB.PostInsertProductToPurchaseList(basketList[i].SH_HOME_INDEX.ToString(),
                                    basketList[i].SH_BASKET_IMAGE,
                                    basketList[i].SH_BASKET_COUNT.ToString(),
                                    basketList[i].SH_BASKET_COLOR,
                                    basketList[i].SH_BASKET_SIZE,
                                    basketList[i].SH_BASKET_NAME,
                                    basketList[i].SH_BASKET_ID,
                                    OrderIndex.ToString(),
                                    basketList[i].SH_BASKET_PRICE.ToString(),
                                    basketList[i].SH_PRODUCT_INDEX.ToString()) == false)
                                {
                                    await DisplayAlert("알림", "오류가 발생했습니다. 다시 한번 시도해주십시오.", "확인"); return;
                                }
                                if (SH_DB.PostDeleteBasketListToBasket(basketList[i].SH_BASKET_INDEX.ToString()) == false)
                                {
                                    await DisplayAlert("알림", "장바구니의 내용을 갱신하는 도중 문제가 발생했습니다. 다시 한번 시도해주십시오.", "확인"); return;
                                }
                            }
                            //Personal, Card, Business, Phone
                            if (payOption == "Card")
                            {
                                if (SH_DB.PostInsertPayCardToPay(card_picker.SelectedItem.ToString(), OrderIndex.ToString()) == false)
                                {
                                    await DisplayAlert("알림", "오류가 발생했습니다. 다시 한번 시도해주십시오.", "확인"); return;
                                }
                            }
                            else if (payOption == "Business")
                            {
                                if (SH_DB.PostInsertPayBusinessToPay(phoneEntry.Text, nameEntry.Text, cash_picker.SelectedItem.ToString(), OrderIndex.ToString()) == false)
                                {
                                    await DisplayAlert("알림", "오류가 발생했습니다. 다시 한번 시도해주십시오.", "확인"); return;
                                }
                            }
                            else if (payOption == "Personal")
                            {
                                if (SH_DB.PostInsertPayPersonalToPay(phoneEntry.Text, nameEntry.Text, cash_picker.SelectedItem.ToString(), OrderIndex.ToString()) == false)
                                {
                                    await DisplayAlert("알림", "오류가 발생했습니다. 다시 한번 시도해주십시오.", "확인"); return;
                                }
                            }
                            else if (payOption == "Phone")
                            {
                                if (SH_DB.PostInsertPayPhoneToPay(card_picker.SelectedItem.ToString(), OrderIndex.ToString()) == false)
                                {
                                    await DisplayAlert("알림", "오류가 발생했습니다. 다시 한번 시도해주십시오.", "확인"); return;
                                }
                            }
                            await DisplayAlert("알림", "결제 성공", "확인");
                            base.OnBackButtonPressed();

                            // 구매 성공시 상품 수량 마이너스 할 것.
                        }
                    }

                    // 결제 완료 페이지 이동
                    // 장바구니 리스트, 유저 아이디, 날짜, 배송정보
                }
            }
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
        }

        // 엔트리 텍스트 내용 초기화
        private void DeliveryContent_Focused(object sender, FocusEventArgs e)
        {

        }


        private void ChangeAdressBtn_Clicked(object sender, EventArgs e)
        {

        }


        private void BackButton_Clicked(object sender, EventArgs e)
        {
            BasketTabPage.isOpenPage = false;
            Navigation.PopModalAsync();
        }
        protected override bool OnBackButtonPressed()
        {
            BasketTabPage.isOpenPage = false;
            return base.OnBackButtonPressed();

        }


        private void InputPointEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (e.NewTextValue.Contains(".") || e.NewTextValue.Equals("-"))
                {
                    if (e.OldTextValue != null)
                    {
                        InputPointEntry.Text = e.OldTextValue;
                    }
                    else
                    {
                        InputPointEntry.Text = "";
                    }
                    return;
                }
                else
                {
                    if (int.Parse(InputPointEntry.Text) > AmountOfPay) // 입력한 포인트가 결제금액보다 클 경우
                    {
                        InputPointEntry.Text = MyPoint.ToString();
                    }
                    else
                    {
                        if (int.Parse(InputPointEntry.Text) > int.Parse(MyPoint.ToString().Replace(",", "")))
                        {
                            InputPointEntry.Text = (AmountOfPay + DeliveryPrice).ToString().Replace(",", "");
                        }
                    }
                }
            }
            catch
            {

            }
        }

        private void ChangePhoneBtn_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.PushAsync(popup_phone = new PopupPhoneEntry(this));
        }

        private void ChangNameBtn_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.PushAsync(popup_name = new PopupNameEntry(this));
        }

    }
}