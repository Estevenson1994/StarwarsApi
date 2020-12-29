using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using StarWarsApi.DataAccess.Entities;

namespace StarWarsApi.DataAccess
{
    public class DataGenerator
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new StarWarsDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<StarWarsDbContext>>()))
            {
                context.Database.EnsureCreated();
                if (context.Films.Any()
                    || context.Characters.Any()
                    || context.Species.Any()
                    || context.Planets.Any())
                {
                    return;
                } 
		    
                var films = ReadDataFromJsonFiles<List<Film>>("DataAccess/starwarsdata/films.json");
                context.Films.AddRange(films);

                var characters = ReadDataFromJsonFiles<List<Character>>("DataAccess/starwarsdata/people.json");
                context.Characters.AddRange(characters);

                var species = ReadDataFromJsonFiles<List<Species>>("DataAccess/starwarsdata/species.json");
                context.Species.AddRange(species);

                var planets = ReadDataFromJsonFiles<List<Planet>>("DataAccess/starwarsdata/planets.json");
                context.Planets.AddRange(planets);

                using (StreamReader file = File.OpenText("DataAccess/starwarsdata/films.json"))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    dynamic dynJson = (dynamic)serializer.Deserialize(file, typeof(Object));
		            
		            foreach (var film in dynJson)
                    {
                        foreach (var characterId in film.characterIds)
                        { 
                            context.FilmCharacterMappings.Add(
                                new FilmCharacterMapping
                                {
                                    FilmId = film.id,
                                    CharacterId = characterId
                                });
			            }
                        foreach (var speciesId in film.speciesIds)
                        {
                            context.FilmSpeciesMapping.Add(
                                new FilmSpeciesMapping
                                {
                                    FilmId = film.id,
                                    SpeciesId = speciesId
                                });
                        }
                        foreach (var planetId in film.planetIds)
                        {
                            context.FilmPlanetMappings.Add(
                                new FilmPlanetMapping
                                {
                                    FilmId = film.id,
                                    PlanetId = planetId
                                });
                        }
		            } 
		        }

                    context.SaveChangesAsync();
            }
        }

        private static T ReadDataFromJsonFiles<T>(string fileName)
        {
            using(StreamReader file = File.OpenText(fileName))
            {
                JsonSerializer serializer = new JsonSerializer();
                var entities = (T)serializer.Deserialize(file, typeof(T));

                return entities;
                
	        }
	    }

    }
}
