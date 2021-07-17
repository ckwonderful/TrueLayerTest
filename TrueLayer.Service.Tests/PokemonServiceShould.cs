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
                    {new FlavorText {flavor_text = "flavor1", language = new Language {name = "en" }}}
            };

            var httpService = new Mock<IHttpService>();
            httpService.Setup(x => x.Get<PokemonSpecies>("https://pokeapi.co/api/v2/pokemon-species/mewtwo"))
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

    
   
}
