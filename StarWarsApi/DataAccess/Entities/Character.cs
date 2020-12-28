using System;
using Newtonsoft.Json;

namespace StarWarsApi.DataAccess.Entities
{
    public class Character
    {
        [JsonProperty("id")]
        public int CharacterId { get; set; }
        public string Name { get; set; }
    }
}
