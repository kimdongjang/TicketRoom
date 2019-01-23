using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.Shop.GridImage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopupImage
    {
        string image_source = "";
        public PopupImage(string name)
        {
            InitializeComponent();
            image_source = name.Remove(0, 6); // 문자열 형식 File: 파일이름으로 들어오기 때문에 0부터 6개 문자를 제거함.
            SeenImage.Source = image_source;
        }
    }
}