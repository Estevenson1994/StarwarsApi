using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace StarWarsApi.DataAccess.Entities
{
    public class Film
    {
        [JsonProperty("id")]
        public int FilmId { get; set; }
        public string Title { get; set; }
        public virtual Collection<FilmCharacterMapping> Characters { get; set; }
        public virtual Collection<FilmSpeciesMapping> Species { get; set; }
        public virtual Collection<FilmPlanetMapping> Planets { get; set; }
    }
}
