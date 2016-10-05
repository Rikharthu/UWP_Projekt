using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{
    /** Model object */
    class FeedItem
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
        // TODO parse to normal date and get return according to locale
        public string PubDate { get; set; }
        public string Category { get; set; }

        public virtual string ToString()
        {
            string str = "Title: " + Title + "\nDescription: " + Description
                         + "\nLink: " + Link + "\nPubDate: " + PubDate
                         +"\nCategory: "+Category;
            return str;
        }
    }
}
