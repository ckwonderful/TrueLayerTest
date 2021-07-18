using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using TrueLayer.Model;

namespace TrueLayer.Service.Tests
{
    [TestFixture]
    public class PokemonServiceShould
    {
        [Test]
        public async Task ReturnAYodaTranslationForIsLegendaryPokemon()
        {
            var pokemonSpecies = PokemonSpecies(true, "anywhere");
            var translated = "flavor1-Yoda";

            var httpService = AssumePokemonHttpServiceReturns(pokemonSpecies);

            var yodaHttpService = AssumeTranslationServiceReturns("https://api.funtranslations.com/translate/Yoda.json", pokemonSpecies, translated);

            var translateServiceFactory = AssumeYodaTranslationServiceReturns(yodaHttpService);

            var expectedResult = new Pokemon
            {
                Name = pokemonSpecies.name,
                Description = translated,
                Habitat = pokemonSpecies.habitat.name,
                IsLegendary = pokemonSpecies.is_legendary
            };
            var sut = new PokemonService(httpService.Object, translateServiceFactory.Object);

            (await sut.GetPokemonBasicDetails("mewtwo")).Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public async Task ReturnAYodaTranslationForPokemonWithAHabitOfCave()
        {
            var pokemonSpecies = PokemonSpecies(false, "cave");
            var translated = "flavor1-Yoda";

            var httpService = AssumePokemonHttpServiceReturns(pokemonSpecies);

            var yodaHttpService = AssumeTranslationServiceReturns(
                "https://api.funtranslations.com/translate/Yoda.json", pokemonSpecies, translated);

            var translateServiceFactory = AssumeYodaTranslationServiceReturns(yodaHttpService);

            var expectedResult = new Pokemon
            {
                Name = pokemonSpecies.name,
                Description = translated,
                Habitat = pokemonSpecies.habitat.name,
                IsLegendary = pokemonSpecies.is_legendary
            };
            var sut = new PokemonService(httpService.Object, translateServiceFactory.Object);

            (await sut.GetPokemonBasicDetails("mewtwo")).Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public async Task ReturnAShakespeareTranslationForPokemonThatIsNotLegendaryAndDoesNotHaveAHabitOfCave()
        {
            var pokemonSpecies = PokemonSpecies(false, "notacave");
            var translated = "flavor1-Shakespeare";

            var httpService = AssumePokemonHttpServiceReturns(pokemonSpecies);

            var yodaHttpService = AssumeTranslationServiceReturns(
                "https://api.funtranslations.com/translate/shakespeare", pokemonSpecies, translated);

            var translateServiceFactory = AssumeShakespeareTranslationServiceReturns(yodaHttpService);

            var expectedResult = new Pokemon
            {
                Name = pokemonSpecies.name,
                Description = translated,
                Habitat = pokemonSpecies.habitat.name,
                IsLegendary = pokemonSpecies.is_legendary
            };
            var sut = new PokemonService(httpService.Object, translateServiceFactory.Object);

            (await sut.GetPokemonBasicDetails("mewtwo")).Should().BeEquivalentTo(expectedResult);
        }

        private static Mock<ITranslationServiceFactory> AssumeYodaTranslationServiceReturns(Mock<IHttpService<TranslateResponse>> httpService)
        {
            var translateServiceFactory = new Mock<ITranslationServiceFactory>();
            translateServiceFactory.Setup(x => x.Create("yoda"))
                .Returns(new YodaTranslationService(httpService.Object));
            return translateServiceFactory;
        }

        private static Mock<ITranslationServiceFactory> AssumeShakespeareTranslationServiceReturns(Mock<IHttpService<TranslateResponse>> httpService)
        {
            var translateServiceFactory = new Mock<ITranslationServiceFactory>();
            translateServiceFactory.Setup(x => x.Create("shakespeare"))
                .Returns(new ShakespeareTranslationService(httpService.Object));
            return translateServiceFactory;
        }

        private static Mock<IHttpService<TranslateResponse>> AssumeTranslationServiceReturns(
            string url, PokemonSpecies pokemonSpecies, string translated)
        {
            var translationHttpService
                = new Mock<IHttpService<TranslateResponse>>();
            translationHttpService.Setup(x => x.Post(
                    url,
                    It.Is<TranslateRequest>(y => y.text == pokemonSpecies.flavor_text_entries.First().flavor_text)))
                .ReturnsAsync(new TranslateResponse {contents = new Contents {translated = translated}});
            return translationHttpService;
        }

        private static Mock<IHttpService<PokemonSpecies>> AssumePokemonHttpServiceReturns(PokemonSpecies pokemonSpecies)
        {
            var httpService = new Mock<IHttpService<PokemonSpecies>>();
            httpService.Setup(x => x.Get("https://pokeapi.co/api/v2/pokemon-species/mewtwo"))
                .ReturnsAsync(pokemonSpecies);
            return httpService;
        }

        private static PokemonSpecies PokemonSpecies(bool isLegendary, string habit)
        {
            var pokemonSpecies = new PokemonSpecies
            {
                name = "mewtwo",
                habitat = new Habitat {name = habit},
                is_legendary = isLegendary,
                flavor_text_entries = new List<FlavorText>
                    {new FlavorText {flavor_text = "flavor1", language = new Language {name = "en"}}}
            };
            return pokemonSpecies;
        }
    }

    
   
}
