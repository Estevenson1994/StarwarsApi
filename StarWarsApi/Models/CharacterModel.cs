using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using StarWarsApi.Services;

namespace StarWarsApi.Models
{
    public class CharacterModel : IValidatableObject
    {
        #region Public properties

        public string Name { get; set; }
        public string BirthYear { get; set; }
        public List<string> Films { get; set; }
        public string Species { get; set; }

        #endregion

        #region IValidatableObject

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Films.Count < 1)
            {
                yield return new ValidationResult(
                    $"Character must be in at least one film");
            }

            var starWarsService = validationContext.GetService<IFilmService>();

            // HACK: Having to run async tasks asynchronously

            if(starWarsService.CharacterExists(Name).Result)
            {
                yield return new ValidationResult(
                    $"Character with name '{Name}' already exists");
            }

            foreach(var filmTitle in Films)
            {
                if (!starWarsService.FilmExists(filmTitle).Result)
                {
                    yield return new ValidationResult(
                        $"Film with title '{filmTitle}' doesn't exist");
                }
            }
        }

        #endregion

    }
}
