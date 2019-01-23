using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.Shop
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShopOrderPage : ContentPage
    {
        List<string> SH_ProductNameList = new List<string>();
        List<string> PhonePicker = new List<string>();
        List<string> DeliveryPicker = new List<string>();

        bool b_DeliveryPay = true;
        bool b_DeliveryArrive = false;

        bool b_Card = true;
        bool b_Cash = false;
        bool b_Phone = false;
        int i_MyPoint = 100; // 잔여 포인트
        int i_AmountOfPay = 36000; // 결제금액
        string DeliveryPrice = "3000원"; // 배송비

        // 결제할 금액을 생성자로 받아와야함
        #region 생성자
        public ShopOrderPage()
        {
            InitializeComponent();
            Init();
        }
        public ShopOrderPage(List<string> sl)
        {
            InitializeComponent();
            SH_ProductNameList = sl;
            Init();
        }
        #endregion


        private void Get_SH_ProductPrice()
        {
            int tempPrice = 0;
            // DB에서 상품이름 기반으로 상품 금액을 초기화함
            i_AmountOfPay = tempPrice;
            UsedPay.Text = i_AmountOfPay.ToString("N0") + "원";
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
                    b_DeliveryPay = true;
                    b_DeliveryArrive = false;
                    deliveryPayLabel.Text = "배송비: " + DeliveryPrice;
                })
            });
            ArriveRadioGrid.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    ArriveRadioImage.Source = "radio_checked_icon.png";
                    PayRadioImage.Source = "radio_unchecked_icon.png";
                    b_DeliveryPay = false;
                    b_DeliveryArrive = true;
                    deliveryPayLabel.Text = "";
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
                    b_Card = true;
                    b_Cash = false;
                    b_Phone = false;
                })
            });
            CashPay.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    CardRadio.Source = "radio_unchecked_icon.png";
                    CashRadio.Source = "radio_checked_icon.png";
                    PhoneRadio.Source = "radio_unchecked_icon.png";
                    b_Card = false;
                    b_Cash = true;
                    b_Phone = false;
                })
            });
            PhonePay.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    CardRadio.Source = "radio_unchecked_icon.png";
                    CashRadio.Source = "radio_unchecked_icon.png";
                    PhoneRadio.Source = "radio_checked_icon.png";
                    b_Card = false;
                    b_Cash = false;
                    b_Phone = true;
                })
            });
            #endregion

            #region 통신사 피커 초기화
            PhonePicker.Add("KT");
            PhonePicker.Add("LGU+");
            PhonePicker.Add("SKT");

            foreach (string name in PhonePicker)
            {
                PhoneSelectPicker.Items.Add(name);
            }
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
            DeliveryContentPicker.SelectedIndexChanged += (object sender, EventArgs e) =>
            {
                if (DeliveryContentPicker.SelectedIndex == 3) // 직접 입력이 선택되었을 경우
                {
                    DeliveryContentPicker.Unfocus();
                    Entry entry = new Entry
                    {

                    };
                    DeliveryGrid.Children.Add(entry, 0, 1); // 피커 바로 아래에 입력사항 엔트리 추가
                    entry.Focus();
                }
                else
                {
                    DeliveryGrid.Children.RemoveAt(1);
                }
            };
            #endregion

            PointUpdate();
        }

        private void PointUpdate()
        {
            // 잔여 포인트 갱신
            RestPoint.Text = "잔여 포인트 : " + i_MyPoint.ToString();
        }

        private async void PointUseBtn_ClickedAsync(object sender, EventArgs e)
        {
            bool check = await DisplayAlert("포인트 사용", "포인트를 사용하시겠습니까?", "확인", "취소");
            if (check == false)
            {
                return;
            }

            // 입력한 엔트리의 값이 숫자가 아니거나 없을 경우 리턴.
            if (Regex.Replace(InputPoint.Text, @"\D", "") == "")
            {
                await DisplayAlert("주의", "사용할 포인트를 입력해주십시오!", "확인");
                return;
            }
            else
            {
                if (int.Parse(InputPoint.Text) > i_MyPoint)
                {
                    await DisplayAlert("주의", "사용 가능한 포인트 금액을 초과했습니다!", "확인");
                    return;
                }
                else
                {
                    i_AmountOfPay -= int.Parse(InputPoint.Text); // 결제금액 갱신
                    UsedPay.Text = i_AmountOfPay.ToString() + "원";
                    i_MyPoint -= int.Parse(InputPoint.Text); // 소유한 포인트 갱신
                    PointUpdate(); // xaml 잔여 포인트 갱신
                }
            }
        }


        private async void PaymentBtn_Clicked(object sender, EventArgs e)
        {
            string select_phone = "";
            int selectedIndex = PhoneSelectPicker.SelectedIndex;
            if (selectedIndex != -1)
            {
                select_phone = PhoneSelectPicker.Items[selectedIndex];
            }
            if (select_phone != "")
            {
                //장바구니로 이동
                var answer = await DisplayAlert("결제금액 : " + UsedPay.Text, "결제 정보가 맞습니까?", "확인", "취소");
                if (answer)
                {
                    // 결제 완료 페이지 이동
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

        // 엔트리 텍스트 내용 초기화
        private void InputPoint_Focused(object sender, FocusEventArgs e)
        {
            InputPoint.Text = "";
        }
    }
}