using System.Collections.Generic;
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
                Title = "Title 1",
                Description = "Description 1",
                Url = "Url 1"
            };

            var pageDetailTwo = new PageDetails 
            {
                Title = "Title 2",
                Description = "Description 2",
                Url = "Url 2"
            };
            _mockCraigslistServices.Setup(x => x.GetPageDetails(It.IsAny<List<string>>(), It.IsAny<string>())).Returns(new List<PageDetails> { pageDetailOne, pageDetailTwo });
            _mockCraigslistServices.Setup(x => x.GetMainPageLinks(It.IsAny<string>())).Returns(new List<string> {"this", "that", "deez", "dooees"});            

            _craiglistServices = _mockCraigslistServices.Object;
        }

        [Test]
        public void CraigslistService_GetPageDetails_Success()
        {
            var response = _craiglistServices.GetPageDetails(new List<string>() { "this", "that" }, "this");
            Assert.IsNotNull(response);
            Assert.AreEqual(response.Count, 2);
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