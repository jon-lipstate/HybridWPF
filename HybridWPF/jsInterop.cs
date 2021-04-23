using Microsoft.Web.WebView2.Wpf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HybridWPF
{
    public class jsInterop
    {
        private WebView2 WebView { get; }
        public string Method { get; set; }
        public object[] Parameters = { };

        //CTOR
        public jsInterop(WebView2 webView)
        {
            WebView = webView;
        }

        public async Task InitializeInteropAsync()
        {
            var cmd = "initializeInterop()";
            await WebView.CoreWebView2.ExecuteScriptAsync(cmd);
        }

        public async Task CallMethodWithJson(string method, object parameter = null)
        {
            string cmd = "textEditor." + method;

            if (parameter != null)
            {
                var jsonParm = JsonConvert.SerializeObject(parameter, Formatting.None);
                cmd += "(" + jsonParm + ")";
            }

            await WebView.CoreWebView2.ExecuteScriptAsync(cmd);
        }
        
    }
}
