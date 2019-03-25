using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TicketRoom.Models.PointData;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.MyPage.Point
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PointWidhdrawView : ContentView
    {
        PointDBFunc PT_DB = PointDBFunc.Instance();
        private bool isChangeText = false;
        private int usepoint = 0;
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
            if (usepoint < 10000)
            {
                await App.Current.MainPage.DisplayAlert("알림", "포인트 출금은 만원 이상부터 가능합니다.", "확인");
                return;
            }
            if (BankPicker.SelectedIndex == -1)
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
            if (usepoint == 0)
            {
                await App.Current.MainPage.DisplayAlert("알림", "출금 금액이 입력되지 않았습니다.", "확인");
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
                if (PT_DB.PostInsertPointWithDrawToID("포인트 출금", // 내용
                    BankPicker.SelectedItem.ToString(), // 출금은행
                    AccountEntry.Text, // 출금계좌
                    NameEntry.Text, // 예금주
                    pp.USER_ID, // 유저아이디
                    usepoint.ToString(), // 출금금액
                    pp.PT_POINT_INDEX.ToString()) == false)  // 포인트 인덱스
                {
                    await App.Current.MainPage.DisplayAlert("알림", "포인트 출금에 실패했습니다. 다시 한번 시도해주십시오.", "확인");
                    return;
                }
            }
            #endregion
            
            await App.Current.MainPage.DisplayAlert("알림", "포인트 출금에 성공했습니다.", "확인");
            await Navigation.PopAsync();
        }

        private void MyUsedPointButton_Clicked(object sender, EventArgs e)
        {
            if(WidhdrawPointEntry.Text != "") // null이 아닐 경우
            {
                usepoint = int.Parse(Regex.Replace(WidhdrawPointEntry.Text, @"\D", "")); // 숫자만 추출
                if(pp.PT_POINT_HAVEPOINT > usepoint) // 보유 포인트가 사용 포인트보다 많을 경우
                {
                    MyUsedPointLabel.Text = "사용 포인트 : " + usepoint.ToString();
                    WidhdrawPointEntry.Text = "";
                }
                else if(pp.PT_POINT_HAVEPOINT == 0)// 원래는 0
                {
                    App.Current.MainPage.DisplayAlert("알림", "출금 가능한 포인트가 없습니다.", "확인");
                    WidhdrawPointEntry.Text = "";
                }
                else // 많게 입력할 경우 전부 사용
                {
                    MyUsedPointLabel.Text = "사용 포인트 : " + pp.PT_POINT_HAVEPOINT.ToString();
                    WidhdrawPointEntry.Text = "";
                }
            }
        }
    }
}