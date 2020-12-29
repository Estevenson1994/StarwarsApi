using Newtonsoft.Json;

namespace StarWarsApi.DataAccess.Entities
{
    public class Planet
    {
        [JsonProperty("id")]
        public int PlanetId { get; set; }
        public string Name { get; set; }
    }
}
