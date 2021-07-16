using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace TrueLayer.Service.Tests
{
    [TestFixture]
    public class PokemonServiceShould
    {
        [Test]
        public void ReturnBasicPokemonInformationForAValidName()
        {
            var pokemonSpecies = new PokemonSpecies
            {
                name = "mewtwo",
                habitat = new Habitat { name = "home" },
                is_legendary = true,
                flavor_text_entries = new List<FlavorText>
                    {new FlavorText {flavor_text = "flavor1", language = "en"}}
            };

            var httpService = new Mock<IHttpService>();
            httpService.Setup(x => x.Get<PokemonSpecies>("mewtwo"))
                .Returns(pokemonSpecies);

            var expectedResult = new Pokemon
            {
                Name = pokemonSpecies.name,
                Description = pokemonSpecies.flavor_text_entries.First().flavor_text,
                Habitat = pokemonSpecies.habitat.name,
                IsLegendary = pokemonSpecies.is_legendary
            };
            var sut = new PokemonService(httpService.Object);

            sut.GetPokemonBasicDetails("mewtwo").Should().BeEquivalentTo(expectedResult);
        }
    }

    public class Pokemon
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Habitat { get; set; }
        public bool IsLegendary { get; set; }
    }

    public class PokemonService : IPokemonService
    {
        private readonly IHttpService _httpService;

        public PokemonService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public Pokemon GetPokemonBasicDetails(string mewtwo)
        {
            var pokemonSpeciesDetails = _httpService.Get<PokemonSpecies>(mewtwo);
            return new Pokemon
            {
                Name = pokemonSpeciesDetails.name,
                Description = pokemonSpeciesDetails.flavor_text_entries.FirstOrDefault(x => x.language == "en").flavor_text,
                Habitat = pokemonSpeciesDetails.habitat.name,
                IsLegendary = pokemonSpeciesDetails.is_legendary
            };
        }
    }

    public class PokemonSpecies
    {
        public string name;
        public List<FlavorText> flavor_text_entries;
        public Habitat habitat;
        public bool is_legendary;
    }

    public class Habitat
    {
        public string name;
    }

    public class FlavorText
    {
        public string language;
        public string flavor_text;
    }

    public interface IHttpService
    {
        T Get<T>(string mewtwo);
    }

    public interface IPokemonService
    {
        Pokemon GetPokemonBasicDetails(string mewtwo);
    }
}
