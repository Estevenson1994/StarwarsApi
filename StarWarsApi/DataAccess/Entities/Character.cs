using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace StarWarsApi.DataAccess.Entities
{
    public class Character
    {
        [JsonProperty("id")]
        public int CharacterId { get; set; }
        public string Name { get; set; }
        public string BirthYear { get; set; }
        public virtual Collection<FilmCharacterMapping> Films { get; set; }
        public int? SpeciesId { get; set; }
        public virtual Species Species { get; set; }
    }
}
