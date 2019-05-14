using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;

namespace LakewoodScoop.Data
{
    public class LakewoodScoopPost
    {
        public string Title { get; set; }
        public string Image { get; set; }
        public string Url { get; set; }
        public DateTime Date { get; set; }
    }

    public class LkwdScoop
    {
        public IEnumerable<LakewoodScoopPost> ScrapeLS()
        {
            var html = GetLShtml();
            return GetPosts(html);
        }           

        public static string GetLShtml()
        {
            var handler = new HttpClientHandler();
            handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            using (var client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Add("user-agent", "hi");
                var url = "https://www.thelakewoodscoop.com/";
                var html = client.GetStringAsync(url).Result;
                return html;
            }
        }

        static IEnumerable<LakewoodScoopPost> GetPosts(string html)
        {
            var parser = new HtmlParser();
            IHtmlDocument document = parser.ParseDocument(html);
            var postDivs = document.QuerySelectorAll(".post");
            List<LakewoodScoopPost> posts = new List<LakewoodScoopPost>();
            foreach (var div in postDivs)
            {
                LakewoodScoopPost post = new LakewoodScoopPost();
                var href = div.QuerySelectorAll("a").First();
                if (href != null)
                {
                    post.Title = href.TextContent.Trim();
                    post.Url = href.Attributes["href"].Value;
                }

                var image = div.QuerySelector("img.aligncenter");
                if (image != null)
                {
                    post.Image = image.Attributes["src"].Value;
                }

                var date = div.QuerySelector("div.postmetadata-top");
                if (date != null && date.TextContent.Trim() != "")
                {
                    post.Date = DateTime.Parse(date.TextContent.Trim());
                }
                                   
                posts.Add(post);
            }

            return posts;
        }
    }
}
