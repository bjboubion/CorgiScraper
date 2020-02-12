using System.Collections.Generic;
using CorgiScraper.API.Models;

namespace CorgiScraper.API.Services.Interfaces
{
    public interface ICraigslistServices
    {
        List<string> GetMainPageLinks(string url);
        List<PageDetails> GetPageDetails(List<string> urls, string searchTerm); 
    }
}