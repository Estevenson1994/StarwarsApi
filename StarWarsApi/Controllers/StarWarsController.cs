using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarWarsApi.Models;
using StarWarsApi.Services;

namespace StarWarsApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StarWarsController : ControllerBase
    {
        #region Private properties

        private readonly IFilmService _filmService;

        #endregion

        #region Constructor

        public StarWarsController(
            IFilmService filmService)
        {
            _filmService = filmService;
        }

        #endregion

        #region StarWarsController

        [HttpGet("Films")]
        [HttpGet("Films/pageNumber/{pageNumber}/pageSize/{pageSize}")]
        public async Task<ActionResult<List<FilmModel>>> GetFilms(
            int? pageNumber,
            int? pageSize,
            string species,
            string planet)
        {
            var films = await _filmService.GetFilms(pageNumber,
                pageSize,
                species,
                planet);

            return Ok(films);
        }

        [HttpGet("Characters")]
        public async Task<ActionResult<List<CharacterModel>>> GetCharacters()
        {
            var characters = await _filmService.GetCharacters();

            return Ok(characters);
        }

        [HttpPost("Characters")]
        public async Task<ActionResult<CharacterModel>> AddCharacter(
            CharacterModel character)
        {
            if(!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            await _filmService.AddCharacter(character);

            return CreatedAtAction(nameof(GetCharacters), new { id = character.Name }, character);
        }

        #endregion


    }
}
