using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using TrueLayer.Model;

namespace TrueLayer.Service.Tests
{
    [TestFixture]
    public class TranslationServiceShould
    {
        [Test]
        public async Task TranslateTheGivenText()
        {
            var translateText = "translate-me";
            var translated = "translate-me-translated";
            var httpService = new Mock<IHttpService<TranslateResponse>>();
            httpService.Setup(x => x.Post(
                    It.Is<string>(x => x == "https://api.funtranslations.com/translate/shakespeare"), 
                    It.Is<TranslateRequest>(y => y.text ==translateText)))
                .ReturnsAsync(new TranslateResponse { translated = translated});
            var sut = new ShakespeareTranslationService(httpService.Object);
            (await sut.Translate(translateText)).Should().Be(translated);
        }
    }
   

   
}
