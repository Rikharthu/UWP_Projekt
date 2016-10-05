using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace HelloWorld
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

            int x1, x2, y1, y2;
            x1 = Convert.ToInt32(X1TextBox.Text);
            y1 = Convert.ToInt32(Y1TextBox.Text);
            x2 = Convert.ToInt32(X2TextBox.Text);
            y2 = Convert.ToInt32(Y2TextBox.Text);

            var line = new Line();
            line.Stroke = new SolidColorBrush(Colors.Black);
            line.StrokeThickness = 3;
            //line.Width = 50;
            line.X1 = x1;
            line.Y1 = y1;
            line.X2 = x2;
            line.Y2 = y2;

            Canvas.SetTop(line, 50);
            Canvas.SetLeft(line, 50);

            TheCanvas.Children.Add(line);
        }

        private async void button1_Click(object sender, RoutedEventArgs e)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetStreamAsync("http://rus.delfi.lv/rss.php");
            XDocument xmlDoc = XDocument.Load(response);

            StringBuilder output = new StringBuilder();

            String xmlString =xmlDoc.Document.ToString();
            progressBar.Value = 50;
            
            // Create an XmlReader
            using (XmlReader reader = XmlReader.Create(new StringReader(xmlString)))
            {
                XmlWriterSettings ws = new XmlWriterSettings();
                ws.Indent = true;
                using (XmlWriter writer = XmlWriter.Create(output, ws))
                {

                    // Parse the file and display each of the nodes.
                    while (reader.Read())
                    {
                        switch (reader.NodeType)
                        {
                            case XmlNodeType.Element:
                                if (reader.Name == "item")
                                {
                                    
                                }
                                textBlock.Text = reader.Name;
                                writer.WriteStartElement(reader.Name);
                                break;
                            case XmlNodeType.Text:
                                //textBlock.Text = reader.Value;
                                writer.WriteString(reader.Value);
                                break;
                            case XmlNodeType.XmlDeclaration:
                            case XmlNodeType.ProcessingInstruction:
                                writer.WriteProcessingInstruction(reader.Name, reader.Value);
                                break;
                            case XmlNodeType.Comment:
                                writer.WriteComment(reader.Value);
                                break;
                            case XmlNodeType.EndElement:
                                if (reader.Name == "item")
                                {
                                    // finish item and add to list
                                }
                                writer.WriteFullEndElement();
                                break;
                        }
                    }

                }
            }
            //textBlock.Text = output.ToString();

            FeedItem item = new FeedItem();
            item.Title = "sadas";
        }

       
    }
}
