using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TicketRoom.Models;
using TicketRoom.Models.Custom;
using TicketRoom.Models.Gift.Purchase;
using TicketRoom.Models.PointData;
using TicketRoom.Models.Users;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.MyPage.Point
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PointChargeView : ContentView
	{
        PointDBFunc PT_DB = PointDBFunc.Instance();
        UserDBFunc USER_DB = UserDBFunc.Instance();

        CustomPicker account_picker = new CustomPicker();
        string payOption = "Card";

        PointCheckPage pcp;
        PT_Point pp;


        Entry priceEntry = new Entry();

        public PointChargeView (PointCheckPage pcp, PT_Point pp)
		{
			InitializeComponent ();
            this.pcp = pcp;
            this.pp = pp;
            Init();
            CardOptionEnable();
        }




        private void Init()
        {
            MyPointLabel.Text = pp.PT_POINT_HAVEPOINT.ToString("N0") + "포인트";

            PayOptionGrid.Children.Clear();
            
            CardOptionGrid.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    CardRadioImage.Source = "radio_checked_icon.png";
                    payOption = "Card";
                    PayOptionGrid.Children.Clear();
                    CardOptionEnable();
                })
            });
        }
        private void CardOptionEnable()
        {
            PayOptionGrid.Children.Clear();
            account_picker.Items.Clear();
            PayOptionGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            PayOptionGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            PayOptionGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            PayOptionGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            PayOptionGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            PayOptionGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            CustomLabel label = new CustomLabel
            {
                Text = "결제 계좌 선택",
                Size = 18,
                Margin = new Thickness(15, 0, 0, 0),
            };
            account_picker = new CustomPicker
            {
                Title = "계좌를 선택해주세요.",
                FontSize = 18,
            };

            #region 네트워크 상태 확인
            List<AccountInfo> accountlist = new List<AccountInfo>();
            var current_network = Connectivity.NetworkAccess; // 현재 네트워크 상태
            if (current_network != NetworkAccess.Internet) // 네트워크 연결 불가
            {
                accountlist = null;
            }
            #endregion
            #region 네트워크 연결 가능
            else
            {
                accountlist = USER_DB.GetSelectAllAccount();
            }
            #endregion

            if(accountlist == null)
            {
                App.Current.MainPage.DisplayAlert("알림", "네트워크 연결이 되지 않았습니다!", "확인");
                return;
            }
            #region 계좌 피커 초기화
            else
            {
                for (int i = 0; i < accountlist.Count; i++)
                {
                    account_picker.Items.Clear();
                    account_picker.Items.Add(accountlist[i].AC_BANKNAME + ": " + accountlist[i].AC_ACCOUNTNUM + " " + accountlist[i].AC_NAME);
                }
            }

            #endregion

            PayOptionGrid.Children.Add(label, 0, 0);
            PayOptionGrid.Children.Add(account_picker, 0, 1);


            CustomLabel priceLabel = new CustomLabel
            {
                Text = "금액 선택",
                Size = 18,
                Margin = new Thickness(15, 0, 0, 0),
            };
            priceEntry = new Entry
            {
                Placeholder = "0원(숫자만 입력해주십시오.)",
                FontSize = 18,
                HorizontalTextAlignment = TextAlignment.Center,
                Keyboard = Keyboard.Numeric,
            };
            PayOptionGrid.Children.Add(priceLabel, 0, 2);
            PayOptionGrid.Children.Add(priceEntry, 0, 3);
            priceEntry.TextChanged += InputPointEntry_TextChanged;

            CustomLabel alretLabel1 = new CustomLabel
            {
                Text = "[유의 사항]",
                Size = 12,
                TextColor = Color.LightGray,
                Margin = new Thickness(15, 0, 0, 0),
            };
            CustomLabel alretLabel2 = new CustomLabel
            {
                Text = "* 충전된 포인트는 1년간 유효합니다.",
                Size = 12,
                TextColor = Color.LightGray,
                Margin = new Thickness(15, 0, 0, 0),
            };
            PayOptionGrid.Children.Add(alretLabel1, 0, 4);
            PayOptionGrid.Children.Add(alretLabel2, 0, 5);
        }


        private void InputPointEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("retsd");
                priceEntry.Text = Regex.Replace(priceEntry.Text, @"\D", "");
            }
            catch
            {

            }
        }

        public async void PurchaseSuccessProcessAsync(IMP_RValue rvalue) // 결제가 진행되었을 경우
        {
            #region 네트워크 상태 확인
            var current_network = Connectivity.NetworkAccess; // 현재 네트워크 상태
            if (current_network != NetworkAccess.Internet) // 네트워크 연결 불가
            {
                await App.Current.MainPage.DisplayAlert("알림", "네트워크에 연결할 수 없습니다. 고객센터에 문의해주세요.", "확인");
                return;
            }
            #endregion
            #region 네트워크 연결 가능
            else
            {
                // ready(미결제), paid(결제완료), cancelled(결제취소, 부분취소포함), failed(결제실패)
                if (rvalue.SH_STATUS == "paid") // 결제 완료 상태라면 포인트 페이지에서 결제 진행
                {
                    if (PT_DB.PostInsertPointChargeToID(rvalue, pp.PT_POINT_INDEX.ToString()) == false)  // 포인트 인덱스
                    {
                        await App.Current.MainPage.DisplayAlert("알림", "포인트 충전에 실패했습니다. 다시 한번 시도해주십시오.", "확인");
                        return;
                    }
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("알림", "포인트 충전에 실패했습니다. 다시 한번 시도해주십시오.", "확인");
                    return;
                }
                await App.Current.MainPage.DisplayAlert("알림", "포인트 충전에 성공했습니다.", "확인");
                await Navigation.PopAsync();
            }
            #endregion
        }

        private async void ConfirmBtn_ClickedAsync(object sender, EventArgs e)
        {
            if (payOption == "Card")
            {
                if (account_picker.SelectedIndex == -1)
                {
                    await App.Current.MainPage.DisplayAlert("알림", "결제 카드가 선택되지 않았습니다.", "확인");
                    return;
                }
                if (priceEntry.Text == "")
                {
                    await App.Current.MainPage.DisplayAlert("알림", "충전 금액이 입력되지 않았습니다.", "확인");
                    return;
                }

                IMPParam param = new IMPParam
                {
                    pg = "inicis",
                    pay_method = "card",
                    merchant_uid = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"),
                    name = "포인트 충전 " + int.Parse(priceEntry.Text).ToString("N0"),
                    amount = int.Parse(priceEntry.Text),
                    buyer_email = Global.user.EMAIL/*이메일*/,
                    buyer_name = Global.ID/*아이디*/,
                    buyer_tel = Global.user.PHONENUM/*휴대폰번호*/,
                    buyer_addr = Global.adress.ROADADDR/*배송지*/,
                    buyer_postcode = Global.adress.ZIPNO.ToString()/*우편번호*/
                };

                IMP_RValue rvalue = new IMP_RValue
                {
                    SH_BANK_NAME = account_picker.SelectedIndex.ToString(),
                    SH_BUYER_EMAIL = Global.user.EMAIL/*이메일*/,
                    SH_BUYER_NAME = Global.ID/*아이디*/,
                    SH_BUYER_TEL = Global.user.PHONENUM/*휴대폰번호*/,
                    SH_BUYER_ADDR = Global.adress.ROADADDR/*배송지*/,
                    SH_BUYER_POSTCODE = Global.adress.ZIPNO.ToString()/*우편번호*/,
                    SH_PAY_METHOD = "account", // 계좌이체(무통장입금)
                    SH_MERCHANT_UID = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"),
                    SH_NAME = "포인트 충전 " + int.Parse(priceEntry.Text).ToString("N0"),
                    SH_AMOUNT = priceEntry.Text /*충전포인트(입금금액)*/,
                    SH_STATUS = "입금대기",
                    SH_POINT_INDEX = pp.PT_POINT_INDEX.ToString(), // 포인트 인덱스
                };
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
                    if (PT_DB.PostInsertPointChargeToID(rvalue, pp.PT_POINT_INDEX.ToString()) == false)  // 포인트 인덱스
                    {
                        await App.Current.MainPage.DisplayAlert("알림", "포인트 충전에 실패했습니다. 다시 한번 시도해주십시오.", "확인");
                        return;
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("알림", "포인트 충전에 성공했습니다.", "확인");
                        await Navigation.PopAsync();
                    }
                }
                #endregion

                //await Navigation.PushAsync(new IMPHybridWebView(param, this)); // PG사 연결
            }
        }
    }
}