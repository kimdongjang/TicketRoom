//------------------------------------------------------------------------------
// <auto-generated>
//     이 코드는 도구를 사용하여 생성되었습니다.
//     런타임 버전:4.0.30319.42000
//
//     파일 내용을 변경하면 잘못된 동작이 발생할 수 있으며, 코드를 다시 생성하면
//     이러한 변경 내용이 손실됩니다.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: global::Xamarin.Forms.Xaml.XamlResourceIdAttribute("TicketRoom.Views.MainTab.MyPage.PurchaseList.DeliveryLookup.xaml", "Views/MainTab/MyPage/PurchaseList/DeliveryLookup.xaml", typeof(global::TicketRoom.Views.MainTab.MyPage.PurchaseList.DeliveryLookup))]

namespace TicketRoom.Views.MainTab.MyPage.PurchaseList {
    
    
    [global::Xamarin.Forms.Xaml.XamlFilePathAttribute("Views\\MainTab\\MyPage\\PurchaseList\\DeliveryLookup.xaml")]
    public partial class DeliveryLookup : global::Xamarin.Forms.ContentPage {
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private global::Xamarin.Forms.Grid TabGrid;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private global::TicketRoom.Models.Custom.CustomLabel TitleName;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private global::Xamarin.Forms.WebView DeliveryWeb;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private void InitializeComponent() {
            global::Xamarin.Forms.Xaml.Extensions.LoadFromXaml(this, typeof(DeliveryLookup));
            TabGrid = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::Xamarin.Forms.Grid>(this, "TabGrid");
            TitleName = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::TicketRoom.Models.Custom.CustomLabel>(this, "TitleName");
            DeliveryWeb = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::Xamarin.Forms.WebView>(this, "DeliveryWeb");
        }
    }
}
