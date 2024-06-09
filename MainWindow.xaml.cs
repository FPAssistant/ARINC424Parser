using FpAssistantCore.Arinc424;
using FpAssistantCore.General;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FPAssistantArinc424Parser
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            Title = "FPAssistant ARINC 424 Parser Example";
            this.AppWindow.SetIcon("Assets/fpaIcon.ico");

            // Assign the license for the FPAssistant SDK basic free version
            DeveloperLicense.License = "\u009dD\u008b\u0085\u0083\u0091\u0092\u0083\u0090\u0095\u0091\u0092\u0095D\\\u0088\u0083\u008e\u0095\u0087ND\u0088\u0083\u0083\u0096\u0087\u0094\u0092\u0095D\\\u0088\u0083\u008e\u0095\u0087ND\u0091\u0084\u0095\u0096\u0083\u0085\u008e\u0087\u0087\u0098\u0083\u008e\u0097\u0083\u0096\u008b\u0091\u0090D\\\u0088\u0083\u008e\u0095\u0087ND\u0083\u0094\u008b\u0090\u0085VTVD\\\u0088\u0083\u008e\u0095\u0087ND\u0088\u0092\u0083\u0095\u0095\u008b\u0095\u0096\u0083\u0090\u0096\u0095\u0086\u008d\u0084\u0083\u0095\u008b\u0085D\\\u0096\u0094\u0097\u0087ND\u008b\u0085\u0083\u0091\u0083\u0092\u008b\u0086\u0083\u0096\u0083\u0095\u0087\u0094\u0098\u008b\u0085\u0087D\\\u0088\u0083\u008e\u0095\u0087ND\u0086\u0087\u0098\u0087\u008e\u0091\u0092\u0087\u0094\u0090\u0083\u008f\u0087D\\Dhrc\u0095\u0095\u008b\u0095\u0096\u0083\u0090\u0096BufmBd\u0083\u0095\u008b\u0085Bn\u008b\u0085\u0087\u0090\u0095\u0087BOBecf\u0091\u008e\u0091\u0089\u009bNBwmDND\u0086\u0087\u0098\u0087\u008e\u0091\u0092\u0087\u0094\u008d\u0087\u009bD\\DRRRRRRRRORRRRORRRRORRRRORRRRRRRRRRRRD\u009f";
            Arinc424Io arinc424Io = new();
            arinc424Io.Open(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Arinc424Data\FAACIFP18.A424"));
        }

        public readonly Arinc424Io arinc424Io = null; // Create instance of ARINC 424 parser


        private void MyButton_Click(object sender, RoutedEventArgs e)
        {
            myButton.Content = "Clicked";
        }
    }
}
