using Poke_Connector.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Poke_Connector.Services
{
    public class PokeService: BaseService
    {
        public PokeService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {

        }
        public async Task<PokemonDto?> GetPokeAsync(int id)
        {
            var result = await GetAsync<PokemonDto>($"pokemon/{id}");
            return result;
        }
    }
}
