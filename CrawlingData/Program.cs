using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace CrawlingData
{
    class Program
    {
        static void Main(string[] args)
        {
#pragma warning disable 4014
            StartCrawlerAsync();
#pragma warning restore 4014
            Console.ReadLine();

        }

        private static async Task StartCrawlerAsync()
        {
            var url = "https://www.automobile.tn/fr/neuf/bmw"; // the url of the page you wanna crawl data
            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            // 
            var divs = htmlDocument.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "").Equals("versions-item"));
            var cars = divs.Select(div => new Car()
            {
                Model = div.Descendants("h2").FirstOrDefault()?.InnerHtml,
                Price = div.Descendants("div").FirstOrDefault()?.ChildNodes.FirstOrDefault()?.Descendants("span").FirstOrDefault()?.InnerText,
                Link = div.Descendants("a").FirstOrDefault()?.ChildAttributes("href").FirstOrDefault()?.Value,
                ImageUrl = div.Descendants("img").FirstOrDefault()?.ChildAttributes("src").FirstOrDefault()?.Value,
            })
                .ToList();
        }
    }
}
