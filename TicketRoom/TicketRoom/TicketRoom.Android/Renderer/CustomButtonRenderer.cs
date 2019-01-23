using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using TicketRoom.Droid.Renderer;
using TicketRoom.Models.Custom;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomButton), typeof(CustomButtonRenderer))]
namespace TicketRoom.Droid.Renderer
{
    public class CustomButtonRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);

            CustomButton control = e.NewElement as CustomButton;
            var label = (TextView)Control;
            label.SetTextSize(Android.Util.ComplexUnitType.Dip, control.Size);
        }
    }
}