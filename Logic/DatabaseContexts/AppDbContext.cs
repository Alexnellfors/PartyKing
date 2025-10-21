using Logic.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poke_Connector.Models;

namespace Logic.DatabaseContexts
{
    public class AppDbContext : DbContext
    {
        public DbSet<PokemonDto> Pokemons => Set<PokemonDto>();

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PokeTableConfiguration());
        }
    }
}

