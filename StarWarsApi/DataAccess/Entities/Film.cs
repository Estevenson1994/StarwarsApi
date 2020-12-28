using System;
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
    }
}
