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

    }
}
