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

[assembly: ExportRenderer(typeof(CustomButton), typeof(CustomButtonRenderer))]
namespace TicketRoom.iOS.Renderer
{
    public class CustomButtonRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            var label = Control as UIButton;
            if (label != null)
            {
                //label.AdjustsFontSizeToFitWidth = true;
                //label.AdjustsFontForContentSizeCategory = true;
                //label.BaselineAdjustment = UIBaselineAdjustment.AlignCenters;
                //label.LineBreakMode = UILineBreakMode.Clip;

                CustomButton control_label = e.NewElement as CustomButton;
                label.Font = UIFont.FromName("NANUMSQUAREROUNDB.TTF", control_label.Size);
            }
        }
    }
}