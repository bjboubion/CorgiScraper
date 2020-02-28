using System.Collections.Generic;
using CorgiScraper.API.Models;
using CorgiScraper.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CorgiScraper.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CraigslistController : ControllerBase
    {
        private readonly ILogger<CraigslistController> _logger;
        private readonly ICraigslistServices _craigslistServices;

        public CraigslistController(ILogger<CraigslistController> logger, ICraigslistServices craigslistServices)
        {
            _logger = logger;
            _craigslistServices = craigslistServices;
        }

        [HttpGet]
        public IEnumerable<PageDetails> Get()
        {
            var searchTerm = "corgi";
            var mainLinks = _craigslistServices.GetMainPageLinks($"https://orangecounty.craigslist.org/search/pet?query={searchTerm}");
            var lstPageDetails = _craigslistServices.GetPageDetails(mainLinks, searchTerm);

            return lstPageDetails;
        }

        [HttpGet("{searchTerm}")]
        public IEnumerable<PageDetails> GetBySearchTerm(string searchTerm)
        {
            // TODO: filter out duplicate details
            _logger.LogInformation($"Search term: {searchTerm}");
            var mainLinks = _craigslistServices.GetMainPageLinks($"https://orangecounty.craigslist.org/search/pet?query={searchTerm}");
            var lstPageDetails = _craigslistServices.GetPageDetails(mainLinks, searchTerm);

            return lstPageDetails;
        }
    }
}