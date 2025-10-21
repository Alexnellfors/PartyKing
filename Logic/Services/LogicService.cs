using Microsoft.Extensions.Configuration;
using Poke_Connector.Models;
using Poke_Connector.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services
{
    public class LogicService
    {
        private readonly PokeService _pokeService;
        private readonly IConfiguration _configuration;

        public LogicService(PokeService pokeService, IConfiguration configuration)
        {
            _pokeService = pokeService;
            _configuration = configuration;
        }
        public async Task<PokemonDto?> GetPokeAsync(int id)
        {
            var pokemon = await _pokeService.GetPokeAsync(id);

            if (pokemon == null)
            {
                return null;
            }
            return pokemon;
        }
        public async Task<PokemonDto?> GetRandomPokeAsync()
        {
            int maxPokeIds = 0;

            try
            {
                var maxIdsValue = _configuration["Pokemon:MaxPokemonIds"];

                if (!int.TryParse(maxIdsValue, out maxPokeIds))
                {
                    //default value
                    maxPokeIds = 1025;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading configuration: {ex.Message}");
                maxPokeIds = 1025; //default value
            }

            var random = new Random();
            int randomId = random.Next(1, maxPokeIds + 1);

            PokemonDto? pokemon = new PokemonDto();
            pokemon = await _pokeService.GetPokeAsync(randomId);

            if (pokemon == null)
            {
                return null;
            }
            return pokemon;
        }
    }
}
