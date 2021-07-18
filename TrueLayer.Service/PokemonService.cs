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
        private readonly ITranslationServiceFactory _translationServiceFactory;
        private string _pokemonDetailsBaseUrl = "https://pokeapi.co/api/v2/pokemon-species";

        public PokemonService(IHttpService<PokemonSpecies> httpService, ITranslationServiceFactory translationServiceFactory)
        {
            _httpService = httpService;
            _translationServiceFactory = translationServiceFactory;
        }

        public async Task<Pokemon> GetPokemonBasicDetails(string name)
        {
            var pokemonSpeciesDetails = await _httpService.Get($"{_pokemonDetailsBaseUrl}/{name}");

            var description = pokemonSpeciesDetails.flavor_text_entries
                .FirstOrDefault(x => x.language.name == "en")
                ?.flavor_text;

            ITranslationService translationService;
            if (pokemonSpeciesDetails.is_legendary || pokemonSpeciesDetails.habitat.name.Equals("cave"))
            {
                translationService = _translationServiceFactory.Create("yoda");
                
            }
            else
            {
                translationService = _translationServiceFactory.Create("shakespeare");
            }

            description = await translationService.Translate(
                pokemonSpeciesDetails.flavor_text_entries
                    .FirstOrDefault(x => x.language.name == "en")?
                    .flavor_text);

            return new Pokemon
            {
                Name = pokemonSpeciesDetails.name,
                Description = description,
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
