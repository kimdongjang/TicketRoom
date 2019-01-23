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

[assembly: ExportRenderer(typeof(CustomEditor), typeof(CustomEditorRenderer))]
namespace TicketRoom.iOS.Renderer
{
    public class CustomEditorRenderer : EditorRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.Layer.BorderWidth = 0;
            }
        }
    }
}