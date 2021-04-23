using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Web.WebView2.Core;
using Newtonsoft.Json;
//Reference: https://weblog.west-wind.com/posts/2021/Jan/14/Taking-the-new-Chromium-WebView2-Control-for-a-Spin-in-NET-Part-1
namespace HybridWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public csInterop Interop;
        public jsInterop JsInterop;
        public MainWindow()
        {
            InitializeComponent();

            JsInterop = new jsInterop(webView);
            Interop = new csInterop(this, JsInterop);

            webView.Visibility = Visibility.Hidden;
            InitializeAsync();

            webView.NavigationCompleted += WebView_NavigationCompleted;
            webView.Unloaded += (s, a) => webView.Visibility = Visibility.Hidden;
            webView.ContentLoading += WebView_ContentLoading;

            webView.Source = new Uri("https://web.ui/index.html");
        }
        async void InitializeAsync()
        {
            // must create a data folder if running out of a secured folder that can't write like Program Files
            var env = await CoreWebView2Environment.CreateAsync(userDataFolder: System.IO.Path.Combine(System.IO.Path.GetTempPath(), "web_ui"));
            await webView.EnsureCoreWebView2Async(env);

            // Map a folder from the Executable Folder to a virtual domain
            webView.CoreWebView2.SetVirtualHostNameToFolderMapping(
                    "web.ui", "WebUI",
                    CoreWebView2HostResourceAccessKind.Allow);

            webView.CoreWebView2.OpenDevToolsWindow();

            webView.CoreWebView2.WebMessageReceived += CoreWebView2_WebMessageReceived;

            webView.CoreWebView2.AddHostObjectToScript("mm", Interop);

            await JsInterop.InitializeInteropAsync();
        }
        private void CoreWebView2_WebMessageReceived(object sender, Microsoft.Web.WebView2.Core.CoreWebView2WebMessageReceivedEventArgs e)
        {
            var text = e.TryGetWebMessageAsString();

            var callbackJson = JsonConvert.DeserializeObject(text, typeof(JsonCallbackObject)) as JsonCallbackObject;

            if (callbackJson.Method == "showMessage")
            {
                MessageBox.Show(callbackJson.Parameters[0] + "\r\n" + text, "WPF Window");
            }


        }
        private void WebView_ContentLoading(object sender, CoreWebView2ContentLoadingEventArgs e)
        {
        }
        private void WebView_NavigationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs e)
        {
            
            webView.Visibility = Visibility.Visible;

            if (e.IsSuccess)
                webView.Source.ToString();

            
            
        }

        private async void sendButton_Click(object sender, RoutedEventArgs e)
        {
            //await webView.ExecuteScriptAsync($"window.alert('Hello from .NET. Time is: {DateTime.Now.ToString("HH:mm:ss")}.')");

            //var cmd = "testReturn()";
            //var res = await webView.CoreWebView2.ExecuteScriptAsync(cmd);
            //MessageBox.Show(res);

            var res = await webView.CoreWebView2.ExecuteScriptAsync("testReturn()");
            MessageBox.Show(res);
        }
    }
}
