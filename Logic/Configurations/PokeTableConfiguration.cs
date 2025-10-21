using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poke_Connector.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Configurations
{
    public class PokeTableConfiguration : IEntityTypeConfiguration<PokemonDto>
    {
        public void Configure(EntityTypeBuilder<PokemonDto> builder)
        {
            builder.ToTable("Pokemons", "dbo");
            builder.HasKey(a => a.Name);
        }
    }
}
