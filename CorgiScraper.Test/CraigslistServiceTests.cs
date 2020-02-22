using System.Collections.Generic;
using System.Linq;
using CorgiScraper.API.Models;
using CorgiScraper.API.Services.Interfaces;
using Moq;
using NUnit.Framework;

namespace CorgiScraper.Test
{
    public class CraigslistServiceTests
    {
        private readonly Mock<ICraigslistServices> _mockCraigslistServices;
        private ICraigslistServices _craiglistServices;
        
        public CraigslistServiceTests()
        {
            _mockCraigslistServices = new Mock<ICraigslistServices>();
        }

        [SetUp]
        public void Setup()
        {
            var pageDetailOne = new PageDetails 
            {
                Title = "Auggies half corgi half mini aussie - pets",
                Description = "Pembroke Welsh corgi and mini aussie mix ,health guaranteed, shots and dewormed, blue and red merles and red and black tris with blue eyes. Ready for there new homes, come meet them in person on the ranch ,located in temecula wine country. ",
                Url = "https://orangecounty.craigslist.org/pet/d/temecula-auggies-half-corgi-half-mini/7065175741.html"
            };

            var repeatedPageDetailOne = new PageDetails 
            {
                Title = "Auggies half corgi half mini aussie - pets",
                Description = "Pembroke Welsh corgi and mini aussie mix ,health guaranteed, shots and dewormed, blue and red merles and red and black tris with blue eyes. Ready for there new homes, come meet them in person on the ranch ,located in temecula wine country. ",
                Url = "https://orangecounty.craigslist.org/pet/d/temecula-auggies-half-corgi-half-mini/7065175741.html"
            };

            var pageDetailTwo = new PageDetails 
            {
                Title = "Corgi studs - pets",
                Description = "3 gorgeous corgis studs! Akc, Ckc &amp; Ukc registered Tri Fluffy Pembroke welsh corgi Triple clear Akc, Ckc, Ukc Red bluie Pembroke welsh corgi Dm Carrier, vwd Clear &amp; eic Clear Ckc registered Blue Merle 1/8 mini Aussie Pembroke welsh corgi Vwd &amp; Eic Clear Live or a.i Available Check out uniquecountrycorgis.com for past litters and more pictures of the boys :) ",
                Url = "https://inlandempire.craigslist.org/pet/d/temecula-corgi-studs/7079304820.html"
            };

            var listOfDetails = new List<PageDetails> {  };

            _mockCraigslistServices.Setup(x => x.GetMainPageLinks(It.IsAny<string>())).Returns(new List<string> {"this", "that", "deez", "dooees"});            

            _craiglistServices = _mockCraigslistServices.Object;
        }

        private List<PageDetails> FilterOutRepeatedPageDetails(List<PageDetails> originalList)
        {
            var distinctList = originalList.GroupBy(x => x.Url).Select(x => x.First()).ToList();

            return distinctList;
        }

        [Test]
        public void CraigslistService_GetPageDetails_Success()
        {
            var pageDetailOne = new PageDetails 
            {
                Title = "Auggies half corgi half mini aussie - pets",
                Description = "Pembroke Welsh corgi and mini aussie mix ,health guaranteed, shots and dewormed, blue and red merles and red and black tris with blue eyes. Ready for there new homes, come meet them in person on the ranch ,located in temecula wine country. ",
                Url = "https://orangecounty.craigslist.org/pet/d/temecula-auggies-half-corgi-half-mini/7065175741.html"
            };

            var repeatedPageDetailOne = new PageDetails 
            {
                Title = "Auggies half corgi half mini aussie - pets",
                Description = "Pembroke Welsh corgi and mini aussie mix ,health guaranteed, shots and dewormed, blue and red merles and red and black tris with blue eyes. Ready for there new homes, come meet them in person on the ranch ,located in temecula wine country. ",
                Url = "https://orangecounty.craigslist.org/pet/d/temecula-auggies-half-corgi-half-mini/7065175741.html"
            };

            var pageDetailTwo = new PageDetails 
            {
                Title = "Corgi studs - pets",
                Description = "3 gorgeous corgis studs! Akc, Ckc &amp; Ukc registered Tri Fluffy Pembroke welsh corgi Triple clear Akc, Ckc, Ukc Red bluie Pembroke welsh corgi Dm Carrier, vwd Clear &amp; eic Clear Ckc registered Blue Merle 1/8 mini Aussie Pembroke welsh corgi Vwd &amp; Eic Clear Live or a.i Available Check out uniquecountrycorgis.com for past litters and more pictures of the boys :) ",
                Url = "https://inlandempire.craigslist.org/pet/d/temecula-corgi-studs/7079304820.html"
            };

            _mockCraigslistServices.Setup(x => x.GetPageDetails(It.IsAny<List<string>>(), It.IsAny<string>())).Returns(new List<PageDetails> { pageDetailOne, repeatedPageDetailOne , pageDetailTwo });

            var response = _craiglistServices.GetPageDetails(new List<string>() { "this", "that" }, "this");
            var filteredResponse = FilterOutRepeatedPageDetails(response);
            Assert.IsNotNull(response);
            Assert.AreEqual(2, filteredResponse.Count);
        }

        [Test]
        public void CraigslistService_GetMainPageLinks_Success()
        {
            var response = _craiglistServices.GetMainPageLinks("a url");
            Assert.IsNotNull(response);
            Assert.AreEqual(response.Count, 4);
        }
    }
}