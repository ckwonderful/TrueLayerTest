using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;

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
            var httpService = new Mock<IHttpService<string>>();
            httpService.Setup(x => x.Post(
                    It.Is<string>(x => x == "https://api.funtranslations.com/translate/shakespeare"), 
                    It.Is<TranslateRequest>(y => y.text ==translateText)))
                .ReturnsAsync(translated);
            var sut = new TranslationService(httpService.Object);
            (await sut.Translate(translateText)).Should().Be(translated);
        }
    }
    public class TranslateRequest
    {
        public string text { get; set; }
    }

    public class TranslationService: ITranslationService
    {
        private string _url = "https://api.funtranslations.com/translate/shakespeare";
        private readonly IHttpService<string> _httpService;

        public TranslationService(IHttpService<string> httpService)
        {
            _httpService = httpService;
        }

        public async Task<string> Translate(string text)
        {
            var translate = await _httpService.Post<TranslateRequest>(_url, new TranslateRequest
            {
                text = text
            });
            return translate;
        }
    }

    public interface ITranslationService
    {
        Task<string> Translate(string text);
    }
}
