using System;
using Newtonsoft.Json;

namespace StarWarsApi.DataAccess.Entities
{
    public class Film
    {
        [JsonProperty("id")]
        public int FilmId { get; set; }
        public string Title { get; set; }
    }
}
