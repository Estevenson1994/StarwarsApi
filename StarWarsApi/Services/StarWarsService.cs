﻿using System.Collections.Generic;
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
            int? pageSize)
        {
            var filmsQueryable = _context.Films
                .Include(f => f.Characters)
                .ThenInclude(c => c.Character)
                .Select(f => new FilmModel
                {
                    Title = f.Title,
                    Characters = f.Characters.Select(c => c.Character.Name).ToList()
                });
            if (pageNumber.HasValue && pageSize.HasValue)
            {
                return await PaginatedList<FilmModel>.Create(
                    filmsQueryable,
                    (int)pageNumber,
                    (int)pageSize);

            }
            else
            {
                return await PaginatedList<FilmModel>.Create(
                    filmsQueryable,
                    1,
                    filmsQueryable.Count());
            }
           
        }

        public async Task<List<CharacterModel>> GetCharacters()
        {
            return await _context.Characters
                .Include(c => c.Films)
                .ThenInclude(f => f.Film)
                .Select(c => new CharacterModel
                {
                    Name = c.Name,
                    BirthYear = c.BirthYear,
                    Films = c.Films.Select(f => f.Film.Title).ToList()
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
        #endregion

    }
}
