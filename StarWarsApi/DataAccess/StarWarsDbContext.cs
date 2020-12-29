using System;
using Microsoft.EntityFrameworkCore;
using StarWarsApi.DataAccess.Entities;

namespace StarWarsApi.DataAccess
{
    public class StarWarsDbContext : DbContext
    {
        #region Constructor

        public StarWarsDbContext(DbContextOptions<StarWarsDbContext> options)
            : base(options)
        { }

        #endregion

        #region DbSets
       
        public DbSet<Film> Films { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<Species> Species { get; set; }
        public DbSet<Planet> Planets { get; set; }
        public DbSet<FilmCharacterMapping> FilmCharacterMappings { get; set; }
        public DbSet<FilmSpeciesMapping> FilmSpeciesMapping { get; set; }
        public DbSet<FilmPlanetMapping> FilmPlanetMappings { get; set; }

        #endregion

    }
}
