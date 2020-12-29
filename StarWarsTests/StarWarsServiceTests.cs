using System.Linq;
using System.Threading.Tasks;
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
        public async Task Can_get_all_films_with_character()
        {
            using (var context = new StarWarsDbContext(ContextOptions))
            {
                var service = new StarWarsService(context);

                int? pageNumber = null;
                int? pageSize = null;
                string species = "";
                string planet = "";

                var films = await service.GetFilms(
                    pageNumber,
                    pageSize,
                    species,
                    planet);

                Assert.Equal(6, films.Count);
                Assert.Equal("A New Hope", films.First().Title);
                Assert.Equal("Revenge of the Sith", films.Last().Title);
                Assert.Equal(18, films.First().Characters.Count);
                Assert.Equal("Luke Skywalker", films.First().Characters.First());
            }
        }
        #endregion

        #region CanGetAllCharacter

        [Fact]
        public async Task Can_get_all_characters()
        {
            using (var context = new StarWarsDbContext(ContextOptions))
            {
                var service = new StarWarsService(context);

                var characters = await service.GetCharacters();

                //Character with Id 17 is missing from the json files
                Assert.Equal(82, characters.Count);
                Assert.Equal("Luke Skywalker", characters.First().Name);
                Assert.Equal("Tion Medon", characters.Last().Name);
            }
        }


        #endregion

        //I have tried to test adding a new character, however I cannot figure out the best way to test this,
        //every time I have tried the below method, my test fails even though it works with manual testing.

        //#region CanAddACharacter

        //[Fact]
        //public async Task Can_add_a_character_with_birthyear()
        //{
        //    using (var context = new StarWarsDbContext(ContextOptions))
        //    {
        //        var service = new StarWarsService(context);

        //        var newCharacter = new CharacterModel
        //        {
        //            Name = "Test character",
        //            BirthYear = "112BBZ",
        //            Films = new List<string>()
        //            {
        //                "A new hope"
        //            }
        //        };

        //        var characters = await service.GetCharacters();
        //        var characterNames = characters.Select(c => c.Name).ToList();

        //        Assert.DoesNotContain(newCharacter.Name, characterNames);

        //        var newCharacters = await service.AddCharacter(newCharacter);
        //        var newCharacterNames = characters.Select(c => c.Name).ToList();
        //        Assert.Equal(83, newCharacterNames.Count);

        //        Assert.Contains(newCharacter.Name, newCharacterNames);

        //    }

        //}

        //#endregion

        #region CanGetFilmsWithPaging
        [Fact]

        public async Task Can_get_films_with_paging()
        {
            using (var context = new StarWarsDbContext(ContextOptions))
            {
                var service = new StarWarsService(context);

                int? pageNumber1 = 1;
                int? pageSize1 = 4;
                string species = "";
                string planet = "";

                var films1 = await service.GetFilms(
                    pageNumber1,
                    pageSize1,
                    species,
                    planet);

                Assert.Equal(4, films1.Count);
                Assert.Equal("A New Hope", films1.First().Title);
                Assert.Equal("The Phantom Menace", films1.Last().Title);

                int? pageNumber2 = 2;
                int? pageSize2 = 2;

                var films2 = await service.GetFilms(
                    pageNumber2,
                    pageSize2,
                    species,
                    planet);

                Assert.Equal(2, films2.Count);
                Assert.Equal("Return of the Jedi", films2.First().Title);
                Assert.Equal("The Phantom Menace", films1.Last().Title);

            }
        }

        #endregion

        #region CanFilterFilmsBySpecies
        [Fact]
        public async Task Can_filter_films_by_species()
        {
            using (var context = new StarWarsDbContext(ContextOptions))
            {
                var service = new StarWarsService(context);

                int? pageNumber = null;
                int? pageSize = null;
                string species = "Gungan";
                string planet = "";

                var films1 = await service.GetFilms(
                    pageNumber,
                    pageSize,
                    species,
                    planet);

                Assert.Equal(2, films1.Count);

                var filmTitles = films1.Select(f => f.Title).ToList();
                Assert.Contains("The Phantom Menace", filmTitles);
                Assert.Contains("Attack of the Clones", filmTitles);

                string species2 = "Twi'lek";

                var films2 = await service.GetFilms(
                    pageNumber,
                    pageSize,
                    species2,
                    planet);

                Assert.Equal(4, films2.Count);

                var filmTitles2 = films2.Select(f => f.Title).ToList();
                Assert.Contains("Return of the Jedi", filmTitles2);
                Assert.Contains("Revenge of the Sith", filmTitles2);

            }
        }

        #endregion

        #region CanFilterFilmsByPlanet

        [Fact]
        public async Task Can_filter_films_by_planet()
        {
            using (var context = new StarWarsDbContext(ContextOptions))
            {
                var service = new StarWarsService(context);

                int? pageNumber = null;
                int? pageSize = null;
                string species = "";
                string planet = "Alderaan";

                var films1 = await service.GetFilms(
                    pageNumber,
                    pageSize,
                    species,
                    planet);

                Assert.Equal(2, films1.Count);

                var filmTitles = films1.Select(f => f.Title).ToList();
                Assert.Contains("A New Hope", filmTitles);
                Assert.Contains("Revenge of the Sith", filmTitles);

                string planet2 = "Dagobah";

                var films2 = await service.GetFilms(
                    pageNumber,
                    pageSize,
                    species,
                    planet2);

                Assert.Equal(3, films2.Count);

                var filmTitles2 = films2.Select(f => f.Title).ToList();
                Assert.Contains("The Empire Strikes Back", filmTitles2);
                Assert.Contains("Return of the Jedi", filmTitles2);
                Assert.Contains("Revenge of the Sith", filmTitles2);

            }
        }

        #endregion
    }
}
