using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TicketRoom.Models.PointData;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.MyPage.Point
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PointWidhdrawView : ContentView
    {
        PointDBFunc PT_DB = PointDBFunc.Instance();

        PointCheckPage pcp;
        PT_Point pp;

        public PointWidhdrawView(PointCheckPage pcp, PT_Point pp)
        {
            InitializeComponent();
            this.pp = pp;
            this.pcp = pcp;
            Init();
        }
        private void Init()
        {
            MyPointLabel.Text = pp.PT_POINT_HAVEPOINT.ToString("N0") + "포인트"; // 보유 포인트

            #region 카드결제 피커 초기화
            BankPicker.Items.Add("농협");
            BankPicker.Items.Add("신한은행");
            BankPicker.Items.Add("하나은행");
            BankPicker.Items.Add("국민은행");
            BankPicker.Items.Add("기업은행");
            #endregion
        }

        private async void ConfirmBtn_ClickedAsync(object sender, EventArgs e)
        {
            if(BankPicker.SelectedIndex == -1)
            {
                await App.Current.MainPage.DisplayAlert("알림", "출금 은행이 선택되지 않았습니다.", "확인");
                return;
            }
            if (AccountEntry.Text == "")
            {
                await App.Current.MainPage.DisplayAlert("알림", "출금 계좌가 입력되지 않았습니다.", "확인");
                return;
            }
            if (NameEntry.Text == "")
            {
                await App.Current.MainPage.DisplayAlert("알림", "예금주가 입력되지 않았습니다.", "확인");
                return;
            }
            if (WidhdrawPointEntry.Text == "")
            {
                await App.Current.MainPage.DisplayAlert("알림", "출금 금액이 입력되지 않았습니다.", "확인");
                return;
            }
            if (PT_DB.PostInsertPointWithDrawToID("포인트 출금", // 내용
                BankPicker.SelectedItem.ToString(), // 출금은행
                AccountEntry.Text, // 출금계좌
                NameEntry.Text, // 예금주
                pp.USER_ID, // 유저아이디
                WidhdrawPointEntry.Text, // 출금금액
                System.DateTime.Now.ToString(), // 날짜
                pp.PT_POINT_INDEX.ToString()) == false)  // 포인트 인덱스
            {
                await App.Current.MainPage.DisplayAlert("알림", "포인트 출금에 실패했습니다. 다시 한번 시도해주십시오.", "확인");
                return;
            }
            
            await App.Current.MainPage.DisplayAlert("알림", "포인트 출금에 성공했습니다.", "확인");
            await Navigation.PopModalAsync();
        }

        private void WidhdrawPointEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                WidhdrawPointEntry.Text = Regex.Replace(WidhdrawPointEntry.Text, @"\D", "");
                if (e.NewTextValue.Contains(".") || e.NewTextValue.Equals("-"))
                {
                    if (e.OldTextValue != null)
                    {
                        WidhdrawPointEntry.Text = e.OldTextValue;
                    }
                    else
                    {
                        WidhdrawPointEntry.Text = "";
                    }
                    return;
                }
                else
                {
                    if (int.Parse(WidhdrawPointEntry.Text) > pp.PT_POINT_HAVEPOINT) // 입력한 포인트가 보유 포인트보다 클 경우
                    {
                        WidhdrawPointEntry.Text = pp.PT_POINT_HAVEPOINT.ToString();
                    }
                    else
                    {
                    }
                }
            }
            catch
            {

            }
        }
    }
}