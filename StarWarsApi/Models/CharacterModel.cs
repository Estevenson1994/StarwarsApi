using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StarWarsApi.Models
{
    public class CharacterModel : IValidatableObject
    {
        #region Public properties

        public string Name { get; set; }
        public string BirthYear { get; set; }
        public List<string> Films { get; set; }

        #endregion

        #region IValidatableObject

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Films.Count < 1)
            {
                yield return new ValidationResult(
                    $"Character must be in at least one film");
            }
        }

        #endregion

    }
}
