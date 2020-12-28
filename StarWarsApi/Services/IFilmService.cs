﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StarWarsApi.Models;

namespace StarWarsApi.Services
{
    public interface IFilmService
    {
        Task<List<FilmModel>> GetFilms();
        Task<List<CharacterModel>> GetCharacters();
    }
}
