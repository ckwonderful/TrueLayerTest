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
                is_legendary = true,
                flavor_text_entries = new List<FlavorText>
                    {new FlavorText {flavor_text = "flavor1", language = "en"}}
            };

            var httpService = new Mock<IHttpService>();
            httpService.Setup(x => x.Get<PokemonSpecies>("mewtwo"))
                .ReturnsAsync(pokemonSpecies);

            var expectedResult = new Pokemon
            {
                Name = pokemonSpecies.name,
                Description = pokemonSpecies.flavor_text_entries.First().flavor_text,
                Habitat = pokemonSpecies.habitat.name,
                IsLegendary = pokemonSpecies.is_legendary
            };
            var sut = new PokemonService(httpService.Object);

           (await sut.GetPokemonBasicDetails("mewtwo")).Should().BeEquivalentTo(expectedResult);
        }
    }

    
    public class PokemonService : IPokemonService
    {
        private readonly IHttpService _httpService;

        public PokemonService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<Pokemon> GetPokemonBasicDetails(string mewtwo)
        {
            var pokemonSpeciesDetails = await _httpService.Get<PokemonSpecies>(mewtwo);
            return new Pokemon
            {
                Name = pokemonSpeciesDetails.name,
                Description = pokemonSpeciesDetails.flavor_text_entries.FirstOrDefault(x => x.language == "en")?.flavor_text,
                Habitat = pokemonSpeciesDetails.habitat.name,
                IsLegendary = pokemonSpeciesDetails.is_legendary
            };
        }
    }
    

    public interface IPokemonService
    {
        Task<Pokemon> GetPokemonBasicDetails(string mewtwo);
    }
}
