using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.Users.CreateUser
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AcceptTermsPage : ContentPage
    {
        Dictionary<Image, bool> RadioGroup = new Dictionary<Image, bool>();
        List<string> termstitle = new List<string> { "상품권 거래 이용약관 동의", "전자금융 거래 이용약관 동의", "개인정보 수집이용 동의", "마케팅 정보 메일 SMS 수신동의(선택)" };

        public AcceptTermsPage()
        {
            InitializeComponent();

            #region 라벨 클릭 이벤트
            // Your label tap event
            var label_tap = new TapGestureRecognizer();
            label_tap.Tapped += (s, e) =>
            {
                CheckContent_Clicked(s, e);
            };
            #endregion

            #region 이미지 클릭 이벤트
            // Your label tap event
            var image_tap = new TapGestureRecognizer();
            image_tap.Tapped += (s, e) =>
            {
                ChecRbtn_Checked(s, e);
            };
            #endregion

            for (int i = 0; i < termstitle.Count; i++)
            {
                RadioGrid.RowDefinitions.Add(new RowDefinition { Height = 40 });

                #region 약관 그리드
                Grid grid = new Grid
                {
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    RowSpacing = 0,
                    ColumnSpacing = 0,
                    ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                        new ColumnDefinition { Width = 15 },
                    }
                };
                #endregion

                #region 약관 내용 Label
                Label label = new Label
                {
                    Text = termstitle[i],
                    TextDecorations = TextDecorations.Underline,
                    FontSize = 18,
                    TextColor = Color.Black,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    YAlign = TextAlignment.Center,
                    HorizontalOptions = LayoutOptions.Start
                };
                #endregion

                #region 약관 Radio
                Image image = new Image
                {
                    Source = "radio_unchecked_icon.png",
                    BackgroundColor = Color.White,
                    VerticalOptions = LayoutOptions.Center
                };
                #endregion

                #region 그리드에 추가
                RadioGrid.Children.Add(grid, 0, i + 1); //부모그리드에 약관 그리드 추가
                grid.Children.Add(label, 0, 0);         //약관 그리드에 라벨추가
                grid.Children.Add(image, 1, 0);         //약관 그리드에 Radio이미지 추가
                #endregion

                RadioGroup.Add(image, false); //라디오 그룹 관리 true : checked , false : unchecked
                label.GestureRecognizers.Add(label_tap); //라벨 클릭 이벤트 등록
                image.GestureRecognizers.Add(image_tap); //이미지 클릭 이벤트 등록

            }
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void CheckAll_Rbtn_Clicked(object sender, EventArgs e)
        {
            try
            {
                //체크안된것이 있다면
                if (RadioGroup.ContainsValue(false))
                {
                    for (int i = 0; i < RadioGroup.Count; i++)
                    {
                        if (!RadioGroup.Values.ToList()[i])
                        {
                            RadioGroup.Keys.ToList()[i].Source = "radio_checked_icon.png";
                            RadioGroup[RadioGroup.Keys.ToList()[i]] = !RadioGroup[RadioGroup.Keys.ToList()[i]];

                        }
                    }
                    selectallradio.Source = "radio_checked_icon.png";
                }
                else
                {
                    for (int i = 0; i < RadioGroup.Count; i++)
                    {
                        if (RadioGroup.Values.ToList()[i])
                        {
                            RadioGroup.Keys.ToList()[i].Source = "radio_unchecked_icon.png";
                            RadioGroup[RadioGroup.Keys.ToList()[i]] = !RadioGroup[RadioGroup.Keys.ToList()[i]];
                        }
                    }
                    selectallradio.Source = "radio_unchecked_icon.png";
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("오류", ex.ToString(), "OK");
            }
        }

        private void CheckContent_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new TermsContentPage());
        }


        private void ChecRbtn_Checked(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < RadioGroup.Count; i++)
                {
                    if (sender == RadioGroup.Keys.ToList()[i])
                    {
                        if (RadioGroup.Values.ToList()[i])
                        {
                            RadioGroup.Keys.ToList()[i].Source = "radio_unchecked_icon.png";
                            RadioGroup[RadioGroup.Keys.ToList()[i]] = !RadioGroup[RadioGroup.Keys.ToList()[i]];
                        }
                        else
                        {
                            RadioGroup.Keys.ToList()[i].Source = "radio_checked_icon.png";
                            RadioGroup[RadioGroup.Keys.ToList()[i]] = !RadioGroup[RadioGroup.Keys.ToList()[i]];
                        }
                    }
                }

                if (RadioGroup.ContainsValue(false))
                {
                    selectallradio.Source = "radio_unchecked_icon.png";
                }
                else
                {
                    selectallradio.Source = "radio_checked_icon.png";
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("오류", ex.ToString(), "OK");
            }
        }

        private void ShowContent_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("내용보기 보여주셈", "클릭", "ok");
        }

        private void ChangeRadioimage(Image item)
        {
            if (RadioGroup[item])
            {
                item.Source = "radio_unchecked_icon.png";
                RadioGroup[item] = !RadioGroup[item];
            }
            else
            {
                item.Source = "radio_checked_icon.png";
                RadioGroup[item] = !RadioGroup[item];
            }
        }

        private void NextBtn_Clicked(object sender, EventArgs e)
        {
            Dictionary<string, bool> sendlist = new Dictionary<string, bool>();//전달할 객체


            for (int i = 0; i < RadioGroup.Count; i++)
            {
                sendlist.Add(termstitle[i], RadioGroup.Values.ToList()[i]);
                if (i != 3)
                {
                    if (!RadioGroup.Values.ToList()[i])
                    {
                        DisplayAlert("알림", "약관을 동의해주세요", "OK");
                        return;
                    }
                }
            }

            Navigation.PushAsync(new CreateUserpage(sendlist));
        }
    }
}