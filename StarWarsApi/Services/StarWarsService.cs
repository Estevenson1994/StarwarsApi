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

        #region StarWarsService

        public async Task<List<FilmModel>> GetFilms(
            int? pageNumber,
            int? pageSize,
            string species,
            string planet)
        {
            var filmsQueryable = GetFilmsQueryable();

            if (!string.IsNullOrWhiteSpace(species))
                filmsQueryable = filmsQueryable
                    .Where(f => f.Species.Contains(species));
            if (!string.IsNullOrWhiteSpace(planet))
                filmsQueryable = filmsQueryable
                    .Where(f => f.Planets.Contains(planet));

            return await PaginatedList<FilmModel>.Create(
                filmsQueryable,
                pageNumber,
                pageSize);           
        }

        public async Task<List<CharacterModel>> GetCharacters()
        {
            return await _context.Characters
                .Include(c => c.Films)
                .ThenInclude(f => f.Film)
                .Include(c => c.Species)
                .Select(c => new CharacterModel
                {
                    Name = c.Name,
                    BirthYear = c.BirthYear,
                    Films = c.Films.Select(f => f.Film.Title).ToList(),
                    Species = c.Species.Name
                }).ToListAsync();
        }

        public async Task AddCharacter(CharacterModel character)
        {
            var species = await _context.Species
                .Where(s => s.Name == character.Species)
                .FirstOrDefaultAsync();

            var newCharacter = new Character
            {
                Name = character.Name,
                BirthYear = character.BirthYear ?? "unknown",
                Species = species
            };

            _context.Characters.Add(newCharacter);

            await AddFilmCharacterMapping(character, newCharacter);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> CharacterExists(string name)
        {
            return await _context.Characters
                .Where(c => c.Name == name)
                .AnyAsync();
        }

        public async Task<bool> FilmExists(string title)
        {
            return await _context.Films
                .Where(f => f.Title == title)
                .AnyAsync();
        }

        public async Task<bool> SpeciesExists(string name)
        {
            return await _context.Species
                .Where(s => s.Name == name)
                .AnyAsync();
        }


        #endregion

        #region Private methods

        private async Task AddFilmCharacterMapping(
            CharacterModel character, Character newCharacter)
        {
            foreach (var filmTitle in character.Films)
            {
                var film = await _context.Films
                    .Where(f => f.Title == filmTitle)
                    .FirstOrDefaultAsync();

                _context.FilmCharacterMappings.Add(
                    new FilmCharacterMapping
                    {
                        Film = film,
                        Character = newCharacter
                    });
            }
        }

        private IQueryable<FilmModel> GetFilmsQueryable()
        {
            return _context.Films
                .Include(f => f.Characters)
                .ThenInclude(c => c.Character)
                .Include(f => f.Species)
                .ThenInclude(s => s.Species)
                .Include(f => f.Planets)
                .ThenInclude(p => p.Planet)
                .Select(f => new FilmModel
                {
                    Title = f.Title,
                    Characters = f.Characters.Select(c => c.Character.Name).ToList(),
                    Species = f.Species.Select(s => s.Species.Name).ToList(),
                    Planets = f.Planets.Select(p => p.Planet.Name).ToList()
                });
        }

        #endregion

    }
}
