using System.Collections.Generic;

namespace StarWarsApi.Models
{
    public class FilmModel
    {
        public string Title { get; set; }
        public List<CharacterModel> Characters { get; set; }

    }
}
