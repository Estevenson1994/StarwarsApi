using System.Collections.Generic;

namespace StarWarsApi.Models
{
    public class FilmModel
    {
        public string Title { get; set; }
        public List<string> Characters { get; set; }
        public List<string> Species { get; set; }
        public List<string> Planets { get; set; }

    }
}
