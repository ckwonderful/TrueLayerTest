using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace TrueLayer.Service.Tests
{
    [TestFixture]
    public class TranslationServiceShould
    {
        [Test]
        public async Task TranslateTheGivenText()
        {
            var sut = new TranslationService();
            (await sut.Translate("translate-me")).Should().Be("translate-me-translated");
        }
    }

    public class TranslationService: ITranslationService
    {
        public async Task<string> Translate(string translateMe)
        {
            throw new NotImplementedException();
        }
    }

    public interface ITranslationService
    {
        Task<string> Translate(string translateMe);
    }
}
