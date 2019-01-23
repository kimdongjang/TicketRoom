using System;
using System.Collections.Generic;
using TicketRoom.Models.Custom;
using TicketRoom.Models.ShopData;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.Shop
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShopMainPage : ContentPage
    {
        string myShopName = "";
        ShopDataFunc dataclass = new ShopDataFunc();
        List<Button> tablist = new List<Button>();
        Queue<CustomButton> SelectTap_Queue = new Queue<CustomButton>();

        public ShopMainPage(string name)
        {
            InitializeComponent();
            myShopName = name;
            Init();
        }

        private void Init()
        {
            // 타이틀 탭 초기화
            TitleName.Text = myShopName;

            // 리스트에 버튼 추가
            tablist.Add(Content_Sale);
            tablist.Add(Content_Info);
            tablist.Add(Content_Review);

            Content_Changed(tablist[0], null); // Default로 보여질 콘텐츠 뷰
        }



        /// <summary>
        /// 콘텐츠 뷰 변경 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Content_Changed(object sender, EventArgs e)
        {
            CustomButton selectedtab = (CustomButton)sender;

            if (SelectTap_Queue.Count < 2)
            {
                if (SelectTap_Queue.Count != 0)
                {
                    CustomButton temp = SelectTap_Queue.Dequeue();
                    temp.TextColor = Color.Black;
                    temp.BackgroundColor = Color.White;
                }
                selectedtab.TextColor = Color.White;
                selectedtab.BackgroundColor = Color.Black;
                SelectTap_Queue.Enqueue(selectedtab);

            }

            if (selectedtab.Text.Equals("판매품"))
            {
                ShopContentView.Content = new ShopSaleView(myShopName);
            }
            else if (selectedtab.Text.Equals("정보"))
            {
                ShopContentView.Content = new ShopInfoView(myShopName);
            }
            else if (selectedtab.Text.Equals("리뷰"))
            {
                ShopContentView.Content = new ShopReviewView(myShopName);
            }

        }



        private void ImageButton_Clicked(object sender, EventArgs e)
        {

        }

        private void Tab_Changed(object sender, EventArgs e)
        {

        }

        private void CallBtn_Clicked(object sender, EventArgs e)
        {

        }

        private void BasketBtn_Clicked(object sender, EventArgs e)
        {

        }

        private void Insta_btn_Clicked(object sender, EventArgs e)
        {
            Uri uri = new Uri("http://www.naver.com");
            Xamarin.Forms.Device.OpenUri(uri);
        }

        private void MoveShop_btn_Clicked(object sender, EventArgs e)
        {
            Uri uri = new Uri("http://www.naver.com");
            Xamarin.Forms.Device.OpenUri(uri);
        }
    }
}