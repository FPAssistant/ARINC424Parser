using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Cryptography.Core;

using FpAssistantCore.Arinc424;
using FpAssistantCore.Arinc424.Records;
using FpAssistantCore.General;
using Windows.UI.StartScreen;
using Microsoft.UI.Xaml.Documents;
using Windows.UI.Text;


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
            
            arinc424Io = new();
            arinc424Io.Open(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Arinc424Data\FAACIFP18.A424"));

            PopulateTreeView(ref arinc424Io);
        }

        public static Arinc424Io arinc424Io = null; // Create instance of ARINC 424 parser

        void PopulateTreeView(ref Arinc424Io arinc424Io)
        {
            Arinc424TreeView.RootNodes.Clear();
            IList<TreeViewNode> rootNodes = Arinc424TreeView.RootNodes;

            #region Header
            TreeViewNode treeViewNodeHeader = new()
            {
                Content = "Header (1)"
            };
            NodeData nodeData = new("Header01", arinc424Io.Arinc424Data.Header01.Id);
            TreeViewNode treeViewNode = new()
            {
                Content = nodeData
            };
            treeViewNodeHeader.Children.Add(treeViewNode);

            #endregion

            #region Airports
            TreeViewNode treeViewNodeAirports = new()
            {
                Content = "Airports (" + arinc424Io.Arinc424Data.Airports.Count.ToString() + ")"
            };

            foreach (Airport airport in arinc424Io.Arinc424Data.Airports)
            {
                nodeData = new(airport.AirportName + " " + airport.AirportICAOIdentifier, airport.Id);
                treeViewNode = new()
                {
                    Content = nodeData 
                };
                treeViewNodeAirports.Children.Add(treeViewNode);
            }
            #endregion

            rootNodes.Add(treeViewNodeHeader);
            rootNodes.Add(treeViewNodeAirports);
        }

        private void Arinc424TreeView_ItemInvoked(TreeView sender, TreeViewItemInvokedEventArgs args)
        {
            TreeViewNode treeViewNode = args.InvokedItem as TreeViewNode;
            if (treeViewNode != null)
            {
                NodeData nodeData = treeViewNode.Content as NodeData;
                if ((nodeData != null))
                {
                    BaseRecord baseRecord = new BaseRecord("");

                    if (arinc424Io.Arinc424Data.FindByGuid(nodeData.Guid, ref baseRecord) == true)
                    {
                        Dictionary<string, string> propertyNamesAndValues = baseRecord.PropertyNameAndValues();
                        Arinc424DataGrid.Children.Clear();

                        TextBlock col1Header = new()
                        {
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Center,
                            Text = "ARINC 424 Field Name",
                            
                        };
                        Grid.SetColumn(col1Header, 0);
                        Grid.SetRow(col1Header, 0);

                        TextBlock col2Header = new()
                        {
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Center,
                            Text = "ARINC 424 Value"
                        };
                        Grid.SetColumn(col2Header, 1);
                        Grid.SetRow(col2Header, 0);

                        Arinc424DataGrid.Children.Add(col1Header);
                        Arinc424DataGrid.Children.Add(col2Header);

                        int i = 0;
                        foreach (KeyValuePair<string, string> propertyNameValue in propertyNamesAndValues ) 
                        {
                            TextBlock col1 = new()
                            {
                                HorizontalAlignment = HorizontalAlignment.Left,
                                VerticalAlignment = VerticalAlignment.Center,
                                Text = propertyNameValue.Key
                            };
                            Grid.SetColumn(col1, 0);
                            Grid.SetRow(col1, i+1);

                            TextBlock col2 = new()
                            {
                                HorizontalAlignment = HorizontalAlignment.Left,
                                VerticalAlignment = VerticalAlignment.Center,
                                Text = propertyNameValue.Value
                            };
                            Grid.SetColumn(col2, 1);
                            Grid.SetRow(col2, i+1);

                            Arinc424DataGrid.Children.Add(col1);
                            Arinc424DataGrid.Children.Add(col2);
                            i++;
                        }
                    }
                }
            }
        }

        private class NodeData(string description, Guid guid)
        {
            private readonly string _Description = description;
            private readonly Guid _Guid = guid;

            public override string ToString()
            {
                return _Description;
            }

            public string Description { get { return _Description; } }
            public Guid Guid { get { return _Guid; } }
        }
    }
}
