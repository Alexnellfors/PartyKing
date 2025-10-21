using Logic.DatabaseContexts;
using Microsoft.EntityFrameworkCore;
using Poke_Connector.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services
{
    public class DatabaseService
    {
        private readonly AppDbContext _dbContext;
        public DatabaseService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddToDatabase(PokemonDto poke)
        {
            try
            {
                await _dbContext.AddAsync(poke);
                await _dbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error adding to database: {ex.Message}");
            }       
        }
        public async Task<List<PokemonDto>> FetchFromDatabase()
        {
            try
            {
                var pokeList = await _dbContext.Pokemons.ToListAsync();
                return pokeList;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving from database: {ex.Message}");
                return null;
            }
        }
        public async Task<List<PokemonDto>?> GetPokemonsByNameAsync(string name)
        {
            try
            {
                var pokemons = await _dbContext.Pokemons
                    .Where(x => x.Name.ToLower().Equals(name.ToLower())).ToListAsync();

                return pokemons;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving from database: {ex.Message}");
                return null;
            }
        }
    }
}
