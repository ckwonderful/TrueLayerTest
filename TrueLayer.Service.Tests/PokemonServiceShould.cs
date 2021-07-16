using System;
using FluentAssertions;
using NUnit.Framework;

namespace TrueLayer.Service.Tests
{
    [TestFixture]
    public class PokemonServiceShould
    {
        [Test]
        public void ReturnBasicPokemonInformationForAValidName()
        {
            var expectedResult = new Pokemon
            {
                Name = "mewtwo",
                Description = "something",
                Habitat = "home",
                IsLegendary = true
            };
            var sut = new PokemonService();
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
        public Pokemon GetPokemonBasicDetails(string mewtwo)
        {
            throw new NotImplementedException();
        }
    }

    public interface IPokemonService
    {
        Pokemon GetPokemonBasicDetails(string mewtwo);
    }
}
