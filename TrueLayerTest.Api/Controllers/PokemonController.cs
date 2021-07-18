using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueLayer.Model;
using TrueLayer.Service;

namespace TrueLayerTest.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PokemonController : ControllerBase
    {
        private readonly ILogger<PokemonController> _logger;
        private readonly IPokemonService _pokemonService;

        public PokemonController(ILogger<PokemonController> logger, IPokemonService pokemonService)
        {
            _logger = logger;
            _pokemonService = pokemonService;
        }

        [HttpGet]
        [Route("{name}")]
        public async Task<ActionResult<Pokemon>> Get(string name)
        {
            var pokemon = await _pokemonService.GetPokemonDetails(name, false);

            return Ok(pokemon);
        }

        [HttpGet]
        [Route("translated/{name}")]
        public async Task<ActionResult<Pokemon>> GetTranslated(string name)
        {
            var pokemon = await _pokemonService.GetPokemonDetails(name, true);

            return Ok(pokemon);
        }
    }
}
