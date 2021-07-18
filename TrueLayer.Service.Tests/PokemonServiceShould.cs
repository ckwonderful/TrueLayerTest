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
        public async Task ReturnBasicPokemonInformationForAValidName()
        {
            var pokemonSpecies = new PokemonSpecies
            {
                name = "mewtwo",
                habitat = new Habitat { name = "home" },
                is_legendary = false,
                flavor_text_entries = new List<FlavorText>
                    {new FlavorText {flavor_text = "flavor1", language = new Language {name = "en" }}}
            };

            var httpService = new Mock<IHttpService<PokemonSpecies>>();
            httpService.Setup(x => x.Get("https://pokeapi.co/api/v2/pokemon-species/mewtwo"))
                .ReturnsAsync(pokemonSpecies);
            var translateServiceFactory = new TranslationServiceFactory(new List<ITranslationService>());

            var expectedResult = new Pokemon
            {
                Name = pokemonSpecies.name,
                Description = pokemonSpecies.flavor_text_entries.First().flavor_text,
                Habitat = pokemonSpecies.habitat.name,
                IsLegendary = pokemonSpecies.is_legendary
            };
            var sut = new PokemonService(httpService.Object, translateServiceFactory);

           (await sut.GetPokemonBasicDetails("mewtwo")).Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public async Task ReturnAShakespeareTranslationForIsLegendaryPokemon()
        {
            var pokemonSpecies = new PokemonSpecies
            {
                name = "mewtwo",
                habitat = new Habitat { name = "home" },
                is_legendary = true,
                flavor_text_entries = new List<FlavorText>
                    {new FlavorText {flavor_text = "flavor1", language = new Language {name = "en" }}}
            };
            var translated = "flavor1-shakespeare";


            var httpService = new Mock<IHttpService<PokemonSpecies>>();
            httpService.Setup(x => x.Get("https://pokeapi.co/api/v2/pokemon-species/mewtwo"))
                .ReturnsAsync(pokemonSpecies);

            var shakespeareHttpService = new Mock<IHttpService<TranslateResponse>>();
            shakespeareHttpService.Setup(x => x.Post(
                    "https://api.funtranslations.com/translate/shakespeare",
                    It.Is<TranslateRequest>(y => y.text == pokemonSpecies.flavor_text_entries.First().flavor_text)))
                .ReturnsAsync(new TranslateResponse { text = translated});

            var translateServiceFactory = new Mock<ITranslationServiceFactory>();
            translateServiceFactory.Setup(x => x.Create("shakespeare"))
                .Returns(new ShakespeareTranslationService(shakespeareHttpService.Object));

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
    }

    
   
}
