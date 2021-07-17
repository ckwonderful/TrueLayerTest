using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueLayer.Model;

namespace TrueLayer.Service
{
    public class PokemonService : IPokemonService
    {
        private readonly IHttpService<PokemonSpecies> _httpService;
        private string _pokemonDetailsBaseUrl = "https://pokeapi.co/api/v2/pokemon-species";

        public PokemonService(IHttpService<PokemonSpecies> httpService)
        {
            _httpService = httpService;
        }

        public async Task<Pokemon> GetPokemonBasicDetails(string name)
        {
            var pokemonSpeciesDetails = await _httpService.Get($"{_pokemonDetailsBaseUrl}/{name}");

            return new Pokemon
            {
                Name = pokemonSpeciesDetails.name,
                Description = pokemonSpeciesDetails.flavor_text_entries.FirstOrDefault(x => x.language.name == "en")?.flavor_text,
                Habitat = pokemonSpeciesDetails.habitat.name,
                IsLegendary = pokemonSpeciesDetails.is_legendary
            };
        }
    }


    public interface IPokemonService
    {
        Task<Pokemon> GetPokemonBasicDetails(string name);
    }
}
