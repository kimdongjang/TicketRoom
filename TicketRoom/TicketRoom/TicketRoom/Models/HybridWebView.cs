using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TicketRoom.Models
{
    public class HybridWebView : View
    {
        Action<string> action;
        public static readonly BindableProperty UriProperty = BindableProperty.Create(
          propertyName: "Uri",
          returnType: typeof(string),
          declaringType: typeof(HybridWebView),
          defaultValue: default(string));

        public string Uri
        {
            get { return (string)GetValue(UriProperty); }
            set { SetValue(UriProperty, value); }
        }


        // PG사 결제 연동 객체
        public static readonly BindableProperty ParamProperty = BindableProperty.Create(
          propertyName: "Param",
          returnType: typeof(IMPParam),
          declaringType: typeof(HybridWebView),
          defaultValue: default(IMPParam));

        public IMPParam Param
        {
            get { return (IMPParam)GetValue(ParamProperty); }
            set { SetValue(ParamProperty, value); }
        }

        public void RegisterAction(Action<string> callback)
        {
            action = callback;
        }

        public void Cleanup()
        {
            action = null;
        }

        public void InvokeAction(string data)
        {
            if (action == null || data == null)
            {
                return;
            }
            action.Invoke(data);
        }
    }
}
