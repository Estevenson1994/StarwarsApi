using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarWarsApi.DataAccess;
using StarWarsApi.Models;

namespace StarWarsApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StarWarsController : ControllerBase
    {
        #region Private properties

        private readonly StarWarsDbContext _context;

        #endregion

        #region Constructor

        public StarWarsController(StarWarsDbContext context)
        {
            _context = context;
        }

        #endregion

        #region StarWarsController

        [HttpGet("Films")]
        public async Task<ActionResult<List<FilmModel>>> GetFilms()
        {
            var films = await _context.Films
                .Include(f => f.Characters)
                .ThenInclude(c => c.Character)
                .Select(f => new FilmModel
                {
                    Title = f.Title,
                    Characters = f.Characters.Select(c => c.Character.Name).ToList()
                })
                .ToListAsync();

            return Ok(films);
        }

        [HttpGet("Characters")]
        public async Task<ActionResult<List<CharacterModel>>> GetCharacters()
        {
            var characters = await _context.Characters
                .Select(c => new CharacterModel
                {
                    Name = c.Name,
                    BirthYear = c.BirthYear
                }).ToListAsync();

            return Ok(characters);
        }

        #endregion


    }
}
