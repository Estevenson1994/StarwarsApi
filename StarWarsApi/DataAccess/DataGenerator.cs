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
                if (context.Films.Any())
                {
                    return;
                }

                using (StreamReader file = File.OpenText("DataAccess/starwarsdata/films.json"))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    List<Film> films = (List<Film>)serializer.Deserialize(file, typeof(List<Film>));

                    context.Films.AddRange(films);
                }

                context.SaveChangesAsync();
            }
        }
    }
}
