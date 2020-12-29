using Newtonsoft.Json;

namespace StarWarsApi.DataAccess.Entities
{
    public class FilmPlanetMapping
    {
        [JsonProperty("id")]
        public int FilmPlanetMappingId { get; set; }
        public int FilmId { get; set; }
        public Film Film { get; set; }
        public int PlanetId { get; set; }
        public Planet Planet { get; set; }

    }
}
