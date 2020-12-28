using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarWarsApi.DataAccess;
using StarWarsApi.DataAccess.Entities;

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
        public async Task<ActionResult<List<Film>>> GetFilms()
        {
            var films = await _context.Films
                .ToListAsync();

            return Ok(films);
        }

        #endregion


    }
}
