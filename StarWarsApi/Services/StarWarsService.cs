using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StarWarsApi.DataAccess;
using StarWarsApi.DataAccess.Entities;
using StarWarsApi.Models;

namespace StarWarsApi.Services
{
    public class StarWarsService : IFilmService
    {
        #region Private properties

        private readonly StarWarsDbContext _context;

        #endregion

        #region Constructor

        public StarWarsService(
            StarWarsDbContext context)
        {
            _context = context;
        }

        #endregion

        public async Task<List<FilmModel>> GetFilms()
        {
            return await _context.Films
                .Include(f => f.Characters)
                .ThenInclude(c => c.Character)
                .Select(f => new FilmModel
                {
                    Title = f.Title,
                    Characters = f.Characters.Select(c => c.Character.Name).ToList()
                })
                .ToListAsync();
        }

        public async Task<List<CharacterModel>> GetCharacters()
        {
            return await _context.Characters
                .Select(c => new CharacterModel
                {
                    Name = c.Name,
                    BirthYear = c.BirthYear
                }).ToListAsync();
        }

        public async Task AddCharacter(CharacterModel character)
        {
            var newCharacter = new Character
            {
                Name = character.Name,
                BirthYear = character.BirthYear ?? "unknown"
            };

            _context.Characters.Add(newCharacter);
            await _context.SaveChangesAsync();
        }

    }
}
