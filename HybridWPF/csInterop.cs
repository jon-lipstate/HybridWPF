using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HybridWPF
{
    /// <summary>
    /// Object that gets called from JavaScript
    /// </summary>
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ComVisible(true)]
    public class csInterop
    {
        public jsInterop JsInterop { get; }
        public MainWindow Window { get; set; }

        public csInterop(MainWindow mainWindow, jsInterop jsInterop)
        {
            JsInterop = jsInterop;
            Window = mainWindow;
        }

        public string HelloWorldSync(string name)
        {
            var res = $"Hello Sync World, {name}! - Message: 'This text was retrieved from .NET and shown here in JavaScript.'\n" +
                      "Button click in .NET -> JavaScript Function calls Hello World in .NET -> Returns message to JavaScript";
            MessageBox.Show("definitely in dotnet");
            return res;
        }
    }

    [DebuggerDisplay("JsonCallback: {Method}")]
    public class JsonCallbackObject
    {
        public string Method { get; set; }
        public List<object> Parameters { get; } = new List<object>();
    }
}
