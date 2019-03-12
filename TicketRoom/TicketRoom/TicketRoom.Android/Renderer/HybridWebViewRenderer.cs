using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using Android.Net;
using Java.Interop;
using Java.Net;
using TicketRoom.Droid.Renderer;
using TicketRoom.Models;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Uri = Android.Net.Uri;

[assembly: ExportRenderer(typeof(HybridWebView), typeof(HybridWebViewRenderer))]
namespace TicketRoom.Droid.Renderer
{
    public class HybridWebViewRenderer : ViewRenderer<HybridWebView, Android.Webkit.WebView>
    {
        const string JavascriptFunction = "function func1(data){jsBridge.invokeAction(data);}";
        Context _context;

        public HybridWebViewRenderer(Context context) : base(context)
        {
            _context = context;
            System.Diagnostics.Debug.WriteLine(context);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<HybridWebView> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                var webView = new Android.Webkit.WebView(_context);
                webView.Settings.JavaScriptEnabled = true;
                //webView.SetWebViewClient(new JavascriptWebViewClient($"javascript: {JavascriptFunction}"));
                webView.SetWebViewClient(new JavaScriptWebViewClient2(this.Context, this)); 
                SetNativeControl(webView);
            }
            if (e.OldElement != null)
            {
                Control.RemoveJavascriptInterface("jsBridge");
                var hybridWebView = e.OldElement as HybridWebView;
                hybridWebView.Cleanup();
            }
            if (e.NewElement != null)
            {
                Control.AddJavascriptInterface(new JSBridge(this), "jsBridge");
                Control.LoadUrl($"{Element.Uri}");
            }
        }
    }
    public class JSBridge : Java.Lang.Object
    {
        readonly WeakReference<HybridWebViewRenderer> hybridWebViewRenderer;

        public JSBridge(HybridWebViewRenderer hybridRenderer)
        {
            hybridWebViewRenderer = new WeakReference<HybridWebViewRenderer>(hybridRenderer);
        }

        [JavascriptInterface]
        [Export("invokeAction")]
        public void InvokeAction(string data)
        {
            HybridWebViewRenderer hybridRenderer;

            if (hybridWebViewRenderer != null && hybridWebViewRenderer.TryGetTarget(out hybridRenderer))
            {
                hybridRenderer.Element.InvokeAction(data);
            }
        }
    }
    public class JavascriptWebViewClient : WebViewClient
    {
        string _javascript;

        public JavascriptWebViewClient(string javascript)
        {
            _javascript = javascript;
        }

        public override void OnPageFinished(Android.Webkit.WebView view, string url)
        {
            base.OnPageFinished(view, url);
            view.EvaluateJavascript(_javascript, null);
        }
    }

    #region LDH 추가부분
    public class JavaScriptWebViewClient2 : WebViewClient
    {
        private Context context;
        HybridWebViewRenderer hybridWebViewRenderer;
        public JavaScriptWebViewClient2(Context context, HybridWebViewRenderer hybridWebViewRenderer)
        {
            this.context = context;
            this.hybridWebViewRenderer = hybridWebViewRenderer;
        }
        public override void OnPageFinished(Android.Webkit.WebView view, string url)
        {
            base.OnPageFinished(view, url);
            IMPParam param = new IMPParam();
            param.pg = "inicis";
            param.pay_method = "card";
            param.merchant_uid = "merchant_" + System.DateTime.Now;
            param.name = $"test";
            param.buyer_email = "iamport@siot.do";
            param.buyer_name = "구매자이름";
            param.buyer_tel = "010-1234-5678";
            param.amount = 5000;
            //retorno = await browser.EvaluateJavaScriptAsync($"func1( \"inicis\",\"card\",\"merchant_\",\"stemp\",\"stemp\",\"stemp\",\"stemp\",\"stemp\",\"stemp\")");
            String blank = "\"";
            string script = string.Format("javascript:func4(" + blank + param.pg.ToString() + blank + "," +
                                                                                           blank + param.pay_method.ToString() + blank + "," +
                                                                                            blank + param.merchant_uid.ToString() + blank + "," +
                                                                                            blank + param.name.ToString() + blank + "," +
                                                                                            param.amount.ToString() + "," +
                                                                                            "\"stemp\",\"stemp\",\"stemp\",\"stemp\")");
            view.EvaluateJavascript(script, null);
        }
        public override bool ShouldOverrideUrlLoading(Android.Webkit.WebView view, string url)
        {
            if (!url.StartsWith("http://") && !url.StartsWith("https://") && !url.StartsWith("javascript:")&& !url.StartsWith("file:"))
            {
                Intent intent = null;

                try
                {
                    intent = Intent.ParseUri(url, Intent.UriIntentScheme); //IntentURI처리
                    Uri uri = Uri.Parse(intent.DataString);
                    
                    context.StartActivity(new Intent(Intent.ActionView, uri)); //해당되는 Activity 실행
                    return true;
                }
                catch (URISyntaxException ex)
                {
                    return false;
                }
                catch (ActivityNotFoundException e)
                {
                    if (intent == null) return false;

                    if (handleNotFoundPaymentScheme(intent.Scheme)) return true; //설치되지 않은 앱에 대해 사전 처리(Google Play이동 등 필요한 처리)

                    String packageName = intent.Package;
                    if (packageName != null)
                    { //packageName이 있는 경우에는 Google Play에서 검색을 기본
                        Uri uri = Uri.Parse("market://details?id=" + packageName);
                        context.StartActivity(new Intent(Intent.ActionView, uri));
                        return true;
                    }

                    return false;
                }
            }
            else if (url.StartsWith("http://175.115.110.17:8088/Service1.svc"))
            {
                try
                {
                    string temp = url.Replace("http://175.115.110.17:8088/Service1.svc?imp_uid=", "");
                    if (temp.Contains("&"))
                    {
                        string[] imp_uid = temp.Split('&');
                        //$"{Element.Uri}"
                        hybridWebViewRenderer.Element.InvokeAction(imp_uid[0]);
                        hybridWebViewRenderer.Element.Uri = null;
                        hybridWebViewRenderer.RemoveAllViews();
                        hybridWebViewRenderer.RemoveView(view);
                        
                    }
                }
                catch (Exception)
                {
                    throw;
                }

            }

            return false;
        }

        protected bool handleNotFoundPaymentScheme(String scheme)
        {
            //PG사에서 호출하는 url에 package정보가 없어 ActivityNotFoundException이 난 후 market 실행이 안되는 경우
            if (PaymentScheme.ISP.Equals(scheme, StringComparison.OrdinalIgnoreCase))
            {
                Uri uri = Uri.Parse("market://details?id=" + PaymentScheme.PACKAGE_ISP);
                context.StartActivity(new Intent(Intent.ActionView, uri));
                return true;
            }
            else if (PaymentScheme.BANKPAY.Equals(scheme, StringComparison.OrdinalIgnoreCase))
            {
                Uri uri = Uri.Parse("market://details?id=" + PaymentScheme.PACKAGE_BANKPAY);
                context.StartActivity(new Intent(Intent.ActionView, uri));
                return true;
            }

            return false;
        }

        public class PaymentScheme
        {
            public static String ISP = "ispmobile"; //	ISP모바일 : ispmobile://TID=nictest00m01011606281506341724
            public static String BANKPAY = "kftc-bankpay";

            public static String LOTTE_APPCARD = "lotteappcard";                  //	롯데앱카드 : intent://lottecard/data?acctid=120160628150229605882165497397&apptid=964241&IOS_RETURN_APP=#Intent;scheme=lotteappcard;package=com.lcacApp;end
            public static String HYUNDAI_APPCARD = "hdcardappcardansimclick";     //		현대앱카드 : intent:hdcardappcardansimclick://appcard?acctid=201606281503270019917080296121#Intent;package=com.hyundaicard.appcard;end;
            public static String SAMSUNG_APPCARD = "mpocket.online.ansimclick";   //	삼성앱카드 : intent://xid=4752902#Intent;scheme=mpocket.online.ansimclick;package=kr.co.samsungcard.mpocket;end;
            public static String NH_APPCARD = "nhappcardansimclick";              //	NH 앱카드 : intent://appcard?ACCTID=201606281507175365309074630161&P1=1532151#Intent;scheme=nhappcardansimclick;package=nh.smart.mobilecard;end;
            public static String KB_APPCARD = "kb-acp";                           //	KB 앱카드 : intent://pay?srCode=0613325&kb-acp://#Intent;scheme=kb-acp;package=com.kbcard.cxh.appcard;end;
            public static String MOBIPAY = "cloudpay";                            //	하나(모비페이) : intent://?tid=2238606309025172#Intent;scheme=cloudpay;package=com.hanaskcard.paycla;end;

            public static String PACKAGE_ISP = "kvp.jjy.MispAndroid320";
            public static String PACKAGE_BANKPAY = "com.kftc.bankpay.android";
        }

    }
    #endregion
}