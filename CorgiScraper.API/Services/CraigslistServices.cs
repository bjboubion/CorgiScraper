using System;
using System.Collections.Generic;
using System.Linq;
using CorgiScraper.API.Models;
using CorgiScraper.API.Services.Interfaces;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using ScrapySharp.Extensions;
using ScrapySharp.Network;

namespace CorgiScraper.API.Services
{
    public class CraigslistServices : ICraigslistServices
    {
        private readonly ILogger<CraigslistServices> _logger;
        private readonly ScrapingBrowser _scrapingBrowser;

        public CraigslistServices(ILogger<CraigslistServices> logger)
        {
            _logger = logger;
            _scrapingBrowser = new ScrapingBrowser();
        }

        public List<string> GetMainPageLinks(string url) 
        {
            var homePageLinks = new List<string> ();
            var html = GetHtml(url);
            var links = html.CssSelect("a");

            foreach(var link in links) 
            {
                if (link.Attributes["href"].Value.Contains(".html")) 
                {
                    homePageLinks.Add(link.Attributes["href"].Value);
                }
            }

            return homePageLinks;
        }

        public List<PageDetails> GetPageDetails(List<string> urls, string searchTerm) 
        {
            var lstPageDetails = new List<PageDetails>();
            foreach(var url in urls) 
            {
                var htmlNode = GetHtml(url);
                var pageDetails = new PageDetails();

                pageDetails.Title = htmlNode.OwnerDocument.DocumentNode.SelectSingleNode("//html/head/title").InnerText;
                var description = htmlNode.OwnerDocument.DocumentNode.SelectSingleNode("//html/body/section/section/section/section").InnerText;
                pageDetails.Description = description.Replace("\n        \n            QR Code Link to This Post\n            \n        \n", "");
                pageDetails.Url = url;

                var imageOfInterest = htmlNode.OwnerDocument.DocumentNode.SelectSingleNode("//html/body/section/section/section/figure/div/div/div/div/img");
                var imageUrl = "";
                
                if (imageOfInterest != null)
                    imageUrl = imageOfInterest.Attributes["src"].Value;

                if (!string.IsNullOrEmpty(imageUrl))
                    pageDetails.ImageUrl = imageUrl;

                // var searchTermInTitle = pageDetails.Title.ToLower().Contains(searchTerm.ToLower());
                // var searchTermInDescription = pageDetails.Description.ToLower().Contains(searchTerm.ToLower());

                // if (searchTermInTitle || searchTermInDescription) {
                    lstPageDetails.Add(pageDetails);
                // }
            }

            var filteredList = FilterOutRepeatedPageDetails(lstPageDetails);
            return filteredList;
        }

        private List<PageDetails> FilterOutRepeatedPageDetails(List<PageDetails> originalList)
        {
            var distinctList = originalList.GroupBy(x => x.Url).Select(x => x.First()).ToList();

            return distinctList;
        }

        private HtmlNode GetHtml(string url) 
        {
            WebPage webpage = _scrapingBrowser.NavigateToPage(new Uri(url));
            return webpage.Html;
        }
    }
}