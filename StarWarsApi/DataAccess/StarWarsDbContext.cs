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

        #endregion

    }
}
