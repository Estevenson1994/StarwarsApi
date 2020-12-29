using Newtonsoft.Json;

namespace StarWarsApi.DataAccess.Entities
{
    public class Species
    {
        [JsonProperty("id")]
        public int SpeciesId { get; set; }
        public string Name { get; set; }        
    }
}
