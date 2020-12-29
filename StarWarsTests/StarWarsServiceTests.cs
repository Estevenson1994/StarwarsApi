using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StarWarsApi.DataAccess;
using StarWarsApi.Services;
using Xunit;

namespace StarWarsTests
{
    public abstract class StarWarsServiceTest
    {
        #region Seeding test database
        protected StarWarsServiceTest(DbContextOptions<StarWarsDbContext> contextOptions)
        {
            ContextOptions = contextOptions;

            Seed();

        }

        protected DbContextOptions<StarWarsDbContext> ContextOptions { get; }

        private void Seed()
        {
            using (var context = new StarWarsDbContext(ContextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                DataGenerator.CheckEntitiesDontAlreadyExist(context);
                DataGenerator.AddEntities(context);
                DataGenerator.AddFilmEntityMappings(context);

                context.SaveChanges();

            }

        }

        #endregion

        #region CanGetAllFilmsWithCharacters

        [Fact]
        public void Can_get_all_films_with_character()
        {
            using (var context = new StarWarsDbContext(ContextOptions))
            {
                var service = new StarWarsService(context);

                int? pageNumber = null;
                int? pageSize = null;
                string species = "";
                string planet = "";

                var films = service.GetFilms(
                    pageNumber,
                    pageSize,
                    species,
                    planet).Result;

                Assert.Equal(6, films.Count);
                Assert.Equal("A New Hope", films.First().Title);
                Assert.Equal("Revenge of the Sith", films.Last().Title);
                Assert.Equal(18, films.First().Characters.Count);
                Assert.Equal("Luke Skywalker", films.First().Characters.First());
            }
        }
        #endregion
    }
}
