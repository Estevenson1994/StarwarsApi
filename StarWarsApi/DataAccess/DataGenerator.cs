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
                if (context.Films.Any() || context.Characters.Any())
                {
                    return;
                } 
		    
                var films = ReadDataFromJsonFiles<List<Film>>("DataAccess/starwarsdata/films.json");
                context.Films.AddRange(films);

                var characters = ReadDataFromJsonFiles<List<Character>>("DataAccess/starwarsdata/people.json");
                context.Characters.AddRange(characters);


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
