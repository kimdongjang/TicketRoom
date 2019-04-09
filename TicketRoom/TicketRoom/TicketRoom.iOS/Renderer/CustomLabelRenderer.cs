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

[assembly: ExportRenderer(typeof(CustomLabel), typeof(CustomLabelRenderer))]
namespace TicketRoom.iOS.Renderer
{
    public class CustomLabelRenderer : LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            if (e == null) return;
            if (e.NewElement == null) return;

            var label = Control as UILabel;

            if (label != null)
            {
                //label.AdjustsFontSizeToFitWidth = true;
                //label.AdjustsFontForContentSizeCategory = true;
                label.BaselineAdjustment = UIBaselineAdjustment.AlignCenters;
                label.LineBreakMode = UILineBreakMode.Clip;

                CustomLabel control_label = e.NewElement as CustomLabel;
                label.Font = UIFont.FromName("NANUMSQUAREROUNDB", control_label.Size - Global.font_size_minus_value);
            }
        }
    }
}