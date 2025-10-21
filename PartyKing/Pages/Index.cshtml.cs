using Logic.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Poke_Connector.Models;
using System.Runtime.CompilerServices;

public class IndexModel : PageModel
{
    private readonly LogicService _service;
    private readonly DatabaseService _databaseService;
    private readonly IConfiguration _configuration;
    public IndexModel(LogicService service, DatabaseService databaseService, IConfiguration configuration)
    {
        _service = service;
        _databaseService = databaseService;
        _configuration = configuration;
    }
    public List<PokemonDto>? Pokemons { get; private set; }
    public PokemonDto? Pokemon { get; private set; }
    public bool AlreadyExists { get; private set; }
    public string CatchDate { get; private set; }



    public async Task<IActionResult?> OnPostFetchAllAsync()
    {
        var all = await _databaseService.FetchFromDatabase();

        if (all != null)
        {
            Pokemons = all;
            return Page();
        }
        return null;
    }
    public async Task<IActionResult?> OnPostCatchPokemonAsync()
    {
        var result = await _service.GetRandomPokeAsync();

        if (result != null)
        {
            CatchDate = DateTime.Now.ToString();
            //Check if we already have this pokémon in pokéindex.
            var pokemons = _databaseService.GetPokemonsByNameAsync(result.Name);

            if (pokemons != null && pokemons?.Result?.Count > 0)
            {
                AlreadyExists = true;
            }
            else
            {
                AlreadyExists = false;
            }

            Pokemon = result;
            return Page();
        }
        return null;
    }
    public async Task<IActionResult> OnPostAddToIndexAsync(string pokemonName, string pokemonUrl, string pokemonDateTime, int pokemonId)
    {
        try
        {
            //Not an ideal solution but had problem with deserialization so for now we fetch image url via config + id
            var imageUrl = _configuration["Pokemon:Other_Dreamworld_Front_Default"];
            var fullUrl = imageUrl + pokemonId + ".svg";

            var pokemon = new PokemonDto
            {
                Name = pokemonName,
                Id = pokemonId,
                DateTime = pokemonDateTime,
                Image = fullUrl
            };



            //Add to pokéIndex
            await _databaseService.AddToDatabase(pokemon);

            TempData["Message"] = $"{pokemonName} added to your PokéIndex!";
        }
        catch (Exception ex)
        {
            TempData["Message"] = $"Error adding Pokémon: {ex.Message}";
        }

        return RedirectToPage(); 
    }
    public async Task<IActionResult> OnPostSkipAddAsync(string pokemonName)
    {
        TempData["Message"] = $"Bye {pokemonName}!";
        return RedirectToPage();
    }
}
