using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using TicketRoom.iOS.Renderer;
using TicketRoom.Models.Custom;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomPicker), typeof(CustomPickerRenderer))]
namespace TicketRoom.iOS.Renderer
{
    public class CustomPickerRenderer : PickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.TextAlignment = UITextAlignment.Center;
            }
        }
    }
}