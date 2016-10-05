﻿using System;
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

        private bool isTitle = false;
        private bool isDescription = false;
        private bool isLink = false;
        private bool isPubDate = false;
        private bool isCategory = false;
        private bool isTopTitle = true;
        private bool isImageTitle = true;
        private bool isItem = false;
        private bool isChannel = false;
        private FeedItem feedItem;
        private RSSFeed rssFeed;

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
                                
                                switch (reader.Name)
                                {
                                    case "title":
                                        isTitle = true;
                                        break;
                                    case "link":
                                        isLink = true;
                                        break;
                                    case "category":
                                        isCategory = true;
                                        break;
                                    case "description":
                                        isDescription = true;
                                        break;
                                    case "pubDate":
                                        isPubDate = true;
                                        break;
                                    case "item":
                                        isItem = true;
                                        isChannel = false;
                                        feedItem=new FeedItem();
                                        break;
                                    case "channel":
                                        isChannel = true;
                                        rssFeed = new RSSFeed();
                                        break;
                                }
                                break;
                            case XmlNodeType.CDATA: // because some attributes are CDATA
                            case XmlNodeType.Text:
                                // Типа имитируем работу SAX парсера в андроиде
                                // now parse text
                                if (isChannel)
                                {
                                   // set feed properties
                                    if (isTitle)
                                    {
                                        isTitle = false;
                                        rssFeed.Title = reader.Value;
                                    }
                                    else if (isDescription)
                                    {
                                        isDescription = false;
                                        rssFeed.Description = reader.Value;
                                    }
                                }
                                else if (isItem)
                                {
                                    if (isTitle)
                                    {
                                        isTitle = false;
                                        feedItem.Title = reader.Value;
                                    }
                                    else if (isCategory)
                                    {
                                        isCategory = false;
                                        feedItem.Category = reader.Value;
                                    }
                                    else if (isDescription)
                                    {
                                        isDescription = false;
                                        feedItem.Description = reader.Value;
                                    }
                                    else if (isLink)
                                    {
                                        isLink = false;
                                        feedItem.Link = reader.Value;
                                    }
                                    else if (isPubDate)
                                    {
                                        // TODO в первый раз не сэтит
                                        isPubDate = false;
                                        feedItem.PubDate = reader.Value;
                                    }
                                }
                                break;
                            case XmlNodeType.EndElement:
                                if (reader.Name == "item")
                                {
                                    // finish item and add to list
                                    //textBlock.Text += feedItem.ToString()+"\n\n";
                                    isItem = false;
                                    rssFeed.Items.Add(feedItem);
                                }else if (reader.Name == "channel")
                                {
                                    //isChannel = false;
                                    // finished
                                }
                                break;
                            case XmlNodeType.XmlDeclaration:
                            case XmlNodeType.ProcessingInstruction:
                                writer.WriteProcessingInstruction(reader.Name, reader.Value);
                                break;
                            case XmlNodeType.Comment:
                               break;
                        }

                        
                    }
                    String data="";
                    foreach (var rssFeedItem in rssFeed.Items)
                    {
                        // TODO сделать всем метод ToHtmlString() для WebView
                        //textBlock.Text += rssFeedItem.ToString() + "\n\n";
                        data += rssFeedItem.ToString()+"<hr/>";
                    }
                    MyWebView.NavigateToString(data);
                }
            }
            //textBlock.Text = feedItem.ToString();
            
        }

       
    }
}
