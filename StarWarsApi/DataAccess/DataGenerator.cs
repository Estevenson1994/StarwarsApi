﻿using System;
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
        #region Initialise Database

        public static void Initialise(IServiceProvider serviceProvider)
        {
            using (var context = new StarWarsDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<StarWarsDbContext>>()))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                if (CheckEntitiesDontAlreadyExist(context))
                {
                    return;
                }

                AddEntities(context);
                AddCharacterEntities(context);

                AddFilmEntityMappings(context);

                context.SaveChangesAsync();
            }
        }

        public static bool CheckEntitiesDontAlreadyExist(StarWarsDbContext context)
        {
            return context.Films.Any()
                    || context.Characters.Any()
                    || context.Species.Any()
                    || context.Planets.Any();
        }


        public static void AddEntities(StarWarsDbContext context)
        {
            var films = ReadDataFromJsonFiles<List<Film>>("DataAccess/starwarsdata/films.json");
            context.Films.AddRange(films);

            var species = ReadDataFromJsonFiles<List<Species>>("DataAccess/starwarsdata/species.json");
            context.Species.AddRange(species);

            var planets = ReadDataFromJsonFiles<List<Planet>>("DataAccess/starwarsdata/planets.json");
            context.Planets.AddRange(planets);

        }
        public static void AddFilmEntityMappings(StarWarsDbContext context)
        {
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
        }

        public static void AddCharacterEntities(StarWarsDbContext context)
        {
            using (StreamReader file = File.OpenText("DataAccess/starwarsdata/people.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                dynamic dynJson = (dynamic)serializer.Deserialize(file, typeof(Object));
                {
                    foreach (var character in dynJson)
                    {
                        context.Characters.Add(
                            new Character
                            {                              
                                Name = character.name,
                                BirthYear = character.birthYear,
                                SpeciesId = character.speciesIds.Count > 0 ? character.speciesIds[0] : null,
                                CharacterId = character.id
                            });
                    }
                }
            }
        }

        #endregion

        #region Private methods

        private static T ReadDataFromJsonFiles<T>(string fileName)
        {
            using (StreamReader file = File.OpenText(fileName))
            {
                JsonSerializer serializer = new JsonSerializer();
                var entities = (T)serializer.Deserialize(file, typeof(T));

                return entities;

            }
        }

        #endregion
    }
}
