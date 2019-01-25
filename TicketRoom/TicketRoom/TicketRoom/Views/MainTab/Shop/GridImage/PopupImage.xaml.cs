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
            image_source = name.Replace("Uri: ","");
            SeenImage.Source = image_source;
        }
    }
}