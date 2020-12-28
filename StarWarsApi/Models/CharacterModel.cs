using System.Collections.Generic;

namespace StarWarsApi.Models
{
    public class CharacterModel
    {
        public string Name { get; set; }
        public string BirthYear { get; set; }
        public List<string> Films { get; set; }

    }
}
