using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StarWarsApi.Models;

namespace StarWarsApi.Services
{
    public interface IFilmService
    {
        Task<List<FilmModel>> GetFilms(int? pageNumber,
            int? pageSize,
            string species);
        Task<List<CharacterModel>> GetCharacters();
        Task AddCharacter(CharacterModel character);
        Task<bool> CharacterExists(string name);
        Task<bool> FilmExists(string title);
    }
}
